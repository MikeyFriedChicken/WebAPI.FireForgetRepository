using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MikeyFriedChicken.WebAPI.FireForgetRepository.Database;
using MikeyFriedChicken.WebAPI.FireForgetRepository.Model;

namespace MikeyFriedChicken.WebAPI.FireForgetRepository.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly IFireForgetRepositoryHandler _fireForgetRepositoryHandler;


        public BlogController(
            IRepository repository, IFireForgetRepositoryHandler fireForgetRepositoryHandler)
        {
            _repository = repository;
            _fireForgetRepositoryHandler = fireForgetRepositoryHandler;
        }

        [HttpGet("UpdateBlog")]
        public async Task<ActionResult> UpdateBlog(string blogtext)
        {
            Blog blog = new Blog() { Text = blogtext };

            // Updated the blog
            await _repository.UpdateBlog(blog);

            // Need to wait for blog auditing to complete before returning the result
            await _repository.AuditBlogUpdate(new BlogAudit(blog));

            // Returns once both async methods complete
            return Ok();
        }

        [HttpGet("UpdateBlogFastIncorrectly")]
        public async Task<ActionResult> UpdateBlogFastIncorrectly(string blogtext)
        {
            Blog blog = new Blog() { Text = blogtext };

            // Updated the blog
            await _repository.UpdateBlog(blog);

            // INCORRECT.  As the task is not awaited it will probably 
            // run after the request has returned and the repository
            // containing the scoped database context is no longer available

            _ = Task.Run(async () =>
            {
                try
                {
                    await _repository.AuditBlogUpdate(new BlogAudit(blog));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });

            // Can return to the requester before the above auditing completes
            return Ok();
        }


        [HttpGet("UpdateBlogFastMessy")]
        public async Task<ActionResult> UpdateBlogFastMessy(string blogtext, 
            [FromServices]IServiceScopeFactory serviceScopeFactory)
        {
            Blog blog = new Blog() { Text = blogtext };

            // Updated the blog
            await _repository.UpdateBlog(blog);

            // CORRECT. BUT would be nicer to hide the scope / ioc container references away (i.e no service locator pattern here)
            _ = Task.Run(async () =>
            {
                try
                {
                    using var scope = serviceScopeFactory.CreateScope();
                    var repository = scope.ServiceProvider.GetRequiredService<IRepository>();
                    await repository.AuditBlogUpdate(new BlogAudit(blog));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });

            // Can return to the requester before the above auditing completes
            return Ok();
        }


        [HttpGet("UpdateBlogFast")]
        public async Task<ActionResult> UpdateBlogFast(string blogtext)
        {
            Blog blog = new Blog() { Text = blogtext };

            // Updated the blog
            await _repository.UpdateBlog(blog);

            // Delegate the blog auditing to another task on the threadpool
            _fireForgetRepositoryHandler.Execute(async repository =>
            {
                // Will receive its own scoped repository on the executing task
                await repository.AuditBlogUpdate(new BlogAudit(blog));
            });

            // Can return to the requester before the above auditing completes
            return Ok();
        }
    }
}

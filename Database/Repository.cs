using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MikeyFriedChicken.WebAPI.FireForgetRepository.Model;

namespace MikeyFriedChicken.WebAPI.FireForgetRepository.Database
{
    public class Repository: IRepository
    {
        private readonly BloggingContext _context;

        public Repository(BloggingContext bloggingContext)
        {
            _context = bloggingContext;
        }

        public async Task UpdateBlog(Blog blog)
        {
            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();
        }

        public async Task AuditBlogUpdate(BlogAudit audit)
        {
            _context.BlogAudits.Add(audit);
            await _context.SaveChangesAsync();
        }
    }
}

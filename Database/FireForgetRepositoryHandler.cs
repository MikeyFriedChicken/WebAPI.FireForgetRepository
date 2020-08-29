using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace MikeyFriedChicken.WebAPI.FireForgetRepository.Database
{
    public class FireForgetRepositoryHandler : IFireForgetRepositoryHandler
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public FireForgetRepositoryHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Execute(Func<IRepository, Task> databaseWork)
        {
            // Fire off the task, but don't await the result
            Task.Run(async () =>
            {
                // Exceptions must be caught
                try
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var repository = scope.ServiceProvider.GetRequiredService<IRepository>();
                    await databaseWork(repository);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
        }
    }
}
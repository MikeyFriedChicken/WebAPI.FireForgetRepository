using System;
using System.Threading.Tasks;

namespace MikeyFriedChicken.WebAPI.FireForgetRepository.Database
{
    public interface IFireForgetRepositoryHandler
    {
        void Execute(Func<IRepository, Task> databaseWork);
    }
}
using System.Threading.Tasks;
using MikeyFriedChicken.WebAPI.FireForgetRepository.Model;

namespace MikeyFriedChicken.WebAPI.FireForgetRepository.Database
{
    public interface IRepository
    {
        Task UpdateBlog(Blog blog);

        Task AuditBlogUpdate(BlogAudit audit);
    }
}
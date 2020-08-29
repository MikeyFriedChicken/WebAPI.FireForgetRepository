using Microsoft.EntityFrameworkCore;
using MikeyFriedChicken.WebAPI.FireForgetRepository.Model;

namespace MikeyFriedChicken.WebAPI.FireForgetRepository
{
    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogAudit> BlogAudits { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("BloggingDatabase");
        }
    }
}
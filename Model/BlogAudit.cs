namespace MikeyFriedChicken.WebAPI.FireForgetRepository.Model
{
    public class BlogAudit
    {
        public BlogAudit()
        {

        }
        public BlogAudit(Blog blog)
        {
            BlogName = blog.Name;
            BlogText = blog.Text;
        }

        public int BlogAuditId { get; set; }

        public string BlogName { get; set; }

        public string BlogText { get; set; }

    }
}
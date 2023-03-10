using Microsoft.EntityFrameworkCore;

namespace BlogApp.Models
{
    public class BlogContext :DbContext  {
       private readonly IConfiguration _configuration;

        public BlogContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("PostgreSQL"));

        }

        
        public DbSet<Blog> Blogs { get; set; }
    }

}

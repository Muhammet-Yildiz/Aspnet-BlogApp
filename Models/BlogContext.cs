using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BlogApp.Models
{
    public class BlogContext :DbContext  {
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {

        }
        public DbSet<Blog> Blogs { get; set; }
    }

}

using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authorization;
namespace BlogApp.Controllers
{
   

    [Authorize(Roles="admin")]
    public class AdminController : Controller
    {
        private readonly MainContext _context;

        public AdminController(MainContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            // var blogs = _context.Blogs.ToList();
        // var blogs = _context.Blogs.Include(b => b.User).ToList();

        //     return View(blogs);

        return RedirectToAction("Blogs");
           
        }

         public IActionResult Comments() {

            var comments = _context.Comments.
            Include(c => c.Blog).
            Include(c => c.User).
            ToList();

            return View(comments);
           
        }
          public IActionResult Users()  {

            var users = _context.Users.ToList();

            return View(users);
           
        }

          public IActionResult Blogs()  {

    // var blogs = _context.Blogs.ToList();
        var blogs = _context.Blogs.Include(b => b.User).ToList();
            return View(blogs);
           
        }

    }
}
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

        [HttpGet]
         public async  Task<IActionResult> EditUser(Guid? id)
        {
                if (id == null){
                return RedirectToAction("NotFound", "Error");

                }

               var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);

                if (user == null){
                     return RedirectToAction("NotFound", "Error");

                }

            var model = new UpdateUserViewModel {
                Email = user.Email,
                Role = user.Role,
                FullName = user.FullName,
                ProfileImage = user.ProfileImage
            };

            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> EditUser(Guid id, UpdateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
System.Console.WriteLine("AdminController.EditUser() user: " + user.FullName);
                if (user == null)
                {
                    return RedirectToAction("NotFound", "Error");
                }

                user.Email = model.Email;
                user.Role = model.Role;
                user.FullName = model.FullName;
                // user.ProfileImage = model.ProfileImage;

                await _context.SaveChangesAsync();

                return RedirectToAction("Users");
            }

            return View(model);
        }


    }
}
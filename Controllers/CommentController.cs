using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims ;
using Microsoft.AspNetCore.Authorization;

namespace BlogApp.Controllers
{
    public class CommentController : Controller
    {
        private readonly MainContext _context;

        public CommentController(MainContext context)
        {
            _context = context;
        }

        [HttpGet]
        public PartialViewResult Create()
        {
            return PartialView();

        }

         [HttpPost]
         [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Comment comment) {

            Console.WriteLine("CommentController.Create() comment.BlogId: " + comment);
            Console.WriteLine("comment.BlogId "+comment.BlogId );
            // Console.WriteLine( comment.BlogId + comment.Content + comment.CreatedDateTime + comment.Author );


            
            if (ModelState.IsValid)
            {
                 Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                 User user = _context.Users.SingleOrDefault(u => u.Id == userId);

                 comment.UserId = user.Id ;

                comment.CreatedDateTime = DateTime.UtcNow;
                _context.Add(comment);

                await _context.SaveChangesAsync();
                // return RedirectToAction("Detail", "Blog", new { id = comment.BlogId });
                return RedirectToAction("Detail", "Blog", new { id = comment.BlogId });
                // return RedirectToAction("Index", "Blog");


            }
            return View(comment);


        }


    }
}
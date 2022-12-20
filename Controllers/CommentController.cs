using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Comment comment) {

            Console.WriteLine("CommentController.Create() comment.BlogId: " + comment);
            Console.WriteLine("comment.BlogId "+comment.BlogId );
            Console.WriteLine( comment.BlogId + comment.Content + comment.CreatedDateTime + comment.Author );

            
            if (ModelState.IsValid)
            {
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
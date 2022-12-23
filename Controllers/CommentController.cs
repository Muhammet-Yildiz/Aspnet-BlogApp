using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
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
        public async Task<IActionResult> Create(Comment comment)
        {
            if (ModelState.IsValid)
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                User user = _context.Users.SingleOrDefault(u => u.Id == userId);

                comment.UserId = user.Id;

                comment.CreatedDateTime = DateTime.UtcNow;
                _context.Add(comment);

                await _context.SaveChangesAsync();
                return RedirectToAction("Detail", "Blog", new { id = comment.BlogId });

            }
            return View(comment);

        }

        [HttpGet]
        [Authorize]

        public async Task<IActionResult> Edit(int id)
        {
            var comment = await _context.Comments.SingleOrDefaultAsync(m => m.CommentId == id);

            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (!comment.UserId.Equals(userId))
            {

                if (!User.IsInRole("admin"))
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }

            if (comment == null)
            {
                return RedirectToAction("Error", "Home");
            }
            var model = new UpdateCommentViewModel
            {
                Content = comment.Content
            };


            return View(model);

        }

        [HttpPost]
        [Authorize]

        // belli alanları guncellemek ıcın bu view modellerı kullanmak karısıklıktan kurtarır
        public async Task<IActionResult> Edit(int id, UpdateCommentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var comment = await _context.Comments.SingleOrDefaultAsync(m => m.CommentId == id);

            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (!comment.UserId.Equals(userId))
            {

                if (!User.IsInRole("admin"))
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }
            comment.Content = model.Content;
            await _context.SaveChangesAsync();

            if (User.IsInRole("admin"))
            {
                return RedirectToAction("Comments", "Admin");
            }

            return RedirectToAction("Detail", "Blog", new { id = comment.BlogId });
        }


        [Authorize]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            var comment = await _context.Comments
                .SingleOrDefaultAsync(m => m.CommentId == id);

            if (comment == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (!comment.UserId.Equals(userId))
            {

                if (!User.IsInRole("admin"))
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Detail", "Blog", new { id = comment.BlogId });

        }

    }
}
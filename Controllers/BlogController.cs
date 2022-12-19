using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly BlogContext _context;

        public BlogController(BlogContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var blogs = _context.Blogs;
            return View(await blogs.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog)
        {
            if (ModelState.IsValid)
            {

                 var image = Request.Form.Files[0];

                if (image != null && image.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await image.CopyToAsync(stream);
                        blog.Image = Convert.ToBase64String(stream.ToArray());
                    }
                }

                _context.Add(blog);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Blog");
            }
            return View(blog);
        }


        public async Task<IActionResult> Detail(int? id)
        {
            // burda not found sayfası eklenmelı 

            if (id == null)
            {
                  return RedirectToAction("NotFound", "Error");
            }

            var blog = await _context.Blogs.FindAsync(id);

            if (blog == null)
            {
             return RedirectToAction("NotFound", "Error");
            }

            return View(blog);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
               return RedirectToAction("NotFound", "Error");
            }

            var blog = await _context.Blogs.FindAsync(id);

            if (blog == null)
            {
               return RedirectToAction("NotFound", "Error");
            }
            return View(blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
     
        public async Task<IActionResult> Edit( int id , Blog blog)
        {
            // var target = await _context.Blogs.FindAsync(id);


            if (id != blog.BlogId)
            {
                // return NotFound();
               return RedirectToAction("NotFound", "Error");

            }
   

            if (ModelState.IsValid)
            {
                try
                {

                    var image = Request.Form.Files[0];

                    if (image != null && image.Length > 0)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await image.CopyToAsync(stream);
                            blog.Image = Convert.ToBase64String(stream.ToArray());
                        }

                    }

                     _context.Update(blog);
                      await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
               return RedirectToAction("NotFound", "Error");
                }

                return RedirectToAction("Index");

            }
            return View(blog);
        }

        public async Task<IActionResult> Delete(int id)
        {       

            var blog = await _context.Blogs.FindAsync(id);

            
            if (blog == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            _context.Blogs.Remove(blog);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");

            // return View(blog);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            var blog = await _context.Blogs.FindAsync(id);

            if (blog == null)
            {
                 return RedirectToAction("NotFound", "Error");
            }

            _context.Remove(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}

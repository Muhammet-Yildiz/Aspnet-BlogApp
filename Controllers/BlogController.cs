using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims ;

namespace BlogApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly MainContext _context;

        public BlogController(MainContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var blogs = _context.Blogs;
            return View(await blogs.ToListAsync());
        }
       
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(Blog blog)
        {
            // BURASI
            // BURASI
            // BURASI
            // BURASI
            if (ModelState.IsValid)
            {   

                if(Request.Form.Files.Count > 0){
                    var image = Request.Form.Files[0];

                    if (image != null && image.Length > 0)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await image.CopyToAsync(stream);
                            blog.Image = Convert.ToBase64String(stream.ToArray());
                        }
                    }

                }

                
      Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

          User user = _context.Users.SingleOrDefault(u => u.Id == userId);

    blog.UserId = user.Id ;
                
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
// var blog = _context.Blogs
//     .Include(b => b.Comments)
//     .FirstOrDefault(b => b.blogId == Id);
            // var blog = await _context.Blogs
            //     .Include(b => b.User)
            //     .FirstOrDefaultAsync(m => m.BlogId == id);

            // _context.Entry(blog).Collection(b => b.Comments).Query().Include(c => c.User).Load();




            var blog = await _context.Blogs
                .Include(b => b.Comments).Include(b => b.Comments).ThenInclude(c => c.User)
                .FirstOrDefaultAsync(m => m.BlogId == id);
_context.Entry(blog)
    .Reference(b => b.User)
    .Load();

            if (blog == null)
            {
             return RedirectToAction("NotFound", "Error");
            }

            return View(blog);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
               return RedirectToAction("NotFound", "Error");
            }

            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

          User user = _context.Users.SingleOrDefault(u => u.Id == userId);
            var blog = await _context.Blogs.FindAsync(id);

            if(blog.UserId != user.Id){
                return RedirectToAction("AccessDenied", "Home");
            }



            if (blog == null)
            {
               return RedirectToAction("NotFound", "Error");
            }
            return View(blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
     

        [Authorize]
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

                    if(Request.Form.Files.Count > 0){

                        var image = Request.Form.Files[0];

                        if (image != null && image.Length > 0)
                        {
                            using (var stream = new MemoryStream())
                            {
                                await image.CopyToAsync(stream);
                                blog.Image = Convert.ToBase64String(stream.ToArray());
                            }

                        }
                    }
                
 Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

          User user = _context.Users.SingleOrDefault(u => u.Id == userId);

    blog.UserId = user.Id ;

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

        [Authorize]
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
        [Authorize]
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

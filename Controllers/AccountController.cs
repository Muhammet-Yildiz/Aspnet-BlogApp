using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using NETCore.Encrypt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
namespace BlogApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly MainContext _context;

        //  pass hashlerken sona ekstra strıng eklıyoruz bunu sıtelerde dırek sıfreyı cozemesın dıye appsettıngsda tanımladıgımız stryı alıcaz ve passi hashlicez
        private readonly IConfiguration _configuration;

        public AccountController(MainContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginUser)
        {
            if (ModelState.IsValid)
            {
                string md5Salt = _configuration.GetValue<string>("AppSettings:MD5SaltStr");

                string saltedPassword = loginUser.Password + md5Salt;

                string hashedPassword = EncryptProvider.Md5(saltedPassword);

                User user = _context.Users.SingleOrDefault(u => u.Email.ToLower() == loginUser.Email.ToLower() && u.Password == hashedPassword);


                if (user != null)
                {

                    if (user.Locked)
                    {
                        ModelState.AddModelError("", "Your account is locked");
                        return View(loginUser);
                    }
                    // login işlemi başarılı
                    // session oluştur

                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                    claims.Add(new Claim(ClaimTypes.Name, user.FullName ?? string.Empty));
                    claims.Add(new Claim(ClaimTypes.Role, user.Role));
                    claims.Add(new Claim("Email", user.Email));

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                    return RedirectToAction("Index", "Blog");

                }
                else
                {
                    ModelState.AddModelError("", "Email or password is wrong");
                }

            }
            return View(loginUser);

        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerUser)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u => u.Email.ToLower() == registerUser.Email.ToLower()))
                {
                    ModelState.AddModelError("Email", "Email already exists");
                    return View(registerUser);
                }

                string md5Salt = _configuration.GetValue<string>("AppSettings:MD5SaltStr");

                string saltedPassword = registerUser.Password + md5Salt;

                string hashedPassword = EncryptProvider.Md5(saltedPassword);


                User user = new User()
                {
                    Email = registerUser.Email,
                    Password = hashedPassword
                };

                _context.Add(user);
                _context.SaveChanges();
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "Account");

            }
            return View(registerUser);
        }

        [Authorize]
        public IActionResult Profile()
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            User user = _context.Users.SingleOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            return View(user);

        }

        [Authorize]
        [HttpGet]
        public IActionResult EditProfile()
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            User user = _context.Users.SingleOrDefault(u => u.Id == userId);

            return View(user);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditProfile(User user)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            User userDb = _context.Users.SingleOrDefault(u => u.Id == userId);

            if (userDb == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            if (userDb.Email != user.Email)
            {
                if (_context.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email already exists");
                    return View(user);
                }
            }

            if (Request.Form.Files.Count > 0)
            {
                var image = Request.Form.Files[0];

                if (image != null && image.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await image.CopyToAsync(stream);
                        userDb.ProfileImage = Convert.ToBase64String(stream.ToArray());
                    }
                }

            }

            userDb.FullName = user.FullName;
            userDb.Email = user.Email;

            _context.SaveChanges();

            return RedirectToAction("Profile", "Account");

        }



        public IActionResult Logout()
        {

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }


      [Authorize]
      [HttpGet]
        public async Task<IActionResult> DeleteAccount(Guid id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            User user = _context.Users.SingleOrDefault(u => u.Id == id);

            if (user == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            var relatedBlogs = _context.Blogs.Where(b => b.UserId == user.Id).ToList();

            var relatedComments = _context.Comments.Where(c => c.UserId == user.Id).ToList();

            foreach (var blog in relatedBlogs)
            {
                _context.Blogs.Remove(blog);
            }

            foreach (var comment in relatedComments)
            {
                _context.Comments.Remove(comment);
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

    }


}
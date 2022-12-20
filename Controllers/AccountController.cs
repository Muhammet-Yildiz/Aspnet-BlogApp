using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NETCore.Encrypt;
using System.Security.Claims ;
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

                  if(user.Locked) {
                    ModelState.AddModelError("", "Your account is locked");
                    return View(loginUser);
                  }
                    // login işlemi başarılı
                    // session oluştur
 
List<Claim> claims =new List<Claim>();
claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
claims.Add(new Claim(ClaimTypes.Name, user.FullName ?? string.Empty ));
claims.Add(new Claim(ClaimTypes.Role, user.Role ));
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

                // registerUser.Password + _configuration["AppSettings:PasswordSalt"];

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
       
          Console.WriteLine("Profile Get", user, user.Email);

          if(user == null) {
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
        public IActionResult EditProfile(User user)
        {
          Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

          User userDb = _context.Users.SingleOrDefault(u => u.Id == userId);

          if(userDb == null) {
            return RedirectToAction("NotFound", "Error");
          }

          userDb.FullName = user.FullName;
          userDb.Email = user.Email;

          _context.SaveChanges();

          return RedirectToAction("Profile", "Account");
          
          
          // Console.WriteLine("EditProfile Post", user.FullName, user.Email);
          // Console.WriteLine("EMAİL", user.Email);
          
          

        }







      public IActionResult Logout()
        {

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }



    }

}
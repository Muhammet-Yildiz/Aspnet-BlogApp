using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BlogApp.Controllers
{
    [Authorize(Roles="admin")]
    public class AdminController : Controller
    {

        public IActionResult Index()
        {
            return View();
           
        }

    }
}
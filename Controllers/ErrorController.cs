using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
namespace BlogApp.Controllers
{
    public class ErrorController : Controller
    {
        
        public IActionResult NotFound( )
        {
           var model = new ErrorViewModel
        {
            RequestId =  HttpContext.TraceIdentifier,
            Message = "Sorry, the page you requested could not be found."
        };
            return View(model);
        }

    }
}
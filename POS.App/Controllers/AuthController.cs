using Microsoft.AspNetCore.Mvc;

namespace POS.App.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

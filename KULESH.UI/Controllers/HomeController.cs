using Microsoft.AspNetCore.Mvc;

namespace YourLastName.UI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
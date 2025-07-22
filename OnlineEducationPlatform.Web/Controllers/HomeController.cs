using Microsoft.AspNetCore.Mvc;

namespace OnlineEducationPlatform.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

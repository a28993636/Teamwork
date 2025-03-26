using Microsoft.AspNetCore.Mvc;

namespace Teamwork.Controllers
{
    public class testController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

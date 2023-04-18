using Microsoft.AspNetCore.Mvc;

namespace CIProjectweb.Controllers
{
    public class test : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

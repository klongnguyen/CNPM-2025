using Microsoft.AspNetCore.Mvc;

namespace FashionStore.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

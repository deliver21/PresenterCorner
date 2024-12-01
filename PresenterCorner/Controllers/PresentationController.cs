using Microsoft.AspNetCore.Mvc;

namespace PresenterCorner.Controllers
{
    public class PresentationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

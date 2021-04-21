using Microsoft.AspNetCore.Mvc;

namespace ServiceHelp.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController()
        {
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}
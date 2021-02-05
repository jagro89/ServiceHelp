using Microsoft.AspNetCore.Mvc;

namespace ServiceHelp.Controllers
{
    public class InformationController : BaseController
    {
        // GET: Contact
        public ActionResult Contact()
        {
            return View();
        }

        // GET: KnowledgeBase
        public ActionResult KnowledgeBase()
        {
            return View();
        }
    }
}
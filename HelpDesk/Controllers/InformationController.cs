using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelpDesk.Controllers
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
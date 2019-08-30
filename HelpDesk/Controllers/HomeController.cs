using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelpDesk.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<string> lista = new List<string>();
            lista.Add("sadsada");
            lista.Add("dddddddd");
            lista.Add("ccccccccc");
            lista.Add("yyyyyyyy");


            ViewBag.Dupa = lista;
            return View();
        }
    }
}
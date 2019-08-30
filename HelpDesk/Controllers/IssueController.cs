using HelpDesk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelpDesk.Controllers
{
    public class IssueController : Controller
    {
        // GET: Issues
        public ActionResult Index()
        { tu skonczone na utworzeniu listy zgloszen, nie dociagaja sie same powiazane elementy bo nie sa virtual
            DbContext db = new DbContext();
            List<Issue> list = db.Issues.Include("Status").Include("Prioritet").Include("Category").Where(i => i.Status.CodeName != "close").OrderByDescending(i => i.Date).ToList();
            return View(list);
        }

        // GET: Issues/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Issues/Create
        public ActionResult Create()
        {
            DbContext db = new DbContext();

            return View();
        }

        // POST: Issues/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Issues/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Issues/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Issues/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Issues/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

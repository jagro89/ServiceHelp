using HelpDesk.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HelpDesk.Controllers
{
    public class IssueController : BaseController
    {
        // GET: Issues
        public ActionResult Index()
        { //tu skonczone na utworzeniu listy zgloszen, nie dociagaja sie same powiazane elementy bo nie sa virtual
            List<Issue> list = DB.Issues.Where(i => i.Status.CodeName != "close").OrderByDescending(i => i.Date).ToList();
            //List<Issue> list = db.Issues.Include("Status").Include("Prioritet").Include("Category").Where(i => i.Status.CodeName != "close").OrderByDescending(i => i.Date).ToList();
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
            Issue issue = DB.Issues.Find(id);
            bool is_admin = User.IsInRole(Consts.DEF_ADMIN_ROLE);
            bool is_srvice = User.IsInRole(Consts.DEF_SERVICE_MAN_ROLE);

            if (issue.User.Id != User.Identity.GetUserId() && !is_admin && !is_srvice)
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            return View(issue); Tu skonczylismy zrobi widok edycji
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

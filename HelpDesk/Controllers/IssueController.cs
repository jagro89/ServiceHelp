using HelpDesk.Models;
using HelpDesk.ViewModels;
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
        {
            List<Issue> list = DB.Issues
                .Include("Status")
                .Include("Prioritet")
                .Include("Category")
                .Include("User")
                .Where(i => i.Status.CodeName != "close")
                .OrderByDescending(i => i.Date)
                .ToList();
            return View(list);
        }

        // GET: Issues/Details/5
        public ActionResult Details(int id)
        {
            IssueViewModelDetails issue = new IssueViewModelDetails();
            var issueDB = DB.Issues.Include("User").Include("Category").Include("Prioritet").Include("Status").First(a => a.Id == id);

            issue.Id = issueDB.Id;
            issue.Date = issueDB.Date;
            issue.Prioritet = issueDB.Prioritet.Name;
            issue.Title = issueDB.Title;
            issue.Description = issueDB.Description;
            issue.Status = issueDB.Status.Name;
            issue.CategoryNames = issueDB.Category.Select(a => a.Name).ToArray();
            issue.User = issueDB.User.UserName;
            issue.ServiceUser = issueDB.ServiceUser == null ? "" : issueDB.ServiceUser.UserName;

            return View(issue);
        }

        // GET: Issues/Edit/5
        public ActionResult AddEdit(int? id)
        {
            var statuses = DB.Statuses.Select(a => new SelectListItem() { Text = a.Name, Value = a.Id.ToString(), Selected = a.CodeName == "new" }).ToList();
            var priorytets = DB.Prioritets.Select(a => new SelectListItem() { Text = a.Name, Value = a.Id.ToString(), Selected = a.CodeName == "mid" }).ToList();
            var categories = DB.CategoryIssues.Select(a => new SelectListItem() { Text = a.Name, Value = a.Id.ToString() }).ToList();

            IssueViewModel issue = new IssueViewModel();
            if (id == null || id == 0)
            {
                issue.Date = DateTime.Now;
                issue.IdPrioritet = DB.Prioritets.Where(a => a.CodeName == "mid").First().Id;
                issue.IdStatus = DB.Statuses.Where(a => a.CodeName == "new").First().Id;
            }
            else
            {
                var issueDB = DB.Issues.Include("User").Include("Category").Include("Prioritet").Include("Status").First(a => a.Id == id);

                issue.Id = issueDB.Id;
                issue.Date = issueDB.Date;
                issue.IdPrioritet = issueDB.Prioritet.Id;
                issue.Title = issueDB.Title;
                issue.Description = issueDB.Description;
                issue.IdStatus = issueDB.Status.Id;
                issue.CategoryIds = issueDB.Category.Select(a => a.Id).ToArray();                               
                
                statuses.ForEach(a => { if (a.Value == issueDB.Status.Id.ToString()) a.Selected = true; });
                priorytets.ForEach(a => { if (a.Value == issueDB.Prioritet.Id.ToString()) a.Selected = true; });
                categories.ForEach(a => { if (issueDB.Category.Select(b => b.Id.ToString()).Contains(a.Value)) a.Selected = true; });

                bool is_admin = User.IsInRole(Consts.DEF_ADMIN_ROLE);
                bool is_srvice = User.IsInRole(Consts.DEF_SERVICE_MAN_ROLE);
                if (issueDB.User.Id != User.Identity.GetUserId() && !is_admin && !is_srvice)
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            ViewData["Status"] = statuses;
            ViewData["Prioritet"] = priorytets;
            ViewData["Category"] = categories;

            return View(issue);
        }

        // POST: Issues/Edit/5
        [HttpPost]
        public ActionResult AddEdit(IssueViewModel issue)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(issue);

                Issue to_save;
                if (issue.Id > 0)
                {
                    to_save = DB.Issues.Include("User").First(a => a.Id == issue.Id);
                }
                else
                {
                    to_save = new Issue();
                    to_save.User = DB.Users.Find(User.Identity.GetUserId());
                    DB.Entry(to_save).State = System.Data.Entity.EntityState.Added;
                }

                to_save.Date = issue.Date;
                to_save.Prioritet = DB.Prioritets.Find(issue.IdPrioritet); 
                to_save.Title = issue.Title; 
                to_save.Description = issue.Description;
                to_save.Status = DB.Statuses.Find(issue.IdStatus);
              //  to_save.Category = DB.CategoryIssues.Where(a => issue.CategoryIds.Contains(a.Id)).ToList();

                DB.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View(issue);
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

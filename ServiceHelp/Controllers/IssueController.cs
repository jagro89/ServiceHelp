using ServiceHelp.Models;
using ServiceHelp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using ServiceHelp.Utils;
using ServiceHelp.Data;

namespace ServiceHelp.Controllers
{
    public class IssueController : BaseController
    {
        private readonly ApplicationDbContext _db;

        public IssueController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: Issues
        public ActionResult Index()
        {
            List<Issue> list = _db.Issue
                .Include(a => a.User)
                .Include(a => a.IssueCategory)
                    .ThenInclude(a => a.Category)
                .Include(a => a.Prioritet)
                .Include(a => a.Status)
                .Where(i => i.Status.CodeName != "close")
                .OrderByDescending(i => i.Date)
                .ToList();
            return View(list);
        }

        // GET: Issues/Details/5
        public ActionResult Details(int id)
        {
            IssueViewModelDetails issue = new IssueViewModelDetails();
            var issueDB = _db.Issue
                .Include(a => a.User)
                .Include(a => a.IssueCategory)
                    .ThenInclude(a => a.Category)
                .Include(a => a.Prioritet)
                .Include(a => a.Status)
                .First(a => a.IdIssue == id);

            issue.Id = issueDB.IdIssue;
            issue.Date = issueDB.Date;
            issue.Prioritet = issueDB.Prioritet.Name;
            issue.Title = issueDB.Title;
            issue.Description = issueDB.Description;
            issue.Status = issueDB.Status.Name;
            issue.CategoryNames = issueDB.IssueCategory.Select(a => a.Category.Name).ToArray();
            issue.User = issueDB.User.UserName;
            issue.ServiceUser = issueDB.ServiceUser == null ? "" : issueDB.ServiceUser.UserName;

            return View(issue);
        }

        // GET: Issues/Edit/5
        public ActionResult AddEdit(int? id)
        {
            IssueViewModel issue = new IssueViewModel();
            if (id == null || id == 0)
            {
                issue.Date = DateTime.Now;
                issue.IdPrioritet = _db.Prioritet.Where(a => a.CodeName == "mid").First().IdPrioritet;
                issue.IdStatus = _db.Status.Where(a => a.CodeName == "new").First().IdStatus;
            }
            else
            {
                var issueDB = _db.Issue
                    .Include(a => a.User)
                    .Include(a => a.IssueCategory)
                    //.ThenInclude(a => a.Category)
                    .Include(a => a.Prioritet)
                    .Include(a => a.Status)
                    .First(a => a.IdIssue == id);

                issue.Id = issueDB.IdIssue;
                issue.Date = issueDB.Date;
                issue.IdPrioritet = issueDB.Prioritet.IdPrioritet;
                issue.Title = issueDB.Title;
                issue.Description = issueDB.Description;
                issue.IdStatus = issueDB.Status.IdStatus;
                issue.CategoryIds = issueDB.IssueCategory.Select(a => a.IdCategory).ToArray();

                bool is_admin = User.IsInRole(Consts.DEF_ADMIN_ROLE);
                bool is_srvice = User.IsInRole(Consts.DEF_SERVICE_MAN_ROLE);
                if (issueDB.User.Id != GetCurrentUserID() && !is_admin && !is_srvice)
                    return Forbid();
            }

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
                    to_save = _db.Issue
                        .Include(a => a.User)
                        .Include(a => a.IssueCategory)
                            .ThenInclude(a => a.Category)
                        .First(a => a.IdIssue == issue.Id);
                }
                else
                {
                    to_save = new Issue();
                    to_save.User = _db.Users.Find(GetCurrentUserID());
                    _db.Entry(to_save).State = EntityState.Added;
                }

                to_save.Date = issue.Date;
                to_save.Prioritet = _db.Prioritet.Find(issue.IdPrioritet);
                to_save.Title = issue.Title;
                to_save.Description = issue.Description;
                to_save.Status = _db.Status.Find(issue.IdStatus);

                var to_delete = to_save.IssueCategory.Where(a => !issue.CategoryIds.Contains(a.IdCategory)).ToList();
                _db.RemoveRange(to_delete);

                var to_add = issue.CategoryIds.Where(a => !to_save.IssueCategory.Select(a => a.IdCategory).Contains(a)).ToList();
                var cates = _db.Category.Where(a => to_add.Contains(a.IdCategory)).ToList();
                cates.ForEach(a => to_save.IssueCategory.Add(new IssueCategory() { Issue = to_save, Category = a }));

                _db.SaveChanges();

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
            IssueViewModel issue = new IssueViewModel();
            var issueDB = _db.Issue
              .Include(a => a.User)
              .Include(a => a.IssueCategory)
              .Include(a => a.Prioritet)
              .Include(a => a.Status)
              .First(a => a.IdIssue == id);

            issue.Id = issueDB.IdIssue;
            issue.Date = issueDB.Date;
            issue.IdPrioritet = issueDB.Prioritet.IdPrioritet;
            issue.Title = issueDB.Title;
            issue.Description = issueDB.Description;
            issue.IdStatus = issueDB.Status.IdStatus;
            issue.CategoryIds = issueDB.IssueCategory.Select(a => a.IdCategory).ToArray();

            return View(issue);
        }

        // POST: Issues/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, string aaaa)
        {
            try
            {
                var obj = _db.Issue.Include(a => a.IssueCategory).First(a => a.IdIssue == id);
                _db.RemoveRange(obj.IssueCategory.ToList());
                _db.Remove(obj);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}

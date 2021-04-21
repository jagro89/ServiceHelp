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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ServiceHelp.Services;

namespace ServiceHelp.Controllers
{
    [Authorize]
    public class IssueController : BaseController
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IssueMailReaderService _issueMailReaderService;

        public IssueController(ApplicationDbContext db, UserManager<IdentityUser> userManager, IssueMailReaderService issueMailReaderService)
        {
            _db = db;
            _userManager = userManager;
            _issueMailReaderService = issueMailReaderService;
        }

        // GET: Issues
        public ActionResult Index(int? status, int? prioritet, int[] category)
        {
            ViewBag.SelectStatus = status;
            ViewBag.SelectPrioritet = prioritet;
            ViewBag.SelectCategory = category;

            var criteria = _db.Issue
                .Include(a => a.User)
                .Include(a => a.IssueCategory)
                    .ThenInclude(a => a.Category)
                .Include(a => a.Prioritet)
                .Include(a => a.Status)
                .Include(a => a.ServiceUser)
                .Where(i => ((status == null && i.Status.CodeName != "close") || (status != null && i.IdStatus == status)) &&
                    (prioritet != null ? i.IdPrioritet == prioritet : 1 == 1) &&
                    (category.Count() == 0 ? 1 == 1 : i.IssueCategory.Select(a => a.IdCategory).Where(b => category.Contains(b)).Any()));

            var user = _userManager.GetUserAsync(GetCurrentUser()).Result;
            if (_userManager.IsInRoleAsync(user, Consts.DEF_USER_ROLE).Result)
                criteria = criteria.Where(a => a.IdUser == user.Id);

            List<Issue> list = criteria.OrderByDescending(i => i.IdIssue).ToList();
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
                if (issueDB.ServiceUser != null)
                    issue.IdServiceUser = issueDB.ServiceUser.Id;

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

                var user = _userManager.GetUserAsync(GetCurrentUser()).Result;
                if (_userManager.IsInRoleAsync(user, Consts.DEF_ADMIN_ROLE).Result || _userManager.IsInRoleAsync(user, Consts.DEF_SERVICE_MAN_ROLE).Result)
                    to_save.IdServiceUser = issue.IdServiceUser;

                if (_userManager.IsInRoleAsync(user, Consts.DEF_ADMIN_ROLE).Result || _userManager.IsInRoleAsync(user, Consts.DEF_SERVICE_MAN_ROLE).Result)
                    to_save.Status = _db.Status.Find(issue.IdStatus);
                else if (_userManager.IsInRoleAsync(user, Consts.DEF_USER_ROLE).Result && issue.Id == 0)
                    to_save.Status = _db.Status.First(a => a.CodeName == "new");

                var to_delete = to_save.IssueCategory.Where(a => !issue.CategoryIds.Contains(a.IdCategory)).ToList();
                _db.RemoveRange(to_delete);

                if (issue.CategoryIds != null)
                {
                    var to_add = issue.CategoryIds.Where(a => !to_save.IssueCategory.Select(a => a.IdCategory).Contains(a)).ToList();
                    var cates = _db.Category.Where(a => to_add.Contains(a.IdCategory)).ToList();
                    cates.ForEach(a => to_save.IssueCategory.Add(new IssueCategory() { Issue = to_save, Category = a }));
                }

                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Błąd zapisu formularza");
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
        public ActionResult DeleteConfirm(int id)
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

        public ActionResult GetNewIssue()
        {
            _issueMailReaderService.Do();
            return RedirectToAction("Index", "Issue");
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceHelp.Data;
using ServiceHelp.ViewModels;
using System.Linq;

namespace ServiceHelp.Controllers
{
    [Authorize(Roles = "Serwisant,Administrator")]
    public class ReportController : BaseController
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public ReportController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public ActionResult Index()
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;

            var category = _db.Category
                .Include(a => a.IssueCategory)
                .ThenInclude(a => a.Issue)
                .Where(a => a.IssueCategory.Any(b => b.Issue.IdServiceUser == user.Id))
                .ToList();

            var list = _db.Issue
                .Include(a => a.User)
                .Include(a => a.IssueCategory)
                    .ThenInclude(a => a.Category)
                .Include(a => a.Prioritet)
                .Include(a => a.Status)
                .Where(i => i.IdServiceUser == user.Id).OrderByDescending(i => i.Date).ToList();

            ReportViewModels result = new ReportViewModels()
            {
                Issues = list.Select(a => new ReportIssueViewModels() { Date = a.Date, Name = a.Title}).ToList(),
                ReportItems = category.Select(a => new ReportItemViewModels { Category = a.Name, CountIssue = a.IssueCategory.Count() } ).ToList()
            };

            return View(result);
        }
    }
}

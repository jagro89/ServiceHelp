using ServiceHelp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ServiceHelp.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IssueMailReaderService _issueMailReaderService;
        public HomeController(IssueMailReaderService issueMailReaderService)
        {
            _issueMailReaderService = issueMailReaderService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Get()
        {
            _issueMailReaderService.Do();
            return RedirectToAction("Index", "Issue");
        }
    }
}
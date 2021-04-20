using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceHelp.Data;
using ServiceHelp.Models;
using System.Linq;

namespace ServiceHelp.Controllers
{
    public class InformationController : BaseController
    {
        private readonly ApplicationDbContext _db;

        public InformationController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: Contact
        public ActionResult Contact()
        {
            return View();
        }

        // GET: KnowledgeBase
        [Authorize]
        public ActionResult KnowledgeBase()
        {
            return View(_db.KnowledgeBase.ToList());
        }

        [Authorize(Roles = "Serwisant,Administrator")]
        public ActionResult AddEdit(int? id = null)
        {
            KnowledgeBase result;
            if (id == null || id == 0)
                result = new KnowledgeBase();
            else
                result = _db.KnowledgeBase.First(a => a.IdKnowledgeBase == id);
            return View(result);
        }

        [Authorize(Roles = "Serwisant,Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEdit(KnowledgeBase model)
        {
            if (model.IdKnowledgeBase == 0)
                _db.Add(model);
            else
                _db.Update(model);
            _db.SaveChanges();
            return RedirectToAction("KnowledgeBase");
        }
    }
}
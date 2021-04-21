using ServiceHelp.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using ServiceHelp.Utils;

namespace ServiceHelp.Dictionary
{
    public class IssueDictionary
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        public IssueDictionary(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public List<SelectListItem> GetStatusDictionary(int? idSelectItem = null)
        {
            var list = _db.Status.Select(a => new SelectListItem() { Text = a.Name, Value = a.IdStatus.ToString(), Selected = (idSelectItem != null && idSelectItem == 0) ? a.CodeName == "new" : false }).ToList();

            if (idSelectItem != null)
                list.ForEach(a => { if (a.Value == idSelectItem.ToString()) a.Selected = true; });

            return list;
        }

        public List<SelectListItem> GetPrioritetDictionary(int? idSelectItem = null)
        {
            var list = _db.Prioritet.Select(a => new SelectListItem() { Text = a.Name, Value = a.IdPrioritet.ToString(), Selected = (idSelectItem != null && idSelectItem == 0) ? a.CodeName == "mid" : false }).ToList();

            if (idSelectItem != null)
                list.ForEach(a => { if (a.Value == idSelectItem.ToString()) a.Selected = true; });

            return list;
        }

        public List<SelectListItem> GetCategoryIssueDictionary(int[] idSelectItems = null)
        {
            var list = _db.Category.Select(a => new SelectListItem() { Text = a.Name, Value = a.IdCategory.ToString() }).ToList();

            if (idSelectItems != null)
                list.ForEach(a =>
                {
                    if (idSelectItems.Select(a => a.ToString()).Contains(a.Value))
                        a.Selected = true;
                });

            return list;
        }

        public List<SelectListItem> GetServiceMans(string idSelectItem = null)
        {
            var serviceUsers = _userManager.GetUsersInRoleAsync(Consts.DEF_SERVICE_MAN_ROLE).Result;
            var adminUsers = _userManager.GetUsersInRoleAsync(Consts.DEF_ADMIN_ROLE).Result;

            List<IdentityUser> all = new List<IdentityUser>();
            all.AddRange(serviceUsers);
            all.AddRange(adminUsers);

            var list = all.Select(a => new SelectListItem() { Text = a.UserName, Value = a.Id }).ToList();

            if (!string.IsNullOrEmpty(idSelectItem))
                list.ForEach(a => { if (a.Value == idSelectItem) a.Selected = true; });

            return list;
        }
    }
}

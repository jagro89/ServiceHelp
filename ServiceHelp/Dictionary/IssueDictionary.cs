using ServiceHelp.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceHelp.Dictionary
{
    public class IssueDictionary
    {
        private readonly ApplicationDbContext _db;
        public IssueDictionary(ApplicationDbContext db)
        {
            _db = db;
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
    }
}

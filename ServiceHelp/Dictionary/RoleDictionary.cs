using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceHelp.Dictionary
{
    public class RoleDictionary
    {
        private readonly RoleManager<IdentityRole> _roleMenager;
        public RoleDictionary(RoleManager<IdentityRole> roleMenager)
        {
            _roleMenager = roleMenager;
        }

        public List<SelectListItem> GetRolesDictionary(string[] selectItems = null)
        {
            var list = _roleMenager.Roles.Select(a => new SelectListItem() { Text = a.Name, Value = a.Name }).ToList();

            if (selectItems != null)           
                list.ForEach(a => { if (selectItems.Contains(a.Value)) a.Selected = true; });

            return list;
        }

    }
}

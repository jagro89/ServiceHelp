using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceHelp.Data;
using ServiceHelp.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace ServiceHelp.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ManagementController : BaseController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ManagementController(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public ActionResult Index()
        {
            var users = _userManager.Users.ToList();
            var usreRoles = users.Select(a => new { user = a, roles = _userManager.GetRolesAsync(a).Result.ToList() });

            ManagementViewModels result = new ManagementViewModels();
            result.AddRange(usreRoles.Select(a =>
                new UserItemViewModels()
                {
                    Id = a.user.Id,
                    Email = a.user.Email,
                    Username = a.user.UserName,
                    Roles = _userManager.GetRolesAsync(a.user).Result.ToArray()
                }).ToList());
            return View(result);
        }

        public ActionResult AddEdit(string id = null)
        {
            UserViewModels result = new UserViewModels();
            if (!string.IsNullOrEmpty(id))
            {
                var user = _userManager.FindByIdAsync(id).Result;
                result.Id = user.Id;
                result.Email = user.Email;
                result.Username = user.UserName;
                result.Phone = user.PhoneNumber;
                result.Roles = _userManager.GetRolesAsync(user).Result.ToArray();
            }
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEdit(UserViewModels model)
        {
            if (!ModelState.IsValid)
                return View(model);

            IdentityUser user;
            if (string.IsNullOrEmpty(model.Id))
                user = new IdentityUser() { UserName = model.Username, EmailConfirmed = true };
            else
                user = _userManager.FindByIdAsync(model.Id).Result;

            user.Email = model.Email;
            user.PhoneNumber = model.Phone;


            if (string.IsNullOrEmpty(model.Id))
                _userManager.CreateAsync(user).Wait();
            else
                _userManager.UpdateAsync(user).Wait();

            if (string.IsNullOrEmpty(model.Id)) // zapis hasla gdy nowy user
                _userManager.AddPasswordAsync(user, model.Password).Wait();

            List<string> roles = new List<string>();
            roles.AddRange(_userManager.GetRolesAsync(user).Result);

            foreach (var role in model.Roles)
                if (!roles.Contains(role))
                    _userManager.AddToRoleAsync(user, role).Wait();

            foreach (var role in roles)
                if (!model.Roles.Contains(role))
                    _userManager.RemoveFromRoleAsync(user, role).Wait();

            return RedirectToAction("Index");
        }
    }
}

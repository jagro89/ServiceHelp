using ServiceHelp.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using ServiceHelp.Utils;

namespace ServiceHelp.Data
{
    public static class DataSeed
    {
        public static void AddSeed(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext db)
        {

            if (!userManager.Users.Any())
            {
                IdentityUser user = new IdentityUser() { Email = "admin@servicehelp.pl", UserName = "admin@servicehelp.pl", EmailConfirmed = true, PhoneNumber = "1" };
                userManager.CreateAsync(user, "1234Qwer.?").Wait();
            }

            if (!roleManager.Roles.Any())
            {
                var role = new IdentityRole { Name = Consts.DEF_ADMIN_ROLE };
                var service = new IdentityRole { Name = Consts.DEF_SERVICE_MAN_ROLE };
                var user = new IdentityRole { Name = Consts.DEF_USER_ROLE };
                roleManager.CreateAsync(role).Wait();
                roleManager.CreateAsync(service).Wait();
                roleManager.CreateAsync(user).Wait();

                var usr = userManager.FindByEmailAsync("admin@servicehelp.pl").Result;
                if (!userManager.IsInRoleAsync(usr, Consts.DEF_ADMIN_ROLE).Result)
                    userManager.AddToRoleAsync(usr, Consts.DEF_ADMIN_ROLE).Wait();
            }

            var testuser = userManager.FindByEmailAsync("test@test.pl").Result;
            if (testuser == null)
            {
                testuser = new IdentityUser() { Email = "test@test.pl", UserName = "test@test.pl", EmailConfirmed = true, PhoneNumber = "2" };
                userManager.CreateAsync(testuser, "1234Qwer.?").Wait();
                userManager.AddToRoleAsync(testuser, Consts.DEF_USER_ROLE).Wait();
            }

            if (!db.Prioritet.Any())
            {
                List<Prioritet> prioritest = new List<Prioritet>()
                {
                    new Prioritet {Name = "Niski", CodeName = "low"},
                    new Prioritet {Name = "Średni", CodeName = "mid"},
                    new Prioritet {Name = "Wysoki", CodeName = "high"}
                };

                db.Prioritet.AddRange(prioritest);
                db.SaveChangesAsync();
            }

            if (!db.Status.Any())
            {
                List<Status> statuses = new List<Status>()
                {
                    new Status {Name = "Nowe", CodeName = "new"},
                    new Status {Name = "Otwarte", CodeName = "open"},
                    new Status {Name = "Zamknięte", CodeName = "close"},
                    new Status {Name = "Przeterminowane", CodeName = "overdue"}
                };

                db.Status.AddRange(statuses);
                db.SaveChangesAsync();
            }

            if (!db.Category.Any())
            {
                List<Category> categories = new List<Category>()
                {
                    new Category {Name = "Awarie"},
                    new Category {Name = "Zakupy"},
                    new Category {Name = "Sieć"}
                };

                db.Category.AddRange(categories);
                db.SaveChangesAsync();
            }
        }
    }
}
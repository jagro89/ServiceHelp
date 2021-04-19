using ServiceHelp.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

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
                var role = new IdentityRole { Name = "Administrator" };
                var service = new IdentityRole { Name = "Serwisant" };
                var user = new IdentityRole { Name = "Użytkownik" };
                roleManager.CreateAsync(role).Wait();
                roleManager.CreateAsync(service).Wait();
                roleManager.CreateAsync(user).Wait();

                var usr = userManager.FindByEmailAsync("admin@servicehelp.pl").Result;
                if (!userManager.IsInRoleAsync(usr, "Administrator").Result)
                    userManager.AddToRoleAsync(usr, "Administrator").Wait();
            }

            var testuser = userManager.FindByEmailAsync("test@test.pl").Result;
            if (testuser == null)
            {
                testuser = new IdentityUser() { Email = "test@test.pl", UserName = "test@test.pl", EmailConfirmed = true, PhoneNumber = "2" };
                userManager.CreateAsync(testuser, "1234Qwer.?").Wait();
                userManager.AddToRoleAsync(testuser, "Użytkownik").Wait();
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
using ServiceHelp.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace ServiceHelp.Data
{
    public static class DataSeed
    {
        public static void AddSeed(UserManager<IdentityUser> userManager, ApplicationDbContext db)
        {
            if (!userManager.Users.Any())
            {
                List<Prioritet> prioritest = new List<Prioritet>()
                {
                    new Prioritet {Name = "Niski", CodeName = "low"},
                    new Prioritet {Name = "Średni", CodeName = "mid"},
                    new Prioritet {Name = "Wysoki", CodeName = "high"}
                };

                List<Status> statuses = new List<Status>()
                {
                    new Status {Name = "Nowe", CodeName = "new"},
                    new Status {Name = "Otwarte", CodeName = "open"},
                    new Status {Name = "Zamknięte", CodeName = "close"},
                    new Status {Name = "Przeterminowane", CodeName = "overdue"}
                };

                List<Category> categories = new List<Category>()
                {
                    new Category {Name = "Awarie"},
                    new Category {Name = "Zakupy"},
                    new Category {Name = "Sieć"}
                };

                db.Prioritet.AddRange(prioritest);
                db.Status.AddRange(statuses);
                db.Category.AddRange(categories);
                db.SaveChangesAsync();

                IdentityUser user = new IdentityUser() { Email = "admin@servicehelp.pl", UserName = "admin@servicehelp.pl", EmailConfirmed = true, PhoneNumber = "1" };

                userManager.CreateAsync(user, "1234Qwer.?").Wait();
            }
        }
    }
}
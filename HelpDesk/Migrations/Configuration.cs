namespace HelpDesk.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using HelpDesk.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Collections.Generic;
    using HelpDesk.Controllers;

    internal sealed class Configuration : DbMigrationsConfiguration<HelpDesk.Models.DbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(HelpDesk.Models.DbContext context)
        {
            string admin_role = Consts.DEF_ADMIN_ROLE;
            string service_role = Consts.DEF_SERVICE_MAN_ROLE;
            RoleManager<IdentityRole> RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            IdentityRole roleadmin = RoleManager.FindByName(admin_role);
            IdentityRole roleservice = RoleManager.FindByName(service_role);
            if (roleadmin == null)
            {
                roleadmin = new IdentityRole { Name = admin_role };
                RoleManager.Create(roleadmin);
            }
            if (roleservice == null)
            {
                roleservice = new IdentityRole { Name = service_role };
                RoleManager.Create(roleservice);
            }

            const string name = "admin@servicehelp.pl";
            UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = UserManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser { UserName = name, Email = name };
                UserManager.Create(user, "1234Qwer.?");
                UserManager.AddToRole(user.Id, admin_role);
            }

            context.Prioritets.AddOrUpdate(
              p => p.Name,
              new Prioritet { Name = "Niski", CodeName = "low" },
              new Prioritet { Name = "Œredni", CodeName = "mid" },
              new Prioritet { Name = "Wysoki", CodeName = "high" }
            );

            context.Statuses.AddOrUpdate(
              p => p.Name,
              new Status { Name = "Przyjête", CodeName = "coming" },
              new Status { Name = "Otwarte", CodeName = "open" },
              new Status { Name = "Zamkniête", CodeName = "close" },
              new Status { Name = "Przeterminowane", CodeName = "overdue" }
            );
        }
    }
}

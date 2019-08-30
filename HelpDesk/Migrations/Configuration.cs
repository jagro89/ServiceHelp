namespace HelpDesk.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using HelpDesk.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<HelpDesk.Models.DbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(HelpDesk.Models.DbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
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

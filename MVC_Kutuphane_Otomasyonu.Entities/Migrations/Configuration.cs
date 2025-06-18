namespace MVC_Kutuphane_Otomasyonu.Entities.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MVC_Kutuphane_Otomasyonu.Entities.Model.Context.KutuphaneContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;   
        }

        protected override void Seed(MVC_Kutuphane_Otomasyonu.Entities.Model.Context.KutuphaneContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}

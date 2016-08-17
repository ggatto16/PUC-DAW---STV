namespace STV.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using STV.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<STV.DAL.STVDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(STV.DAL.STVDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.Role.AddOrUpdate(x => x.Idrole,
                new Role() { Idrole = 1, Nome = "Admin" },
                new Role() { Idrole = 2, Nome = "Default" });

            context.Medalha.AddOrUpdate(x => x.Idmedalha,
                new Medalha() { Idmedalha = 1, Descricao = "Sortudo" },
                new Medalha() { Idmedalha = 2, Descricao = "Nerd" },
                new Medalha() { Idmedalha = 3, Descricao = "Gênio" },
                new Medalha() { Idmedalha = 5, Descricao = "Interessado" },
                new Medalha() { Idmedalha = 6, Descricao = "Estudioso" },
                new Medalha() { Idmedalha = 7, Descricao = "Bronze" },
                new Medalha() { Idmedalha = 8, Descricao = "Prata" },
                new Medalha() { Idmedalha = 9, Descricao = "Ouro" }
        ); }

    }
}

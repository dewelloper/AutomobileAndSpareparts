
namespace Mvc4WebRole.Migrations
{
    using Dal;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Web.Security;
    using WebMatrix.WebData;

    internal sealed class Configuration : DbMigrationsConfiguration<Entities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Entities context)
        {
            if (!WebMatrix.WebData.WebSecurity.Initialized)
            {
                WebSecurity.InitializeDatabaseConnection(
                   "DefaultConnection",
                   "UserProfile",
                   "UserId",
                   "UserName", autoCreateTables: true);
            }
            if (!Roles.RoleExists("Administrator"))
                Roles.CreateRole("Administrator");
            if (!Roles.RoleExists("User"))
                Roles.CreateRole("User");

            if (!WebSecurity.UserExists("Admin"))
                WebSecurity.CreateUserAndAccount(
                    "Admin",
                    "pass",
                    new { FirstName = "testname", LastName = "testsurname", Email = "email@email.com", Balance = 0, Enabled = true });

            List<string> list = new List<string>(Roles.GetRolesForUser("Admin"));

            if (!list.Contains("Administrator"))
                Roles.AddUsersToRoles(new[] { "Admin" }, new[] { "Administrator" });
        }



    }


}

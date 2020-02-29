using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MVCWebUI.Entity;

namespace MVCWebUI.Identity
{
    public class IdentityInitializer : CreateDatabaseIfNotExists<IdentityDataContext>
    {
        protected override void Seed(IdentityDataContext context)
        {
            if (!context.Roles.Any(x=>x.Name == "admin"))
            {
                var store = new RoleStore<ApplicationRole>(context);
                var manager = new RoleManager<ApplicationRole>(store);

                var role = new ApplicationRole()
                {
                    Name = "admin",
                    Description = "yönetici rolü"
                };

                manager.Create(role);
            }

            if (!context.Roles.Any(x => x.Name == "user"))
            {
                var store = new RoleStore<ApplicationRole>(context);
                var manager = new RoleManager<ApplicationRole>(store);

                var role = new ApplicationRole()
                {
                    Name = "user",
                    Description = "user rolü"
                };

                manager.Create(role);
            }

            if (!context.Roles.Any(x => x.Name == "emregulten"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);

                var user = new ApplicationUser()
                {
                    Name = "emre",
                    Surname = "Gulten",
                    UserName = "emregulten",
                    Email = "emregulten035@gmail.com"
                };

                manager.Create(user,"1234567");
                manager.AddToRole(user.Id, "admin");
                manager.AddToRole(user.Id, "user");
            }

            if (!context.Roles.Any(x => x.Name == "aligulten"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);

                var user = new ApplicationUser()
                {
                    Name = "ali",
                    Surname = "Gulten",
                    UserName = "emregulten",
                    Email = "aligulten035@gmail.com"
                };

                manager.Create(user, "1234567");
              
                manager.AddToRole(user.Id, "user");
            }

            base.Seed(context);
        }
    }
}
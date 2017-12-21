using EP.Data.AspNetIdentity;
using EP.Data.Constants;
using EP.Data.DbContext;
using EP.Data.Entities.Emails;
using EP.Data.Entities.Logs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EP.Data
{
    public static class DbInitializerApplicationBuilderExtensions
    {
        public static IApplicationBuilder InitDefaultData(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

                var dbContext = serviceProvider.GetRequiredService<MongoDbContext>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

                // This protects from deadlocks by starting the async method on the ThreadPool.
                Task.Run(() => Initialize(dbContext, roleManager, userManager)).Wait();
            }

            return app;
        }

        private static async Task Initialize(
            MongoDbContext dbContext,
            RoleManager<AppRole> roleManager,
            UserManager<AppUser> userManager)
        {
            await SeedIdentityAsync(roleManager, userManager);

            await SeedEmailAccountAsync(dbContext);

            await SeedActivityLogTypeAsync(dbContext);
        }

        private static async Task SeedIdentityAsync(
            RoleManager<AppRole> roleManager,
            UserManager<AppUser> userManager)
        {
            var currentTime = DateTime.UtcNow;
            var roles = new string[] { "Administrators", "Supervisors", "Moderators", "Registereds", "Guests" };

            foreach (var roleName in roles)
            {
                var identityRole = await roleManager.FindByNameAsync(roleName.ToUpperInvariant());

                if (identityRole == null)
                {
                    await roleManager.CreateAsync(new AppRole(roleName) { CreatedOn = currentTime });
                }
            }

            string email = "ankn85@yahoo.com";
            string password = "1qazXSW@";

            var user = await userManager.FindByNameAsync(email.ToUpperInvariant());

            if (user == null)
            {
                user = new AppUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    //FullName = "System Administrator"
                    Roles = roles.ToList(),
                    Claims = new List<AppUserClaim>
                    {
                        new AppUserClaim("name", email),
                        new AppUserClaim("email", email),
                        new AppUserClaim("website", "http://localhost:5000/api/admin/dashboard")
                    },
                    CreatedOn = currentTime
                };

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRolesAsync(user, roles);
                }
            }
        }

        private static async Task SeedEmailAccountAsync(MongoDbContext dbContext)
        {
           string email = "eschoolapi@gmail.com";
           string password = "1qaw3(OLP_";

           var filter = Builders<EmailAccount>.Filter.Eq(e => e.Email, email);
           var options = new FindOptions<EmailAccount, EmailAccount>
           {
               Projection = Builders<EmailAccount>.Projection.Include(e => e.Id)
           };

           var emailAcc = await dbContext.EmailAccounts.FindAsync(filter, options);

           if (emailAcc == null)
           {
               emailAcc = new EmailAccount
               {
                   Email = email,
                   DisplayName = "No Reply",
                   Host = "smtp.gmail.com",
                   Port = 587,
                   UserName = email,
                   Password = password,
                   EnableSsl = false,
                   UseDefaultCredentials = true,
                   IsDefault = true
               };

               await dbContext.EmailAccounts.CreateAsync(emailAcc);
           }
        }

        private static async Task SeedActivityLogTypeAsync(MongoDbContext dbContext)
        {
            var count = await dbContext.ActivityLogTypes.CountAsync();

            if (count == 0)
            {
                var activityLogTypes = new ActivityLogType[]
                {
                    new ActivityLogType
                    {
                        SystemKeyword = SystemKeyword.UpdateActivityLogType,
                        Name = "Update an Activity Log Type",
                        Enabled = true
                    },
                    new ActivityLogType
                    {
                        SystemKeyword = SystemKeyword.CreateEmailAccount,
                        Name = "Create a new Email Account",
                        Enabled = true
                    },
                    new ActivityLogType
                    {
                        SystemKeyword = SystemKeyword.UpdateEmailAccount,
                        Name = "Update an Email Account",
                        Enabled = true
                    },
                    new ActivityLogType
                    {
                        SystemKeyword = SystemKeyword.DeleteEmailAccount,
                        Name = "Delete an Email Account",
                        Enabled = true
                    },
                    new ActivityLogType
                    {
                        SystemKeyword = SystemKeyword.CreateUser,
                        Name = "Create a new User",
                        Enabled = true
                    },
                    new ActivityLogType
                    {
                        SystemKeyword = SystemKeyword.UpdateUser,
                        Name = "Update an User",
                        Enabled = true
                    },
                    new ActivityLogType
                    {
                        SystemKeyword = SystemKeyword.DeleteUser,
                        Name = "Delete an User",
                        Enabled = true
                    },
                    new ActivityLogType
                    {
                        SystemKeyword = SystemKeyword.ResetUserPassword,
                        Name = "Reset password of an User",
                        Enabled = true
                    },
                    new ActivityLogType
                    {
                        SystemKeyword = SystemKeyword.CreateBlob,
                        Name = "Create a new Blob",
                        Enabled = true
                    },
                    new ActivityLogType
                    {
                        SystemKeyword = SystemKeyword.DeleteBlob,
                        Name = "Delete a Blob",
                        Enabled = true
                    },
                    new ActivityLogType
                    {
                        SystemKeyword = SystemKeyword.CreateNews,
                        Name = "Create a new News",
                        Enabled = true
                    },
                    new ActivityLogType
                    {
                        SystemKeyword = SystemKeyword.UpdateNews,
                        Name = "Update a News",
                        Enabled = true
                    },
                    new ActivityLogType
                    {
                        SystemKeyword = SystemKeyword.DeleteNews,
                        Name = "Delete a News",
                        Enabled = true
                    }
                };

                await dbContext.ActivityLogTypes.CreateAsync(activityLogTypes);
            }
        }
    }
}
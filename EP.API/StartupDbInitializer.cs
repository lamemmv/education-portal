using EP.Data.AspNetIdentity;
using EP.Data.DbContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;

namespace EP.API
{
    public static class StartupDbInitializer
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
            //await dbContext.Database.EnsureCreatedAsync();

            await SeedIdentityAsync(roleManager, userManager);

            //await SeedEmailAccountAsync(dbContext);

            //await SeedActivityLogTypeAsync(dbContext);
        }

        private static async Task SeedIdentityAsync(
            RoleManager<AppRole> roleManager,
            UserManager<AppUser> userManager)
        {
            var roles = new string[] { "Administrators", "Supervisors", "Moderators", "Registereds", "Guests" };

            foreach (var roleName in roles)
            {
                var identityRole = await roleManager.FindByNameAsync(roleName.ToUpperInvariant());

                if (identityRole == null)
                {
                    await roleManager.CreateAsync(new AppRole(roleName));
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
                    Roles = roles.ToList()
                };

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRolesAsync(user, roles);
                }
            }
        }

        //private static async Task SeedEmailAccountAsync(ObjectDbContext dbContext)
        //{
        //    string email = "eschoolapi@gmail.com";
        //    string password = "1qaw3(OLP_";

        //    var emailAccountDbSet = dbContext.Set<EmailAccount>();

        //    if (!await emailAccountDbSet.AnyAsync(e => e.Email == email))
        //    {
        //        await emailAccountDbSet.AddAsync(new EmailAccount
        //        {
        //            Email = email,
        //            DisplayName = "No Reply",
        //            Host = "smtp.gmail.com",
        //            Port = 587,
        //            UserName = email,
        //            Password = password,
        //            EnableSsl = false,
        //            UseDefaultCredentials = true,
        //            IsDefaultEmailAccount = true
        //        });

        //        await dbContext.SaveChangesAsync();
        //    }
        //}

        //private static async Task SeedActivityLogTypeAsync(ObjectDbContext dbContext)
        //{
        //    var activityLogTypeDbSet = dbContext.Set<ActivityLogType>();

        //    if (!await activityLogTypeDbSet.AnyAsync())
        //    {
        //        await activityLogTypeDbSet.AddRangeAsync(new ActivityLogType[]
        //        {
        //            new ActivityLogType
        //            {
        //                SystemKeyword = Apex.Services.Constants.SystemKeyword.UpdateActivityLogTypes,
        //                Name = "Update an Activity Log Type",
        //                Enabled = true
        //            },
        //            new ActivityLogType
        //            {
        //                SystemKeyword = Apex.Services.Constants.SystemKeyword.CreateEmailAccounts,
        //                Name = "Add a new Email Account",
        //                Enabled = true
        //            },
        //            new ActivityLogType
        //            {
        //                SystemKeyword = Apex.Services.Constants.SystemKeyword.UpdateEmailAccounts,
        //                Name = "Update an Email Account",
        //                Enabled = true
        //            },
        //            new ActivityLogType
        //            {
        //                SystemKeyword = Apex.Services.Constants.SystemKeyword.DeleteEmailAccounts,
        //                Name = "Delete an Email Account",
        //                Enabled = true
        //            },
        //            new ActivityLogType
        //            {
        //                SystemKeyword = Apex.Services.Constants.SystemKeyword.CreateUsers,
        //                Name = "Add a new User",
        //                Enabled = true
        //            },
        //            new ActivityLogType
        //            {
        //                SystemKeyword = Apex.Services.Constants.SystemKeyword.UpdateUsers,
        //                Name = "Update an User",
        //                Enabled = true
        //            },
        //            new ActivityLogType
        //            {
        //                SystemKeyword = Apex.Services.Constants.SystemKeyword.DeleteUsers,
        //                Name = "Delete an User",
        //                Enabled = true
        //            },
        //            new ActivityLogType
        //            {
        //                SystemKeyword = Apex.Services.Constants.SystemKeyword.ResetPasswordUsers,
        //                Name = "Reset password of an User",
        //                Enabled = true
        //            }
        //        });

        //        await dbContext.SaveChangesAsync();
        //    }
        //}
    }
}

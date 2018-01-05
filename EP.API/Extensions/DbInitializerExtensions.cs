using EP.Data.AspNetIdentity;
using EP.Data.DbContext;
using EP.Data.Entities.Blobs;
using EP.Data.Entities.Emails;
using EP.Data.Entities.Logs;
using EP.Services.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace EP.API.Extensions
{
    public static class DbInitializerExtensions
    {
        public static IApplicationBuilder InitDefaultBlob(
            this IApplicationBuilder app,
            string publicBlob,
            string publicBlobPath,
            string privateBlob,
            string privateBlobPath)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                // This protects from deadlocks by starting the async method on the ThreadPool.
                Task.Run(() => SeedBlobAsync(
                    scope.ServiceProvider,
                    publicBlob,
                    publicBlobPath,
                    privateBlob,
                    privateBlobPath)).Wait();
            }

            return app;
        }

        public static IApplicationBuilder InitDefaultData(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MongoDbContext>();

                // This protects from deadlocks by starting the async method on the ThreadPool.
                Task.Run(() => Initialize(scope.ServiceProvider, dbContext)).Wait();
            }

            return app;
        }

        private static async Task SeedBlobAsync(
            IServiceProvider serviceProvider,
            string publicBlob,
            string publicBlobPath,
            string privateBlob,
            string privateBlobPath)
        {
            var dbContext = serviceProvider.GetRequiredService<MongoDbContext>();
            
            var filter = Builders<Blob>.Filter.Eq(e => e.Name, publicBlob);
            var projection = Builders<Blob>.Projection.Include(e => e.Id);
            var blob = await dbContext.Blobs.GetSingleAsync(filter, projection);

            if (blob == null)
            {
                blob = new Blob
                {
                    Name = publicBlob,
                    PhysicalPath = publicBlobPath,
                    CreatedOn = DateTime.UtcNow
                };

                await dbContext.Blobs.CreateAsync(blob);
            }

            filter = Builders<Blob>.Filter.Eq(e => e.Name, privateBlob);
            blob = await dbContext.Blobs.GetSingleAsync(filter, projection);

            if (blob == null)
            {
                blob = new Blob
                {
                    Name = privateBlob,
                    PhysicalPath = privateBlobPath,
                    CreatedOn = DateTime.UtcNow
                };

                await dbContext.Blobs.CreateAsync(blob);
            }
        }

        private static async Task Initialize(IServiceProvider serviceProvider, MongoDbContext dbContext)
        {
            await SeedIdentityAsync(serviceProvider);

            await SeedEmailAccountAsync(dbContext);

            await SeedActivityLogTypeAsync(dbContext);
        }

        private static async Task SeedIdentityAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
            var roles = new string[] { "Administrators", "Supervisors", "Moderators", "Registereds", "Guests" };

            foreach (var roleName in roles)
            {
                var identityRole = await roleManager.FindByNameAsync(roleName.ToUpperInvariant());

                if (identityRole == null)
                {
                    await roleManager.CreateAsync(new AppRole(roleName) { CreatedOn = DateTime.UtcNow });
                }
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
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
                    CreatedOn = DateTime.UtcNow
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
            var dbCount = await dbContext.EmailAccounts.CountAsync();

            if (dbCount == 0)
            {
                string email = "eschoolapi@gmail.com";
                string password = "1qaw3(OLP_";

                var emailAcc = new EmailAccount
                {
                    Email = email,
                    DisplayName = "No Reply",
                    Host = "smtp.gmail.com",
                    Port = 587,
                    UserName = email,
                    Password = password,
                    EnableSsl = false,
                    UseDefaultCredentials = true,
                    IsDefault = true,
                    CreatedOn = DateTime.UtcNow
                };

                await dbContext.EmailAccounts.CreateAsync(emailAcc);
            }
        }

        private static async Task SeedActivityLogTypeAsync(MongoDbContext dbContext)
        {
            var activityLogTypes = new[]
            {
                new ActivityLogType
                {
                    SystemKeyword = SystemKeyword.UpdateActivityLogType,
                    Name = "Update an Activity Log Type",
                    Enabled = true,
                    CreatedOn = DateTime.UtcNow
                },
                new ActivityLogType
                {
                    SystemKeyword = SystemKeyword.CreateEmailAccount,
                    Name = "Create a new Email Account",
                    Enabled = true,
                    CreatedOn = DateTime.UtcNow
                },
                new ActivityLogType
                {
                    SystemKeyword = SystemKeyword.UpdateEmailAccount,
                    Name = "Update an Email Account",
                    Enabled = true,
                    CreatedOn = DateTime.UtcNow
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
                    Enabled = true,
                    CreatedOn = DateTime.UtcNow
                },
                new ActivityLogType
                {
                    SystemKeyword = SystemKeyword.UpdateUser,
                    Name = "Update an User",
                    Enabled = true,
                    CreatedOn = DateTime.UtcNow
                },
                new ActivityLogType
                {
                    SystemKeyword = SystemKeyword.DeleteUser,
                    Name = "Delete an User",
                    Enabled = true,
                    CreatedOn = DateTime.UtcNow
                },
                new ActivityLogType
                {
                    SystemKeyword = SystemKeyword.ResetUserPassword,
                    Name = "Reset password of an User",
                    Enabled = true,
                    CreatedOn = DateTime.UtcNow
                },
                new ActivityLogType
                {
                    SystemKeyword = SystemKeyword.CreateBlob,
                    Name = "Create a new Blob",
                    Enabled = true,
                    CreatedOn = DateTime.UtcNow
                },
                new ActivityLogType
                {
                    SystemKeyword = SystemKeyword.DeleteBlob,
                    Name = "Delete a Blob",
                    Enabled = true,
                    CreatedOn = DateTime.UtcNow
                },
                new ActivityLogType
                {
                    SystemKeyword = SystemKeyword.CreateNews,
                    Name = "Create a new News",
                    Enabled = true,
                    CreatedOn = DateTime.UtcNow
                },
                new ActivityLogType
                {
                    SystemKeyword = SystemKeyword.UpdateNews,
                    Name = "Update a News",
                    Enabled = true,
                    CreatedOn = DateTime.UtcNow
                },
                new ActivityLogType
                {
                    SystemKeyword = SystemKeyword.DeleteNews,
                    Name = "Delete a News",
                    Enabled = true,
                    CreatedOn = DateTime.UtcNow
                }
            };

            var dbCount = await dbContext.ActivityLogTypes.CountAsync();

            if (dbCount != activityLogTypes.LongLength)
            {
                await dbContext.ActivityLogTypes.DeleteAsync();
                await dbContext.ActivityLogTypes.CreateAsync(activityLogTypes);
            }
        }
    }
}
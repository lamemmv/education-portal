using EP.Data.AspNetIdentity;
using EP.Data.DbContext;
using EP.Data.Entities.Blobs;
using EP.Data.Entities.Emails;
using EP.Data.Entities.Logs;
using EP.Services.Constants;
using EP.Services.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace EP.API.Extensions
{
    public static class DbInitializerExtensions
    {
        public static IApplicationBuilder InitDefaultBlob(this IApplicationBuilder app, params Blob[] blobs)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                // This protects from deadlocks by starting the async method on the ThreadPool.
                Task.Run(() => SeedBlobAsync(scope.ServiceProvider, blobs)).Wait();
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

        private static async Task SeedBlobAsync(IServiceProvider serviceProvider, params Blob[] blobs)
        {
            var dbContext = serviceProvider.GetRequiredService<MongoDbContext>();
            FilterDefinition<Blob> filter;
            long dbCount;

            foreach (var blob in blobs)
            {
                filter = Builders<Blob>.Filter.Eq(e => e.Name, blob.Name);
                dbCount = await dbContext.Blobs.CountAsync(filter);

                if (dbCount == 0)
                {
                    await dbContext.Blobs.CreateAsync(blob);
                }
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
            var roles = new AppRole[]
            {
                new AppRole("Administrators")
                {
                    Claims = new List<AppClaim>
                    {
                        new AppClaim(FunctionName.BlobManagement, ((int)Permission.All).ToString()),
                        new AppClaim(FunctionName.NewsManagement, ((int)Permission.All).ToString())
                    }
                }
            };

            foreach (var role in roles)
            {
                var identityRole = await roleManager.FindByNameAsync(role.Name.ToUpperInvariant());

                if (identityRole == null)
                {
                    await roleManager.CreateAsync(role);
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
                    Roles = roles.Select(r => r.Name).ToList()
                };

                var result = await userManager.CreateAsync(user, password);
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
                    IsDefault = true
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
                    SystemKeyword = SystemKeyword.UpdateBlob,
                    Name = "Update a Blob",
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

            long dbCount;

            foreach (var logType in activityLogTypes)
            {
                dbCount = await dbContext.ActivityLogTypes.CountAsync();

                if (dbCount == 0)
                {
                    await dbContext.ActivityLogTypes.CreateAsync(logType);
                }
            }
        }
    }
}
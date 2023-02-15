using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NewsWeb.Domain.Entities.Identity;
using NewsWeb.Persistence.Contexts;

namespace NewsWeb.Application.Seed
{
    public static class SeedData
    {
        public static async ValueTask EnsurePopulatedAsync(this IServiceProvider services)
        {
            using var cancelTokenSource = new CancellationTokenSource();

            using var scope = services.CreateScope();

            using var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await appDbContext.Database.MigrateAsync(cancelTokenSource.Token);

            using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            #region User

            #region Admin

            if (await userManager.FindByEmailAsync("admin@email.com") == null)
            {
                var user = new AppUser();
                user.UserName = "adminTest06";
                user.Email = "admin@email.com";
                user.FullName = "Admin Test";

                var result = await userManager.CreateAsync(user, "123456");
            }

            #endregion

            #endregion

            cancelTokenSource.Cancel();

            cancelTokenSource.Dispose();
            scope.Dispose();
            appDbContext.Dispose();
            userManager.Dispose();

        }

    }
}

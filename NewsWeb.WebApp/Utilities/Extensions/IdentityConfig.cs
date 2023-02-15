using Microsoft.AspNetCore.Identity;
using NewsWeb.Domain.Entities.Identity;
using NewsWeb.Persistence.Contexts;

namespace NewsWeb.WebApp.Utilities.Extensions
{
    public static class IdentityConfig
    {
        /// <summary>
        /// Identity kurulumu ve ayarları
        /// </summary>
        public static IServiceCollection AddIdentityConfig(this IServiceCollection services)
        {
            services.AddIdentity<AppUser,IdentityRole<Guid>>(options =>
            {
                // User settings
                options.User.AllowedUserNameCharacters = "abcçdefgğhiıjklmnoöpqrsştuüvwxyzABCÇDEFGĞHIİJKLMNOÖPQRSŞTUÜVWXYZ0123456789-_";
                options.User.RequireUniqueEmail = true;//eposta adresi eşsiz olacak

                // Password settings
                options.Password.RequireDigit = false;//şifre belirlerken; sayı zorunluğu yok
                options.Password.RequiredLength = 6;//şifre belirlerken;min 6 karakter olacak
                options.Password.RequiredUniqueChars = 0;//şifre belirlerken;eşsiz karakter zorunluluğu yok 
                options.Password.RequireLowercase = false;//şifre belirlerken;küçük harf zorunluluğu yok
                options.Password.RequireUppercase = false;//şifre belirlerken;büyük harf zorunluluğu yok
                options.Password.RequireNonAlphanumeric = false;//şifre belirlerken;özel karakter zorunluluğu yok

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            })
            .AddEntityFrameworkStores<AppDbContext>();

            return services;
        }

        public static IApplicationBuilder UseIdentityAuth(this IApplicationBuilder builder)
        {
            builder.UseAuthentication();
            builder.UseAuthorization();

            return builder;
        }
    }
}

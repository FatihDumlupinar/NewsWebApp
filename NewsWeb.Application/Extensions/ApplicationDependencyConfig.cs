using Microsoft.Extensions.DependencyInjection;
using NewsWeb.Application.Interfaces;
using NewsWeb.Application.Repositories;

namespace NewsWeb.Application.Extensions
{
    public static class ApplicationDependencyConfig
    {
        /// <summary>
        /// Application katmanında kullanılan dependency ler
        /// </summary>
        public static IServiceCollection AddApplicationDependecyConfig(this IServiceCollection services)
        {
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}

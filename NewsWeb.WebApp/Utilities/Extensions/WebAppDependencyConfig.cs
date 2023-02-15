namespace NewsWeb.WebApp.Utilities.Extensions
{
    public static class WebAppDependencyConfig
    {
        /// <summary>
        /// WebApp katmanında kullanılan dependency ler
        /// </summary>
        public static IServiceCollection AddWebApiDependecyConfig(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return services;
        }
    }
}

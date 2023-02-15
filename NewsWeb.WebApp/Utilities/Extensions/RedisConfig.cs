using NewsWeb.Infrastructure.Services.Chache;

namespace NewsWeb.WebApp.Utilities.Extensions
{
    public static class RedisConfig
    {
        public static IServiceCollection AddRedisConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ICacheService, CacheService>();

            var test = $"{configuration.GetValue<string>("Redis:Server")}:{configuration.GetValue<int>("Redis:Port")}";

            services.AddStackExchangeRedisCache(options =>
            {
                options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
                {
                    SyncTimeout = 500000,
                    AbortOnConnectFail = false,
                    EndPoints =
                    {
                        {configuration.GetValue<string>("Redis:Server"),configuration.GetValue<int>("Redis:Port") }
                    },
                    Password = configuration.GetValue<string>("Redis:Password"),
                    User = configuration.GetValue<string>("Redis:UserName"),
                };
            });

            return services;
        }
    }
}

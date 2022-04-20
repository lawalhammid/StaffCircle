using EFCore.EFContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;


namespace api.Configuartions
{
    public static class ConnectionConfiguration
    {
        //To get the Database enviroment. i.e Developer, Staging(Testing) or Production(live) below 
        public static IServiceCollection DatabaseConnection(this IServiceCollection services,
           IConfiguration configuration)
        {
            var connection = String.Empty;
            string ConnectionEnvironment = AppSettingsConfig.EnvironmentAppSettings();
            connection = AppSettingsConfig.EnvironmentAppSettings() != null ? configuration.GetConnectionString(ConnectionEnvironment) : string.Empty;

            services.AddDbContextPool<EfDataContext>(options => options.UseSqlServer(connection));
            
            return services;
        }
    }
}

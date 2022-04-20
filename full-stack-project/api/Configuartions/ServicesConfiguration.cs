using api.MappingModels;
using AutoMapper;
using BusinessLogic.Contracts;
using BusinessLogic.Services;
using EFCore.UOF;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using AutoMapper;

namespace api.Configuartions
{
    public static class ServicesConfiguration
    {
        public static void ConfigureRepositories(this IServiceCollection services)
        { 
            services.AddScoped<IUsers, UsersService>()
                .AddScoped<IUnitOfWork, UnitOfWorkService>()
                .AddScoped<IMessages, MessagesService>()
                .AddScoped<ISendSMSTwilo, SendSMSTwiloService>();
        }
        // add cross orgin policy
        public static void AddCORS(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .AllowAnyOrigin()// disable this option for security reasons
                       // .WithOrigins(AppSettingsConfig.CorsPolicyAppSettings())
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
        }
        // Auto Mapper Configurations
        public static void AddMapper(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}

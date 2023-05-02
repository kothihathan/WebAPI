using Microsoft.Extensions.DependencyInjection;
using WebApi.Interfaces;
using WebApi.Services;

namespace WebApi
{
    public static class ServiceExtensions
    {
        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICostCenterService, CostCenterService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IBusinessAreaService, BusinessAreaService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IUserRoleService, UserRoleService>();
            services.AddScoped<IUserRightService, UserRightService>();
            services.AddScoped<IModuleService, ModuleService>();
            services.AddScoped<IModulePageService, ModulePageService>();
        }
    }
}

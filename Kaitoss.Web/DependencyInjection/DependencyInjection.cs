using AutoMapper;
using Kaitoss.Web.Data;
using Kaitoss.Web.Mapper;
using Kaitoss.Web.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Kaitoss.Web.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataLayer(
                    this IServiceCollection services, 
                    ConfigurationManager configurationManager)
        {
            services.AddDbContext<KaitossProjectContext>(options =>
            {
                options.UseSqlServer(configurationManager.
                    GetConnectionString("DefaultConnection"));
            });
            
            services.AddIdentity<ApplicationUser, IdentityRole>()
               .AddEntityFrameworkStores<KaitossProjectContext>()
               .AddDefaultTokenProviders();
            services.AddSingleton(
                       new MapperConfiguration(
                       cfg => { cfg.AddProfile(new Mapping()); }
                       ).
                       CreateMapper()
                       );
            return services;
        }
        public static IServiceCollection AddAuthLayer(this IServiceCollection services)
        {

            #region AddAuthentication
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });

            #endregion
            return services;
        }
    }
}

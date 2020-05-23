using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using UnipiLabs.DataAccess;
using UnipiLabs.Hubs;
using UnipiLabs.Models;

[assembly: OwinStartup(typeof(UnipiLabs.Startup))]
namespace UnipiLabs
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
         
        }

        public void Configuration(IAppBuilder app)
        {
            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => new UserIDProvider());

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator
                .OnValidateIdentity<UnipiUsersManager, Users, int>(
                    validateInterval: TimeSpan.FromMinutes(300),
                    regenerateIdentityCallback: (manager, user) =>
                        user.GenerateUserIdentityAsync(manager),
                    getUserIdCallback: (id) => (id.GetUserId<int>()))
                }
            });

            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }
    }
}
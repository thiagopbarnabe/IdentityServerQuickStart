using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer4;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;

namespace IdentityServerQuickStart
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            //AddIdentityServer registers the IdentityServer services in DI. It also registers an in-memory store for runtime state.
            //The AddDeveloperSigningCredential extension creates temporary key material for signing tokens. 
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddTestUsers(Config.GetUsers())
                //.AddProfileService<MyProfileService>()       
                .AddConfigurationStore()
                .AddInMemoryIdentityResources(Config.GetIdentityResources());
            
            // login page
            //services.Configure<IdentityServer4.Configuration.IdentityServerOptions>(options=> 
            //{
            //    options.UserInteraction.LoginUrl = "";
            //});

            services.AddAuthentication()
                .AddGoogle("Google", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.ClientId = "931441181996-mkjk82ok1m9a4eqde6tfkmpv8kqj7o4v.apps.googleusercontent.com";
                    options.ClientSecret = "twubokDw9XV_91dnmS0TVUNT";
                    options.Scope.Add("https://www.googleapis.com/auth/plus.login");

                    options.ClaimActions.MapJsonSubKey("trolha", "ageRange", "min");
                    options.ClaimActions.MapJsonKey("language", "language");
                    
                })
                .AddOpenIdConnect("oidc","OpenId Connect",options=>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.SignOutScheme = IdentityServerConstants.SignoutScheme;

                    options.Authority = "https://demo.identityserver.io/";
                    options.ClientId = "implicit";

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "name",
                        RoleClaimType = "role"
                    };
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}

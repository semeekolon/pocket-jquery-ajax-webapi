﻿using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Pocket.WebAPI.Providers;

[assembly: OwinStartup(typeof(Pocket.WebAPI.Startup))]

namespace Pocket.WebAPI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888

            HttpConfiguration config = new HttpConfiguration();

            ConfigureOAuth(app);

            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);

        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(90),
                Provider = new AuthProvider(),
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            //Owin
            //Microsoft.Owin
            //Microsoft.Owin.Cors
            //Microsoft.Owin.Host.SystemWeb
            //Microsoft.Owin.Security
            //Microsoft.Owin.Security.OAuth
            //Newtonsoft.Json
            //Microsoft.AspNet.WebApi.Owin

        }
    }
}

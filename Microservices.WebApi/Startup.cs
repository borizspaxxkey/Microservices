using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microservices.WebApi.Helpers;
using Microservices.WebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Microservices.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string domain = "https://iamebuka-angular-azure.auth0.com/";

            services.AddAuthorization(options =>
            {
                options.AddPolicy("read:forecasts", policy => policy.Requirements.Add(new HasScopeRequirement("read:forecasts", domain)));
                options.AddPolicy("write:forecasts", policy => policy.Requirements.Add(new HasScopeRequirement("write:forecasts", domain)));
            });

            // 1. Add Auth0 Authentication Services
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // TODO : Move to configuration
                    ValidIssuer = domain,
                    ValidAudience = "api://forecasts",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("fQMFDe8k1m3j911cH1X3xV3P0uV98KeL"))
                };
            });


            services.AddScoped<IForecastService, ForecastService>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // 2. Enable authentication middleware
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseMvc();
        }
    }
}

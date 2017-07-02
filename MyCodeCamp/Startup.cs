using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyCodeCamp.Data;
using Newtonsoft.Json;
using AutoMapper;
using MyCodeCamp.Data.Entities;
using MyCodeCamp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using MyCodeCamp.Controllers;

namespace MyCodeCamp
{
    public class Startup
    {
		private IConfigurationRoot _config { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            _config = builder.Build();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Ajoute un service pour injecter plus tard dans le constructeur
            services.AddSingleton(_config);
            services.AddDbContext<CampContext>(ServiceLifetime.Scoped);
            services.AddScoped<ICampRepository, CampRepository>();
			services.AddTransient<CampDbInitializer>();
			services.AddTransient<CampIdentityInitializer>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			services.AddAutoMapper();

            services.AddMemoryCache();

            services.AddIdentity<CampUser, IdentityRole>()
                .AddEntityFrameworkStores<CampContext>();

            services.Configure<IdentityOptions>(config => 
            {
                config.Cookies.ApplicationCookie.Events =
                    new CookieAuthenticationEvents()
                    {
                        OnRedirectToLogin = (ctx) =>
                        {
                            if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
                            {
                                ctx.Response.StatusCode = 401;
                            }

                            return Task.CompletedTask;
                        },
                    OnRedirectToAccessDenied = (ctx) =>
						{
							if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
							{
								ctx.Response.StatusCode = 403;
							}

							return Task.CompletedTask;
						}
                    };
            });

			/*services.AddCors(cfg =>
            {
			    cfg.AddPolicy("AnyCanGET", bldr =>
			    {
			        bldr.AllowAnyHeader()
			            .WithMethods("GET")
			            .AllowAnyOrigin();

			    });

			});*/

			services.AddApiVersioning(cfg => 
            {
                cfg.DefaultApiVersion = new ApiVersion(1, 1);
                cfg.AssumeDefaultVersionWhenUnspecified = true;
                cfg.ReportApiVersions = true;
                var rdr = new QueryStringOrHeaderApiVersionReader("ver"); 
                rdr.HeaderNames.Add("X-MyCodeCamp-Version");
                cfg.ApiVersionReader = rdr;

                /*cfg.Conventions.Controller<TalksController>()
                   .HasApiVersion(new ApiVersion(1, 0))
				   .HasApiVersion(new ApiVersion(1, 1))
                   .HasApiVersion(new ApiVersion(2, 0))
                   .Action(m => m.Post(default(string), default(int), default(TalkModel)))
                   .MapToApiVersion(new ApiVersion(2,0));*/
            });

            services.AddAuthorization(cfg => {
                cfg.AddPolicy("SuperUsers", p => p.RequireClaim("SuperUser", "True"));
            });

            // Add framework services.
            services.AddMvc()
            .AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
                              IHostingEnvironment env,
                              ILoggerFactory loggerFactory,
                              CampDbInitializer campDbInit,
                              CampIdentityInitializer campIdInit)
        {
            loggerFactory.AddConsole(_config.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseIdentity();

			/*app.UseCors(cfg =>
			{
			    cfg.AllowAnyHeader()
			        .AllowAnyMethod()
			        .AllowAnyOrigin();
			});*/

			app.UseJwtBearerAuthentication(new JwtBearerOptions()
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidIssuer = _config["Tokens:Issuer"],
                    ValidAudience = _config["Tokens:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"])),
                    ValidateLifetime = true
                }
            });

            app.UseMvc();

            campDbInit.Seed().Wait();
            campIdInit.Seed().Wait();
        }
    }
}

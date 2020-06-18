using System;
using System.IO;
using System.Text;
using AutoMapper;
using back_modelo.BLL;
using back_modelo.DAL.DAO;
using back_modelo.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using back_modelo.Extensions;
using back_modelo.Extensions.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using NLog;
using MongoDB.Driver;

namespace back_modelo
{
    public class Startup
    {
        private readonly MapperConfiguration _mapperConfiguration;
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

            // AUTOMAPPER PARA MAPEAR DTO E MODEL
            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfileConfiguration());
            });

            Configuration = configuration;
        }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Configuracoes>(
                options =>
                {
                    options.ConnectionString =
                        Configuration.GetSection("MongoDb:ConnectionString").Value;
                    options.Database = Configuration.GetSection("MongoDb:Database").Value;
                });

            services.AddCors(
                options =>
                {
                    options.AddPolicy(MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:8080",
                                "http://localhost:8081",
                                "http://localhost:8082")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                    });
                });

            services.AddSingleton<IMongoClient, MongoClient>(
                _ => new MongoClient(Configuration.GetSection("MongoDb:ConnectionString").Value));

            services.AddRazorPages();

            // JWT
            services.AddAuthentication(o =>
            {
                o.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                o.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                //options.AccessDeniedPath = new PathString("/Account/Login/");
                //options.LoginPath = new PathString("/Account/Login/");
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("HJaM5dvQy5WUEQ6LPR7yRrcO4m2Mse4u94FMsgXtMjrc66XeM34sdPWQ2ilEA9fo")),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromDays(1)
                };
            });

            // SWAGGER
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Modelo Back-End", Version = "v1" });
            });

            // LOG
            services.AddSingleton<ILoggerManager, LoggerManager>();

            // AUTOMAPPER
            services.AddSingleton(sp => _mapperConfiguration.CreateMapper());

            // DI
            services.AddScoped<IMongoContext, MongoContext>();

            services.AddScoped<IPessoaDAO, PessoaDAO>();
            services.AddScoped<IPessoaBLL, PessoaBLL>();

            services.AddScoped<ICorDAO, CorDAO>();
            services.AddScoped<ICorBLL, CorBLL>();

            services.AddScoped<IMBDAO, MBDAO>();

            services.AddControllers();
            services.AddScoped<SeedingService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SeedingService seedingService, ILoggerManager logger)
        {
            app.UseCors(MyAllowSpecificOrigins);

            if (env.IsDevelopment())
            {
                seedingService.Seed();
                app.UseDeveloperExceptionPage();
            }
            if (env.IsProduction())
            {
                seedingService.Seed();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "swagger";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Modelo Back-End");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
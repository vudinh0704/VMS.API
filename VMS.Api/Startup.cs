using Autofac;
using Autofac.Extras.AggregateService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Data;
using System.Net;
using System.Text;
using VMS.Api.Exceptions;
using VMS.Api.Helpers;
using VMS.Api.Interfaces;
using VMS.Core.Interfaces.Repositories;
using VMS.Infrastructure.Data;

namespace VMS.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            AppSettings.LoadSettings(configuration);
        }

        public IConfiguration Configuration { get; }

        public ILifetimeScope AutofacContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.ModelBinderProviders.Insert(0, new CustomBinderProvider());
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
                options.SerializerSettings.Converters.Add(new StringJsonConverter());
            }).ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressConsumesConstraintForFormFileParameters = true;
                options.SuppressInferBindingSourcesForParameters = false;
                options.SuppressModelStateInvalidFilter = true;
                options.SuppressMapClientErrors = true;
            });
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });
            services.AddDbContext<VMSDbContext>(options => options.UseSqlServer("name=ConnectionStrings:VMSConnection").UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer(options =>
                 {
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuer = true,
                         ValidateAudience = false,
                         ValidateLifetime = true,
                         ValidateIssuerSigningKey = true,
                         ValidIssuer = AppSettings.JwtIssuer,
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.JwtKey)),
                         ClockSkew = TimeSpan.FromMinutes(60)
                     };
                 });
            services.AddSingleton<IAuthorizationPolicyProvider, VmsAuthorizationPolicyProvider>();
            services.AddSingleton<IAuthorizationMiddlewareResultHandler, VmsAuthorizationMiddlewareResultHandler>();
            services.AddTransient<IAuthorizationHandler, VmsAuthorizationHandler>();

            services.AddTransient<IDbConnection>(db => new SqlConnection(Configuration.GetConnectionString("VMSConnection")));

            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<ICampaignRepository, CampaignRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IFunctionRepository, FunctionRepository>();
            services.AddTransient<IGroupRepository, GroupRepository>();
            services.AddTransient<IPostRepository, PostRepository>();
            services.AddTransient<IWardRepository, WardRepository>();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterAggregateService<IBaseApiService>();
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        public void Configure(IApplicationBuilder app)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var exception = exceptionHandlerPathFeature?.Error;
                    if (exception is BaseException ex)
                    {
                        string message = $"{{\"code\":\"{ex.Code}\",\"message\":\"{ex.Message}\"}}";
                        context.Response.StatusCode = Convert.ToInt32(ex.StatusCode);
                        context.Response.ContentLength = Encoding.UTF8.GetByteCount(message);
                        await context.Response.WriteAsync(message);
                    }
                    else
                    {
                        string message = $"{{\"code\":\"internal_server_error\",\"message\":\"{exception.Message}\"}}";
                        context.Response.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                        context.Response.ContentLength = Encoding.UTF8.GetByteCount(message);
                        await context.Response.WriteAsync(message);
                    }
                });
            });

            app.UseCors();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
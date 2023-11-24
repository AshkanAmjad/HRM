using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP.Data;
using Microsoft.EntityFrameworkCore;
using ERP.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using ERP.Models;
using ERP.Convertors;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;

namespace ERP
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

            services.AddControllersWithViews();

            #region Authenication
            services.AddAuthentication()
            .AddCookie("ProvinceArea", options =>
             {
                 options.Cookie.Name = "ProvinceArea";
                 options.LoginPath = "/PLogin";
                 options.LogoutPath = "/PLogout";
                 options.ExpireTimeSpan =TimeSpan.FromHours(10);
             }).AddCookie("CountyArea", options =>
             {
                 options.Cookie.Name = "CountyArea";
                 options.LoginPath = "/CLogin";
                 options.LogoutPath = "/CLogout";
                 options.ExpireTimeSpan = TimeSpan.FromHours(10);
             }).AddCookie("DistrictArea", options =>
             {
                 options.Cookie.Name = "DistrictArea";
                 options.LoginPath = "/DLogin";
                 options.LogoutPath = "/DLogout";
                 options.ExpireTimeSpan = TimeSpan.FromHours(10);
             });


            #endregion

            #region Db Context

            services.AddDbContext<ERPContext>(options =>
                { options.UseSqlServer("Data Source =.;Initial Catalog=ERP_DB;Integrated Security=true;TrustServerCertificate=True"); });

            #endregion

            #region IOC
            services.AddTransient<IManagementService, ManagementService>();
            services.AddTransient<IViewRenderService, RenderViewToString>();
            #endregion

            #region Forgot Password
            services.Configure<KavenegarInfoViewModel>(Configuration.GetSection("KavenegarInfo"));
            services.AddScoped<ISMSService, SMSService>();
            #endregion

            services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.Arabic }));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = context =>
                {
                    context.Context.Response.Headers.Add("Cache-Control", "no-cache, no-store");
                    context.Context.Response.Headers.Add("Expires", "-1");
                }
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
             );
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

using Microsoft.AspNetCore.Authentication.Cookies;
using BM.Models;
using Microsoft.EntityFrameworkCore;

using Insya.Localization;
using Portal.Utilities;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net.Mail;
using System.Net;
using System.Net.Http;

namespace BM.Controllers
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Register EmailService
            services.AddTransient<IEmailService, EmailService>(); // Ensure EmailService is implemented properly

            // Register other services (like DbContext if needed)
            services.AddDbContext<BIMSContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("YourConnectionString")));

            // Add MVC services
            services.AddControllersWithViews();
         

            // Register options to read SMTP settings from configuration
            services.AddOptions<EmailSettings>()
                .Bind(Configuration.GetSection("EmailSettings"))
                .ValidateOnStart();

            // Register Email Sender
            services.AddTransient<ISmtpClientFactory, SmtpClientFactory>();
        }




        //public void ConfigureServices(IServiceCollection services)
        //{
        //    services.AddDbContext<BIMSContext>(options =>
        //    options.UseSqlServer(Configuration.GetConnectionString("BIMSConnection")));

        //        services.AddControllers(config =>{config.Filters.Add(new AccessFilters());});
        //        services.Configure<EmailConfiguration>(Configuration.GetSection("EmailConfiguration"));
        //        services.AddTransient<MailManager>();

        //        services.AddControllersWithViews();
        //        services.AddDistributedMemoryCache();
        //        services.AddSession(options =>
        //        {
        //            options.Cookie.Name = "webportal.Session";
        //            options.IdleTimeout = TimeSpan.FromMinutes(50);
        //            options.Cookie.HttpOnly = true;
        //            options.Cookie.IsEssential = true;
        //        });

        //        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        //        _ = services.AddAuthentication(options =>
        //        {

        //            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        //            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        //        })
        //        .AddCookie(cookie =>
        //        {
        //            cookie.Cookie.Name = "webportal.cookie";
        //            cookie.Cookie.MaxAge = TimeSpan.FromMinutes(50);
        //            cookie.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        //            cookie.SlidingExpiration = true;
        //        });
        //        services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
        //        services.Configure<AppConfig>(Configuration.GetSection("Appconfig"));
        //        services.AddTransient<IMailService, MailService>();
        //        services.AddTransient<IValidateRequest, ValidateRequestConcrete>();
        //        services.AddScoped<IExcelImporter, ExcelImporter>();
        //        services.AddScoped<IExcelExport, ExcelExport>();
        //        services.AddScoped<Localization, Localization>();
        //        services.AddScoped<CurrentUser, CurrentUser>();
        //        services.AddScoped<UserManagement, UserManagement>();
        //        services.AddScoped<Configurations, Configurations>();
        //        services.AddControllers(config =>
        //        {
        //            config.Filters.Add(new AccessFilters());
        //        });
        //        services.AddControllersWithViews();
        //        services.AddControllersWithViews(config => config.Filters.Add(typeof(CustomExceptionFilter)));

        //    }
        //    private void loadUserMenus(IServiceCollection services)
        //    {
        //        ServiceProvider serviceProvider = services.BuildServiceProvider();

        //    }
        //    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        //    {
        //        if (env.IsDevelopment())
        //        {
        //            app.UseDeveloperExceptionPage();
        //        }
        //        else
        //        {
        //            app.Use((context, next) =>
        //            {
        //                context.Request.Scheme = "https";
        //                return next();
        //            });
        //            app.UseHsts();
        //        }
        //        app.UseStatusCodePagesWithReExecute("/Home/Error", "?code={0}");
        //        app.UseStatusCodePages("text/plain", "Status code page, status code: {0}");
        //        app.UseHttpsRedirection();
        //        app.UseStaticFiles();
        //        app.UseRouting();
        //        app.UseSession();
        //        app.UseAuthentication();
        //        app.UseAuthorization();
        //        app.UseEndpoints(endpoints =>
        //        {
        //            endpoints.MapAreaControllerRoute(
        //                name: "Administrator",
        //                areaName: "Administrator",
        //                pattern: "Administrator/{controller=Home}/{action=Index}/{id?}");
        //                endpoints.MapControllerRoute(
        //                name: "default",
        //                pattern: "{controller=Home}/{action=Index}/{id?}");
        //        });
        //    }

        //    private class AccessFilters : IFilterMetadata
        //    {
        //    }
        //}

        //public class StartupX
        //{
        //    public static class Settings
        //    {
        //        public static IConfiguration Configuration;
        //    }
        //    public Startup(IConfiguration configuration)
        //    {
        //        Configuration = configuration;
        //        Settings.Configuration = configuration;
        //    }
        //    public IConfiguration Configuration { get; }

        //    public void ConfigureServices(IServiceCollection services)
        //    {
        //        services.AddDistributedMemoryCache();
        //        services.AddHttpContextAccessor();
        //        services.AddSession(options =>
        //        {
        //            options.IdleTimeout = TimeSpan.FromMinutes(30);
        //            options.Cookie.HttpOnly = true;
        //            options.Cookie.IsEssential = true;
        //        });
        //        services.AddDbContext<NexttimsContext>(options =>
        //        options.UseSqlServer(Configuration.GetConnectionString("NextTIMSConnection"))
        //        );
        //        services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
        //        services.Configure<AppConfig>(Configuration.GetSection("Appconfig"));
        //        services.AddTransient<IMailService, MailService>();
        //        services.AddTransient<IValidateRequest, ValidateRequestConcrete>();
        //        services.AddScoped<IExcelImporter, ExcelImporter>();
        //        services.AddScoped<IExcelExport, ExcelExport>();
        //        services.AddScoped<Localization, Localization>();
        //        services.AddScoped<UserManagement, UserManagement>();
        //        services.AddScoped<CurrentUser, CurrentUser>();
        //        services.AddScoped<Configurations, Configurations>();
        //        services.AddScoped<DocumentManagement, DocumentManagement>();
        //        services.AddControllers(config =>
        //        {
        //            config.Filters.Add(new AccessFilters());
        //        });
        //        services.AddControllersWithViews();
        //        services.AddControllersWithViews(config => config.Filters.Add(typeof(CustomExceptionFilter)));

        //    }
        //    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        //    {
        //        if (env.IsDevelopment())
        //        {
        //            app.UseDeveloperExceptionPage();
        //        }
        //        else
        //        {
        //            app.Use((context, next) =>
        //            {
        //                context.Request.Scheme = "https";
        //                return next();
        //            });
        //            app.UseHsts();
        //        }
        //        app.UseHttpsRedirection();
        //        app.UseStaticFiles();
        //        app.UseRouting();
        //        app.UseSession();
        //        app.UseAuthentication();
        //        app.UseAuthorization();
        //        app.UseEndpoints(endpoints =>
        //        {
        //            endpoints.MapControllerRoute(
        //                name: "default",
        //                pattern: "{controller=Home}/{action=Index}/{id?}");
        //        });
        //    }
        //}

    }

  
}



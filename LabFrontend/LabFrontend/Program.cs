using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;

namespace LabFrontend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddHttpClient();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });

            ConfigureServices(builder.Services);

            var app = builder.Build();
            app.UseSession();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            var options = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);
            app.Run();

            static void ConfigureServices(IServiceCollection services)
            {
                services.AddLocalization(options =>
                {
                    options.ResourcesPath = "Resources";
                });

                services.Configure<RequestLocalizationOptions>(options =>
                {
                    options.SetDefaultCulture("en-Us");
                    options.AddSupportedUICultures("en-US", "uk-UA");
                    options.FallBackToParentUICultures = true;
                });

                services
                    .AddRazorPages()
                    .AddViewLocalization();

                services.AddScoped<RequestLocalizationCookiesMiddleware>();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace WebApp
{
	public class Startup
	{
        private const string ResourcePath = "Resources";

		public IConfigurationRoot Configuration { get; }
		public RequestLocalizationOptions LocalizationOptions { get; set; }

		protected static List<CultureInfo> supportedCultures = new List<CultureInfo>
		{
			new CultureInfo("se")
		};
		protected static List<CultureInfo> supportedUICultures = new List<CultureInfo>
		{
			new CultureInfo("sv-SE"),
			new CultureInfo("en-US"),
			new CultureInfo("no")
		};

        public Startup(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder();

			Configuration = builder.Build();

			LocalizationOptions = new RequestLocalizationOptions()
			{
				DefaultRequestCulture = new RequestCulture("sv-SE"),
				// These are the cultures the app supports for formatting numbers, dates, etc.
				SupportedCultures = supportedCultures,
				// These are the cultures the app supports for UI strings, i.e. we have localized resources for.
				SupportedUICultures = supportedUICultures
			};
		}

        public void ConfigureServices(IServiceCollection services)
		{
			// Setup localization
			services.AddLocalization((LocalizationOptions options) => options.ResourcesPath = ResourcePath);

			// Add access to generic IConfiguration injection
			services.AddSingleton<IConfiguration>(Configuration);

			// Localization configuration for DI
			services.Configure<RequestLocalizationOptions>((options) =>
			{
				options.DefaultRequestCulture = LocalizationOptions.DefaultRequestCulture;
				options.SupportedCultures = LocalizationOptions.SupportedCultures;
				options.SupportedUICultures = LocalizationOptions.SupportedUICultures;
			});

			// Add application services here!
            //services.AddSingleton(typeof(IStringLocalizerFactory), typeof(ClassLibraryStringLocalizerFactory));
            
			// Enable MVC
			services
                .AddMvc()
                .AddViewLocalization(options => options.ResourcesPath = ResourcePath)
                .AddDataAnnotationsLocalization();
			//			services.AddMvc()
			//				// Add support for finding localized views, based on file name suffix, e.g. Index.fr.cshtml
			//				.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
			//				// Add support for localizing strings in data annotations (e.g. validation messages) via the
			//				// IStringLocalizer abstractions.
			//				.AddDataAnnotationsLocalization();

		}

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
            {
                app.UseDeveloperExceptionPage();

                loggerFactory.AddConsole();

                app.UseRequestLocalization(LocalizationOptions);


                app.UseStaticFiles();

                app.UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");

                    /*routes.MapSpaFallbackRoute(
                        name: "spa-fallback",
                        defaults: new { controller = "Home", action = "Index" });*/
                });
            }
    }
}
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace PracticalAspNetCore
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env, ILoggerFactory logger, IConfiguration configuration)
        {
            //These are three services available at constructor
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddApiVersioning(o => o.ReportApiVersions = true);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory logger, IConfiguration configuration)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class HelloWorldController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get(ApiVersion apiVersion) => Ok(new { Controller = GetType().Name, Version = apiVersion.ToString() });
    }

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return new ContentResult
            {
                Content = @"
                <html>
                    <body>
                    <ul>
                        <li><a href=""/api/v1/helloWorld"">Click here for Version 1</a></li>
                        <li><a href=""/api/v2/helloWorld"">Click here for Version 2</a></li>
                    </ul>
                    </body>
                </html>",
                ContentType = "text/html"
            };
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                    webBuilder.UseStartup<Startup>()
                );
    }
}
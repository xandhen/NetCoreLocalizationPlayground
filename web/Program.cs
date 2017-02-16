using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace WebApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new WebHostBuilder()
				.UseKestrel()
				.UseContentRoot(Directory.GetCurrentDirectory())
				.UseIISIntegration()
				.UseStartup<Startup>()
				.Build();

			host.Run();
        }
    }
}
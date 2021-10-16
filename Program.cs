using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Tweetly_MVC.Init;
using Tweetly_MVC.Tweetly;

namespace Tweetly_MVC
{
    public class Programk
    {
        public static void Main(string[] args)
        {
            Repo.Ins.Liste = new();
            Drivers.KillChromeProcces();
            Drivers.CreateDrivers();
            /* var config = new ConfigurationBuilder()
               .AddCommandLine(args)
               .AddEnvironmentVariables(prefix: "ASPNETCORE_")
               .Build();

             var host = new WebHostBuilder()
                 .UseConfiguration(config)
                 .UseKestrel()
                 .UseContentRoot(Directory.GetCurrentDirectory())
                 .UseIISIntegration()
                 .UseStartup<Startup>()
                 .Build();

             host.Run();*/
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}
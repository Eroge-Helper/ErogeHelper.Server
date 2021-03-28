using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ErogeHelper.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            // json setting, logger etc
            Host.CreateDefaultBuilder(args)
                // begin configures
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // FIXME: Not safe
                    webBuilder.UseUrls("http://*:5000");
                    webBuilder.UseStartup<Startup>();
                });
    }
}

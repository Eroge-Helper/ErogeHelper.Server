using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace ErogeHelper.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://*:5000");
                    // NOTE: Pay attention to the locate of db.sqlite, if set a wrong sqlite position would get `SQLite Error 1: 'no such table:`
                    // XXX: Set content root ensure appsettings.json can be loaded
                    // XXX: 默认目录（注释掉）不知为啥log信息贼详细
                    //var path = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly()?.Location) ?? string.Empty;
                    //webBuilder.UseContentRoot(path + @"/../../../../");
                    webBuilder.UseStartup<Startup>();
                });
    }
}

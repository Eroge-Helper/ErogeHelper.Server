using ErogeHelper.Server.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.IO;
using System;
using System.Diagnostics;
using Microsoft.Data.Sqlite;

namespace ErogeHelper.Server
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
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                });
            });

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                });

            // XXX: ��Ӧѹ��
            services.AddResponseCompression();

            // var builder = new SqliteConnectionStringBuilder();       
            // builder.DataSource = Path.GetFullPath(
            //     Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory") as string ?? AppDomain.CurrentDomain.BaseDirectory,
            //     builder.DataSource));
            // var connectionString = builder.ToString(); // Data Source=/home/ErogeHelper.Server/ErogeHelper.Server/bin/Release/net5.0/publish/
            // FIXME: System.ArgumentNullException: Value cannot be null. (Parameter 'connectionString')
            // ↑ cause appsettings.json hasn't been loaded correctly
            Trace.WriteLine(Configuration.GetConnectionString("MainDatabase"));
            services.AddDbContext<MainDbContext>(options =>
                // options.UseSqlite(Configuration.GetConnectionString("MainDatabase")));
                options.UseSqlite("DataSource=/home/ErogeHelper.Server/ErogeHelper.Server/db.sqlite"));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ErogeHelper.Server", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (true) //env.IsDevelopment())
            {
                // logger.LogInformation("App under development!");
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ErogeHelper.Server v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseResponseCompression();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
            // FIXME: 即使有新的Migrations这个也不工作
            Task.Run(async () => 
            {
                var dbContext = new MainDbContext(new());
                if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
                {
                    logger.LogInformation("Found migration stuffs");
                    await dbContext.Database.MigrateAsync();
                    logger.LogInformation("Migration complete!");
                }
            });
        }
    }
}

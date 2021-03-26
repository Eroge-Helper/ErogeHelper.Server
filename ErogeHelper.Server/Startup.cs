using ErogeHelper.Server.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Linq;

namespace ErogeHelper.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
            // idk if this would be help https://stackoverflow.com/a/63170513/12559031
            Configuration = configuration;
        }

        private readonly IWebHostEnvironment _hostEnvironment;
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

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            // 返回压缩
            services.AddResponseCompression();

            services.AddDbContext<MainDbContext>(options =>
                //options.UseSqlite(Configuration.GetConnectionString("MainDatabase")));
                //options.UseSqlite("DataSource=/home/ErogeHelper.Server/ErogeHelper.Server/db.sqlite"));
                options.UseSqlite($"Data Source={_hostEnvironment.ContentRootPath}/db.sqlite"));


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ErogeHelper.Server", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, ILogger<Startup> logger, MainDbContext dbContext)
        {
            if (_hostEnvironment.IsDevelopment())
            {
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

            logger.LogInformation("Checking migrations...");
            if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
            {
                logger.LogWarning("Found migration stuffs");
                await dbContext.Database.MigrateAsync();
                logger.LogInformation("Migration complete!");
            }
            else
            {
                logger.LogInformation("Database is latest");
            }

            // After migration the db file should exist
            var dbPath = $"{_hostEnvironment.ContentRootPath}/db.sqlite";
            if (!File.Exists(dbPath))
                throw new FileNotFoundException("db.sqlite file not found", dbPath);
        }
    }
}

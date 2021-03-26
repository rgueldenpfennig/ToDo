using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Todo.Api.Bootstrapping;
using Todo.Persistence;

namespace Todo.Api
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
            services.AddPersistence(Configuration);
            services.AddControllers();
            services.AddApiDocumentation();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                EnsureDatabase(app, logger);
                LogConfigurationSettings(Configuration, logger);
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseSerilogRequestLogging();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDo API v1");
                options.RoutePrefix = string.Empty;
            });

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void EnsureDatabase(IApplicationBuilder app, ILogger<Startup> logger)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<TodoDbContext>();
                    var created = context.Database.EnsureCreated();
                    if (created) logger.LogInformation("Database has been created");
                    else logger.LogInformation("Database already exists");

                    logger.LogWarning($"Replace {nameof(EnsureDatabase)} with migrations");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred creating the DB");
                }
            }
        }

        private static void LogConfigurationSettings(IConfiguration configuration, ILogger<Startup> logger)
        {
            var config = ((IConfigurationRoot)configuration).GetDebugView();
            logger.LogDebug("Logging configuration:\n{configuration}", config);
        }
    }
}

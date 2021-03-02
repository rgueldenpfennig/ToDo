using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using Todo.Api.Bootstrapping;
using Todo.Persistence;

namespace Todo.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPersistence(Configuration);
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ToDo API",
                    Description = "API for just another ToDo app",
                    TermsOfService = new Uri("https://github.com/rgueldenpfennig/ToDo"),
                    Contact = new OpenApiContact
                    {
                        Name = "Robin Gueldenpfennig",
                        Email = string.Empty,
                        Url = new Uri("https://twitter.com/gueldenpfennigr"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under MIT License",
                        Url = new Uri("https://github.com/rgueldenpfennig/ToDo/blob/main/LICENSE"),
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                EnsureDatabase(app, logger);
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
    }
}

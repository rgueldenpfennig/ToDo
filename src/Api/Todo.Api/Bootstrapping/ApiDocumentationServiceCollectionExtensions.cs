using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Todo.Api.Bootstrapping
{
    public static class ApiDocumentationServiceCollectionExtensions
    {
        public static IServiceCollection AddApiDocumentation(this IServiceCollection services)
        {
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

            return services;
        }
    }
}

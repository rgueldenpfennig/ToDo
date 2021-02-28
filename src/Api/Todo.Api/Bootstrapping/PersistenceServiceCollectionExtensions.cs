using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Todo.Persistence;

namespace Todo.Api.Bootstrapping
{
    public static class PersistenceServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<TodoDbContext>(builder =>
            {
                builder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                builder.UseNpgsql(
                    configuration.GetConnectionString("TodoDbContext"),
                    options => options.EnableRetryOnFailure());
            });

            return services;
        }
    }
}

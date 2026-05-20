using Microsoft.EntityFrameworkCore;
using Template.Api.Domain.Abstractions;
using Template.Api.Domain.Queries;
using Template.Api.Infrastructure.Data.Queries;

namespace Template.Api.Infrastructure.Data;

public static class AppDbContextExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddAppDbContext(string connectionString)
        {
            services.AddScoped<EventDispatchInterceptor>();

            services.AddDbContext<AppDbContext>((serviceProvider, options) =>
            {
                options.UseNpgsql(connectionString)
                    .AddInterceptors(serviceProvider.GetRequiredService<EventDispatchInterceptor>());
            });


            services.AddScoped<IUnitOfWork>(x => x.GetRequiredService<AppDbContext>());
            services.AddHostedService<AppDbContextMigrationService>();

            services.AddScoped<ICarQueries, CarQueries>();

            return services;
        }
    }
}

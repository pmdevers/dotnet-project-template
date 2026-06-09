namespace Template.Api.Infrastructure;

public static class ServiceCollectionExtensions
{

    extension(IServiceCollection services)
    {
        public IServiceCollection AddWhenNotRegisterd<T>(Action<IServiceCollection> register)
        {
            if (services.Any(x => x.ServiceType == typeof(T)))
            {
                return services;
            }

            register(services);
            return services;
        }
    }
}

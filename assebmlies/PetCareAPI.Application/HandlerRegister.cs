using Microsoft.Extensions.DependencyInjection;

namespace PetCareAPI.Application;

public static class HandlerRegister
{
    public static void RegisterRequestHandlers(this IServiceCollection services)
    {
        services
            .AddMediatR(cf => cf.RegisterServicesFromAssembly(typeof(HandlerRegister).Assembly));
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetCareAPI.Application.Repositories;
using PetCareAPI.Application.Services;
using PetCareAPI.Repository.Persistent;
using PetCareAPI.Repository.Services;

namespace PetCareAPI.Repository.Extensions;

public static class ServiceExtensions
{
    public static void AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPetRepository, PetRepository>();
        services.AddScoped<IOutboxRepository, OutboxRepository>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.AddSingleton<IMessagePublisher, ServiceBusPublisher>();
    }
}
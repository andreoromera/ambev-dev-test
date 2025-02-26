using Ambev.Dev.Test.Application.Services;
using Ambev.Dev.Test.Domain.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.Dev.Test.IoC.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}

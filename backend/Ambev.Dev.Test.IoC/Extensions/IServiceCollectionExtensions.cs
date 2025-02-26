using Ambev.Dev.Test.Application.Services;
using Ambev.Dev.Test.Data.Repositories;
using Ambev.Dev.Test.Domain.Contracts.Repositories;
using Ambev.Dev.Test.Domain.Contracts.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.Dev.Test.IoC.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}

using Ambev.Dev.Test.Application.Services;
using Ambev.Dev.Test.Data;
using Ambev.Dev.Test.Data.Repositories;
using Ambev.Dev.Test.Domain.Configs;
using Ambev.Dev.Test.Domain.Contracts.Repositories;
using Ambev.Dev.Test.Domain.Contracts.Services;
using Ambev.Dev.Test.Domain.Models;
using Ambev.Dev.Test.Domain.Security;
using Ambev.Dev.Test.Domain.Validation;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

namespace Ambev.Dev.Test.IoC.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmployeeService, EmployeeService>();

        services.AddHttpClient();
        services.AddSingleton(new JsonSerializerOptions(JsonSerializerDefaults.Web));

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();

        return services;
    }

    public static IServiceCollection AddConfigs(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var config = serviceProvider.GetService<IConfiguration>();
        
        services.Configure<JwtConfig>(config.GetSection("Jwt"));

        return services;
    }

    public static IServiceCollection AddAuth(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var config = serviceProvider.GetService<IConfiguration>();
        var jwtConfig = serviceProvider.GetService<IOptions<JwtConfig>>().Value;
        var key = Encoding.ASCII.GetBytes(jwtConfig.SecretKey);

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

        services.AddTransient(sp => sp.GetService<IHttpContextAccessor>().HttpContext.User);

        //Configuring default authorization for all endpoints
        services.AddAuthorization(x => x.FallbackPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build());

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var config = serviceProvider.GetService<IConfiguration>();
        
        services.AddDbContext<DefaultContext>(options =>
            options.UseNpgsql(
                config.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("Ambev.Dev.Test.Data")
            ).ConfigureWarnings(warnings => warnings.Log(RelationalEventId.PendingModelChangesWarning))
        );

        return services;
    }

    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services.AddScoped<IValidator<SignInCredentials>, SignInCredentialsValidator>();
        services.AddScoped<IValidator<EmployeeManageModel>, CreateEmployeeModelValidator>();
        services.AddFluentValidationAutoValidation();
        return services;
    }
}

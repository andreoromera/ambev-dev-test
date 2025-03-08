using Ambev.Dev.Test.Domain.Contracts.Services;
using Ambev.Dev.Test.Domain.Security;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ambev.Dev.Test.Domain.Contracts.Repositories;
using Ambev.Dev.Test.Domain.Entities;
using Ambev.Dev.Test.Domain.Exceptions;
using Microsoft.Extensions.Options;
using Ambev.Dev.Test.Domain.Configs;
using Microsoft.Extensions.Logging;

namespace Ambev.Dev.Test.Application.Services;

/// <summary>
/// Authentication Service
/// </summary>
public class AuthService(IOptions<JwtConfig> jwtConfigOptions, IEmployeeRepository employeeRepository, ILogger<AuthService> logger) : IAuthService
{
    private readonly JwtConfig jwtConfigOptions = jwtConfigOptions.Value;

    /// <summary>
    /// Sign the employee in, given the credentials
    /// </summary>
    /// <remarks>
    /// Using BCrypt implementation for stored passwords
    /// </remarks>
    /// <exception cref="CustomException"></exception>
    public async Task<SignInResponse> SignIn(SignInCredentials credentials, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Trying to sign the employee in");

        var employee = await employeeRepository.GetByEmail(credentials.Email, cancellationToken);

        if (employee is not null)
        {
            logger.LogInformation($"Employee found, validating password");
            var verified = BCrypt.Net.BCrypt.Verify(credentials.Password, employee.Password);

            if (verified)
            {
                logger.LogInformation($"Password OK");

                var response = new SignInResponse()
                {
                    Id =  employee.Id,
                    FirstName =  employee.FirstName,
                    LastName =  employee.LastName,
                    Email = employee.Email,
                    Role = employee.Role.ToString(),
                    Token = GenerateToken(employee)
                };

                logger.LogInformation("Logged user data {@response}", response);

                return response;
            }
        }

        logger.LogWarning($"Invalid Credentials");
        throw new CustomException("Invalid Credentials");
    }

    /// <summary>
    /// Generate a token with employee claims
    /// </summary>
    private string GenerateToken(Employee employee)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtConfigOptions.SecretKey);

        var claims = new[]
        {
           new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
           new Claim(ClaimTypes.GivenName, employee.FirstName),
           new Claim(ClaimTypes.Surname, employee.LastName),
           new Claim(ClaimTypes.Email, employee.Email),
           new Claim(ClaimTypes.Role, employee.Role.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(jwtConfigOptions.ExpiresIn),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);

    }
}

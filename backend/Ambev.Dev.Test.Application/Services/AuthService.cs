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

namespace Ambev.Dev.Test.Application.Services;

public class AuthService(IOptions<JwtConfig> jwtConfigOptions, IUserRepository userRepository) : IAuthService
{
    private readonly JwtConfig jwtConfigOptions = jwtConfigOptions.Value;

    public async Task<SignInResponse> SignIn(SignInCredentials credentials, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmail(credentials.Email, cancellationToken);

        if (user is not null)
        {
            var verified = BCrypt.Net.BCrypt.Verify(credentials.Password, user.Password);

            if (verified)
            {
                return new()
                {
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role.ToString(),
                    Token = GenerateToken(user)
                };
            }
        }

        throw new CustomException("Invalid Credentials");
    }

    private string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtConfigOptions.SecretKey);

        var claims = new[]
        {
           new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
           new Claim(ClaimTypes.Name, user.Name),
           new Claim(ClaimTypes.Email, user.Email),
           new Claim(ClaimTypes.Role, user.Role.ToString())
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

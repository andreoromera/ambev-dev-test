using Ambev.Dev.Test.Domain.Auth;
using Ambev.Dev.Test.Domain.Contracts;

namespace Ambev.Dev.Test.Application.Services;

public class AuthService : IAuthService
{
    public async Task<SignInResponse> SignIn(Credentials credentials)
    {
        return new SignInResponse
        {
            Token = "token",
            Name = "José",
            Email = credentials.Email,
            Role = "President"
        };
    }
}

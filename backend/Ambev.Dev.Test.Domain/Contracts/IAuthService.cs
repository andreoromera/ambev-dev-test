using Ambev.Dev.Test.Domain.Auth;

namespace Ambev.Dev.Test.Domain.Contracts;

public interface IAuthService
{
    Task<SignInResponse> SignIn(Credentials credentials);
}

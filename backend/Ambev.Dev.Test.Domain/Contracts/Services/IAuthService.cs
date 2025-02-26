using Ambev.Dev.Test.Domain.Security;

namespace Ambev.Dev.Test.Domain.Contracts.Services;

public interface IAuthService
{
    Task<SignInResponse> SignIn(SignInCredentials credentials, CancellationToken cancellationToken);
}

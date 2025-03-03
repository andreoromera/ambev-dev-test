namespace Ambev.Dev.Test.Domain.Security;

/// <summary>
/// Credentials to sign the employee in
/// </summary>
public class SignInCredentials
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

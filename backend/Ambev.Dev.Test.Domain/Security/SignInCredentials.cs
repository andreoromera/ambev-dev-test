namespace Ambev.Dev.Test.Domain.Security;

/// <summary>
/// Credentials to sign the employee in
/// </summary>
public class SignInCredentials
{
    public string Email { get; set; }
    public string Password { get; set; }
}

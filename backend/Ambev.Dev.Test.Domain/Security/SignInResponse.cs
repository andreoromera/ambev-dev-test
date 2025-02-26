namespace Ambev.Dev.Test.Domain.Security;

public class SignInResponse
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string Token { get; set; }
}

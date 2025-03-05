namespace Ambev.Dev.Test.Domain.Security;

public class SignInResponse
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    public string Email { get; set; }
    public string Role { get; set; }
    public string Token { get; set; }
}

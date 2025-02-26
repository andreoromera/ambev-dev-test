using Ambev.Dev.Test.Domain.Enums;

namespace Ambev.Dev.Test.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
}

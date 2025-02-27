using Ambev.Dev.Test.Domain.Enums;

namespace Ambev.Dev.Test.Domain.Entities;

public class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Document { get; set; }
    public int? SuperiorId { get; set; }
    public Role Role { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime CreationDate { get; set; }

    public ICollection<EmployeePhone> Phones { get; set; } = [];
}

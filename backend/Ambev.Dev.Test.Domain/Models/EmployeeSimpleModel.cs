using Ambev.Dev.Test.Domain.Entities;

namespace Ambev.Dev.Test.Domain.Models;

public class EmployeeSimpleModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    public string Role { get; set; }

    public EmployeeSimpleModel()
    {
    }

    public EmployeeSimpleModel(Employee employee)
    {
        this.Id = employee.Id;
        this.FirstName = employee.FirstName;
        this.LastName = employee.LastName;
        this.Role = employee.Role.ToString();
    }
}

using Ambev.Dev.Test.Domain.Entities;

namespace Ambev.Dev.Test.Domain.Models;

public class EmployeeModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    public string Email { get; set; }
    public string Document { get; set; }
    public string Superior { get; set; }
    public object Role { get; set; }
    public string BirthDate { get; set; }
    public IEnumerable<EmployeePhoneModel> Phones { get; set; } = [];

    public EmployeeModel()
    {
    }

    public EmployeeModel(Employee employee)
    {
        this.Id = employee.Id;
        this.FirstName = employee.FirstName;
        this.LastName = employee.LastName;
        this.Email = employee.Email;
        this.Document = employee.Document;
        this.BirthDate = employee.BirthDate.ToString("yyyy-MM-dd");
        this.Superior = employee.Superior is not null 
            ? $"{employee.Superior.FirstName} {employee.Superior.LastName} ({employee.Superior.Role})"
            : default;
        this.Role = new 
        { 
            Id = (int)employee.Role,
            Name = employee.Role.ToString()
        };
        this.Phones = employee.Phones.Select(p => new EmployeePhoneModel(p));
    }
}

public class EmployeePhoneModel
{
    public int Id { get; set; }
    public string PhonePrefix { get; set; }
    public string PhoneNumber { get; set; }
    public string PhoneType { get; set; }

    public EmployeePhoneModel()
    {
    }

    public EmployeePhoneModel(EmployeePhone phone)
    {
        this.Id = phone.Id;
        this.PhonePrefix = phone.PhonePrefix;
        this.PhoneNumber = phone.PhoneNumber;
        this.PhoneType = phone.PhoneType.ToString();
    }
}

using Ambev.Dev.Test.Domain.Enums;
using Ambev.Dev.Test.Domain.Models;

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
    public Employee Superior { get; set; }

    public ICollection<EmployeePhone> Phones { get; set; } = [];

    public Employee()
    {
    }

    public Employee(CreateEmployeeModel model)
    {
        this.FirstName = model.FirstName;
        this.LastName = model.LastName;
        this.Document = model.Document.Replace("-", "").Replace(".", "").Replace("/", "");
        this.Email = model.Email;
        this.BirthDate = DateTime.Parse(model.BirthDate);
        this.Role = model.ActualRole.Value;
        this.SuperiorId = model.SuperiorId;
        this.CreationDate = DateTime.Now;
        this.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
        this.Phones = model.Phones.Select(phone => new EmployeePhone
        {
            Prefix = phone.Prefix,
            PhoneNumber = phone.PhoneNumber.Replace("-", ""),
            PhoneType = phone.ActualPhoneType,
        }).ToList();

    }
}

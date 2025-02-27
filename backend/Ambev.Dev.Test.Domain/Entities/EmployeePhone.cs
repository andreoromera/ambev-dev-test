using Ambev.Dev.Test.Domain.Enums;

namespace Ambev.Dev.Test.Domain.Entities;

public class EmployeePhone
{
    public int Id { get; set; }
    public string Prefix { get; set; }
    public string PhoneNumber { get; set; }
    public PhoneType PhoneType { get; set; }
    public int EmployeeId { get; set; }

    public Employee Employee { get; set; }
}

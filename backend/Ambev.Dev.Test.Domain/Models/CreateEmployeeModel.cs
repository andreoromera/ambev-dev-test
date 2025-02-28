using Ambev.Dev.Test.Domain.Enums;
using System.Text.Json.Serialization;

namespace Ambev.Dev.Test.Domain.Models;

public class CreateEmployeeModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Document { get; set; }
    public int? SuperiorId { get; set; }
    public string Role { get; set; }
    public string BirthDate { get; set; }
    public IEnumerable<CreateEmployeePhoneModel> Phones { get; set; } = [];

    [JsonIgnore]
    public Role? ActualRole => !string.IsNullOrEmpty(Role)
        ? Enum.Parse<Role>(Role)
        : default;
}

public class CreateEmployeePhoneModel
{
    public string Prefix { get; set; }
    public string PhoneNumber { get; set; }
    public string PhoneType { get; set; }

    [JsonIgnore]
    public PhoneType ActualPhoneType => !string.IsNullOrEmpty(PhoneType)
        ? Enum.Parse<PhoneType>(PhoneType)
        : default;
}

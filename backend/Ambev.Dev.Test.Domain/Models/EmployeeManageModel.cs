using Ambev.Dev.Test.Domain.Enums;
using System.Text.Json.Serialization;

namespace Ambev.Dev.Test.Domain.Models;

public class EmployeeManageModel
{
    public int? Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Document { get; set; }
    public int? SuperiorId { get; set; }
    public Role Role { get; set; }
    public string BirthDate { get; set; }
    public IEnumerable<CreateEmployeePhoneModel> Phones { get; set; } = [];
}

public class CreateEmployeePhoneModel
{
    public string PhonePrefix { get; set; }
    public string PhoneNumber { get; set; }
    public string PhoneType { get; set; }

    [JsonIgnore]
    public PhoneType ActualPhoneType => !string.IsNullOrEmpty(PhoneType)
        ? Enum.Parse<PhoneType>(PhoneType)
        : default;
}

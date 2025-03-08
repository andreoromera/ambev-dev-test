using Ambev.Dev.Test.Domain.Entities;
using Ambev.Dev.Test.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.Dev.Test.Data.Mapping;

public class EmployeePhoneMapping : IEntityTypeConfiguration<EmployeePhone>
{
    public void Configure(EntityTypeBuilder<EmployeePhone> builder)
    {
        builder.ToTable("EmployeePhones");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.PhonePrefix).IsRequired().HasMaxLength(7);
        builder.Property(u => u.PhoneNumber).IsRequired().HasMaxLength(10);
        
        builder.HasData(
            new EmployeePhone
            {
                Id = 1,
                PhonePrefix = "+5511",
                PhoneNumber = "55648899",
                PhoneType = PhoneType.Home,
                EmployeeId = 1,
            },
            new EmployeePhone
            {
                Id = 2,
                PhonePrefix = "+5511",
                PhoneNumber = "984151887",
                PhoneType = PhoneType.CellPhone,
                EmployeeId = 1,
            }
        );
    }
}

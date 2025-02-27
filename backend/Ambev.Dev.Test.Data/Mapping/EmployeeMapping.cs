using Ambev.Dev.Test.Domain.Entities;
using Ambev.Dev.Test.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.Dev.Test.Data.Mapping;

public class EmployeeMapping : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employee");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(u => u.LastName).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Password).IsRequired().HasMaxLength(150);
        builder.Property(u => u.Document).IsRequired().HasMaxLength(20);
        builder.Property(u => u.Role).IsRequired();
        builder.Property(u => u.BirthDate).IsRequired();
        builder.Property(u => u.CreationDate).IsRequired();
        
        builder.HasData(
            new Employee
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@ambev.com.br",
                Password = "$2a$11$E4T2lxYYVNJV4GtIeiv8sebizVWxrTocqL1mOBLYca945lLzafYF2",
                Document = "80560664702",
                Role = Role.Admin,
                BirthDate = new DateTime(1990, 7, 12),
                CreationDate = new DateTime(2025, 2, 27, 13, 17, 8, 105)
            }
        );
    }
}

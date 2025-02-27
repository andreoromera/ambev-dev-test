using Ambev.Dev.Test.Domain.Entities;
using Ambev.Dev.Test.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.Dev.Test.Data.Mapping;

public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Name).IsRequired().HasMaxLength(50);
        builder.Property(u => u.Password).IsRequired().HasMaxLength(150);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Role).IsRequired();
        builder.Property(u => u.CreationDate).IsRequired();
        
        builder.HasData(
            new User
            {
                Id = 1,
                Name = "John Doe",
                Email = "john@ambev.com.br",
                Password = "$2a$11$E4T2lxYYVNJV4GtIeiv8sebizVWxrTocqL1mOBLYca945lLzafYF2",
                Role = Role.Admin,
                CreationDate = new DateTime(2025, 2, 27, 13, 17, 8, 105)
            }
        );
    }
}

using Ambev.Dev.Test.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Ambev.Dev.Test.Data;

public class DefaultContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmployeePhone> EmployeePhones { get; set; }

    public DefaultContext(DbContextOptions<DefaultContext> options) : base(options)
    {
        this.Database.Migrate();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateTime>().HaveColumnType("timestamp without time zone");
        base.ConfigureConventions(configurationBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
#if (DEBUG)
        optionsBuilder
            .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
#endif
        base.OnConfiguring(optionsBuilder);
    }
}

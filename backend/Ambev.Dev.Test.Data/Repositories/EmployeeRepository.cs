using Ambev.Dev.Test.Domain.Contracts.Repositories;
using Ambev.Dev.Test.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ambev.Dev.Test.Data.Repositories;

/// <summary>
/// Employee Repository
/// </summary>
public class EmployeeRepository(DefaultContext context) : IEmployeeRepository
{
    /// <summary>
    /// Get the employee by email
    /// </summary>
    public async Task<Employee> GetByEmail(string email, CancellationToken cancellationToken) => await context
        .Employees
        .FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower(), cancellationToken);
}

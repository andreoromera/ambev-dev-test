using Ambev.Dev.Test.Domain.Entities;

namespace Ambev.Dev.Test.Domain.Contracts.Repositories;

public interface IEmployeeRepository
{
    /// <summary>
    /// Gets de employee by its email
    /// </summary>
    Task<Employee> GetByEmail(string email, CancellationToken cancellationToken);
}

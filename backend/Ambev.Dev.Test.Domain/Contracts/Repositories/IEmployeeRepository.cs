using Ambev.Dev.Test.Domain.Entities;
using System.Linq.Expressions;

namespace Ambev.Dev.Test.Domain.Contracts.Repositories;

public interface IEmployeeRepository
{
    /// <summary>
    /// Does the employee exist?
    /// </summary>
    public Task<bool> Exists(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Search for the employee
    /// </summary>
    Task<List<Employee>> Search(Expression<Func<Employee, bool>> expression, CancellationToken cancellationToken);

    /// <summary>
    /// Gets de employee by its id
    /// </summary>
    Task<Employee> GetById(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Gets de employee by its email
    /// </summary>
    Task<Employee> GetByEmail(string email, CancellationToken cancellationToken);

    /// <summary>
    /// Indicates whether a document is already taken by another employee
    /// </summary>
    Task<bool> IsDocumentAlreadyTaken(string document, CancellationToken cancellationToken);

    /// <summary>
    /// Indicates whether a email is already taken by another employee
    /// </summary>
    Task<bool> IsEmailAlreadyTaken(string email, CancellationToken cancellationToken);
    
    /// <summary>
    /// Creates a new employee
    /// </summary>
    Task Create(Employee employee, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes the employee
    /// </summary>
    public Task Delete(int id, CancellationToken cancellationToken);
    Task<List<Employee>> GetAll(CancellationToken cancellationToken);
}

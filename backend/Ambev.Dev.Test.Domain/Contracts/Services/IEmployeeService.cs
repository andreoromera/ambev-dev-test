using Ambev.Dev.Test.Domain.Models;

namespace Ambev.Dev.Test.Domain.Contracts.Services;

public interface IEmployeeService
{
    /// <summary>
    /// Gets the employee by id
    /// </summary>
    /// <exception cref="CustomException"></exception>
    Task<EmployeeModel> GetById(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the list of all registered employees
    /// </summary>
    Task<List<EmployeeSimpleModel>> GetAll(CancellationToken cancellationToken);

    /// <summary>
    /// Gets a list of filtered employees
    /// </summary>
    Task<List<EmployeeModel>> Search(string firstName, string lastName, CancellationToken cancellationToken);

    /// <summary>
    /// Creates the new employee using the provided employee data
    /// </summary>
    /// <exception cref="CustomException"></exception>
    Task<int> Create(CreateEmployeeModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes the employee with the provided id
    /// </summary>
    /// <exception cref="CustomException"></exception>
    public Task Delete(int id, CancellationToken cancellationToken);
}

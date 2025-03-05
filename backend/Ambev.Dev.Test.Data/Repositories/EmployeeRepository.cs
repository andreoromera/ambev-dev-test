using Ambev.Dev.Test.Domain.Contracts.Repositories;
using Ambev.Dev.Test.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ambev.Dev.Test.Data.Repositories;

/// <summary>
/// Employee Repository
/// </summary>
public class EmployeeRepository(DefaultContext context) : IEmployeeRepository
{
    /// <summary>
    /// Does the employee exist?
    /// </summary>
    public async Task<bool> Exists(int id, CancellationToken cancellationToken) => await context
        .Employees
        .AnyAsync(x => x.Id == id, cancellationToken);

    /// <summary>
    /// Get a list of all employees
    /// </summary>
    public async Task<List<Employee>> GetAll(CancellationToken cancellationToken) => await context
        .Employees
        .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName)
        .ToListAsync(cancellationToken);

    /// <summary>
    /// Search for the employee
    /// </summary>
    public async Task<List<Employee>> Search(Expression<Func<Employee, bool>> expression, CancellationToken cancellationToken) => await context
        .Employees
        .Include(x => x.Superior)
        .Include(x => x.Phones)
        .Where(expression)
        .ToListAsync(cancellationToken);

    /// <summary>
    /// Get the employee by id
    /// </summary>
    public async Task<Employee> GetById(int id, CancellationToken cancellationToken) => await context
        .Employees
        .Include(x => x.Superior)
        .Include(x => x.Phones)
        .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    /// <summary>
    /// Get the employee by email
    /// </summary>
    public async Task<Employee> GetByEmail(string email, CancellationToken cancellationToken) => await context
        .Employees
        .FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower(), cancellationToken);

    /// <summary>
    /// Indicates whether a document is already taken by another employee
    /// </summary>
    public async Task<bool> IsDocumentAlreadyTaken(string document, CancellationToken cancellationToken) => await context
        .Employees
        .AnyAsync(x => x.Document == document, cancellationToken);

    /// <summary>
    /// Indicates whether a email is already taken by another employee
    /// </summary>
    public async Task<bool> IsEmailAlreadyTaken(string email, CancellationToken cancellationToken) => await context
        .Employees
        .AnyAsync(x => x.Email == email, cancellationToken);

    /// <summary>
    /// Creates a new employee
    /// </summary>
    public async Task Create(Employee employee, CancellationToken cancellationToken)
    {
        await context.AddAsync(employee, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Deletes the employee
    /// </summary>
    public async Task Delete(int id, CancellationToken cancellationToken)
    {
        var employee = await context.Employees.FindAsync(id, cancellationToken);
        context.Employees.Remove(employee);
        await context.SaveChangesAsync(cancellationToken);
    }
}

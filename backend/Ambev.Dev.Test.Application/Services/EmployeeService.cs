﻿using Ambev.Dev.Test.Domain.Contracts.Services;
using Ambev.Dev.Test.Domain.Contracts.Repositories;
using Ambev.Dev.Test.Domain.Exceptions;
using Ambev.Dev.Test.Domain.Models;
using System.Security.Claims;
using Ambev.Dev.Test.Domain.Entities;
using LinqKit;

namespace Ambev.Dev.Test.Application.Services;

/// <summary>
/// Employee Service
/// </summary>
public class EmployeeService(ClaimsPrincipal principal, IEmployeeRepository employeeRepository) : IEmployeeService
{
    private readonly int loggedUserId = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier).Value);

    /// <summary>
    /// Gets the list of registered employees
    /// </summary>
    public async Task<List<EmployeeModel>> Search(string firstName, string lastName, CancellationToken cancellationToken)
    {
        var expression = PredicateBuilder.New<Employee>(true);

        if (!string.IsNullOrEmpty(firstName))
            expression.And(x => x.FirstName.ToLower().Contains(firstName.ToLower()));

        if (!string.IsNullOrEmpty(lastName))
            expression.And(x => x.LastName.ToLower().Contains(lastName.ToLower()));

        var employees = await employeeRepository.Search(expression, cancellationToken);

        return employees
            .Select(x => new EmployeeModel(x))
            .ToList();
    }

    /// <summary>
    /// Gets the employee by id
    /// </summary>
    /// <exception cref="CustomException"></exception>
    public async Task<EmployeeModel> GetById(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
            throw new CustomException("Invalid id");

       var employee = await employeeRepository.GetById(id, cancellationToken);

        return employee is null 
            ? throw new CustomException("Employee not found") 
            : new EmployeeModel(employee);
    }

    /// <summary>
    /// Creates the new employee using the provided employee data
    /// </summary>
    /// <exception cref="CustomException"></exception>
    public async Task<int> Create(CreateEmployeeModel model, CancellationToken cancellationToken)
    {
        //Is the informed email being already used by another employee?
        var emailAlreadyTaken = await employeeRepository.IsEmailAlreadyTaken(model.Email.ToLower(), cancellationToken);
        if (emailAlreadyTaken)
            throw new CustomException("There is already an employee using this email. Please provide another valid email");

        //Is the informed document being already used by another employee?
        var documentAlreadyTaken = await employeeRepository.IsDocumentAlreadyTaken(model.Document, cancellationToken);
        if (documentAlreadyTaken)
            throw new CustomException("There is already an employee using this document. Please provide another valid document");

        //Gets the logged in user/employee
        var me = await employeeRepository.GetById(loggedUserId, cancellationToken);

        if (model.ActualRole > me.Role)
            throw new CustomException("You cannot create an employee with a superior role than yours");

        //Maps and creates the employee in database
        var employee = new Employee(model);
        await employeeRepository.Create(employee, cancellationToken);

        return employee.Id;
    }

    /// <summary>
    /// Deletes the employee with the provided id
    /// </summary>
    /// <exception cref="CustomException"></exception>
    public async Task Delete(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
            throw new CustomException("Invalid id");

        if (id == 1)
            throw new CustomException("Superuser cannot be deleted");

        //Employee exists?
        var exists = await employeeRepository.Exists(id, cancellationToken);

        if (!exists)
            throw new CustomException("Employee not found");

        await employeeRepository.Delete(id, cancellationToken);
    }
}

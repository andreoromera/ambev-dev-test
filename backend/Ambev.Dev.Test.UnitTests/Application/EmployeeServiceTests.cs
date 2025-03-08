using Moq;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Ambev.Dev.Test.Domain.Contracts.Repositories;
using Ambev.Dev.Test.Domain.Exceptions;
using Ambev.Dev.Test.Domain.Models;
using Ambev.Dev.Test.Domain.Entities;
using Ambev.Dev.Test.Application.Services;
using LinqKit;
using System.Linq.Expressions;

namespace Ambev.Dev.Test.UnitTests.Application;

public class EmployeeServiceTests
{
    private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
    private readonly Mock<ILogger<EmployeeService>> _loggerMock;
    private readonly ClaimsPrincipal _principal;
    private readonly EmployeeService _employeeService;

    public EmployeeServiceTests()
    {
        _employeeRepositoryMock = new Mock<IEmployeeRepository>();
        _loggerMock = new Mock<ILogger<EmployeeService>>();
        _principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1")
        }));

        _employeeService = new EmployeeService(_principal, _employeeRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnEmployees()
    {
        // Arrange
        var employees = new List<Employee> { new Employee { Id = 1, FirstName = "John", LastName = "Doe" } };
        _employeeRepositoryMock.Setup(repo => repo.GetAll(It.IsAny<CancellationToken>())).ReturnsAsync(employees);

        // Act
        var result = await _employeeService.GetAll(CancellationToken.None);

        // Assert
        Assert.Single(result);
        Assert.Equal("John", result.First().FirstName);
    }

    [Fact]
    public async Task GetAll_ShouldReturnEmptyList_WhenNoEmployees()
    {
        // Arrange
        _employeeRepositoryMock.Setup(repo => repo.GetAll(It.IsAny<CancellationToken>())).ReturnsAsync(new List<Employee>());

        // Act
        var result = await _employeeService.GetAll(CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task Search_ShouldReturnFilteredEmployees_ByFirstName()
    {
        // Arrange
        var employees = new List<Employee> { new() { Id = 1, FirstName = "John", LastName = "Doe" } };
        _employeeRepositoryMock.Setup(repo => repo.Search(It.IsAny<Expression<Func<Employee, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(employees);

        // Act
        var result = await _employeeService.Search("John", null, CancellationToken.None);

        // Assert
        Assert.Single(result);
        Assert.Equal("John", result.First().FirstName);
    }

    [Fact]
    public async Task Search_ShouldReturnFilteredEmployees_ByLastName()
    {
        // Arrange
        var employees = new List<Employee> { new Employee { Id = 1, FirstName = "John", LastName = "Doe" } };
        var expression = PredicateBuilder.New<Employee>(true);
        expression.And(x => x.LastName.ToLower().Contains("Doe".ToLower()));
        _employeeRepositoryMock.Setup(repo => repo.Search(It.IsAny<Expression<Func<Employee, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(employees);

        // Act
        var result = await _employeeService.Search(null, "Doe", CancellationToken.None);

        // Assert
        Assert.Single(result);
        Assert.Equal("Doe", result.First().LastName);
    }

    [Fact]
    public async Task GetById_ShouldReturnEmployee_WhenIdIsValid()
    {
        // Arrange
        var employee = new Employee { Id = 1, FirstName = "John", LastName = "Doe" };
        _employeeRepositoryMock.Setup(repo => repo.GetById(1, It.IsAny<CancellationToken>())).ReturnsAsync(employee);

        // Act
        var result = await _employeeService.GetById(1, CancellationToken.None);

        // Assert
        Assert.Equal("John", result.FirstName);
    }

    [Fact]
    public async Task GetById_ShouldThrowException_WhenIdIsInvalid()
    {
        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _employeeService.GetById(0, CancellationToken.None));
    }

    [Fact]
    public async Task GetById_ShouldThrowException_WhenEmployeeNotFound()
    {
        // Arrange
        _employeeRepositoryMock.Setup(repo => repo.GetById(1, It.IsAny<CancellationToken>())).ReturnsAsync((Employee)null);

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _employeeService.GetById(1, CancellationToken.None));
    }

    [Fact]
    public async Task Create_ShouldThrowException_WhenEmailAlreadyTaken()
    {
        // Arrange
        var model = new EmployeeManageModel { Email = "test@example.com" };
        _employeeRepositoryMock.Setup(repo => repo.IsEmailAlreadyTaken("test@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _employeeService.Create(model, CancellationToken.None));
    }

    [Fact]
    public async Task Create_ShouldThrowException_WhenDocumentAlreadyTaken()
    {
        // Arrange
        var model = new EmployeeManageModel { Email = "test@example.com", Document = "123456" };
        _employeeRepositoryMock.Setup(repo => repo.IsEmailAlreadyTaken("test@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _employeeRepositoryMock.Setup(repo => repo.IsDocumentAlreadyTaken("123456", It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _employeeService.Create(model, CancellationToken.None));
    }

    [Fact]
    public async Task Create_ShouldThrowException_WhenRoleIsSuperior()
    {
        // Arrange
        var model = new EmployeeManageModel { Email = "test@example.com", Document = "123456", Role = Domain.Enums.Role.Admin };
        var me = new Employee { Id = 1, Role = Domain.Enums.Role.President };
        _employeeRepositoryMock.Setup(repo => repo.IsEmailAlreadyTaken("test@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _employeeRepositoryMock.Setup(repo => repo.IsDocumentAlreadyTaken("123456", It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _employeeRepositoryMock.Setup(repo => repo.GetById(1, It.IsAny<CancellationToken>())).ReturnsAsync(me);

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _employeeService.Create(model, CancellationToken.None));
    }

    [Fact]
    public async Task Create_ShouldCreateEmployee_WhenValid()
    {
        // Arrange
        var model = new EmployeeManageModel { Email = "test@example.com", Document = "123456", BirthDate = "1980-01-01", Password = "test", Role = Domain.Enums.Role.President };
        var me = new Employee { Id = 1, Role = Domain.Enums.Role.Admin };
        _employeeRepositoryMock.Setup(repo => repo.IsEmailAlreadyTaken("test@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _employeeRepositoryMock.Setup(repo => repo.IsDocumentAlreadyTaken("123456", It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _employeeRepositoryMock.Setup(repo => repo.GetById(1, It.IsAny<CancellationToken>())).ReturnsAsync(me);
        _employeeRepositoryMock.Setup(repo => repo.Create(It.IsAny<Employee>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // Act
        var result = await _employeeService.Create(model, CancellationToken.None);

        // Assert
        Assert.Equal(0, result); // Assuming the Id is set to 0 initially
    }

    [Fact]
    public async Task Update_ShouldThrowException_WhenEmployeeNotFound()
    {
        // Arrange
        var model = new EmployeeManageModel { Id = 1 };

        _employeeRepositoryMock.Setup(repo => repo.GetById(1, It.IsAny<CancellationToken>())).ReturnsAsync(default(Employee));

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _employeeService.Update(model, CancellationToken.None));
    }

    [Fact]
    public async Task Update_ShouldThrowException_WhenEmailAlreadyTaken()
    {
        // Arrange
        var model = new EmployeeManageModel { Id = 1, Email = "new@example.com" };
        var employee = new Employee { Id = 1, Email = "old@example.com" };

        _employeeRepositoryMock.Setup(repo => repo.GetById(1, It.IsAny<CancellationToken>())).ReturnsAsync(employee);
        _employeeRepositoryMock.Setup(repo => repo.IsEmailAlreadyTaken("new@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _employeeService.Update(model, CancellationToken.None));
    }

    [Fact]
    public async Task Update_ShouldThrowException_WhenDocumentAlreadyTaken()
    {
        // Arrange
        var model = new EmployeeManageModel { Id = 1, Email = "old@example.com", Document = "new-doc" };
        var employee = new Employee { Id = 1, Email = "old@example.com", Document = "old-doc" };

        _employeeRepositoryMock.Setup(repo => repo.GetById(1, It.IsAny<CancellationToken>())).ReturnsAsync(employee);
        _employeeRepositoryMock.Setup(repo => repo.IsDocumentAlreadyTaken("new-doc", It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _employeeService.Update(model, CancellationToken.None));
    }

    [Fact]
    public async Task Update_ShouldThrowException_WhenRoleIsSuperior()
    {
        // Arrange
        var model = new EmployeeManageModel { Id = 1, Email = "old@example.com", Document = "old-doc", Role = Domain.Enums.Role.President };
        var employee = new Employee { Id = 1, Email = "old@example.com", Document = "old-doc", Role = Domain.Enums.Role.Coordinator };
        var me = new Employee { Id = 2, Role = Domain.Enums.Role.Developer };

        _employeeRepositoryMock.Setup(repo => repo.GetById(1, It.IsAny<CancellationToken>())).ReturnsAsync(employee);
        _employeeRepositoryMock.Setup(repo => repo.GetById(2, It.IsAny<CancellationToken>())).ReturnsAsync(me);

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _employeeService.Update(model, CancellationToken.None));
    }

    [Fact]
    public async Task Update_ShouldUpdateEmployee_WhenValid()
    {
        // Arrange
        var model = new EmployeeManageModel { Id = 1, Email = "new@example.com", Document = "new-doc", Role = Domain.Enums.Role.Coordinator };
        var employee = new Employee { Id = 1, Email = "old@example.com", Document = "old-doc", Role = Domain.Enums.Role.Coordinator };
        var me = new Employee { Id = 2, Role = Domain.Enums.Role.Coordinator };

        _employeeRepositoryMock.Setup(repo => repo.GetById(1, It.IsAny<CancellationToken>())).ReturnsAsync(employee);
        _employeeRepositoryMock.Setup(repo => repo.GetById(2, It.IsAny<CancellationToken>())).ReturnsAsync(me);
        _employeeRepositoryMock.Setup(repo => repo.Update(It.IsAny<Employee>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // Act
        await _employeeService.Update(model, CancellationToken.None);

        // Assert
        _employeeRepositoryMock.Verify(repo => repo.Update(It.IsAny<Employee>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Delete_ShouldThrowException_WhenIdIsInvalid()
    {
        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _employeeService.Delete(0, CancellationToken.None));
    }

    [Fact]
    public async Task Delete_ShouldThrowException_WhenEmployeeNotFound()
    {
        // Arrange
        _employeeRepositoryMock.Setup(repo => repo.Exists(1, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _employeeService.Delete(1, CancellationToken.None));
    }

    [Fact]
    public async Task Delete_ShouldDeleteEmployee_WhenValid()
    {
        // Arrange
        _employeeRepositoryMock.Setup(repo => repo.Exists(2, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _employeeRepositoryMock.Setup(repo => repo.Delete(2, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // Act
        await _employeeService.Delete(2, CancellationToken.None);

        // Assert
        _employeeRepositoryMock.Verify(repo => repo.Delete(2, It.IsAny<CancellationToken>()), Times.Once);
    }
}

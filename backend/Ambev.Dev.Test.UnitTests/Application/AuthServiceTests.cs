using Ambev.Dev.Test.Application.Services;
using Ambev.Dev.Test.Domain.Configs;
using Ambev.Dev.Test.Domain.Contracts.Repositories;
using Ambev.Dev.Test.Domain.Entities;
using Ambev.Dev.Test.Domain.Enums;
using Ambev.Dev.Test.Domain.Exceptions;
using Ambev.Dev.Test.Domain.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Ambev.Dev.Test.UnitTests.Application;

public class AuthServiceTests
{
    private readonly Mock<IOptions<JwtConfig>> _mockJwtConfigOptions;
    private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
    private readonly Mock<ILogger<AuthService>> _mockLogger;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _mockJwtConfigOptions = new Mock<IOptions<JwtConfig>>();
        _mockEmployeeRepository = new Mock<IEmployeeRepository>();
        _mockLogger = new Mock<ILogger<AuthService>>();

        var jwtConfig = new JwtConfig { SecretKey = "YourSuperSecretKeyForJwtTokenGenerationThatShouldBeAtLeast32BytesLong", ExpiresIn = 1 };
        _mockJwtConfigOptions.Setup(x => x.Value).Returns(jwtConfig);

        _authService = new AuthService(_mockJwtConfigOptions.Object, _mockEmployeeRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task SignIn_ValidCredentials_ReturnsSignInResponse()
    {
        // Arrange
        var credentials = new SignInCredentials { Email = "test@example.com", Password = "password" };
        var employee = new Employee
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "test@example.com",
            Password = BCrypt.Net.BCrypt.HashPassword("password"),
            Role = Role.Admin
        };

        _mockEmployeeRepository.Setup(x => x.GetByEmail(credentials.Email, It.IsAny<CancellationToken>())).ReturnsAsync(employee);

        // Act
        var result = await _authService.SignIn(credentials, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(employee.Id, result.Id);
        Assert.Equal(employee.FirstName, result.FirstName);
        Assert.Equal(employee.LastName, result.LastName);
        Assert.Equal(employee.Email, result.Email);
        Assert.Equal(employee.Role.ToString(), result.Role);
        Assert.NotNull(result.Token);
    }

    [Fact]
    public async Task SignIn_InvalidCredentials_ThrowsCustomException()
    {
        // Arrange
        var credentials = new SignInCredentials { Email = "test@example.com", Password = "wrongpassword" };
        var employee = new Employee
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "test@example.com",
            Password = BCrypt.Net.BCrypt.HashPassword("password"),
            Role = Role.Admin
        };

        _mockEmployeeRepository.Setup(x => x.GetByEmail(credentials.Email, It.IsAny<CancellationToken>())).ReturnsAsync(employee);

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _authService.SignIn(credentials, CancellationToken.None));
    }

    [Fact]
    public async Task SignIn_NonExistentEmail_ThrowsCustomException()
    {
        // Arrange
        var credentials = new SignInCredentials { Email = "nonexistent@example.com", Password = "password" };

        _mockEmployeeRepository.Setup(x => x.GetByEmail(credentials.Email, It.IsAny<CancellationToken>())).ReturnsAsync((Employee)null);

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _authService.SignIn(credentials, CancellationToken.None));
    }
}

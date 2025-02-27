using Ambev.Dev.Test.Domain.Entities;

namespace Ambev.Dev.Test.Domain.Contracts.Repositories;

public interface IUserRepository
{
    /// <summary>
    /// Gets de user by its email
    /// </summary>
    Task<User> GetByEmail(string email, CancellationToken cancellationToken);
}

using Ambev.Dev.Test.Domain.Entities;

namespace Ambev.Dev.Test.Domain.Contracts.Repositories;

public interface IUserRepository
{
    Task<User> GetByEmail(string email, CancellationToken cancellationToken);
}

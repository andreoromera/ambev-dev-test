using Ambev.Dev.Test.Domain.Contracts.Repositories;
using Ambev.Dev.Test.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ambev.Dev.Test.Data.Repositories;

/// <summary>
/// User Repository
/// </summary>
public class UserRepository(DefaultContext context) : IUserRepository
{
    /// <summary>
    /// Get the user by email
    /// </summary>
    public async Task<User> GetByEmail(string email, CancellationToken cancellationToken) => await context
        .Users
        .FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower(), cancellationToken);
}

using Ambev.Dev.Test.Domain.Contracts.Repositories;
using Ambev.Dev.Test.Domain.Entities;

namespace Ambev.Dev.Test.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        public async Task<User> GetByEmail(string email, CancellationToken cancellationToken)
        {
            return new()
            {
                Id = 1,
                Name = "José",
                Email = email,
                Password = "$2a$11$E4T2lxYYVNJV4GtIeiv8sebizVWxrTocqL1mOBLYca945lLzafYF2",
                Role = Domain.Enums.Role.President
            };
        }
    }
}

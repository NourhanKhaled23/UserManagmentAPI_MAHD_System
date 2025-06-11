using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task UpdateAsync(User user, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken = default);

        // Refresh token methods
        Task StoreRefreshTokenAsync(int userId, string refreshToken, DateTime expiry);
        Task<string?> GetRefreshTokenAsync(int userId);
        Task RevokeRefreshTokenAsync(int userId);
        Task<int> GetUserIdByRefreshTokenAsync(string refreshToken);
    }
}

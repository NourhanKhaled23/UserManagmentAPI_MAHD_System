using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly AppDbContext _context;

        public SettingsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserSettings?> GetByUserIdAsync(int userId) // Fix nullability issue
        {
            return await _context.UserSettings.FirstOrDefaultAsync(s => s.UserId == userId);
        }

        public async Task<bool> AddAsync(UserSettings settings) //Return success/failure
        {
            await _context.UserSettings.AddAsync(settings);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(UserSettings settings) //Return success/failure
        {
            _context.UserSettings.Update(settings);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int userId) // Ensure deletion works correctly
        {
            var settings = await GetByUserIdAsync(userId);
            if (settings == null) return false;

            _context.UserSettings.Remove(settings);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

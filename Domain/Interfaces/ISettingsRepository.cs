using Domain.Entities;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ISettingsRepository
    {
        /// <summary>
        /// Retrieves the settings for a given user by their ID.
        /// Returns null if no settings exist.
        /// </summary>
        Task<UserSettings?> GetByUserIdAsync(int userId); // Prevents nullability warnings

        /// <summary>
        /// Adds new settings for a user.
        /// Returns true if successful.
        /// </summary>
        Task<bool> AddAsync(UserSettings settings); //  Returns a success status

        /// <summary>
        /// Updates the settings for a user.
        /// Returns true if an update occurred.
        /// </summary>
        Task<bool> UpdateAsync(UserSettings settings); // Indicates success/failure

        /// <summary>
        /// Deletes the settings for a user.
        /// Returns true if deletion was successful.
        /// </summary>
        Task<bool> DeleteAsync(int userId); // Avoids silent failures
    }
}

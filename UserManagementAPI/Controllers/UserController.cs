
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.DTOs;

namespace UserManagementAPI.Controllers
{
    [Route("UMS/api/User")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Helper method to safely extract the user ID from the claims.
        /// </summary>
        /// <param name="userId">Parsed user ID if available.</param>
        /// <returns>True if a valid user ID was found; otherwise, false.</returns>
        private bool TryGetUserId(out int userId)
        {
            userId = 0;
            // Use FindFirstValue to get the user ID from claims.
            string? userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Check if it's not null or empty and try parsing it.
            return !string.IsNullOrEmpty(userIdStr) && int.TryParse(userIdStr, out userId);
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
        {
            // Validate and extract the user ID safely.
            if (!TryGetUserId(out int userId))
            {
                return Unauthorized("User ID is missing or invalid.");
            }

            var profile = await _userService.GetUserProfileAsync(userId, cancellationToken);
            return Ok(profile);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            if (!TryGetUserId(out int userId))
            {
                return Unauthorized("User ID is missing or invalid.");
            }

            var result = await _userService.UpdateUserProfileAsync(userId, dto);
            if (!result)
                return BadRequest("Failed to update profile.");
            return Ok(new { message = "Profile updated successfully." });
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            if (!TryGetUserId(out int userId))
            {
                return Unauthorized("User ID is missing or invalid.");
            }

            var result = await _userService.ChangePasswordAsync(userId, dto);
            if (!result)
                return BadRequest("Failed to change password. Check your old password.");
            return Ok(new { message = "Password changed successfully." });
        }

        [HttpDelete("delete-account")]
        public async Task<IActionResult> DeleteAccount()
        {
            if (!TryGetUserId(out int userId))
            {
                return Unauthorized("User ID is missing or invalid.");
            }

            var result = await _userService.DeleteUserAsync(userId);
            if (!result)
                return BadRequest("Failed to delete account.");
            return Ok(new { message = "Account deleted successfully." });
        }
    }
}



using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Utilities.Security;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                return "User already exists. Please log in or use a different email.";
            }

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = PasswordHelper.HashPassword(dto.Password),
                Role = "Student"
            };

            await _userRepository.AddAsync(user);
            return "User registered successfully.";
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null || !PasswordHelper.VerifyPassword(dto.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid credentials. Please check your email or password.");
            }

            var accessToken = JwtHelper.GenerateToken(user.Id, user.Email, user.Role, TimeSpan.FromMinutes(15));
            var refreshToken = GenerateRefreshToken();
            await _userRepository.StoreRefreshTokenAsync(user.Id, refreshToken, DateTime.UtcNow.AddDays(7));

            return new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<LoginResponseDto> RefreshTokenAsync(string refreshToken)
        {
            // Get userId from repository
            var userId = await _userRepository.GetUserIdByRefreshTokenAsync(refreshToken);

            // Verify stored token
            var storedToken = await _userRepository.GetRefreshTokenAsync(userId);
            if (storedToken != refreshToken)
                throw new SecurityTokenException("Invalid refresh token");

            // Revoke all existing tokens for user
            await _userRepository.RevokeRefreshTokenAsync(userId);

            // Fetch user details
            var user = await _userRepository.GetByIdAsync(userId) ?? throw new SecurityTokenException("User not found");

            // Generate new tokens
            var newAccessToken = JwtHelper.GenerateToken(user.Id, user.Email, user.Role, TimeSpan.FromMinutes(15));
            var newRefreshToken = GenerateRefreshToken();
            await _userRepository.StoreRefreshTokenAsync(userId, newRefreshToken, DateTime.UtcNow.AddDays(7));

            return new LoginResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}

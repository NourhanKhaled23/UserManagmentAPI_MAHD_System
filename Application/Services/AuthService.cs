



using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Utilities.Security; // Updated to reference Utilities
using System;
using System.Threading.Tasks;

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
                Role = "Student" // default role
            };

            await _userRepository.AddAsync(user);
            return "User registered successfully.";
        }

        /* public async Task<string> RegisterAsync(RegisterDto dto)
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
                 PasswordHash = Utilities.Security.PasswordHelper.HashPassword(dto.Password),
                 Role = "Student" // default role
             };

             await _userRepository.AddAsync(user);
             return "User registered successfully.";
         }*/
        /*        public async Task<string> LoginAsync(LoginDto dto)
                {
                    var user = await _userRepository.GetByEmailAsync(dto.Email);
                    if (user == null || !PasswordHelper.VerifyPassword(dto.Password, user.PasswordHash))
                    {
                        return "Invalid credentials. Please check your email or password.";
                    }
                    return JwtHelper.GenerateToken(user.Id, user.Email, user.Role);

                }*/
        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);

            if (user == null)
            {
                Console.WriteLine("🔴 Login failed: User not found.");
                return "Invalid credentials. Please check your email or password.";
            }

            bool passwordMatch = PasswordHelper.VerifyPassword(dto.Password, user.PasswordHash);
            if (!passwordMatch)
            {
                Console.WriteLine($"🔴 Login failed: Incorrect password for {dto.Email}.");
                Console.WriteLine($"🔍 Entered: {dto.Password}");
                Console.WriteLine($"🔍 Stored Hash: {user.PasswordHash}");
                return "Invalid credentials. Please check your email or password.";
            }

            Console.WriteLine("✅ Login successful!");
            return JwtHelper.GenerateToken(user.Id, user.Email, user.Role);
        }

    }
}

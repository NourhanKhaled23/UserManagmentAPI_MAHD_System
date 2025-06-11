



using Domain.Entities;
using System;
using System.Linq;
using Utilities.Security;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            try
            {
                // Check if database exists and can connect
                if (context.Database.CanConnect())
                {
                    Console.WriteLine("Database connection successful.");

                    // Apply any pending migrations
                    if (context.Database.GetPendingMigrations().Any())
                    {
                        Console.WriteLine(" Applying pending migrations...");
                        context.Database.Migrate();
                        Console.WriteLine(" Migrations applied successfully.");
                    }
                    else
                    {
                        Console.WriteLine(" Database is up to date.");
                    }
                }
                else
                {
                    Console.WriteLine(" Database doesn't exist. Creating database...");
                    context.Database.EnsureCreated();
                    Console.WriteLine(" Database created successfully.");
                }

                // Seed admin user
                SeedAdminUser(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error initializing database: {ex.Message}");
                Console.WriteLine($" Stack trace: {ex.StackTrace}");

                // Try alternative approach if migration fails
                try
                {
                    Console.WriteLine(" Attempting alternative database creation...");
                    context.Database.EnsureCreated();
                    SeedAdminUser(context);
                }
                catch (Exception fallbackEx)
                {
                    Console.WriteLine($"Fallback also failed: {fallbackEx.Message}");
                }
            }
        }

        private static void SeedAdminUser(AppDbContext context)
        {
            try
            {
                if (!context.Users.Any(u => u.Email == "admin@example.com"))
                {
                    var adminUser = new User
                    {
                        FirstName = "Admin",
                        LastName = "User",
                        Email = "admin@example.com",
                        PasswordHash = PasswordHelper.HashPassword("admin123"),
                        Role = "Admin",
                        CreatedAt = DateTime.UtcNow,
                    };

                    context.Users.Add(adminUser);
                    context.SaveChanges();
                    Console.WriteLine(" Admin user seeded successfully.");
                }
                else
                {
                    Console.WriteLine("Admin user already exists. Skipping seeding.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error seeding admin user: {ex.Message}");
            }
        }
    }
}
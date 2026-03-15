using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace KULESH.UI.Data
{
    public static class DbInit
    {
        public static async Task SeedData(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            // Ensure database is created (in case migrations haven't been applied)
            var db = services.GetService<ApplicationDbContext>();
            if (db != null)
            {
                await db.Database.EnsureCreatedAsync();
            }

            // Create or fix admin user
            var adminEmail = "admin@example.com";
            var normalizedAdminEmail = adminEmail.ToUpperInvariant();
            var adminUsers = await userManager.Users
                .Where(u => u.NormalizedEmail == normalizedAdminEmail)
                .ToListAsync();

            ApplicationUser adminUser = null;
            if (adminUsers.Count == 0)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "admin");
                if (!result.Succeeded)
                {
                    Console.WriteLine("Failed to create admin user: " + string.Join(';', result.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                // If multiple users found, keep the first and remove the rest
                adminUser = adminUsers.First();
                if (adminUsers.Count > 1)
                {
                    for (int i = 1; i < adminUsers.Count; i++)
                    {
                        var du = adminUsers[i];
                        var delResult = await userManager.DeleteAsync(du);
                        if (!delResult.Succeeded)
                        {
                            Console.WriteLine($"Failed to delete duplicate user {du.Id}: " + string.Join(';', delResult.Errors.Select(e => e.Description)));
                        }
                    }
                }

                // Ensure username equals email for login
                if (!string.Equals(adminUser.UserName, adminEmail, StringComparison.OrdinalIgnoreCase))
                {
                    var setNameResult = await userManager.SetUserNameAsync(adminUser, adminEmail);
                    if (!setNameResult.Succeeded)
                    {
                        Console.WriteLine("Failed to set admin username: " + string.Join(';', setNameResult.Errors.Select(e => e.Description)));
                    }
                }

                if (!adminUser.EmailConfirmed)
                {
                    adminUser.EmailConfirmed = true;
                    // Update the user entity
                    var upd = await userManager.UpdateAsync(adminUser);
                    if (!upd.Succeeded)
                    {
                        Console.WriteLine("Failed to confirm admin email: " + string.Join(';', upd.Errors.Select(e => e.Description)));
                    }
                }

                // Reset password to known value 'admin'
                try
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(adminUser);
                    var reset = await userManager.ResetPasswordAsync(adminUser, token, "admin");
                    if (!reset.Succeeded)
                    {
                        Console.WriteLine("Failed to reset admin password: " + string.Join(';', reset.Errors.Select(e => e.Description)));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception while resetting admin password: " + ex.Message);
                }
            }

            // Ensure admin has role claim
            if (adminUser != null)
            {
                var claims = await userManager.GetClaimsAsync(adminUser);
                if (!claims.Any(c => c.Type == "role" && c.Value == "admin"))
                {
                    var addClaimResult = await userManager.AddClaimAsync(adminUser, new Claim("role", "admin"));
                    if (!addClaimResult.Succeeded)
                    {
                        Console.WriteLine("Failed to add admin claim: " + string.Join(';', addClaimResult.Errors.Select(e => e.Description)));
                    }
                }
            }

            // Create a non-admin test user if not exists
            var testEmail = "Kulesh@gmail.com";
            var normalizedTestEmail = testEmail.ToUpperInvariant();
            var testUser = await userManager.Users
                .Where(u => u.NormalizedEmail == normalizedTestEmail)
                .FirstOrDefaultAsync();
            if (testUser == null)
            {
                testUser = new ApplicationUser
                {
                    UserName = testEmail,
                    Email = testEmail,
                    EmailConfirmed = true
                };

                // password is simple for testing purposes
                await userManager.CreateAsync(testUser, "user");
            }
        }
    }
}

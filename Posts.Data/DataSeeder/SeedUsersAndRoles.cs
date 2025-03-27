using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PostsProject.Data.Identity;

namespace Posts.Data.DataSeeder
{
    public static class SeedUsersAndRoles
    {
        public static async Task SeedUsersAndRolesAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

            var roles = new List<string> { "Admin", "Reviewer", "User" };

            // Create roles if they don't exist
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new Role { Name = role });
            }

            // Define users
            var users = new List<(string UserName, string Email, string FullName, string Role)>
            {
                ("Mina" ,"mina@gmail.com", "Mina Bernaba", "Admin"), // for the admin
                ("Mustafa" ,"mostafa@gmail.com", "Mustafa El-Sherbiny", "Reviewer") // for the reviewer
            };

            foreach (var (userName, email, fullName, role) in users)
            {
                // Check if exists before creating
                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    user = new User
                    {
                        UserName = userName,
                        Email = email,
                        FullName = fullName
                    };
                    await userManager.CreateAsync(user, "1");
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }

    }
}

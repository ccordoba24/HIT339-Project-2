using Microsoft.AspNetCore.Identity;

namespace LibraryApp.Data;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var roleManager =
            services.GetRequiredService<RoleManager<IdentityRole>>();

        var userManager =
            services.GetRequiredService<UserManager<IdentityUser>>();

        await EnsureRoleAsync(roleManager, "Admin");
        await EnsureRoleAsync(roleManager, "Receptionist");
        await EnsureRoleAsync(roleManager, "Manager");

        await EnsureUserAsync(
            userManager,
            "admin@library.com",
            "Admin123!",
            "Admin");

        await EnsureUserAsync(
            userManager,
            "reception@library.com",
            "Reception123!",
            "Receptionist");

        await EnsureUserAsync(
            userManager,
            "manager@library.com",
            "Manager123!",
            "Manager");
    }

    private static async Task EnsureRoleAsync(
        RoleManager<IdentityRole> roleManager,
        string roleName)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(
                new IdentityRole(roleName));
        }
    }

    private static async Task EnsureUserAsync(
        UserManager<IdentityUser> userManager,
        string email,
        string password,
        string roleName)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = new IdentityUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(
                user,
                password);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    "; ",
                    result.Errors.Select(error => error.Description));

                throw new InvalidOperationException(errors);
            }
        }

        if (!await userManager.IsInRoleAsync(user, roleName))
        {
            await userManager.AddToRoleAsync(user, roleName);
        }
    }
}

using GameLibrary.Data;
using GameLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Pages.Moderator.Users;

public class IndexModel(ApplicationDbContext context, UserManager<User> userManager) : PageModel
{
    public IList<User> IdentityUsers { get; set; } = [];
    public Dictionary<Guid, string> UserRoles { get; set; } = [];

    public async Task OnGetAsync()
    {
        // Fetch all users
        IdentityUsers = await context.Users.ToListAsync();

        // Fetch roles for each user
        foreach (var user in IdentityUsers)
        {
            var roles = await userManager.GetRolesAsync(user);
            UserRoles[user.Id] = roles.FirstOrDefault() ?? "No Role";
        }
    }
}

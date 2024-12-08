using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GameLibrary.Models;

namespace GameLibrary.Pages.Account.Manage;

public class RoleToolsModel : PageModel
{
    private readonly UserManager<User> _userManager;

    public RoleToolsModel(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public bool IsAdminRole { get; set; }
    public bool IsModeratorRole { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        IsAdminRole = await _userManager.IsInRoleAsync(user, "Administrator");
        IsModeratorRole = await _userManager.IsInRoleAsync(user, "Moderator");

        return Page();
    }
}

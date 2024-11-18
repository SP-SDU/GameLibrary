using GameLibrary.Data;
using GameLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Pages.Admin.Users;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;

    public EditModel(ApplicationDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [BindProperty]
    public User? User { get; set; }

    [BindProperty]
    public string? Password { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        User = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (User == null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (id == null)
        {
            return NotFound();
        }

        var userToUpdate = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (userToUpdate == null)
        {
            return NotFound();
        }

        if (await TryUpdateModelAsync<User>(
            userToUpdate,
            "user",
            u => u.UserName, u => u.Email))
        {
            if (!string.IsNullOrEmpty(Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(userToUpdate);
                var result = await _userManager.ResetPasswordAsync(userToUpdate, token, Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return Page();
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }

        return Page();
    }
}

using GameLibrary.Data;
using GameLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;

namespace GameLibrary.Pages.Admin.Users;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;

    public CreateModel(ApplicationDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IActionResult OnGet()
    {
        User = new User
        {
            UserName = "",
            Email = "",
            CreatedAt = DateTime.UtcNow
        };
        return Page();
    }

    [BindProperty]
    public User? User { get; set; }

    [BindProperty]
    public string? Password { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = new User
        {
            UserName = User!.UserName,
            Email = User.Email,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, Password!);

        if (result.Succeeded)
        {
            return RedirectToPage("./Index");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return Page();
    }
}

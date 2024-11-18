using GameLibrary.Data;
using GameLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameLibrary.Pages.Admin.Roles;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public CreateModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Role Role { get; set; } = new Role();

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Roles.Add(Role);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}

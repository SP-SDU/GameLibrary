using GameLibrary.Data;
using GameLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Pages.Admin.UserLibraries;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public CreateModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        ViewData["Games"] = new SelectList(_context.Games, "Id", "Title");
        ViewData["Users"] = new SelectList(_context.Users, "Id", "UserName");
        return Page();
    }

    [BindProperty]
    public UserLibrary? UserLibrary { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        UserLibrary!.AddedDate = DateTime.UtcNow;
        _context.UserLibraries.Add(UserLibrary);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}

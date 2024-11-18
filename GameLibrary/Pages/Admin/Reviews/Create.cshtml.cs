using GameLibrary.Data;
using GameLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Pages.Admin.Reviews;

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
    public Review? Review { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        Review!.CreatedAt = DateTime.UtcNow;
        _context.Reviews.Add(Review);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}

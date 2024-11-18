using GameLibrary.Data;
using GameLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Pages.Admin.Reviews;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public EditModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Review? Review { get; set; }

    public SelectList Games { get; set; } = null!;
    public SelectList Users { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Review = await _context.Reviews.FirstOrDefaultAsync(m => m.Id == id);

        if (Review == null)
        {
            return NotFound();
        }

        Games = new SelectList(await _context.Games.ToListAsync(), "Id", "Title");
        Users = new SelectList(await _context.Users.ToListAsync(), "Id", "UserName");

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var reviewToUpdate = await _context.Reviews.FindAsync(Review!.Id);

        if (reviewToUpdate == null)
        {
            return NotFound();
        }

        if (await TryUpdateModelAsync<Review>(
            reviewToUpdate,
            "review",
            r => r.GameId, r => r.UserId, r => r.Rating, r => r.Content))
        {
            reviewToUpdate.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }

        return Page();
    }
}

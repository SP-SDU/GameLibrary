using GameLibrary.Data;
using GameLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Pages.Admin.Reviews;

public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DeleteModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Review? Review { get; set; }

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
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Review = await _context.Reviews.FindAsync(id);

        if (Review != null)
        {
            _context.Reviews.Remove(Review);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}

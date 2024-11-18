using GameLibrary.Data;
using GameLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Pages.Admin.UserLibraries;

public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DeleteModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public UserLibrary? UserLibrary { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        UserLibrary = await _context.UserLibraries
            .Include(ul => ul.Game)
            .Include(ul => ul.User)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (UserLibrary == null)
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

        UserLibrary = await _context.UserLibraries.FindAsync(id);

        if (UserLibrary != null)
        {
            _context.UserLibraries.Remove(UserLibrary);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}

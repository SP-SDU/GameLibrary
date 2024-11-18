using GameLibrary.Data;
using GameLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Pages.Admin.UserLibraries;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public EditModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public UserLibrary? UserLibrary { get; set; }

    public SelectList? Games { get; set; }
    public SelectList? Users { get; set; }

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

        Games = new SelectList(_context.Games, "Id", "Title");
        Users = new SelectList(_context.Users, "Id", "UserName");

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Attach(UserLibrary!).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserLibraryExists(UserLibrary!.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private bool UserLibraryExists(Guid id)
    {
        return _context.UserLibraries.Any(e => e.Id == id);
    }
}

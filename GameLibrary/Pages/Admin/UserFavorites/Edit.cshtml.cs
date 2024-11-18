using GameLibrary.Data;
using GameLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Pages.Admin.UserFavorites;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public EditModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public UserFavorite? UserFavorite { get; set; }

    public SelectList? Games { get; set; }
    public SelectList? Users { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        UserFavorite = await _context.UserFavorites
            .Include(uf => uf.Game)
            .Include(uf => uf.User)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (UserFavorite == null)
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

        _context.Attach(UserFavorite!).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserFavoriteExists(UserFavorite!.Id))
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

    private bool UserFavoriteExists(Guid id)
    {
        return _context.UserFavorites.Any(e => e.Id == id);
    }
}

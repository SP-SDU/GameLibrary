using GameLibrary.Data;
using GameLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Pages.Admin.UserFavorites;

public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DeleteModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public UserFavorite? UserFavorite { get; set; }

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
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        UserFavorite = await _context.UserFavorites.FindAsync(id);

        if (UserFavorite != null)
        {
            _context.UserFavorites.Remove(UserFavorite);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}

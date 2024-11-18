using GameLibrary.Data;
using GameLibrary.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Pages.Admin.UserFavorites;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<UserFavorite>? UserFavorites { get; set; }

    public async Task OnGetAsync()
    {
        UserFavorites = await _context.UserFavorites
            .Include(uf => uf.Game)
            .Include(uf => uf.User)
            .ToListAsync();
    }
}

using GameLibrary.Data;
using GameLibrary.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Pages.Admin.UserLibraries;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<UserLibrary>? UserLibraries { get; set; }

    public async Task OnGetAsync()
    {
        UserLibraries = await _context.UserLibraries
            .Include(ul => ul.Game)
            .Include(ul => ul.User)
            .ToListAsync();
    }
}

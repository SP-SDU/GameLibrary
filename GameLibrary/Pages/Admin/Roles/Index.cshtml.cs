using GameLibrary.Data;
using GameLibrary.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Pages.Admin.Roles;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Role>? Roles { get; set; }

    public async Task OnGetAsync()
    {
        Roles = await _context.Roles.ToListAsync();
    }
}

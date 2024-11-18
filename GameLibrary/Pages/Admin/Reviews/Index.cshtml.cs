using GameLibrary.Data;
using GameLibrary.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Pages.Admin.Reviews;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Review>? Reviews { get; set; }

    public async Task OnGetAsync()
    {
        Reviews = await _context.Reviews
            .Include(r => r.Game)
            .Include(r => r.User)
            .ToListAsync();
    }
}

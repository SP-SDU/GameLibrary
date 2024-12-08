using GameLibrary.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Pages.Moderator;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public int TotalReviews { get; set; }
    public int TotalUsers { get; set; }

    public Dictionary<int, int> ReviewRatings { get; set; } = new();

    public async Task OnGetAsync()
    {
        TotalReviews = await _context.Reviews.CountAsync();
        TotalUsers = await _context.Users.CountAsync();

        ReviewRatings = await _context.Reviews
            .GroupBy(r => r.Rating)
            .ToDictionaryAsync(r => r.Key, r => r.Count());
    }
}

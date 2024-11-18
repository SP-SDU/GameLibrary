using GameLibrary.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Pages.Admin;

public class DashboardModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DashboardModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public int TotalGames { get; set; }
    public int TotalReviews { get; set; }
    public int TotalUsers { get; set; }
    public int TotalFavorites { get; set; }

    public Dictionary<string, int> GameGenres { get; set; } = new();
    public Dictionary<string, int> UserActivity { get; set; } = new();
    public Dictionary<int, int> ReviewRatings { get; set; } = new();

    public async Task OnGetAsync()
    {
        TotalGames = await _context.Games.CountAsync();
        TotalReviews = await _context.Reviews.CountAsync();
        TotalUsers = await _context.Users.CountAsync();
        TotalFavorites = await _context.UserFavorites.CountAsync();

        GameGenres = await _context.Games
            .GroupBy(g => g.Genre)
            .ToDictionaryAsync(g => g.Key, g => g.Count());

        UserActivity = await _context.Users
            .GroupBy(u => u.CreatedAt.Date)
            .ToDictionaryAsync(u => u.Key.ToString("yyyy-MM-dd"), u => u.Count());

        ReviewRatings = await _context.Reviews
            .GroupBy(r => r.Rating)
            .ToDictionaryAsync(r => r.Key, r => r.Count());
    }
}

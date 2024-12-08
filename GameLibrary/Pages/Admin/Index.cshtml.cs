// Copyright 2024 Web.Tech. Group17
//
// Licensed under the Apache License, Version 2.0 (the "License"):
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using GameLibrary.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Pages.Admin;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
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

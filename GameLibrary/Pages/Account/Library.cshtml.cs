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
using GameLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Pages.Account;

[Authorize]
public class LibraryModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;

    public LibraryModel(ApplicationDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public class LibraryItemViewModel
    {
        public Game Game { get; set; } = null!;
        public string Status { get; set; } = "";
        public bool IsFavorite { get; set; }
        public int Rating { get; set; }
    }

    public List<LibraryItemViewModel> LibraryItems { get; set; } = [];
    public string Filter { get; set; } = "all";

    public async Task<IActionResult> OnGetAsync(string? filter)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }

        // Default filter
        Filter = filter ?? "all";

        // Get user's library items with related game data
        var libraryItemsQuery = _context.UserLibraries
            .Include(ul => ul.Game)
            .Where(ul => ul.UserId == user.Id);

        // Apply filter if necessary
        if (Filter != "all")
        {
            libraryItemsQuery = libraryItemsQuery.Where(ul => ul.Status == Filter);
        }

        var libraryItems = await libraryItemsQuery.ToListAsync();

        // Get user's favorites
        var favorites = await _context.UserFavorites
            .Where(uf => uf.UserId == user.Id)
            .Select(uf => uf.GameId)
            .ToListAsync();

        // Get user's ratings
        var ratings = await _context.Reviews
            .Where(r => r.UserId == user.Id)
            .GroupBy(r => r.GameId)
            .ToDictionaryAsync(g => g.Key, g => g.OrderByDescending(r => r.CreatedAt).First().Rating);

        // Build view models
        LibraryItems = libraryItems.ConvertAll(li => new LibraryItemViewModel
        {
            Game = li.Game,
            Status = li.Status,
            IsFavorite = favorites.Contains(li.GameId),
            Rating = ratings.GetValueOrDefault(li.GameId)
        });

        return Page();
    }
}

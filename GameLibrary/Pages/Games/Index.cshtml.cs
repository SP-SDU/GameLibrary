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
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Pages.Games;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Game> Games { get; set; } = [];
    public IList<string> Genres { get; set; } = [];
    public string? SearchQuery { get; set; }
    public string? SelectedGenre { get; set; }

    public async Task OnGetAsync(string? search, string? genre)
    {
        SearchQuery = search;
        SelectedGenre = genre;

        // Fetch unique genres
        Genres = await _context.Games
            .Select(g => g.Genre)
            .Distinct()
            .OrderBy(g => g)
            .ToListAsync();

        // Filter games
        var query = _context.Games.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(g => EF.Functions.Like(g.Title, $"%{search}%") ||
                                     EF.Functions.Like(g.Description, $"%{search}%"));
        }

        if (!string.IsNullOrWhiteSpace(genre))
        {
            query = query.Where(g => g.Genre == genre);
        }

        Games = await query
            .OrderByDescending(g => g.Rating)
            .ThenByDescending(g => g.ReleaseDate)
            .ToListAsync();
    }
}

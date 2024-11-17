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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GameLibrary.Data;
using GameLibrary.Models;
using Microsoft.Extensions.Logging;

namespace GameLibrary.Pages.Games;

[Authorize]
public class LibraryModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<LibraryModel> _logger;

    public LibraryModel(ApplicationDbContext context, UserManager<IdentityUser> userManager, ILogger<LibraryModel> logger)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }

    public List<Game> Games { get; set; }
    public Dictionary<Guid, string> LibraryStatuses { get; set; }
    public Dictionary<Guid, bool> FavoriteStatuses { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = _userManager.GetUserId(User);

        var userLibraryGames = await _context.UserLibraries
            .Where(ul => ul.UserId == userId)
            .Include(ul => ul.Game)
            .ToListAsync();

        Games = userLibraryGames.Select(ul => ul.Game).ToList();

        LibraryStatuses = await _context.UserLibraries
            .Where(ul => ul.UserId == userId)
            .ToDictionaryAsync(ul => ul.GameId, ul => ul.Status);

        FavoriteStatuses = await _context.UserFavorites
            .Where(uf => uf.UserId == userId)
            .ToDictionaryAsync(uf => uf.GameId, _ => true);

        return Page();
    }

    public async Task<IActionResult> OnPostUpdateStatusAsync(Guid gameId, string status)
    {
        var userId = _userManager.GetUserId(User);

        var libraryItem = await _context.UserLibraries
            .FirstOrDefaultAsync(ul => ul.GameId == gameId && ul.UserId == userId);

        if (libraryItem != null)
        {
            libraryItem.Status = status;
            await _context.SaveChangesAsync();
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostRemoveFromLibraryAsync(Guid gameId)
    {
        var userId = _userManager.GetUserId(User);

        var libraryItem = await _context.UserLibraries
            .FirstOrDefaultAsync(ul => ul.GameId == gameId && ul.UserId == userId);

        if (libraryItem != null)
        {
            _context.UserLibraries.Remove(libraryItem);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage();
    }
}

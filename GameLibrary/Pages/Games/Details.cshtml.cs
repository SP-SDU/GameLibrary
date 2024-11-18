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

namespace GameLibrary.Pages.Games;

[Authorize]
public class DetailsModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(ApplicationDbContext context, UserManager<User> userManager, ILogger<DetailsModel> logger)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }

    public Game Game { get; set; }
    public bool IsInLibrary { get; set; }
    public bool IsFavorite { get; set; }
    public List<Review> Reviews { get; set; }
    public double AverageRating { get; set; }
    public int? UserRating { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            return NotFound();
        }

        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
        {
            _logger.LogWarning("Invalid UserId: {UserId}", userId);
            return Unauthorized();
        }

        Game = await _context.Games
            .Include(g => g.Reviews)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (Game == null)
        {
            return NotFound();
        }

        IsInLibrary = await _context.UserLibraries
            .AnyAsync(ul => ul.GameId == id && ul.UserId == userGuid);

        IsFavorite = await _context.UserFavorites
            .AnyAsync(uf => uf.GameId == id && uf.UserId == userGuid);

        Reviews = await _context.Reviews
            .Where(r => r.GameId == id)
            .Include(r => r.User)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        var userLibraries = await _context.UserLibraries
            .Where(ul => ul.GameId == id && Reviews.Select(r => r.UserId).Contains(ul.UserId))
            .ToDictionaryAsync(ul => ul.UserId);

        foreach (var review in Reviews)
        {
            if (userLibraries.TryGetValue(review.UserId, out var userLibrary))
            {
                review.UserLibrary = userLibrary;
            }
        }

        if (Reviews.Any())
        {
            AverageRating = Reviews.Average(r => r.Rating);
        }

        var userReview = Reviews.FirstOrDefault(r => r.UserId == userGuid);
        if (userReview != null)
        {
            UserRating = userReview.Rating;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAddToLibraryAsync(Guid id, string status)
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
        {
            _logger.LogWarning("Invalid UserId: {UserId}", userId);
            return Unauthorized();
        }

        var exists = await _context.UserLibraries
            .AnyAsync(ul => ul.GameId == id && ul.UserId == userGuid);

        if (!exists)
        {
            var userLibrary = new UserLibrary
            {
                UserId = userGuid,
                GameId = id,
                AddedDate = DateTime.UtcNow,
                Status = status,
                IsUpcoming = false
            };

            _context.UserLibraries.Add(userLibrary);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostRemoveFromLibraryAsync(Guid id)
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
        {
            _logger.LogWarning("Invalid UserId: {UserId}", userId);
            return Unauthorized();
        }

        var userLibrary = await _context.UserLibraries
            .FirstOrDefaultAsync(ul => ul.GameId == id && ul.UserId == userGuid);

        if (userLibrary != null)
        {
            _context.UserLibraries.Remove(userLibrary);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostAddToFavoritesAsync(Guid id)
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
        {
            _logger.LogWarning("Invalid UserId: {UserId}", userId);
            return Unauthorized();
        }

        var exists = await _context.UserFavorites
            .AnyAsync(uf => uf.GameId == id && uf.UserId == userGuid);

        if (!exists)
        {
            var userFavorite = new UserFavorite
            {
                UserId = userGuid,
                GameId = id
            };

            _context.UserFavorites.Add(userFavorite);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostRemoveFromFavoritesAsync(Guid id)
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
        {
            _logger.LogWarning("Invalid UserId: {UserId}", userId);
            return Unauthorized();
        }

        var userFavorite = await _context.UserFavorites
            .FirstOrDefaultAsync(uf => uf.GameId == id && uf.UserId == userGuid);

        if (userFavorite != null)
        {
            _context.UserFavorites.Remove(userFavorite);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostAddReviewAsync(Guid id, string content, int rating)
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
        {
            _logger.LogWarning("Invalid UserId: {UserId}", userId);
            return Unauthorized();
        }

        var review = new Review
        {
            UserId = userGuid,
            GameId = id,
            Content = content,
            Rating = rating,
            CreatedAt = DateTime.UtcNow
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        // Update game's average rating
        var game = await _context.Games.FindAsync(id);
        if (game != null)
        {
            var ratings = await _context.Reviews
                .Where(r => r.GameId == id)
                .Select(r => r.Rating)
                .ToListAsync();

            game.Rating = ratings.Any() ? ratings.Average() : 0;
            await _context.SaveChangesAsync();
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteReviewAsync(Guid id)
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
        {
            _logger.LogWarning("Invalid UserId: {UserId}", userId);
            return Unauthorized();
        }

        var review = await _context.Reviews
            .FirstOrDefaultAsync(r => r.GameId == id && r.UserId == userGuid);

        if (review != null)
        {
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            // Update game's average rating
            var game = await _context.Games.FindAsync(id);
            if (game != null)
            {
                var ratings = await _context.Reviews
                    .Where(r => r.GameId == id)
                    .Select(r => r.Rating)
                    .ToListAsync();

                game.Rating = ratings.Any() ? ratings.Average() : 0;
                await _context.SaveChangesAsync();
            }
        }

        return RedirectToPage();
    }
}

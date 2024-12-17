using GameLibrary.Data;
using GameLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GameLibrary.Controllers;

[Route("api/games")]
[ApiController]
public class GameDetailsApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public GameDetailsApiController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/games/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetGame(Guid id)
    {
        var game = await _context.Games
            .Include(g => g.Reviews)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (game == null)
        {
            return NotFound();
        }

        return Ok(new
        {
            game.Id,
            game.Title,
            game.Description,
            game.Developer,
            game.Publisher,
            game.ReleaseDate,
            game.ImageUrl,
            game.Rating,
            ReviewCount = game.Reviews.Count
        });
    }

    // GET: api/games/{id}/reviews
    [HttpGet("{id}/reviews")]
    public async Task<IActionResult> GetReviews(Guid id)
    {
        var reviews = await _context.Reviews
            .Include(r => r.User)
            .Where(r => r.GameId == id)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new
            {
                r.Id,
                r.Rating,
                Content = r.Content,
                UserName = r.User.UserName,
                CreatedAt = r.CreatedAt
            })
            .ToListAsync();

        return Ok(reviews);
    }

    // POST: api/games/{id}/reviews
    [HttpPost("{id}/reviews")]
    public async Task<IActionResult> CreateReview(Guid id, [FromBody] ReviewRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        var review = new Review
        {
            GameId = id,
            UserId = Guid.Parse(userId),
            Rating = request.Rating,
            Content = request.Comment,
            CreatedAt = DateTime.UtcNow
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        // Recalculate average rating
        var averageRating = await _context.Reviews
            .Where(r => r.GameId == id)
            .AverageAsync(r => r.Rating);

        var game = await _context.Games.FindAsync(id);
        if (game != null)
        {
            game.Rating = averageRating;
            await _context.SaveChangesAsync();
        }

        // Return the new review with username
        var reviewResponse = await _context.Reviews
            .Include(r => r.User)
            .Where(r => r.Id == review.Id)
            .Select(r => new
            {
                r.Id,
                r.Rating,
                Content = r.Content,
                UserName = r.User.UserName,
                CreatedAt = r.CreatedAt,
                GameRating = averageRating
            })
            .FirstAsync();

        return Ok(reviewResponse);
    }
}

public class ReviewRequest
{
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
}

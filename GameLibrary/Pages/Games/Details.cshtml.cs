using GameLibrary.Data;
using GameLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Pages.Games
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Game? Game { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (_context.Games == null)
            {
                return NotFound();
            }

            var game = await _context.Games.FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }
            else
            {
                Game = game;
            }
            return Page();
        }

        public async Task<IActionResult> OnGetReviewsAsync(int id)
        {
            var reviews = await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.GameId == id)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return Partial("_ReviewsList", reviews);
        }
    }
}

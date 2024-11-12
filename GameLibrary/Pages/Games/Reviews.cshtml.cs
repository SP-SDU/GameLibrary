using GameLibrary.Data;
using GameLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Pages.Games
{
    public class ReviewsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ReviewsModel(ApplicationDbContext context)
        {
            _context = context;
            Reviews = new List<Review>(); // Initialize with an empty list
        }

        public IEnumerable<Review> Reviews { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Reviews = await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.GameId == id)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return Partial("_ReviewsList", Reviews);
        }
    }
}

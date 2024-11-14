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

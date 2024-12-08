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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameLibrary.Pages.Admin.Reviews;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public CreateModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public SelectList Games { get; set; } = null!;
    public SelectList Users { get; set; } = null!;

    [BindProperty]
    public Guid SelectedGameId { get; set; }

    [BindProperty]
    public Guid SelectedUserId { get; set; }

    [BindProperty]
    public int SelectedRating { get; set; }

    [BindProperty]
    public string? SelectedContent { get; set; }

    public IActionResult OnGet()
    {
        Games = new SelectList(_context.Games, "Id", "Title");
        Users = new SelectList(_context.Users, "Id", "UserName");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            // Re-populate lists on validation failure
            Games = new SelectList(_context.Games, "Id", "Title");
            Users = new SelectList(_context.Users, "Id", "UserName");
            return Page();
        }

        var review = new Review
        {
            GameId = SelectedGameId,
            UserId = SelectedUserId,
            Rating = SelectedRating,
            Content = SelectedContent ?? string.Empty,
            CreatedAt = DateTime.UtcNow
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}

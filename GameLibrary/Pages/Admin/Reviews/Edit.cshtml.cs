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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Pages.Admin.Reviews;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public EditModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public SelectList Games { get; set; } = null!;
    public SelectList Users { get; set; } = null!;

    [BindProperty]
    public Guid SelectedId { get; set; }

    [BindProperty]
    public Guid SelectedGameId { get; set; }

    [BindProperty]
    public Guid SelectedUserId { get; set; }

    [BindProperty]
    public int SelectedRating { get; set; }

    [BindProperty]
    public string? SelectedContent { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var review = await _context.Reviews.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
        if (review == null)
        {
            return NotFound();
        }

        // Populate the bindable properties
        SelectedId = review.Id;
        SelectedGameId = review.GameId;
        SelectedUserId = review.UserId;
        SelectedRating = review.Rating;
        SelectedContent = review.Content;

        Games = new SelectList(await _context.Games.ToListAsync(), "Id", "Title");
        Users = new SelectList(await _context.Users.ToListAsync(), "Id", "UserName");

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Games = new SelectList(await _context.Games.ToListAsync(), "Id", "Title");
            Users = new SelectList(await _context.Users.ToListAsync(), "Id", "UserName");
            return Page();
        }

        var reviewToUpdate = await _context.Reviews.FindAsync(SelectedId);
        if (reviewToUpdate == null)
        {
            return NotFound();
        }

        // Update the entity
        reviewToUpdate.GameId = SelectedGameId;
        reviewToUpdate.UserId = SelectedUserId;
        reviewToUpdate.Rating = SelectedRating;
        reviewToUpdate.Content = SelectedContent ?? string.Empty;
        reviewToUpdate.UpdatedAt = DateTime.UtcNow;

        _context.Attach(reviewToUpdate).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ReviewExists(SelectedId))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private bool ReviewExists(Guid id)
    {
        return _context.Reviews.Any(e => e.Id == id);
    }
}

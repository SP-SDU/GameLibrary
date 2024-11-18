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
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Pages.Admin.Reviews;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public EditModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Review? Review { get; set; }

    public SelectList Games { get; set; } = null!;
    public SelectList Users { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Review = await _context.Reviews.FirstOrDefaultAsync(m => m.Id == id);

        if (Review == null)
        {
            return NotFound();
        }

        Games = new SelectList(await _context.Games.ToListAsync(), "Id", "Title");
        Users = new SelectList(await _context.Users.ToListAsync(), "Id", "UserName");

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var reviewToUpdate = await _context.Reviews.FindAsync(Review!.Id);

        if (reviewToUpdate == null)
        {
            return NotFound();
        }

        if (await TryUpdateModelAsync<Review>(
            reviewToUpdate,
            "review",
            r => r.GameId, r => r.UserId, r => r.Rating, r => r.Content))
        {
            reviewToUpdate.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }

        return Page();
    }
}

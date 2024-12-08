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

namespace GameLibrary.Pages.Admin.UserFavorites;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public EditModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public SelectList? Games { get; set; }
    public SelectList? Users { get; set; }

    [BindProperty]
    public Guid SelectedId { get; set; }

    [BindProperty]
    public Guid SelectedUserId { get; set; }

    [BindProperty]
    public Guid SelectedGameId { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var userFavorite = await _context.UserFavorites
            .AsNoTracking() // No need to track here
            .FirstOrDefaultAsync(m => m.Id == id);

        if (userFavorite == null)
        {
            return NotFound();
        }

        // Set the properties for binding back to the form
        SelectedId = userFavorite.Id;
        SelectedUserId = userFavorite.UserId;
        SelectedGameId = userFavorite.GameId;

        Games = new SelectList(_context.Games, "Id", "Title");
        Users = new SelectList(_context.Users, "Id", "UserName");

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            // Re-populate select lists in case of validation failure
            Games = new SelectList(_context.Games, "Id", "Title");
            Users = new SelectList(_context.Users, "Id", "UserName");
            return Page();
        }

        // Retrieve the original entity from the database
        var userFavorite = await _context.UserFavorites.FindAsync(SelectedId);

        if (userFavorite == null)
        {
            return NotFound();
        }

        // Update only the properties that are needed
        userFavorite.UserId = SelectedUserId;
        userFavorite.GameId = SelectedGameId;

        // Mark entity as modified
        _context.Attach(userFavorite).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserFavoriteExists(SelectedId))
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

    private bool UserFavoriteExists(Guid id)
    {
        return _context.UserFavorites.Any(e => e.Id == id);
    }
}

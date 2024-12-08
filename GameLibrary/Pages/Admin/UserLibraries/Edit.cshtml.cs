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

namespace GameLibrary.Pages.Admin.UserLibraries;

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

    [BindProperty]
    public string SelectedStatus { get; set; } = string.Empty;

    [BindProperty]
    public bool SelectedIsUpcoming { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var userLibrary = await _context.UserLibraries.AsNoTracking().FirstOrDefaultAsync(ul => ul.Id == id);
        if (userLibrary == null)
        {
            return NotFound();
        }

        // Populate the bound properties
        SelectedId = userLibrary.Id;
        SelectedUserId = userLibrary.UserId;
        SelectedGameId = userLibrary.GameId;
        SelectedStatus = userLibrary.Status;
        SelectedIsUpcoming = userLibrary.IsUpcoming;

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

        var userLibrary = await _context.UserLibraries.FindAsync(SelectedId);
        if (userLibrary == null)
        {
            return NotFound();
        }

        // Update the entity with the new values
        userLibrary.UserId = SelectedUserId;
        userLibrary.GameId = SelectedGameId;
        userLibrary.Status = SelectedStatus;
        userLibrary.IsUpcoming = SelectedIsUpcoming;

        _context.Attach(userLibrary).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserLibraryExists(SelectedId))
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

    private bool UserLibraryExists(Guid id)
    {
        return _context.UserLibraries.Any(e => e.Id == id);
    }
}

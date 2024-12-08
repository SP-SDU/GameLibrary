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

namespace GameLibrary.Pages.Admin.UserFavorites;

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
    public Guid SelectedUserId { get; set; }

    [BindProperty]
    public Guid SelectedGameId { get; set; }

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
            Games = new SelectList(_context.Games, "Id", "Title");
            Users = new SelectList(_context.Users, "Id", "UserName");
            return Page();
        }

        var userFavorite = new UserFavorite
        {
            UserId = SelectedUserId,
            GameId = SelectedGameId,
            AddedAt = DateTime.UtcNow
        };

        _context.UserFavorites.Add(userFavorite);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}

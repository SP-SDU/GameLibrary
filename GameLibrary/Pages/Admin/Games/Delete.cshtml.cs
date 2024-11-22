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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GameLibrary.Data;
using GameLibrary.Models;

namespace GameLibrary.Pages.Admin.Games;

public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DeleteModel(ApplicationDbContext context, IWebHostEnvironment @object)
    {
        _context = context;
    }

    [BindProperty]
    public Game? Game { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Game = await _context.Games.FirstOrDefaultAsync(m => m.Id == id);

        if (Game == null)
        {
            return NotFound();
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Game = await _context.Games.FindAsync(id);

        if (Game != null)
        {
            if (!string.IsNullOrEmpty(Game.ImageUrl))
            {
                var fileName = Path.GetFileName(Game.ImageUrl);
                var existingFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images",fileName);
                if (System.IO.File.Exists(existingFilePath))
                {
                    System.IO.File.Delete(existingFilePath);
                }
            }

            _context.Games.Remove(Game);
            await _context.SaveChangesAsync();
        }

        else
        {
            return NotFound();
        }

        return RedirectToPage("./Index");
    }
}

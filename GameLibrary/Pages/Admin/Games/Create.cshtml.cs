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
using Microsoft.AspNetCore.Mvc.Rendering;
using GameLibrary.Data;
using GameLibrary.Models;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Cryptography;

namespace GameLibrary.Pages.Admin.Games;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public CreateModel(ApplicationDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public IActionResult OnGet()
    {
        Game = new Game
        {
            Title = "",
            ImageUrl = "",
            Genre = "",
            ReleaseDate = DateTime.Now.Date,
            Description = "",
            Reviews = []
        };
        return Page();
    }

    [BindProperty]
    public Game? Game { get; set; }

    [BindProperty]
    public IFormFile? ImageFile { get; set; }

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync(Guid gameId)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (ImageFile != null)
        {
            // Check file size (max 5MB)
            if (ImageFile.Length > 5 * 1024 * 1024)
            {
                ModelState.AddModelError("ImageFile", "The file is too large.");
                return Page();
            }

            // Check file extension
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(ImageFile.FileName);
            if (!allowedExtensions.Contains(extension.ToLower()))
            {
                ModelState.AddModelError("ImageFile", "The file type is not allowed.");
                return Page();
            }

            var fileName = $"{Game!.Id.ToString()}{extension}";
            var filePath = Path.Combine("wwwroot","images", fileName);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await ImageFile.CopyToAsync(stream);
            }

            Game.ImageUrl = $"/images/{fileName}";
        }

        _context.Games.Add(Game!);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}

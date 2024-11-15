using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameLibrary.Data;
using GameLibrary.Models;
using System.IO;

namespace GameLibrary.Pages.Admin.Games;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public EditModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Game? Game { get; set; }
    public IFormFile? ImageFile { get; set; }

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

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (id == null)
        {
            return NotFound();
        }

        var gameToUpdate = await _context.Games.FindAsync(id);

        if (gameToUpdate == null)
        {
            return NotFound();
        }

        if (await TryUpdateModelAsync<Game>(
            gameToUpdate,
            "game",
            g => g.Title, g => g.Genre, g => g.ReleaseDate, g => g.Description))
        {
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

                var fileName = $"{gameToUpdate.Id}{extension}";
                var filePath = Path.Combine("wwwroot", "images", fileName);

                if (!string.IsNullOrEmpty(gameToUpdate.ImageUrl))
                {
                    var fileImageName = Path.GetFileName(gameToUpdate.ImageUrl);
                    var existingFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileImageName);
                    if (System.IO.File.Exists(existingFilePath))
                    {
                        System.IO.File.Delete(existingFilePath);
                    }
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                gameToUpdate.ImageUrl = $"/images/{fileName}";
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }

        return Page();
    }

    private bool GameExists(Guid id)
    {
        return _context.Games.Any(e => e.Id == id);
    }
}

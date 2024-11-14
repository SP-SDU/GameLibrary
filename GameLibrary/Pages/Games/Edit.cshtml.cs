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

namespace GameLibrary.Pages.Games;

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

        Game = await _context.Games.FindAsync(id);

        // Remove existing image if it exists


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

            if (!string.IsNullOrEmpty(Game.ImageUrl))
            {
                var existingFilePath = Path.Combine("wwwroot", Game.ImageUrl);
                if (System.IO.File.Exists(existingFilePath))
                {
                    System.IO.File.Delete(existingFilePath);
                }
            }

            var fileName = $"{Game.Id}{extension}";
            var filePath = Path.Combine("wwwroot", "images", fileName);



            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await ImageFile.CopyToAsync(stream);
            }

            Game.ImageUrl = $"/images/{fileName}";
        }

        try
        {
            _context.Games.Update(Game!);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!GameExists(Game!.Id))
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

    private bool GameExists(Guid id)
    {
        return _context.Games.Any(e => e.Id == id);
    }
}

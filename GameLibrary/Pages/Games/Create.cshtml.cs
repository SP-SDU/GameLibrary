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

namespace GameLibrary.Pages.Games;

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
            Title = "Sample Game",
            ImageUrl = "Game Image",
            Genre = "Action",
            ReleaseDate = DateTime.Now.ToString("yyyy-MM-dd"),
            Description = "Description of the game",
            Reviews = new List<Review>()
        };
        return Page();
    }

    [BindProperty]
    public Game? Game { get; set; }

    [BindProperty]
    public IFormFile? ImageFile { get; set; }

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
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

            var fileName = $"{Game.Id.ToString()}{extension}";
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

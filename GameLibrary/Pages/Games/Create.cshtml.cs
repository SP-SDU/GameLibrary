using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GameLibrary.Data;
using GameLibrary.Models;

namespace GameLibrary.Pages.Games;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public CreateModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        Game = new Game
        {
            Title = "Sample Game",
            Genre = "Action",
            ReleaseDate = DateTime.Now.ToString("yyyy-MM-dd"),
            Description = "Description of the game",
            Reviews = []
        };
        return Page();
    }

    [BindProperty]
    public Game? Game { get; set; }

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Games.Add(Game!);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}

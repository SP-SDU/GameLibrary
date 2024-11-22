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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GameLibrary.Data;
using GameLibrary.Models;

namespace GameLibrary.Pages.Games;

public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DeleteModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Game? Game { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
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

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Game = await _context.Games.FindAsync(id);

        if (Game != null)
        {
            _context.Games.Remove(Game);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}

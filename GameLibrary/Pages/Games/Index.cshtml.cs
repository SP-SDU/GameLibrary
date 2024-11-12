using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GameLibrary.Data;
using GameLibrary.Models;

namespace GameLibrary.Pages.Games
{
    public class IndexModel : PageModel
    {
        private readonly GameLibrary.Data.ApplicationDbContext _context;

        public IndexModel(GameLibrary.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Game>? Games { get; set; }

        public async Task OnGetAsync()
        {
            Games = await _context.Games.ToListAsync();
        }
    }
}

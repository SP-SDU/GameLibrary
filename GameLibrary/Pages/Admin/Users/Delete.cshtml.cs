using GameLibrary.Data;
using GameLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Pages.Admin.Users;

public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;

    public DeleteModel(ApplicationDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [BindProperty]
    public User? User { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        User = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (User == null)
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

        User = await _userManager.Users.FindAsync(id);

        if (User != null)
        {
            var result = await _userManager.DeleteAsync(User);

            if (result.Succeeded)
            {
                return RedirectToPage("./Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return Page();
    }
}

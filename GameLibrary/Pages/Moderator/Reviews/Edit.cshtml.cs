using GameLibrary.Data;
using GameLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Pages.Moderator.Reviews;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public EditModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public SelectList Games { get; set; } = null!;
    public SelectList Users { get; set; } = null!;

    [BindProperty]
    public Guid SelectedId { get; set; }

    [BindProperty]
    public Guid SelectedGameId { get; set; }

    [BindProperty]
    public Guid SelectedUserId { get; set; }

    [BindProperty]
    public int SelectedRating { get; set; }

    [BindProperty]
    public string? SelectedContent { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var review = await _context.Reviews.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
        if (review == null)
        {
            return NotFound();
        }

        // Populate the bindable properties
        SelectedId = review.Id;
        SelectedGameId = review.GameId;
        SelectedUserId = review.UserId;
        SelectedRating = review.Rating;
        SelectedContent = review.Content;

        Games = new SelectList(await _context.Games.ToListAsync(), "Id", "Title");
        Users = new SelectList(await _context.Users.ToListAsync(), "Id", "UserName");

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string submitButton)
    {
        if (!ModelState.IsValid)
        {
            Games = new SelectList(await _context.Games.ToListAsync(), "Id", "Title");
            Users = new SelectList(await _context.Users.ToListAsync(), "Id", "UserName");
            return Page();
        }

        var reviewToUpdate = await _context.Reviews.FindAsync(SelectedId);
        if (reviewToUpdate == null)
        {
            return NotFound();
        }

        if (submitButton == "Approve")
        {
            reviewToUpdate.Status = "Approved";
        }
        else if (submitButton == "Deny")
        {
            reviewToUpdate.Status = "Denied";
        }

        reviewToUpdate.GameId = SelectedGameId;
        reviewToUpdate.UserId = SelectedUserId;
        reviewToUpdate.Rating = SelectedRating;
        reviewToUpdate.Content = SelectedContent ?? string.Empty;
        reviewToUpdate.UpdatedAt = DateTime.UtcNow;

        _context.Attach(reviewToUpdate).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ReviewExists(SelectedId))
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

    private bool ReviewExists(Guid id)
    {
        return _context.Reviews.Any(e => e.Id == id);
    }
}

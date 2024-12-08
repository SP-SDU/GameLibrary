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

    public static Dictionary<Guid, string> ReviewStatuses { get; set; } = [];

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

        SelectedId = review.Id;
        SelectedGameId = review.GameId;
        SelectedUserId = review.UserId;
        SelectedRating = review.Rating;
        SelectedContent = review.Content;

        Games = new SelectList(await _context.Games.ToListAsync(), "Id", "Title");
        Users = new SelectList(await _context.Users.ToListAsync(), "Id", "UserName");

        // Retrieve status from the dictionary, if exists
        if (ReviewStatuses.TryGetValue(SelectedId, out var status))
        {
            ViewData["Status"] = status;
        }
        else
        {
            ViewData["Status"] = "Pending"; // Default status
        }

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

        // Update the status in the dictionary
        if (submitButton == "Approve")
        {
            ReviewStatuses[SelectedId] = "Approved";

            // Update review details
            reviewToUpdate.GameId = SelectedGameId;
            reviewToUpdate.UserId = SelectedUserId;
            reviewToUpdate.Rating = SelectedRating;
            reviewToUpdate.Content = SelectedContent ?? string.Empty;
            reviewToUpdate.UpdatedAt = DateTime.UtcNow;

            _context.Attach(reviewToUpdate).State = EntityState.Modified;
        }
        else if (submitButton == "Deny")
        {
            ReviewStatuses[SelectedId] = "Denied";

            _context.Reviews.Remove(reviewToUpdate);
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!ReviewExists(SelectedId))
        {
            return NotFound();
        }

        return RedirectToPage("./Index");
    }

    private bool ReviewExists(Guid id)
    {
        return _context.Reviews.Any(e => e.Id == id);
    }
}

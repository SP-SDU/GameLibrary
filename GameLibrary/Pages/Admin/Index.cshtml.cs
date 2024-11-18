using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameLibrary.Pages.Admin;

public class IndexModel : PageModel
{
    public List<string> NavigationLinks { get; set; } = new();

    public void OnGet()
    {
        NavigationLinks.Add("Dashboard");
        NavigationLinks.Add("Games");
        NavigationLinks.Add("Reviews");
        NavigationLinks.Add("User Libraries");
        NavigationLinks.Add("User Favorites");
        NavigationLinks.Add("Users");
        NavigationLinks.Add("Roles");
    }

    public IActionResult OnPostNavigate(string link)
    {
        return link switch
        {
            "Dashboard" => RedirectToPage("Dashboard"),
            "Games" => RedirectToPage("Games/Index"),
            "Reviews" => RedirectToPage("Reviews/Index"),
            "User Libraries" => RedirectToPage("UserLibraries/Index"),
            "User Favorites" => RedirectToPage("UserFavorites/Index"),
            "Users" => RedirectToPage("Users/Index"),
            "Roles" => RedirectToPage("Roles/Index"),
            _ => Page()
        };
    }
}

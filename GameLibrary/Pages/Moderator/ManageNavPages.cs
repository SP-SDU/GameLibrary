using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameLibrary.Pages.Moderator;

public static class ManageNavPages
{
    public static string Index => "Index";
    public static string Reviews => "Reviews/Index";
    public static string Users => "Users/Index";

    public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);
    public static string ReviewsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Reviews);
    public static string UsersNavClass(ViewContext viewContext) => PageNavClass(viewContext, Users);

    public static string PageNavClass(ViewContext viewContext, string page)
    {
        var activePage = viewContext.ViewData["ActivePage"] as string
            ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
        return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null!;
    }
}

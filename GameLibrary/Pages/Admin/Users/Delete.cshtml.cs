// Copyright 2024 Web.Tech. Group17
//
// Licensed under the Apache License, Version 2.0 (the "License"):
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using GameLibrary.Data;
using GameLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Pages.Admin.Users;

public class DeleteModel(UserManager<User> userManager) : PageModel
{
    [BindProperty]
    public User? IdentityUser { get; set; }

    public string? UserRole { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        IdentityUser = await userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (IdentityUser == null)
        {
            return NotFound();
        }

        var roles = await userManager.GetRolesAsync(IdentityUser);
        UserRole = roles.FirstOrDefault() ?? "No Role";

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        IdentityUser = await userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (IdentityUser != null)
        {
            var result = await userManager.DeleteAsync(IdentityUser);

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

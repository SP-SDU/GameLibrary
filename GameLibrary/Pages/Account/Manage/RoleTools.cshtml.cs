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

using GameLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameLibrary.Pages.Account.Manage;

public class RoleToolsModel : PageModel
{
    private readonly UserManager<User> _userManager;

    public RoleToolsModel(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public bool IsAdminRole { get; set; }
    public bool IsModeratorRole { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        IsAdminRole = await _userManager.IsInRoleAsync(user, "Administrator");
        IsModeratorRole = await _userManager.IsInRoleAsync(user, "Moderator");

        return Page();
    }
}

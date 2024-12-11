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
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Pages.Moderator.Users;

public class IndexModel(ApplicationDbContext context, UserManager<User> userManager) : PageModel
{
    public IList<User> IdentityUsers { get; set; } = [];
    public Dictionary<Guid, string> UserRoles { get; set; } = [];

    public async Task OnGetAsync()
    {
        // Fetch all users
        IdentityUsers = await context.Users.ToListAsync();

        // Fetch roles for each user
        foreach (var user in IdentityUsers)
        {
            var roles = await userManager.GetRolesAsync(user);
            UserRoles[user.Id] = roles.FirstOrDefault() ?? "No Role";
        }
    }
}

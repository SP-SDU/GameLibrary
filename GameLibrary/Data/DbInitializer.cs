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
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Data;

public static class DbInitializer
{
    /// <summary> Seeding the database. </summary>
    public static void Initialize(ApplicationDbContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        // Apply any pending migrations
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }

        // Check if we already have games
        if (context.Games.Any())
        {
            return; // DB has been seeded
        }

        // Ensure roles are created
        var roles = new[] { "Administrator", "User" };
        foreach (var role in roles)
        {
            if (!roleManager.RoleExistsAsync(role).Result)
            {
                roleManager.CreateAsync(new Role { Name = role }).Wait();
            }
        }

        var user = new User
        {
            UserName = "admin@example.com",
            Email = "admin@example.com"
        };

        if (userManager.FindByNameAsync(user.UserName).Result == null)
        {
            var result = userManager.CreateAsync(user, "Password123!").Result;
            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(user, "Administrator").Wait();
            }
        }

        context.SaveChanges();

        // Add test games
        var games = new Game[]
        {
            new() {
                Title = "The Legend of Zelda: Breath of the Wild",
                Description = "Step into a world of discovery, exploration, and adventure in The Legend of Zelda: Breath of the Wild. Travel across vast fields, through forests, and to mountain peaks as you discover what has become of the kingdom of Hyrule in this stunning Open-Air Adventure.",
                Genre = "Action-Adventure",
                ReleaseDate = new DateTime(2017, 3, 3, 0, 0, 0, DateTimeKind.Utc),
                Developer = "Nintendo EPD",
                Publisher = "Nintendo"
            }
        };

        context.Games.AddRange(games);
        context.SaveChanges();

        // Add test reviews with validation
        var reviews = new Review[]
        {
            new() {
                GameId = games[0].Id,
                UserId = user.Id, // Use the inherited Id property from IdentityUser<Guid>
                Rating = 5,
                Comment = "One of the best games I've ever played! The open world is breathtaking and there's so much to discover.",
                CreatedAt = DateTime.UtcNow.AddDays(-5)
            },
            new() {
                GameId = games[0].Id,
                UserId = user.Id, // Use the inherited Id property from IdentityUser<Guid>
                Rating = 5,
                Comment = "An absolute masterpiece. The attention to detail and storytelling are unmatched.",
                CreatedAt = DateTime.UtcNow.AddDays(-3)
            },
            new() {
                GameId = games[0].Id,
                UserId = user.Id, // Use the inherited Id property from IdentityUser<Guid>
                Rating = 4,
                Comment = "A visually stunning game with a deep story, though it had some bugs at launch.",
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            }
        };

        context.Reviews.AddRange(reviews);
        context.SaveChanges();
    }
}

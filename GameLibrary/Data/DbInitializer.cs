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
    /// <summary> Applies pending migrations and seeds the database with initial data. </summary>
    public static void Initialize(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();

        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<Role>>();

            // Apply pending migrations
            if (context.Database.GetPendingMigrations().Any())
            {
                logger.LogInformation("Applying pending migrations...");
                context.Database.Migrate();
                logger.LogInformation("Migrations applied successfully.");
            }

            // Seed roles and users
            SeedRoles(roleManager, logger);
            SeedUser(userManager, "admin@example.com", "Administrator", logger);
            SeedUser(userManager, "moderator@example.com", "Moderator", logger);

            // Seed games
            if (context.Games.Any())
            {
                logger.LogInformation("Games already exist, skipping seeding.");
                return;
            }
            var games = GetInitialGames();
            context.Games.AddRange(games);
            _ = context.SaveChanges();

            // Seed reviews
            var reviews = GetInitialReviews(games, userManager);
            context.Reviews.AddRange(reviews);
            _ = context.SaveChanges();
            logger.LogInformation("Seeding completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during migration or seeding.");
        }
    }

    private static void SeedRoles(RoleManager<Role> roleManager, ILogger logger)
    {
        var roles = new[] { "Administrator", "User", "Moderator" };
        foreach (var role in roles)
        {
            if (!roleManager.RoleExistsAsync(role).Result)
            {
                var result = roleManager.CreateAsync(new Role { Name = role }).Result;
                if (result.Succeeded)
                {
                    logger.LogInformation("Role '{Role}' created successfully.", role);
                }
                else
                {
                    logger.LogWarning("Failed to create role '{Role}'. Errors: {Errors}", role, string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                logger.LogInformation("Role '{Role}' already exists, skipping creation.", role);
            }
        }
    }

    private static void SeedUser(UserManager<User> userManager, string email, string role, ILogger logger)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };

        var existingUser = userManager.FindByNameAsync(email).Result;
        if (existingUser == null)
        {
            var result = userManager.CreateAsync(user, "Password123!").Result;
            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(user, role).Wait();
                logger.LogInformation("User '{Email}' created and assigned to role '{Role}'.", email, role);
            }
            else
            {
                logger.LogWarning("Failed to create user '{Email}'. Errors: {Errors}", email, string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
        else
        {
            logger.LogInformation("User '{Email}' already exists, skipping creation.", email);
        }
    }

    private static Game[] GetInitialGames()
    {
        return
        [
            new()
            {
                Title = "Echoes of Ardentia",
                Description = "Embark on a journey through the mystical realm of Ardentia, a land fractured by ancient conflicts and filled with secrets waiting to be uncovered. As the last Sentinel of the Aetherial Order, you must harness elemental powers, forge alliances, and restore harmony to a world on the brink of collapse. Explore lush forests, towering mountains, and forgotten ruins in this immersive Open-World Adventure.",
                Genre = "Action-Adventure",
                ReleaseDate = new DateTime(2024, 5, 15, 0, 0, 0, DateTimeKind.Utc),
                Developer = "Ardent Studios",
                Publisher = "Mythos Interactive",
                ImageUrl = "/images/Ardentia.webp"
            },
            new()
            {
                Title = "Shardbound Chronicles: Lumina's Quest",
                Description = "Dive into a fantastical world of floating islands and ancient magic. As Lumina, a rogue spellweaver, harness powerful shards to bend reality, solve intricate puzzles, and battle the encroaching Void. Discover the secrets of the Shardbound and unite warring factions in this epic Action-Adventure.",
                Genre = "Action-Adventure",
                ReleaseDate = new DateTime(2023, 10, 15, 0, 0, 0, DateTimeKind.Utc),
                Developer = "Ethereal Games",
                Publisher = "Skyforge Interactive",
                ImageUrl = "/images/Lumina.webp"
            },
            new()
            {
                Title = "Iron Horizon: Shadows of Steel",
                Description = "In a dystopian future dominated by warring megacorporations, pilot a customizable mech and lead your resistance against the tyrannical forces of Dominion Core. Engage in tactical battles, upgrade your arsenal, and forge alliances in this thrilling Mech-Combat RPG.",
                Genre = "RPG",
                ReleaseDate = new DateTime(2024, 6, 10, 0, 0, 0, DateTimeKind.Utc),
                Developer = "Titan Forge Studios",
                Publisher = "NovaCore Media",
                ImageUrl = "/images/Iron.webp"
            },
            new()
            {
                Title = "Eclipse Strikers: Nova League",
                Description = "Join the high-octane world of Nova League, where futuristic vehicles equipped with plasma thrusters and EMP cannons compete in gravity-defying arenas. Master unique vehicle classes and climb the ranks in this multiplayer Sports-Action game.",
                Genre = "Sports-Action",
                ReleaseDate = new DateTime(2023, 12, 5, 0, 0, 0, DateTimeKind.Utc),
                Developer = "Luminex Entertainment",
                Publisher = "Flashpoint Interactive",
                ImageUrl = "/images/Eclipse.webp"
            },
            new()
            {
                Title = "Phantom Nexus: Aether Drift",
                Description = "Explore the interconnected dimensions of the Nexus, a surreal realm filled with cryptic entities and deadly foes. As a Spectral Wanderer, master your shifting form to uncover hidden pathways, outwit enemies, and solve intricate puzzles in this atmospheric Metroidvania.",
                Genre = "Metroidvania",
                ReleaseDate = new DateTime(2024, 3, 22, 0, 0, 0, DateTimeKind.Utc),
                Developer = "Obsidian Path",
                Publisher = "Midnight Pixel Studios",
                ImageUrl = "/images/Phantom.webp"
            },
            new()
            {
                Title = "Solarforge: Legends of Astralis",
                Description = "Venture across the celestial world of Astralis, where floating continents are bound together by ancient magic. Gather resources, craft powerful weapons, and build your skyborne fortress to defend against the ever-looming threat of the Eclipse Beasts in this Crafting Survival RPG.",
                Genre = "Survival RPG",
                ReleaseDate = new DateTime(2024, 8, 30, 0, 0, 0, DateTimeKind.Utc),
                Developer = "Stellar Foundry",
                Publisher = "Aetherworks Publishing",
                ImageUrl = "/images/Solarforge.webp"
            },
            new()
            {
                Title = "Radiant Streets: Neon Rebellion",
                Description = "Rule the night in a sprawling cyberpunk city as a charismatic rogue. Assemble your crew, hack into corporate networks, and engage in adrenaline-pumping street battles. Every choice you make shapes the fate of Neon City in this gritty Open-World RPG.",
                Genre = "Open-World RPG",
                ReleaseDate = new DateTime(2024, 11, 18, 0, 0, 0, DateTimeKind.Utc),
                Developer = "Pulsewave Studios",
                Publisher = "NeonArc Media",
                ImageUrl = "/images/Radiant.webp"
            },
            new()
            {
                Title = "Mystic Tides: The Forgotten Depths",
                Description = "Set sail on a sprawling oceanic world, brimming with uncharted islands, hidden treasures, and mythical sea creatures. Customize your ship, form alliances with island clans, and uncover the secrets of the Abyssal Ruins in this Nautical Exploration Adventure.",
                Genre = "Exploration Adventure",
                ReleaseDate = new DateTime(2025, 2, 12, 0, 0, 0, DateTimeKind.Utc),
                Developer = "Azure Horizon",
                Publisher = "Deep Current Studios",
                ImageUrl = "/images/Mystic.webp"
            },
            new()
            {
                Title = "Starlight Vanguard: Infinite Horizons",
                Description = "Command your own starship and explore a galaxy teeming with alien factions, ancient ruins, and cosmic mysteries. Customize your crew, negotiate interstellar alliances, and engage in epic space battles in this Open-World Space RPG.",
                Genre = "Space RPG",
                ReleaseDate = new DateTime(2025, 9, 3, 0, 0, 0, DateTimeKind.Utc),
                Developer = "Galactic Edge Studios",
                Publisher = "Stardust Interactive",
                ImageUrl = "/images/Starlight.webp"
            },
            new()
            {
                Title = "Dreadspire: Nightfall's Curse",
                Description = "Enter the cursed lands of Dreadspire, a sprawling gothic world plagued by eternal darkness. Master arcane abilities, vanquish horrific foes, and unravel the mysteries of the Bloodforged Covenant in this Atmospheric Action RPG.",
                Genre = "Action RPG",
                ReleaseDate = new DateTime(2024, 10, 31, 0, 0, 0, DateTimeKind.Utc),
                Developer = "Darkspire Games",
                Publisher = "Phantom Eclipse Media",
                ImageUrl = "/images/Dreadspire.webp"
            },
            new()
            {
                Title = "Pixel Gauntlet: Heroes of the Void",
                Description = "Step into the retro-styled world of the Pixel Gauntlet, where you and up to three friends take on chaotic, procedurally generated dungeons. Unlock unique pixel-art heroes, each with game-changing abilities, and battle your way to the heart of the Void.",
                Genre = "Co-op Dungeon Crawler",
                ReleaseDate = new DateTime(2023, 11, 25, 0, 0, 0, DateTimeKind.Utc),
                Developer = "Bitforge Entertainment",
                Publisher = "Arcade Blast Studios",
                ImageUrl = "/images/Pixel.webp"
            }
        ];
    }

    private static Review[] GetInitialReviews(Game[] games, UserManager<User> userManager)
    {
        var adminUser = userManager.FindByNameAsync("admin@example.com").Result;
        if (adminUser == null) return [];

        return
        [
            new()
            {
                GameId = games[0].Id,
                UserId = adminUser.Id,
                Rating = 5,
                Content = "One of the best games I've ever played! The open world is breathtaking and there's so much to discover.",
                CreatedAt = DateTime.UtcNow.AddDays(-5)
            },
            new()
            {
                GameId = games[0].Id,
                UserId = adminUser.Id,
                Rating = 5,
                Content = "An absolute masterpiece. The attention to detail and storytelling are unmatched.",
                CreatedAt = DateTime.UtcNow.AddDays(-3)
            },
            new()
            {
                GameId = games[0].Id,
                UserId = adminUser.Id,
                Rating = 4,
                Content = "A visually stunning game with a deep story, though it had some bugs at launch.",
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            }
        ];
    }
}

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
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace GameLibrary.Data;

public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        try
        {
            // Ensure database is created
            context.Database.EnsureCreated();

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

            // Add test user with proper password hashing
            var user = new User
            {
                Username = "testuser",
                Email = "test@example.com",
                // Use higher work factor for better security
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123", workFactor: 12),
                IsAdmin = false,
                CreatedAt = DateTime.UtcNow
            };

            context.Users.Add(user);
            context.SaveChanges(); // Save to get the user ID

            // Add test games
            var games = new Game[]
            {
                new Game
                {
                    Title = "The Legend of Zelda: Breath of the Wild",
                    Description = "Step into a world of discovery, exploration, and adventure in The Legend of Zelda: Breath of the Wild. Travel across vast fields, through forests, and to mountain peaks as you discover what has become of the kingdom of Hyrule in this stunning Open-Air Adventure.",
                    Genre = "Action-Adventure",
                    ReleaseDate = new DateTime(2017, 3, 3),
                    Developer = "Nintendo EPD",
                    Publisher = "Nintendo",
                    ImageUrl = "https://assets.nintendo.com/image/upload/c_pad,f_auto,h_613,q_auto,w_1089/ncom/en_US/games/switch/t/the-legend-of-zelda-breath-of-the-wild-switch/hero?v=2021120201",
                    Rating = 4.9
                },
                new Game
                {
                    Title = "Red Dead Redemption 2",
                    Description = "America, 1899. The end of the Wild West era has begun. After a robbery goes badly wrong in the western town of Blackwater, Arthur Morgan and the Van der Linde gang are forced to flee. With federal agents and the best bounty hunters in the nation massing on their heels, the gang must rob, steal and fight their way across the rugged heartland of America in order to survive.",
                    Genre = "Action",
                    ReleaseDate = new DateTime(2018, 10, 26),
                    Developer = "Rockstar Games",
                    Publisher = "Rockstar Games",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/en/thumb/4/44/Red_Dead_Redemption_II.jpg/220px-Red_Dead_Redemption_II.jpg",
                    Rating = 4.8
                },
                new Game
                {
                    Title = "Cyberpunk 2077",
                    Description = "Cyberpunk 2077 is an open-world, action-adventure story set in Night City, a megalopolis obsessed with power, glamour, and body modification. You play as V, a mercenary outlaw going after a one-of-a-kind implant that is the key to immortality.",
                    Genre = "RPG",
                    ReleaseDate = new DateTime(2020, 12, 10),
                    Developer = "CD Projekt Red",
                    Publisher = "CD Projekt",
                    ImageUrl = "https://static.wikia.nocookie.net/cyberpunk/images/9/9f/Cyberpunk2077_box_art.jpg",
                    Rating = 4.5
                },
                new Game
                {
                    Title = "Super Mario Bros. Wonder",
                    Description = "Join Mario and his friends on a wondrous adventure in a world where anything can happen! Explore a vibrant and unpredictable land filled with secrets, power-ups, and captivating challenges.",
                    Genre = "Platformer",
                    ReleaseDate = new DateTime(2023, 10, 20),
                    Developer = "Nintendo EPD",
                    Publisher = "Nintendo",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/en/3/34/Super_Mario_Bros._Wonder_cover_art.jpg",
                    Rating = 4.7
                },
                new Game
                {
                    Title = "God of War",
                    Description = "Embark on an epic and emotional journey as Kratos, the Spartan warrior, seeks a new life in the Norse realm. Confront powerful gods and mythical creatures while grappling with his past and guiding his son, Atreus.",
                    Genre = "Action-Adventure",
                    ReleaseDate = new DateTime(2018, 4, 20),
                    Developer = "Santa Monica Studio",
                    Publisher = "Sony Interactive Entertainment",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/en/a/a7/God_of_War_4_cover.jpg",
                    Rating = 4.8
                },
                new Game
                {
                    Title = "Elden Ring",
                    Description = "Venture into the Lands Between, a vast and treacherous open world created by Hidetaka Miyazaki and George R. R. Martin. Unravel the mysteries of the Elden Ring, conquer demigods, and become the Elden Lord.",
                    Genre = "Action RPG",
                    ReleaseDate = new DateTime(2022, 2, 25),
                    Developer = "FromSoftware",
                    Publisher = "Bandai Namco Entertainment",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/en/b/b9/Elden_Ring_cover_art.jpg",
                    Rating = 4.6
                }
            };

            context.Games.AddRange(games);
            context.SaveChanges();

            // Add test reviews with validation
            var reviews = new Review[]
            {
                new Review
                {
                    GameId = games[0].Id,
                    UserId = user.Id,
                    Rating = 5,
                    Comment = "One of the best games I've ever played! The open world is breathtaking and there's so much to discover.",
                    CreatedAt = DateTime.UtcNow.AddDays(-5)
                },
                new Review
                {
                    GameId = games[1].Id,
                    UserId = user.Id,
                    Rating = 5,
                    Comment = "An absolute masterpiece. The attention to detail and storytelling are unmatched.",
                    CreatedAt = DateTime.UtcNow.AddDays(-3)
                },
                new Review
                {
                    GameId = games[2].Id,
                    UserId = user.Id,
                    Rating = 4,
                    Comment = "A visually stunning game with a deep story, though it had some bugs at launch.",
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                }
            };

            context.Reviews.AddRange(reviews);
            context.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred while seeding the database.", ex);
        }
    }
}

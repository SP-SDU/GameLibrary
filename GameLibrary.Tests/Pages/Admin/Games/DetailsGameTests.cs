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
using GameLibrary.Pages.Admin.Games;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace GameLibrary.Tests.Pages.Admin.Games
{
    public class DetailsGameTests
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly DetailsModel _detailsModel;

        public DetailsGameTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(dbContextOptions);
            _environment = new Mock<IWebHostEnvironment>().Object;
            _detailsModel = new DetailsModel(_context);
        }

        [Fact]
        public async Task OnGetAsync_WithValidId_PopulatesGameProperty()
        {
            // Arrange
            var game = new Game
            {
                Title = "Test Game",
                Genre = "Test Genre",
                Description = "Test Description",
                ReleaseDate = DateTime.Now,
                Price = 10.00m
            };
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            // Act
            await _detailsModel.OnGetAsync(game.Id);
            // Assert
            Assert.NotNull(_detailsModel.Game);
            Assert.Equal(game.Id, _detailsModel.Game.Id);
            Assert.Equal(game.Title, _detailsModel.Game.Title);
            Assert.Equal(game.Description, _detailsModel.Game.Description);
            Assert.Equal(game.ReleaseDate, _detailsModel.Game.ReleaseDate);
            Assert.Equal(game.Price, _detailsModel.Game.Price);
        }

        [Fact]
        public async Task OnGetAsync_WithInvalidId()
        {
            // Arrange
            // Act
            var result = await _detailsModel.OnGetAsync(Guid.NewGuid());
            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task OnGetAsync_WithGameNull()
        {
            // Arrange
            Game game = new Game{};

            // Act
            var result = await _detailsModel.OnGetAsync(game.Id);
            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

    }
}

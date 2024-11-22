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
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace GameLibrary.Tests.Pages.Admin.Games
{
    public class EditModelTests
    {
        private readonly Mock<IWebHostEnvironment> _mockEnvironment;
        private readonly ApplicationDbContext _context;
        public EditModelTests()
        {
            // Set up in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);

            // Set up mock environment
            _mockEnvironment = new Mock<IWebHostEnvironment>();
            _mockEnvironment.Setup(env => env.WebRootPath).Returns(Path.GetTempPath());
        }

        [Fact]
        public async Task OnGetAsync_ReturnsPageResult_WhenGameExists()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            _context.Games.Add(new Game { Id = gameId, Title = "Test Game" });
            await _context.SaveChangesAsync();

            var editModel = new EditModel(_context, _mockEnvironment.Object);

            // Act
            var result = await editModel.OnGetAsync(gameId);

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.NotNull(editModel.Game);
            Assert.Equal(gameId, editModel.Game.Id);
        }

        [Fact]
        public async Task OnGetAsync_ReturnsNotFoundResult_WhenGameDoesNotExist()
        {
            // Arrange
            var editModel = new EditModel(_context, _mockEnvironment.Object);

            // Act
            var result = await editModel.OnGetAsync(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task OnPostAsync_ReturnsPageResult_WhenModelStateInvalid()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var editModel = new EditModel(_context, _mockEnvironment.Object)
            {
                Game = null
            };
            editModel.ModelState.AddModelError("Error", "Test error");

            // Act
            var result = await editModel.OnPostAsync(gameId);

            // Assert
            Assert.IsType<PageResult>(result);
        }
    }
}

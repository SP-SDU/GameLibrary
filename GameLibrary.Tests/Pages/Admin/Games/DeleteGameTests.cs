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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Tests.Pages.Admin.Games
{
    public class DeleteGameTests
    {
        private readonly ApplicationDbContext _context;
        private readonly Mock<IWebHostEnvironment> _mockEnvironment;
        private readonly DeleteModel _deleteModel;

        public DeleteGameTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new ApplicationDbContext(options);
            _mockEnvironment = new Mock<IWebHostEnvironment>();
            _deleteModel = new DeleteModel(_context, _mockEnvironment.Object);
        }

        [Fact]
        public async Task OnGetAsync_ReturnsPageResult()
        {
            // Arrange
            _context.Games.Add(new Game { Title = "Test First Game" });
            await _context.SaveChangesAsync();
            var gameId = _context.Games.First().Id;

            // Act
            var result = await _deleteModel.OnGetAsync(gameId);
            // Assert
            Assert.IsType<PageResult>(result);
            Assert.NotNull(_deleteModel.Game);
            Assert.Equal(gameId, _deleteModel.Game.Id);
        }

        [Fact]
        public async Task OnGetAsync_ReturnsNotFoundResult()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            _context.Games.Add(new Game { Title = "Test Second Game" });
            await _context.SaveChangesAsync();
           
            // Act
            var result = await _deleteModel.OnGetAsync(gameId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.Null(_deleteModel.Game);
            Assert.NotEqual(gameId, _context.Games.First().Id);
        }

        [Fact]
        public async Task OnPostAsync_ReturnsRedirect()
        {
            // Arrange
            _context.Games.Add(new Game { Title = "Test Third Game" });
            await _context.SaveChangesAsync();
            var gameId = _context.Games.First().Id;
            // Act
            var result = await _deleteModel.OnPostAsync(gameId);
            // Assert
            Assert.IsType<RedirectToPageResult>(result);
            Assert.Null(_context.Games.Find(gameId));
        }

        [Fact]
        public async Task OnPostAsync_ReturnsNotFoundResult()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            _context.Games.Add(new Game { Title = "Test Fourth Game" });
            await _context.SaveChangesAsync();
            // Act
            var result = await _deleteModel.OnPostAsync(gameId);
            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.NotNull(_context.Games.First().Title);
        }
    }
}

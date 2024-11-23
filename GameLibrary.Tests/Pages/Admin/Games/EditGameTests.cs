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

namespace GameLibrary.Tests.Pages.Admin.Games;

public class EditModelTests
{
    private readonly ApplicationDbContext _context;
    private readonly Mock<IWebHostEnvironment> _mockEnvironment;
    private readonly EditModel _editModel;

    public EditModelTests()
    {
        // Set up in-memory database
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new ApplicationDbContext(options);
        _mockEnvironment = new Mock<IWebHostEnvironment>();
        _editModel = new EditModel(_context, _mockEnvironment.Object);
    }

    [Fact]
    public async Task OnGetAsync_GameExists()
    {
        // Arrange
        var _game = new Game
        {
            Title = "Test Game",
            Genre = "Test Genre",
            Description = "Test Description",
            ReleaseDate = new(2024, 2, 2, 0, 0, 0, DateTimeKind.Utc)
        };
        _ = _context.Games.Add(_game);
        _ = await _context.SaveChangesAsync();

        // Act
        var result = await _editModel.OnGetAsync(_game.Id);
        // Assert
        Assert.IsType<PageResult>(result);
        Assert.NotNull(_editModel.Game);
        Assert.Equal(_game.Id, _editModel.Game.Id);
    }

    [Fact]
    public async Task OnGetAsync_ReturnsNotFoundResult()
    {
        // Arrange
        Guid gameId = Guid.NewGuid();

        // Act
        var result = await _editModel.OnGetAsync(gameId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task OnPostAsync_GameIsNull()
    {
        // Arrange
        Game game = new Game();

        // Act
        var result = await _editModel.OnPostAsync(game.Id);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task OnPostAsync_ReturnsNotFoundResult_WhenGameDoesNotExist()
    {
        // Arrange

        // Act
        var result = await _editModel.OnPostAsync(Guid.NewGuid());
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}

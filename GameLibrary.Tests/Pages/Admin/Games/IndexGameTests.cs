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
using Microsoft.EntityFrameworkCore;
using Moq;

namespace GameLibrary.Tests.Pages.Admin.Games;

public class IndexGameTests
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly IndexModel _indexModel;

    public IndexGameTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new ApplicationDbContext(options);
        _environment = new Mock<IWebHostEnvironment>().Object;
        _indexModel = new IndexModel(_context, _environment);
    }

    [Fact]
    public async Task OnGetAsync_PopulatesThePageMode()
    {
        // Arrange
        _ = _context.Games.Add(new Game { Title = "Game_1", Genre = "Genre_1", ReleaseDate = new(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), Description = "Description_1" });
        _ = _context.Games.Add(new Game { Title = "Game_2", Genre = "Genre_2", ReleaseDate = new(2024, 2, 2, 0, 0, 0, DateTimeKind.Utc), Description = "Description_2" });
        _ = _context.Games.Add(new Game { Title = "Game_3", Genre = "Genre_3", ReleaseDate = new(2023, 3, 3, 0, 0, 0, DateTimeKind.Utc), Description = "Description_3" });

        _ = await _context.SaveChangesAsync();

        // Act
        await _indexModel.OnGetAsync();

        // Assert
        var games = Assert.IsAssignableFrom<IList<Game>>(_indexModel.Games);
        Assert.Equal(3, games.Count);
    }
}

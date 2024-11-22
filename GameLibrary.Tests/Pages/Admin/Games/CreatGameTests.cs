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

using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using GameLibrary.Data;
using GameLibrary.Models;
using GameLibrary.Pages.Admin.Games;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace GameLibrary.Tests.Pages.Admin.Games
{
    public class CreateModelTests
    {
        private readonly ApplicationDbContext _context;
        private readonly Mock<IWebHostEnvironment> _mockEnvironment;
        private readonly CreateModel _createModel;
        private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
        private readonly Mock<SignInManager<IdentityUser>> _mockSignInManager;
        private readonly Game _game;

        public CreateModelTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _mockEnvironment = new Mock<IWebHostEnvironment>();

            var store = new Mock<IUserStore<IdentityUser>>();
            _mockUserManager = new Mock<UserManager<IdentityUser>>(store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
            _mockSignInManager = new Mock<SignInManager<IdentityUser>>(_mockUserManager.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<IdentityUser>>(), null!, null!, null!, null!);

            _createModel = new CreateModel(_context, _mockEnvironment.Object);
            _game = new Game
            {
                //Id = Guid.NewGuid(),
                Title = "TestGame",
                Genre = "Action",
                ReleaseDate = DateTime.Now.Date,
                Description = "Test Description"
            };
        }

        [Fact]
        public void OnGet_ReturnsPageResult()
        {
            // Act
            var result = _createModel.OnGet();

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public void OnGet_InitializesGameProperty()
        {
            // Act
            _createModel.OnGet();

            // Assert
            Assert.NotNull(_createModel.Game);
            Assert.Equal("", _createModel.Game.Title);
            Assert.Equal("", _createModel.Game.ImageUrl);
            Assert.Equal("", _createModel.Game.Genre);
            Assert.Equal(DateTime.Now.Date, _createModel.Game.ReleaseDate);
            Assert.Equal("", _createModel.Game.Description);
            Assert.Empty(_createModel.Game.Reviews);
        }

        [Fact]
        public async Task OnPostAsync_ReturnsIsInvalid()
        {
            // Arrange
            _createModel.ModelState.AddModelError("Title", "Title is required");

            // Act
            var result = await _createModel.OnPostAsync(_game.Id);

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Theory]
        [InlineData(6 * 1024 * 1024, "test.jpg", true)]
        [InlineData(1024, "test.txt", true)]
        public async Task OnPostAsync_InvalidImageFile(long fileSize, string fileName, bool modelStateError)
        {
            // Arrange
            _createModel.ImageFile = new FormFile(new MemoryStream(new byte[fileSize]), 0, fileSize, "ImageFile", fileName);

            // Act
            var result = await _createModel.OnPostAsync(_game.Id);

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.Equal(modelStateError, _createModel.ModelState.ContainsKey("ImageFile"));
        }

        [Fact]
        public async Task OnPostAsync_ReturnsRedirectToIndex()
        {
            // Arrange
            _createModel.Game = _game;

            // Act
            var result = await _createModel.OnPostAsync(_game.Id);

            // Assert
            Assert.Empty(_createModel.ModelState);
            Assert.True(_createModel.ModelState.IsValid);
            Assert.Contains(_game, _context.Games);
            var redirectToPageResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("./Index", redirectToPageResult.PageName);
        }
    }
}

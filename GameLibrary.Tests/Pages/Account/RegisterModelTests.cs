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
using GameLibrary.Pages.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Moq;

namespace GameLibrary.Tests.Pages.Account;

public class RegisterModelTests
{
    private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
    private readonly Mock<SignInManager<IdentityUser>> _mockSignInManager;
    private readonly Mock<IUserEmailStore<IdentityUser>> _mockEmailStore;
    private readonly Mock<ILogger<RegisterModel>> _mockLogger;
    private readonly Mock<IEmailSender> _mockEmailSender;
    private readonly RegisterModel _registerModel;

    public RegisterModelTests()
    {
        _mockEmailStore = new Mock<IUserEmailStore<IdentityUser>>();
        _mockUserManager = new Mock<UserManager<IdentityUser>>(
            _mockEmailStore.Object, null!, null!, null!, null!, null!, null!, null!, null!);

        _mockUserManager.Setup(u => u.SupportsUserEmail).Returns(true);

        _mockSignInManager = MockSignInManager();
        _mockLogger = new Mock<ILogger<RegisterModel>>();
        _mockEmailSender = new Mock<IEmailSender>();

        _registerModel = new RegisterModel(
            _mockUserManager.Object,
            _mockEmailStore.Object,
            _mockSignInManager.Object,
            _mockLogger.Object,
            _mockEmailSender.Object);

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Scheme = "http";
        _registerModel.PageContext.HttpContext = httpContext;

        _registerModel.Url = Mock.Of<IUrlHelper>();
    }

    private static Mock<SignInManager<IdentityUser>> MockSignInManager()
    {
        var userManager = new Mock<UserManager<IdentityUser>>(
            Mock.Of<IUserStore<IdentityUser>>(), null!, null!, null!, null!, null!, null!, null!, null!);

        return new Mock<SignInManager<IdentityUser>>(
            userManager.Object,
            Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<IdentityUser>>(),
            null!, null!, null!, null!);
    }

    [Fact]
    public async Task OnPostAsync_WhenUserCreationFails_ReturnsPageWithModelError()
    {
        // Arrange
        _registerModel.Input = new RegisterModel.InputModel
        {
            Email = "test@example.com",
            Password = "Password123!",
            ConfirmPassword = "Password123!"
        };

        _mockUserManager.Setup(u => u.CreateAsync(It.IsAny<IdentityUser>(), _registerModel.Input.Password))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "User creation failed." }));

        // Act
        var result = await _registerModel.OnPostAsync("~/");

        // Assert
        Assert.IsType<PageResult>(result);
        Assert.True(_registerModel.ModelState.ContainsKey(string.Empty));
        Assert.Equal("User creation failed.", _registerModel.ModelState[string.Empty]!.Errors[0].ErrorMessage);
    }

    [Fact]
    public async Task OnPostAsync_RegistrationFails_AddsModelError()
    {
        // Arrange
        _registerModel.Input = new RegisterModel.InputModel
        {
            Email = "test@example.com",
            Password = "Password123!",
            ConfirmPassword = "Password123!"
        };

        _mockUserManager.Setup(u => u.CreateAsync(It.IsAny<IdentityUser>(), _registerModel.Input.Password))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Registration failed." }));

        // Act
        var result = await _registerModel.OnPostAsync("~/");

        // Assert
        Assert.IsType<PageResult>(result);
        Assert.True(_registerModel.ModelState.ContainsKey(string.Empty));
        Assert.Equal("Registration failed.", _registerModel.ModelState[string.Empty]!.Errors[0].ErrorMessage);
    }
}

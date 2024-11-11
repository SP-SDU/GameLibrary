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

using Microsoft.AspNetCore.Identity;

namespace GameLibrary.Models;

public class User : IdentityUser
{
    string FirstName { get; set; }
    string LastName { get; set; }
    string Address { get; set; }
    string City { get; set; }
    string State { get; set; }
    string ZipCode { get; set; }
    string Country { get; set; }
    string PhoneNumber { get; set; }
    string ProfilePicture { get; set; }
    string FavoriteGame { get; set; }
    string FavoriteGenre { get; set; }
    string FavoritePlatform { get; set; }
}

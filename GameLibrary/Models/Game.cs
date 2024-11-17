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

using System.ComponentModel.DataAnnotations;

namespace GameLibrary.Models;

public class Game
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Genre { get; set; } = string.Empty;

    public string Platform { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public DateTime ReleaseDate { get; set; } = DateTime.Now.Date;

    public string Developer { get; set; } = string.Empty;

    public string Publisher { get; set; } = string.Empty;

    public string? ImageUrl { get; set; } = string.Empty;

    public double Rating { get; set; }

    // Navigation properties
    public ICollection<Review> Reviews { get; set; } = [];
    public ICollection<UserFavorite> UserFavorites { get; set; } = [];
}

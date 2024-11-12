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
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Genre { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }

    [Required]
    [StringLength(100)]
    public string Developer { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Publisher { get; set; } = string.Empty;

    [StringLength(500)]
    [DataType(DataType.ImageUrl)]
    public string? ImageUrl { get; set; }

    [Range(0, 5.0)]
    public double Rating { get; set; }

    // Navigation properties
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<UserFavorite> UserFavorites { get; set; } = new List<UserFavorite>();
}

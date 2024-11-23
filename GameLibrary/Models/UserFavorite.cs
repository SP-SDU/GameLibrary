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

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GameLibrary.Models;

[Index(nameof(UserId), nameof(GameId), IsUnique = true, Name = "IX_UserFavorites_UserGame")]
public class UserFavorite
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid UserId { get; set; }

    [Required]
    public Guid GameId { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime AddedAt { get; set; }

    // Navigation properties
    public Game Game { get; set; } = null!;
    public User User { get; set; } = null!;
}

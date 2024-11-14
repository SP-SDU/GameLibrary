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

public class Review
{
    public int Id { get; set; }

    [Required]
    public int GameId { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required]
    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
    public int Rating { get; set; }

    [Required]
    [StringLength(1000, MinimumLength = 10, ErrorMessage = "Comment must be between 10 and 1000 characters")]
    public string Comment { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public Game? Game { get; set; }
    public User? User { get; set; }
}

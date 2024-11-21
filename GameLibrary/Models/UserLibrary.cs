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
using Microsoft.AspNetCore.Identity;

namespace GameLibrary.Models;

public class UserLibrary
{
    [Key]
    public Guid Id { get; set; }

    public string UserId { get; set; } = null!;
    public IdentityUser User { get; set; } = null!;

    public Guid GameId { get; set; }
    public Game Game { get; set; } = null!;

    public DateTime AddedDate { get; set; } = DateTime.UtcNow;
    
    public bool IsUpcoming { get; set; }
    
    // Status can be: Playing, Completed, On Hold, Dropped, Plan to Play
    [Required]
    [StringLength(20)]
    public string Status { get; set; } = "Plan to Play";
}
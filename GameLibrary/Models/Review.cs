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

namespace GameLibrary.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public int UserId { get; set; }
        public required string Content { get; set; }
        public int Rating { get; set; }

        public Game Game { get; set; }
        public required User User { get; set; }
    }
}

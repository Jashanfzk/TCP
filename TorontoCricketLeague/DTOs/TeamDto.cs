using System;
using System.ComponentModel.DataAnnotations;

namespace TorontoCricketLeague.DTOs
{
    /// <summary>
    /// Data Transfer Object for Team entities in the Toronto Cricket League.
    /// Used for API responses to provide team information with franchise details.
    /// Contains basic team information along with associated franchise name.
    /// </summary>
    /// <example>
    /// API Response Example:
    /// {
    ///     "TeamId": 1,
    ///     "Name": "Super Kings A",
    ///     "HomeGround": "Century Ground",
    ///     "FranchiseId": 1,
    ///     "FranchiseName": "Super Kings"
    /// }
    /// </example>
    /// <returns>
    /// Provides a clean data structure for API responses with:
    /// - Core team information
    /// - Associated franchise details
    /// - Consistent data format for external consumers
    /// </returns>
    public class TeamDto
    {
        /// <summary>
        /// The unique identifier for the team.
        /// </summary>
        /// <example>
        /// 1, 2, 3, etc.
        /// </example>
        public int TeamId { get; set; }
        
        /// <summary>
        /// The name of the team.
        /// </summary>
        /// <example>
        /// "Super Kings A", "Royal Challengers B", "Mumbai Indians C"
        /// </example>
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// The home ground where the team plays their matches.
        /// </summary>
        /// <example>
        /// "Century Ground", "Maple Leaf Stadium", "Cricket Arena"
        /// </example>
        [StringLength(100)]
        public string? HomeGround { get; set; }
        
        /// <summary>
        /// The unique identifier of the franchise that owns this team.
        /// </summary>
        /// <example>
        /// 1, 2, 3, etc.
        /// </example>
        public int FranchiseId { get; set; }
        
        /// <summary>
        /// The name of the franchise that owns this team.
        /// This is a computed property that gets populated when the DTO is created.
        /// </summary>
        /// <example>
        /// "Super Kings", "Royal Challengers", "Mumbai Indians"
        /// </example>
        public string FranchiseName { get; set; } = string.Empty;
        
        // Count of players
        public int PlayerCount { get; set; }
    }
} 
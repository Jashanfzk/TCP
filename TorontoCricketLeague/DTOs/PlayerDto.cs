using System;
using System.ComponentModel.DataAnnotations;

namespace TorontoCricketLeague.DTOs
{
    /// <summary>
    /// Data Transfer Object for Player entities in the Toronto Cricket League.
    /// Used for API responses to provide player information with team details.
    /// Contains basic player information along with associated team name.
    /// </summary>
    /// <example>
    /// API Response Example:
    /// {
    ///     "PlayerId": 1,
    ///     "FirstName": "John",
    ///     "LastName": "Doe",
    ///     "DateOfBirth": "1990-01-01T00:00:00",
    ///     "Position": "Batsman",
    ///     "TeamId": 1,
    ///     "TeamName": "Super Kings A"
    /// }
    /// </example>
    /// <returns>
    /// Provides a clean data structure for API responses with:
    /// - Core player information
    /// - Associated team details
    /// - Consistent data format for external consumers
    /// </returns>
    public class PlayerDto
    {
        /// <summary>
        /// The unique identifier for the player.
        /// </summary>
        /// <example>
        /// 1, 2, 3, etc.
        /// </example>
        public int PlayerId { get; set; }

        /// <summary>
        /// The first name of the player.
        /// </summary>
        /// <example>
        /// "John", "Sarah", "Michael"
        /// </example>
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// The last name of the player.
        /// </summary>
        /// <example>
        /// "Doe", "Smith", "Johnson"
        /// </example>
        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// The date of birth of the player.
        /// </summary>
        /// <example>
        /// "1990-01-01T00:00:00", "1985-05-15T00:00:00"
        /// </example>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// The playing position of the player in the team.
        /// </summary>
        /// <example>
        /// "Batsman", "Bowler", "All-rounder", "Wicket-keeper"
        /// </example>
        [Required]
        [StringLength(50)]
        public string Position { get; set; } = string.Empty;

        /// <summary>
        /// The unique identifier of the team that the player belongs to.
        /// </summary>
        /// <example>
        /// 1, 2, 3, etc.
        /// </example>
        public int TeamId { get; set; }

        /// <summary>
        /// The name of the team that the player belongs to.
        /// This is a computed property that gets populated when the DTO is created.
        /// </summary>
        /// <example>
        /// "Super Kings A", "Royal Challengers B", "Mumbai Indians C"
        /// </example>
        public string TeamName { get; set; } = string.Empty;
    }
} 
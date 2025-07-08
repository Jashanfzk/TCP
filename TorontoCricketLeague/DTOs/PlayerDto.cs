using System;
using System.ComponentModel.DataAnnotations;

namespace TorontoCricketLeague.DTOs
{
    /// <summary>
    /// This DTO is used for player data transfer in API responses
    /// It provides player information with associated team details
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
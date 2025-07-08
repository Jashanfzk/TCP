using System.ComponentModel.DataAnnotations;

namespace TorontoCricketLeague.Models
{
    /// <summary>
    /// This model represents a player in the cricket league
    /// It stores information about players and their relationship with teams
    /// </summary>
    /// <example>
    /// var player = new Player 
    /// { 
    ///     Name = "John Doe", 
    ///     Age = 25, 
    ///     Role = "Bowler",
    ///     TeamId = 1
    /// };
    /// </example>
    public class Player
    {
        public int PlayerId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Range(15, 60)]
        public int Age { get; set; }

        [Required]
        [StringLength(50)]
        public string Role { get; set; } = string.Empty; // e.g., Batsman, Bowler, All rounder

        // Foreign Key to link this Player to a Team
        public int TeamId { get; set; }
        
        // Navigation property
        public Team? Team { get; set; }
    }
}
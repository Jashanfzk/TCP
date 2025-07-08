using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TorontoCricketLeague.Models;

namespace TorontoCricketLeague.Models
{
    /// <summary>
    /// This model represents a team in the cricket league
    /// It stores information about teams and their relationships with franchises, players, and sponsors
    /// </summary>
    /// <example>
    /// var team = new Team 
    /// { 
    ///     Name = "Blue Warriors", 
    ///     City = "Mississauga",
    ///     LogoUrl = "team-logo.png",
    ///     FranchiseId = 1
    /// };
    /// </example>
    public class Team
    {
        public int TeamId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(255)]
        public string? LogoUrl { get; set; }

        // Foreign Key to link this Team to a Franchise
        public int FranchiseId { get; set; }
        
        // Navigation Properties
        public Franchise? Franchise { get; set; }
        public ICollection<Player> Players { get; set; } = new List<Player>();
        public ICollection<TeamSponsor> TeamSponsors { get; set; } = new List<TeamSponsor>();
    }
}
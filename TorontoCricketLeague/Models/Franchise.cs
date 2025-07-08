using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TorontoCricketLeague.Models
{
    /// <summary>
    /// This model represents a franchise in the cricket league
    /// It stores information about franchises and their relationships with teams and sponsors
    /// </summary>
    /// <example>
    /// var franchise = new Franchise 
    /// { 
    ///     Name = "Super Kings", 
    ///     HomeCity = "Toronto",
    ///     LogoUrl = "logo.png"
    /// };
    /// </example>
    public class Franchise
    {
        public int FranchiseId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public string? HomeCity { get; set; }

        [StringLength(255)]
        public string? LogoUrl { get; set; }

        // A Franchise has many teams
        public ICollection<Team> Teams { get; set; } = new List<Team>();
        
        // A Franchise has many sponsors
        public ICollection<Sponsor> Sponsors { get; set; } = new List<Sponsor>();
    }
}
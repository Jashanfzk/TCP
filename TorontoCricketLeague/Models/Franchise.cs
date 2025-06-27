using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TorontoCricketLeague.Models
{
    /// <summary>
    /// Represents a franchise in the Toronto Cricket League.
    /// A franchise owns multiple teams and has multiple sponsors.
    /// </summary>
    /// <example>
    /// var franchise = new Franchise 
    /// { 
    ///     Name = "Super Kings", 
    ///     HomeCity = "Toronto",
    ///     LogoUrl = "logo.png"
    /// };
    /// </example>
    /// <result>
    /// The franchise is created and can be saved to the database.
    /// </result>
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
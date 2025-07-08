using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TorontoCricketLeague.Models
{
    /// <summary>
    /// This model represents a sponsor in the cricket league
    /// It stores information about sponsors and their relationship with franchises
    /// </summary>
    /// <example>
    /// var sponsor = new Sponsor 
    /// { 
    ///     Name = "Pizza Place", 
    ///     LogoUrl = "sponsor-logo.png",
    ///     FranchiseId = 1
    /// };
    /// </example>
    public class Sponsor
    {
        public int SponsorId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(255)]
        public string? LogoUrl { get; set; }

        // Foreign key to Franchise
        public int FranchiseId { get; set; }
        public Franchise? Franchise { get; set; }
    }
} 
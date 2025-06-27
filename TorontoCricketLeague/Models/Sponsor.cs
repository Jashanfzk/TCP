using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TorontoCricketLeague.Models
{
    /// <summary>
    /// Represents a sponsor in the Toronto Cricket League.
    /// Sponsors support franchises and can be linked to multiple teams.
    /// </summary>
    /// <example>
    /// var sponsor = new Sponsor 
    /// { 
    ///     Name = "Pizza Place", 
    ///     LogoUrl = "sponsor-logo.png",
    ///     FranchiseId = 1
    /// };
    /// </example>
    /// <result>
    /// The sponsor is created and linked to a franchise.
    /// </result>
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
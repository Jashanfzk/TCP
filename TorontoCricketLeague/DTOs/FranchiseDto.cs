using System;
using System.ComponentModel.DataAnnotations;

namespace TorontoCricketLeague.DTOs
{
    /// <summary>
    /// Data Transfer Object for Franchise entities in the Toronto Cricket League.
    /// Used for API responses to provide franchise information with computed properties.
    /// Contains basic franchise details along with team and sponsor counts.
    /// </summary>
    /// <example>
    /// API Response Example:
    /// {
    ///     "FranchiseId": 1,
    ///     "Name": "Super Kings",
    ///     "HomeCity": "Toronto",
    ///     "LogoUrl": "",
    ///     "TeamCount": 3,
    ///     "SponsorCount": 2
    /// }
    /// </example>
    /// <returns>
    /// Provides a clean data structure for API responses with:
    /// - Core franchise information
    /// - Computed counts of related entities
    /// - Consistent data format for external consumers
    /// </returns>
    public class FranchiseDto
    {
        /// <summary>
        /// The unique identifier for the franchise.
        /// </summary>
        /// <example>
        /// 1, 2, 3, etc.
        /// </example>
        public int FranchiseId { get; set; }
        
        /// <summary>
        /// The name of the franchise.
        /// </summary>
        /// <example>
        /// "Super Kings", "Royal Challengers", "Mumbai Indians"
        /// </example>
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// The home city where the franchise is based.
        /// </summary>
        /// <example>
        /// "Toronto", "Vancouver", "Montreal"
        /// </example>
        [StringLength(100)]
        public string? HomeCity { get; set; }
        
        /// <summary>
        /// The URL to the franchise's logo image.
        /// </summary>
        /// <example>
        /// "https://example.com/logos/superkings.png"
        /// </example>
        [StringLength(255)]
        public string? LogoUrl { get; set; }
        
        /// <summary>
        /// The number of teams associated with this franchise.
        /// This is a computed property that gets populated when the DTO is created.
        /// </summary>
        /// <example>
        /// 0, 1, 2, 3, etc.
        /// </example>
        public int TeamCount { get; set; }
        
        /// <summary>
        /// The number of sponsors associated with this franchise.
        /// This is a computed property that gets populated when the DTO is created.
        /// </summary>
        /// <example>
        /// 0, 1, 2, 3, etc.
        /// </example>
        public int SponsorCount { get; set; }
    }
} 
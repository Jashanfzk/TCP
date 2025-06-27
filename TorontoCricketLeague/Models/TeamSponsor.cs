/// <summary>
/// Represents the many-to-many relationship between teams and sponsors.
/// Links teams with their sponsors using a composite primary key.
/// </summary>
/// <example>
/// var teamSponsor = new TeamSponsor 
/// { 
///     TeamId = 1, 
///     SponsorId = 2 
/// };
/// </example>
/// <result>
/// The sponsor is linked to the team and appears in the team's sponsors list.
/// </result>
namespace TorontoCricketLeague.Models
{
    public class TeamSponsor
    {
        // Composite Primary Key
        public int TeamId { get; set; }
        public int SponsorId { get; set; }

        // Navigation Properties
        public Team? Team { get; set; }
        public Sponsor? Sponsor { get; set; }
    }
}
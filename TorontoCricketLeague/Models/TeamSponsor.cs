/// <summary>
/// This model represents the many-to-many relationship between teams and sponsors
/// It links teams with their sponsors using a composite primary key
/// </summary>
/// <example>
/// var teamSponsor = new TeamSponsor 
/// { 
///     TeamId = 1, 
///     SponsorId = 2 
/// };
/// </example>
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
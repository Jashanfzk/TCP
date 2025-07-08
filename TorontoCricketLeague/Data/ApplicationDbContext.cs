using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TorontoCricketLeague.Models;

namespace TorontoCricketLeague.Data
{
    /// <summary>
    /// This class manages the database context for the cricket league application
    /// It handles all database operations and entity relationships
    /// </summary>
    /// <example>
    /// var context = new ApplicationDbContext(options);
    /// var teams = await context.Teams.Include(t => t.Franchise).ToListAsync();
    /// </example>
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Franchise> Franchises { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Sponsor> Sponsors { get; set; }
        public DbSet<TeamSponsor> TeamSponsors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // One-to-many: Franchise -> Sponsors
            modelBuilder.Entity<Sponsor>()
                .HasOne(s => s.Franchise)
                .WithMany(f => f.Sponsors)
                .HasForeignKey(s => s.FranchiseId)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-many relationship between Franchise and Team
            modelBuilder.Entity<Team>()
                .HasOne(t => t.Franchise)
                .WithMany(f => f.Teams)
                .HasForeignKey(t => t.FranchiseId);

            // Configure composite primary key for TeamSponsor
            modelBuilder.Entity<TeamSponsor>()
                .HasKey(ts => new { ts.TeamId, ts.SponsorId });

            // Configure many-to-many relationship between Team and Sponsor
            modelBuilder.Entity<TeamSponsor>()
                .HasOne(ts => ts.Team)
                .WithMany(t => t.TeamSponsors)
                .HasForeignKey(ts => ts.TeamId);

            modelBuilder.Entity<TeamSponsor>()
                .HasOne(ts => ts.Sponsor)
                .WithMany()
                .HasForeignKey(ts => ts.SponsorId);
        }
    }
}

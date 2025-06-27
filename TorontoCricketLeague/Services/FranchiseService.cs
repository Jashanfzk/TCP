using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TorontoCricketLeague.Data;
using TorontoCricketLeague.DTOs;

namespace TorontoCricketLeague.Services
{
    /// <summary>
    /// Service implementation for Franchise operations in the Toronto Cricket League.
    /// Provides business logic for franchise data retrieval with computed properties.
    /// Implements the IFranchiseService interface for dependency injection.
    /// </summary>
    /// <example>
    /// Service Usage:
    /// var franchiseService = new FranchiseService(context);
    /// var franchises = await franchiseService.GetAllFranchisesAsync();
    /// var franchise = await franchiseService.GetFranchiseByIdAsync(1);
    /// </example>
    /// <returns>
    /// Provides franchise data operations with:
    /// - Team and sponsor count calculations
    /// - Entity Framework data access
    /// - DTO transformation logic
    /// </returns>
    public class FranchiseService : IFranchiseService
    {
        private readonly ApplicationDbContext _context;

        // Dependency injection of database context
        public FranchiseService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all franchises with their team and sponsor counts.
        /// </summary>
        /// <returns>
        /// A collection of FranchiseDto objects containing franchise details with computed counts.
        /// </returns>
        /// <example>
        /// var franchises = await franchiseService.GetAllFranchisesAsync();
        /// foreach (var franchise in franchises)
        /// {
        ///     Console.WriteLine($"{franchise.Name}: {franchise.TeamCount} teams, {franchise.SponsorCount} sponsors");
        /// }
        /// </example>
        public async Task<IEnumerable<FranchiseDto>> GetAllFranchisesAsync()
        {
            // Get all franchises with their related teams and sponsors
            var franchises = await _context.Franchises
                .Include(f => f.Teams)
                .Include(f => f.Sponsors)
                .ToListAsync();

            // Transform entities to DTOs with computed properties
            var franchiseDtos = franchises.Select(f => new FranchiseDto
            {
                FranchiseId = f.FranchiseId,
                Name = f.Name,
                HomeCity = f.HomeCity,
                LogoUrl = f.LogoUrl,
                TeamCount = f.Teams?.Count ?? 0,
                SponsorCount = f.Sponsors?.Count ?? 0
            }).ToList();

            // Return the collection of DTOs
            return franchiseDtos;
        }

        /// <summary>
        /// Retrieves a specific franchise by its ID with team and sponsor counts.
        /// </summary>
        /// <param name="id">The unique identifier of the franchise to retrieve.</param>
        /// <returns>
        /// A FranchiseDto object containing the franchise details, or null if not found.
        /// </returns>
        /// <example>
        /// var franchise = await franchiseService.GetFranchiseByIdAsync(1);
        /// if (franchise != null)
        /// {
        ///     Console.WriteLine($"Franchise: {franchise.Name}");
        ///     Console.WriteLine($"Teams: {franchise.TeamCount}");
        ///     Console.WriteLine($"Sponsors: {franchise.SponsorCount}");
        /// }
        /// </example>
        public async Task<FranchiseDto?> GetFranchiseByIdAsync(int id)
        {
            // Get franchise with related teams and sponsors
            var franchise = await _context.Franchises
                .Include(f => f.Teams)
                .Include(f => f.Sponsors)
                .FirstOrDefaultAsync(f => f.FranchiseId == id);

            // Check if franchise exists
            if (franchise == null)
            {
                return null;
            }

            // Transform entity to DTO with computed properties
            var franchiseDto = new FranchiseDto
            {
                FranchiseId = franchise.FranchiseId,
                Name = franchise.Name,
                HomeCity = franchise.HomeCity,
                LogoUrl = franchise.LogoUrl,
                TeamCount = franchise.Teams?.Count ?? 0,
                SponsorCount = franchise.Sponsors?.Count ?? 0
            };

            // Return the DTO
            return franchiseDto;
        }
    }
} 
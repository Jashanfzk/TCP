using System.Collections.Generic;
using System.Threading.Tasks;
using TorontoCricketLeague.DTOs;

namespace TorontoCricketLeague.Services
{
    /// <summary>
    /// Interface for Franchise service operations in the Toronto Cricket League.
    /// Defines the contract for franchise-related business logic and data operations.
    /// Provides methods for retrieving franchise information with computed properties.
    /// </summary>
    /// <example>
    /// Service Implementation:
    /// var franchiseService = new FranchiseService(context);
    /// var franchises = await franchiseService.GetAllFranchisesAsync();
    /// var franchise = await franchiseService.GetFranchiseByIdAsync(1);
    /// </example>
    /// <returns>
    /// Provides a service layer abstraction for:
    /// - Franchise data retrieval operations
    /// - Business logic encapsulation
    /// - Dependency injection support
    /// </returns>
    public interface IFranchiseService
    {
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
        Task<IEnumerable<FranchiseDto>> GetAllFranchisesAsync();

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
        Task<FranchiseDto?> GetFranchiseByIdAsync(int id);
    }
} 
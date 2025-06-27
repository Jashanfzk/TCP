using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TorontoCricketLeague.Data;
using TorontoCricketLeague.Models;
using TorontoCricketLeague.DTOs;

namespace TorontoCricketLeague.Controllers
{
    /// <summary>
    /// Controller for managing Franchise entities in the Toronto Cricket League.
    /// Handles CRUD operations for franchises including create, read, update, and delete.
    /// Provides both web interface and API endpoints for franchise management.
    /// </summary>
    /// <example>
    /// Web Interface:
    /// GET /Franchises - Shows list of all franchises with team and sponsor counts
    /// GET /Franchises/Details/1 - Shows detailed view of franchise with ID 1
    /// GET /Franchises/Create - Shows form to create a new franchise
    /// POST /Franchises/Create - Creates a new franchise from form data
    /// GET /Franchises/Edit/1 - Shows form to edit franchise with ID 1
    /// POST /Franchises/Edit/1 - Updates franchise with ID 1
    /// GET /Franchises/Delete/1 - Shows confirmation to delete franchise with ID 1
    /// POST /Franchises/Delete/1 - Deletes franchise with ID 1
    /// 
    /// API Endpoints:
    /// GET /Franchises/ListFranchises - Returns JSON list of all franchises
    /// GET /Franchises/FindFranchise/1 - Returns JSON franchise with ID 1
    /// POST /Franchises/CreateFranchise - Creates new franchise via API
    /// PUT /Franchises/UpdateFranchise/1 - Updates franchise via API
    /// DELETE /Franchises/DeleteFranchise/1 - Deletes franchise via API
    /// </example>
    /// <returns>
    /// Provides complete franchise management with:
    /// - Web interface for administrators
    /// - RESTful API endpoints for external access
    /// - Data Transfer Objects for API responses
    /// - Proper validation and error handling
    /// </returns>
    public class FranchisesController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Dependency injection of database context
        public FranchisesController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of all franchises in the system with their team and sponsor counts.
        /// </summary>
        /// <example>
        /// GET /Franchises -> Returns view with list of all franchises
        /// </example>
        /// <returns>
        /// A view containing all franchises with their related data.
        /// </returns>
        public async Task<IActionResult> Index()
        {
            // Get all franchises with their related teams and sponsors
            var franchises = await _context.Franchises
                .Include(f => f.Teams)
                .Include(f => f.Sponsors)
                .ToListAsync();
            
            // Return the view with franchise data
            return View(franchises);
        }

        /// <summary>
        /// Retrieves a specific franchise's details by their ID.
        /// </summary>
        /// <param name="id">The unique identifier of the franchise.</param>
        /// <returns>
        /// A view containing the franchise details with related teams and sponsors.
        /// </returns>
        /// <example>
        /// GET /Franchises/Details/1 -> Shows detailed view of franchise with ID 1
        /// </example>
        public async Task<IActionResult> Details(int? id)
        {
            // Check if ID is provided
            if (id == null || _context.Franchises == null)
            {
                return NotFound();
            }

            // Get franchise with related data
            var franchise = await _context.Franchises
                .Include(f => f.Teams)
                .Include(f => f.Sponsors)
                .FirstOrDefaultAsync(m => m.FranchiseId == id);
            
            // Check if franchise exists
            if (franchise == null)
            {
                return NotFound();
            }

            // Return the view with franchise details
            return View(franchise);
        }

        /// <summary>
        /// Shows the form to create a new franchise.
        /// </summary>
        /// <returns>
        /// A view with the create franchise form.
        /// </returns>
        /// <example>
        /// GET /Franchises/Create -> Shows form to create a new franchise
        /// </example>
        public IActionResult Create()
        {
            // Return the create view
            return View();
        }

        /// <summary>
        /// Creates a new franchise in the database.
        /// </summary>
        /// <param name="franchise">The franchise object to add.</param>
        /// <returns>
        /// Redirects to index page if successful, or returns create view with errors.
        /// </returns>
        /// <example>
        /// POST /Franchises/Create -> Creates a new franchise from form data
        /// </example>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,HomeCity,LogoUrl")] Franchise franchise)
        {
            // Check if model is valid
            if (ModelState.IsValid)
            {
                // Add the new franchise to the database
                _context.Add(franchise);
                await _context.SaveChangesAsync();
                
                // Redirect to the index page
                return RedirectToAction(nameof(Index));
            }
            
            // Return the create view with validation errors
            return View(franchise);
        }

        /// <summary>
        /// Shows the form to edit an existing franchise.
        /// </summary>
        /// <param name="id">The unique identifier of the franchise to edit.</param>
        /// <returns>
        /// A view with the edit franchise form.
        /// </returns>
        /// <example>
        /// GET /Franchises/Edit/1 -> Shows form to edit franchise with ID 1
        /// </example>
        public async Task<IActionResult> Edit(int? id)
        {
            // Check if ID is provided
            if (id == null || _context.Franchises == null)
            {
                return NotFound();
            }

            // Find the franchise to edit
            var franchise = await _context.Franchises.FindAsync(id);
            
            // Check if franchise exists
            if (franchise == null)
            {
                return NotFound();
            }
            
            // Return the edit view
            return View(franchise);
        }

        /// <summary>
        /// Updates an existing franchise in the database.
        /// </summary>
        /// <param name="id">The unique identifier of the franchise.</param>
        /// <param name="franchise">The updated franchise object.</param>
        /// <returns>
        /// Redirects to index page if successful, or returns edit view with errors.
        /// </returns>
        /// <example>
        /// POST /Franchises/Edit/1 -> Updates franchise with ID 1
        /// </example>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FranchiseId,Name,HomeCity,LogoUrl")] Franchise franchise)
        {
            // Check if IDs match
            if (id != franchise.FranchiseId)
            {
                return NotFound();
            }

            // Check if model is valid
            if (ModelState.IsValid)
            {
                try
                {
                    // Update the franchise in the database
                    _context.Update(franchise);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Check if franchise still exists
                    if (!FranchiseExists(franchise.FranchiseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                
                // Redirect to the index page
                return RedirectToAction(nameof(Index));
            }
            
            // Return the edit view with validation errors
            return View(franchise);
        }

        /// <summary>
        /// Shows the confirmation page to delete a franchise.
        /// </summary>
        /// <param name="id">The unique identifier of the franchise to delete.</param>
        /// <returns>
        /// A view with the delete confirmation.
        /// </returns>
        /// <example>
        /// GET /Franchises/Delete/1 -> Shows confirmation to delete franchise with ID 1
        /// </example>
        public async Task<IActionResult> Delete(int? id)
        {
            // Check if ID is provided
            if (id == null || _context.Franchises == null)
            {
                return NotFound();
            }

            // Get franchise with related data for confirmation
            var franchise = await _context.Franchises
                .Include(f => f.Teams)
                .Include(f => f.Sponsors)
                .FirstOrDefaultAsync(m => m.FranchiseId == id);
            
            // Check if franchise exists
            if (franchise == null)
            {
                return NotFound();
            }

            // Return the delete confirmation view
            return View(franchise);
        }

        /// <summary>
        /// Deletes a franchise from the database based on their ID.
        /// </summary>
        /// <param name="id">The ID of the franchise to delete.</param>
        /// <returns>
        /// Redirects to index page after successful deletion.
        /// </returns>
        /// <example>
        /// POST /Franchises/Delete/1 -> Deletes franchise with ID 1
        /// </example>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Check if franchises table exists
            if (_context.Franchises == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Franchises'  is null.");
            }
            
            // Find the franchise to delete
            var franchise = await _context.Franchises.FindAsync(id);
            
            // Delete the franchise if it exists
            if (franchise != null)
            {
                _context.Franchises.Remove(franchise);
            }
            
            // Save changes to the database
            await _context.SaveChangesAsync();
            
            // Redirect to the index page
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Returns a list of all franchises as JSON with team and sponsor counts.
        /// </summary>
        /// <example>
        /// GET /Franchises/ListFranchises -> [{"FranchiseId":1, "Name":"Super Kings", "HomeCity":"Toronto", "TeamCount":3, "SponsorCount":2}, ...]
        /// </example>
        /// <returns>
        /// A JSON array of FranchiseDto objects containing details of all franchises.
        /// </returns>
        [HttpGet]
        [Route("ListFranchises")]
        public async Task<IActionResult> ListFranchises()
        {
            // Get all franchises with their related data
            var franchises = await _context.Franchises
                .Include(f => f.Teams)
                .Include(f => f.Sponsors)
                .ToListAsync();

            // Create DTOs with computed properties
            var franchiseDtos = franchises.Select(f => new FranchiseDto
            {
                FranchiseId = f.FranchiseId,
                Name = f.Name,
                HomeCity = f.HomeCity,
                LogoUrl = f.LogoUrl,
                TeamCount = f.Teams?.Count ?? 0,
                SponsorCount = f.Sponsors?.Count ?? 0
            }).ToList();

            // Return the JSON response
            return Json(franchiseDtos);
        }

        /// <summary>
        /// Retrieves a specific franchise's details by their ID as JSON.
        /// </summary>
        /// <param name="id">The unique identifier of the franchise.</param>
        /// <returns>
        /// A JSON FranchiseDto object containing details of the franchise.
        /// </returns>
        /// <example>
        /// GET /Franchises/FindFranchise/1 -> {"FranchiseId":1, "Name":"Super Kings", "HomeCity":"Toronto", "TeamCount":3, "SponsorCount":2}
        /// </example>
        [HttpGet]
        [Route("FindFranchise/{id}")]
        public async Task<IActionResult> FindFranchise(int id)
        {
            // Get franchise with related data
            var franchise = await _context.Franchises
                .Include(f => f.Teams)
                .Include(f => f.Sponsors)
                .FirstOrDefaultAsync(f => f.FranchiseId == id);

            // Check if franchise exists
            if (franchise == null)
            {
                return NotFound();
            }

            // Create DTO with computed properties
            var franchiseDto = new FranchiseDto
            {
                FranchiseId = franchise.FranchiseId,
                Name = franchise.Name,
                HomeCity = franchise.HomeCity,
                LogoUrl = franchise.LogoUrl,
                TeamCount = franchise.Teams?.Count ?? 0,
                SponsorCount = franchise.Sponsors?.Count ?? 0
            };

            // Return the JSON response
            return Json(franchiseDto);
        }

        /// <summary>
        /// Adds a new franchise to the database via API.
        /// </summary>
        /// <param name="franchiseDto">The franchise DTO object to add.</param>
        /// <returns>
        /// JSON response with the created franchise details.
        /// </returns>
        /// <example>
        /// POST /Franchises/CreateFranchise
        /// Request Body:
        /// {
        ///     "Name": "Super Kings",
        ///     "HomeCity": "Toronto",
        ///     "LogoUrl": "https://example.com/logo.png"
        /// }
        /// </example>
        [HttpPost]
        [Route("CreateFranchise")]
        public async Task<IActionResult> CreateFranchise([FromBody] FranchiseDto franchiseDto)
        {
            // Check if model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Create new franchise from DTO
            var franchise = new Franchise
            {
                Name = franchiseDto.Name,
                HomeCity = franchiseDto.HomeCity,
                LogoUrl = franchiseDto.LogoUrl
            };

            // Add the new franchise to the database
            _context.Franchises.Add(franchise);
            await _context.SaveChangesAsync();

            // Set the ID in the DTO for response
            franchiseDto.FranchiseId = franchise.FranchiseId;
            
            // Return the JSON response
            return Json(franchiseDto);
        }

        /// <summary>
        /// Updates an existing franchise in the database via API.
        /// </summary>
        /// <param name="id">The unique identifier of the franchise.</param>
        /// <param name="franchiseDto">The updated franchise DTO object.</param>
        /// <returns>
        /// JSON response confirming the update operation.
        /// </returns>
        /// <example>
        /// PUT /Franchises/UpdateFranchise/1
        /// Request Body:
        /// {
        ///     "FranchiseId": 1,
        ///     "Name": "Updated Super Kings",
        ///     "HomeCity": "Toronto",
        ///     "LogoUrl": "https://example.com/newlogo.png"
        /// }
        /// </example>
        [HttpPut]
        [Route("UpdateFranchise/{id}")]
        public async Task<IActionResult> UpdateFranchise(int id, [FromBody] FranchiseDto franchiseDto)
        {
            // Check if IDs match
            if (id != franchiseDto.FranchiseId)
            {
                return BadRequest();
            }

            // Find the franchise to update
            var franchise = await _context.Franchises.FindAsync(id);
            
            // Check if franchise exists
            if (franchise == null)
            {
                return NotFound();
            }

            // Update franchise properties
            franchise.Name = franchiseDto.Name;
            franchise.HomeCity = franchiseDto.HomeCity;
            franchise.LogoUrl = franchiseDto.LogoUrl;

            try
            {
                // Update the franchise in the database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Check if franchise still exists
                if (!FranchiseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Return success message
            return Json(new { message = "Franchise updated successfully" });
        }

        /// <summary>
        /// Deletes a franchise from the database based on their ID via API.
        /// </summary>
        /// <param name="id">The ID of the franchise to delete.</param>
        /// <returns>
        /// JSON response confirming the deletion operation.
        /// </returns>
        /// <example>
        /// DELETE /Franchises/DeleteFranchise/1 -> {"message": "Franchise deleted successfully"}
        /// </example>
        [HttpDelete]
        [Route("DeleteFranchise/{id}")]
        public async Task<IActionResult> DeleteFranchise(int id)
        {
            // Find the franchise to delete
            var franchise = await _context.Franchises.FindAsync(id);
            
            // Check if franchise exists
            if (franchise == null)
            {
                return NotFound();
            }

            // Remove the franchise from the database
            _context.Franchises.Remove(franchise);
            await _context.SaveChangesAsync();

            // Return success message
            return Json(new { message = "Franchise deleted successfully" });
        }

        /// <summary>
        /// Checks if a franchise exists in the database.
        /// </summary>
        /// <param name="id">The unique identifier of the franchise.</param>
        /// <returns>
        /// True if the franchise exists, false otherwise.
        /// </returns>
        private bool FranchiseExists(int id)
        {
          return (_context.Franchises?.Any(e => e.FranchiseId == id)).GetValueOrDefault();
        }
    }
}

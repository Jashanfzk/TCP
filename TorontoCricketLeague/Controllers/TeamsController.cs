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
    /// Controller for managing Team entities in the Toronto Cricket League.
    /// Handles CRUD operations for teams including create, read, update, and delete.
    /// Provides both web interface and API endpoints for team management.
    /// </summary>
    /// <example>
    /// Web Interface:
    /// GET /Teams - Shows list of all teams with franchise information
    /// GET /Teams/Details/1 - Shows detailed view of team with ID 1
    /// GET /Teams/Create - Shows form to create a new team
    /// POST /Teams/Create - Creates a new team from form data
    /// GET /Teams/Edit/1 - Shows form to edit team with ID 1
    /// POST /Teams/Edit/1 - Updates team with ID 1
    /// GET /Teams/Delete/1 - Shows confirmation to delete team with ID 1
    /// POST /Teams/Delete/1 - Deletes team with ID 1
    /// 
    /// API Endpoints:
    /// GET /Teams/ListTeams - Returns JSON list of all teams
    /// GET /Teams/FindTeam/1 - Returns JSON team with ID 1
    /// POST /Teams/CreateTeam - Creates new team via API
    /// PUT /Teams/UpdateTeam/1 - Updates team via API
    /// DELETE /Teams/DeleteTeam/1 - Deletes team via API
    /// </example>
    /// <returns>
    /// Provides complete team management with:
    /// - Web interface for administrators
    /// - RESTful API endpoints for external access
    /// - Data Transfer Objects for API responses
    /// - Proper validation and error handling
    /// </returns>
    public class TeamsController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Dependency injection of database context
        public TeamsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of all teams in the system with their franchise information.
        /// </summary>
        /// <example>
        /// GET /Teams -> Returns view with list of all teams
        /// </example>
        /// <returns>
        /// A view containing all teams with their related franchise data.
        /// </returns>
        public async Task<IActionResult> Index()
        {
            // Get all teams with their related franchise data
            var teams = await _context.Teams
                .Include(t => t.Franchise)
                .ToListAsync();
            
            // Return the view with team data
            return View(teams);
        }

        /// <summary>
        /// Retrieves a specific team's details by their ID.
        /// </summary>
        /// <param name="id">The unique identifier of the team.</param>
        /// <returns>
        /// A view containing the team details with related franchise information.
        /// </returns>
        /// <example>
        /// GET /Teams/Details/1 -> Shows detailed view of team with ID 1
        /// </example>
        public async Task<IActionResult> Details(int? id)
        {
            // Check if ID is provided
            if (id == null || _context.Teams == null)
            {
                return NotFound();
            }

            // Get team with related franchise data
            var team = await _context.Teams
                .Include(t => t.Franchise)
                .FirstOrDefaultAsync(m => m.TeamId == id);
            
            // Check if team exists
            if (team == null)
            {
                return NotFound();
            }

            // Return the view with team details
            return View(team);
        }

        /// <summary>
        /// Shows the form to create a new team.
        /// </summary>
        /// <returns>
        /// A view with the create team form including franchise selection.
        /// </returns>
        /// <example>
        /// GET /Teams/Create -> Shows form to create a new team
        /// </example>
        public IActionResult Create()
        {
            // Create a select list of franchises for the dropdown
            ViewData["FranchiseId"] = new SelectList(_context.Franchises, "FranchiseId", "Name");
            
            // Return the create view
            return View();
        }

        /// <summary>
        /// Creates a new team in the database.
        /// </summary>
        /// <param name="team">The team object to add.</param>
        /// <returns>
        /// Redirects to index page if successful, or returns create view with errors.
        /// </returns>
        /// <example>
        /// POST /Teams/Create -> Creates a new team from form data
        /// </example>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,City,LogoUrl,FranchiseId")] Team team)
        {
            // Check if model is valid
            if (ModelState.IsValid)
            {
                // Add the new team to the database
                _context.Add(team);
                await _context.SaveChangesAsync();
                
                // Redirect to the index page
                return RedirectToAction(nameof(Index));
            }
            
            // Create a select list of franchises for the dropdown
            ViewData["FranchiseId"] = new SelectList(_context.Franchises, "FranchiseId", "Name", team.FranchiseId);
            
            // Return the create view with validation errors
            return View(team);
        }

        /// <summary>
        /// Shows the form to edit an existing team.
        /// </summary>
        /// <param name="id">The unique identifier of the team to edit.</param>
        /// <returns>
        /// A view with the edit team form.
        /// </returns>
        /// <example>
        /// GET /Teams/Edit/1 -> Shows form to edit team with ID 1
        /// </example>
        public async Task<IActionResult> Edit(int? id)
        {
            // Check if ID is provided
            if (id == null || _context.Teams == null)
            {
                return NotFound();
            }

            // Find the team to edit
            var team = await _context.Teams.FindAsync(id);
            
            // Check if team exists
            if (team == null)
            {
                return NotFound();
            }
            
            // Create a select list of franchises for the dropdown
            ViewData["FranchiseId"] = new SelectList(_context.Franchises, "FranchiseId", "Name", team.FranchiseId);
            
            // Return the edit view
            return View(team);
        }

        /// <summary>
        /// Updates an existing team in the database.
        /// </summary>
        /// <param name="id">The unique identifier of the team.</param>
        /// <param name="team">The updated team object.</param>
        /// <returns>
        /// Redirects to index page if successful, or returns edit view with errors.
        /// </returns>
        /// <example>
        /// POST /Teams/Edit/1 -> Updates team with ID 1
        /// </example>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TeamId,Name,City,LogoUrl,FranchiseId")] Team team)
        {
            // Check if IDs match
            if (id != team.TeamId)
            {
                return NotFound();
            }

            // Check if model is valid
            if (ModelState.IsValid)
            {
                try
                {
                    // Update the team in the database
                    _context.Update(team);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Check if team still exists
                    if (!TeamExists(team.TeamId))
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
            
            // Create a select list of franchises for the dropdown
            ViewData["FranchiseId"] = new SelectList(_context.Franchises, "FranchiseId", "Name", team.FranchiseId);
            
            // Return the edit view with validation errors
            return View(team);
        }

        /// <summary>
        /// Shows the confirmation page to delete a team.
        /// </summary>
        /// <param name="id">The unique identifier of the team to delete.</param>
        /// <returns>
        /// A view with the delete confirmation.
        /// </returns>
        /// <example>
        /// GET /Teams/Delete/1 -> Shows confirmation to delete team with ID 1
        /// </example>
        public async Task<IActionResult> Delete(int? id)
        {
            // Check if ID is provided
            if (id == null || _context.Teams == null)
            {
                return NotFound();
            }

            // Get team with related franchise data for confirmation
            var team = await _context.Teams
                .Include(t => t.Franchise)
                .FirstOrDefaultAsync(m => m.TeamId == id);
            
            // Check if team exists
            if (team == null)
            {
                return NotFound();
            }

            // Return the delete confirmation view
            return View(team);
        }

        /// <summary>
        /// Deletes a team from the database based on their ID.
        /// </summary>
        /// <param name="id">The ID of the team to delete.</param>
        /// <returns>
        /// Redirects to index page after successful deletion.
        /// </returns>
        /// <example>
        /// POST /Teams/Delete/1 -> Deletes team with ID 1
        /// </example>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Check if teams table exists
            if (_context.Teams == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Teams'  is null.");
            }
            
            // Find the team to delete
            var team = await _context.Teams.FindAsync(id);
            
            // Delete the team if it exists
            if (team != null)
            {
                _context.Teams.Remove(team);
            }
            
            // Save changes to the database
            await _context.SaveChangesAsync();
            
            // Redirect to the index page
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Returns a list of all teams as JSON with franchise information.
        /// </summary>
        /// <example>
        /// GET /Teams/ListTeams -> [{"TeamId":1, "Name":"Super Kings A", "City":"Toronto", "FranchiseName":"Super Kings"}, ...]
        /// </example>
        /// <returns>
        /// A JSON array of TeamDto objects containing details of all teams.
        /// </returns>
        [HttpGet]
        [Route("ListTeams")]
        public async Task<IActionResult> ListTeams()
        {
            // Get all teams with their related franchise data
            var teams = await _context.Teams
                .Include(t => t.Franchise)
                .ToListAsync();

            // Create DTOs with franchise information
            var teamDtos = teams.Select(t => new TeamDto
            {
                TeamId = t.TeamId,
                Name = t.Name,
                HomeGround = t.City,
                FranchiseId = t.FranchiseId,
                FranchiseName = t.Franchise?.Name ?? "Unknown"
            }).ToList();

            // Return the JSON response
            return Json(teamDtos);
        }

        /// <summary>
        /// Retrieves a specific team's details by their ID as JSON.
        /// </summary>
        /// <param name="id">The unique identifier of the team.</param>
        /// <returns>
        /// A JSON TeamDto object containing details of the team.
        /// </returns>
        /// <example>
        /// GET /Teams/FindTeam/1 -> {"TeamId":1, "Name":"Super Kings A", "City":"Toronto", "FranchiseName":"Super Kings"}
        /// </example>
        [HttpGet]
        [Route("FindTeam/{id}")]
        public async Task<IActionResult> FindTeam(int id)
        {
            // Get team with related franchise data
            var team = await _context.Teams
                .Include(t => t.Franchise)
                .FirstOrDefaultAsync(t => t.TeamId == id);

            // Check if team exists
            if (team == null)
            {
                return NotFound();
            }

            // Create DTO with franchise information
            var teamDto = new TeamDto
            {
                TeamId = team.TeamId,
                Name = team.Name,
                HomeGround = team.City,
                FranchiseId = team.FranchiseId,
                FranchiseName = team.Franchise?.Name ?? "Unknown"
            };

            // Return the JSON response
            return Json(teamDto);
        }

        /// <summary>
        /// Adds a new team to the database via API.
        /// </summary>
        /// <param name="teamDto">The team DTO object to add.</param>
        /// <returns>
        /// JSON response with the created team details.
        /// </returns>
        /// <example>
        /// POST /Teams/CreateTeam
        /// Request Body:
        /// {
        ///     "Name": "Super Kings A",
        ///     "HomeGround": "Toronto",
        ///     "FranchiseId": 1
        /// }
        /// </example>
        [HttpPost]
        [Route("CreateTeam")]
        public async Task<IActionResult> CreateTeam([FromBody] TeamDto teamDto)
        {
            // Check if model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Create new team from DTO
            var team = new Team
            {
                Name = teamDto.Name,
                City = teamDto.HomeGround,
                FranchiseId = teamDto.FranchiseId
            };

            // Add the new team to the database
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();

            // Set the ID in the DTO for response
            teamDto.TeamId = team.TeamId;
            
            // Return the JSON response
            return Json(teamDto);
        }

        /// <summary>
        /// Updates an existing team in the database via API.
        /// </summary>
        /// <param name="id">The unique identifier of the team.</param>
        /// <param name="teamDto">The updated team DTO object.</param>
        /// <returns>
        /// JSON response confirming the update operation.
        /// </returns>
        /// <example>
        /// PUT /Teams/UpdateTeam/1
        /// Request Body:
        /// {
        ///     "TeamId": 1,
        ///     "Name": "Updated Super Kings A",
        ///     "HomeGround": "New Toronto",
        ///     "FranchiseId": 1
        /// }
        /// </example>
        [HttpPut]
        [Route("UpdateTeam/{id}")]
        public async Task<IActionResult> UpdateTeam(int id, [FromBody] TeamDto teamDto)
        {
            // Check if IDs match
            if (id != teamDto.TeamId)
            {
                return BadRequest();
            }

            // Find the team to update
            var team = await _context.Teams.FindAsync(id);
            
            // Check if team exists
            if (team == null)
            {
                return NotFound();
            }

            // Update team properties
            team.Name = teamDto.Name;
            team.City = teamDto.HomeGround;
            team.FranchiseId = teamDto.FranchiseId;

            try
            {
                // Update the team in the database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Check if team still exists
                if (!TeamExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Return success message
            return Json(new { message = "Team updated successfully" });
        }

        /// <summary>
        /// Deletes a team from the database based on their ID via API.
        /// </summary>
        /// <param name="id">The ID of the team to delete.</param>
        /// <returns>
        /// JSON response confirming the deletion operation.
        /// </returns>
        /// <example>
        /// DELETE /Teams/DeleteTeam/1 -> {"message": "Team deleted successfully"}
        /// </example>
        [HttpDelete]
        [Route("DeleteTeam/{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            // Find the team to delete
            var team = await _context.Teams.FindAsync(id);
            
            // Check if team exists
            if (team == null)
            {
                return NotFound();
            }

            // Remove the team from the database
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();

            // Return success message
            return Json(new { message = "Team deleted successfully" });
        }

        /// <summary>
        /// Checks if a team exists in the database.
        /// </summary>
        /// <param name="id">The unique identifier of the team.</param>
        /// <returns>
        /// True if the team exists, false otherwise.
        /// </returns>
        private bool TeamExists(int id)
        {
          return (_context.Teams?.Any(e => e.TeamId == id)).GetValueOrDefault();
        }
    }
}
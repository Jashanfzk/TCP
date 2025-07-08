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
    /// This controller handles player operations for the cricket league
    /// It provides endpoints for creating, viewing, editing, and deleting players
    /// </summary>
    /// <example>
    /// GET /Players - Shows list of all players
    /// POST /Players/Create - Creates a new player
    /// GET /Players/ListPlayers - Returns JSON list of players
    /// </example>
    public class PlayersController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Dependency injection of database context
        public PlayersController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Shows a list of all players with their team information
        /// </summary>
        /// <example>
        /// GET /Players -> Returns view with list of all players
        /// </example>
        /// <returns>
        /// A view containing all players with their related team data.
        /// </returns>
        public async Task<IActionResult> Index()
        {
            // Get all players with their related team data
            var players = await _context.Players
                .Include(p => p.Team)
                .ToListAsync();
            
            // Return the view with player data
            return View(players);
        }

        /// <summary>
        /// Shows details for a specific player by ID
        /// </summary>
        /// <param name="id">The unique identifier of the player.</param>
        /// <returns>
        /// A view containing the player details with related team information.
        /// </returns>
        /// <example>
        /// GET /Players/Details/1 -> Shows detailed view of player with ID 1
        /// </example>
        public async Task<IActionResult> Details(int? id)
        {
            // Check if ID is provided
            if (id == null || _context.Players == null)
            {
                return NotFound();
            }

            // Get player with related team data
            var player = await _context.Players
                .Include(p => p.Team)
                .FirstOrDefaultAsync(m => m.PlayerId == id);
            
            // Check if player exists
            if (player == null)
            {
                return NotFound();
            }

            // Return the view with player details
            return View(player);
        }

        /// <summary>
        /// Shows the form to create a new player
        /// </summary>
        /// <returns>
        /// A view with the create player form including team selection.
        /// </returns>
        /// <example>
        /// GET /Players/Create -> Shows form to create a new player
        /// </example>
        public IActionResult Create()
        {
            // Create a select list of teams for the dropdown
            ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "Name");
            
            // Return the create view
            return View();
        }

        /// <summary>
        /// Creates a new player in the database
        /// </summary>
        /// <param name="player">The player object to add.</param>
        /// <returns>
        /// Redirects to index page if successful, or returns create view with errors.
        /// </returns>
        /// <example>
        /// POST /Players/Create -> Creates a new player from form data
        /// </example>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Age,Role,TeamId")] Player player)
        {
            // Check if model is valid
            if (ModelState.IsValid)
            {
                // Add the new player to the database
                _context.Add(player);
                await _context.SaveChangesAsync();
                
                // Redirect to the index page
                return RedirectToAction(nameof(Index));
            }
            
            // Create a select list of teams for the dropdown
            ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "Name", player.TeamId);
            
            // Return the create view with validation errors
            return View(player);
        }

        /// <summary>
        /// Shows the form to edit an existing player
        /// </summary>
        /// <param name="id">The unique identifier of the player to edit.</param>
        /// <returns>
        /// A view with the edit player form.
        /// </returns>
        /// <example>
        /// GET /Players/Edit/1 -> Shows form to edit player with ID 1
        /// </example>
        public async Task<IActionResult> Edit(int? id)
        {
            // Check if ID is provided
            if (id == null || _context.Players == null)
            {
                return NotFound();
            }

            // Find the player to edit
            var player = await _context.Players.FindAsync(id);
            
            // Check if player exists
            if (player == null)
            {
                return NotFound();
            }
            
            // Create a select list of teams for the dropdown
            ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "Name", player.TeamId);
            
            // Return the edit view
            return View(player);
        }

        /// <summary>
        /// Updates an existing player in the database
        /// </summary>
        /// <param name="id">The unique identifier of the player.</param>
        /// <param name="player">The updated player object.</param>
        /// <returns>
        /// Redirects to index page if successful, or returns edit view with errors.
        /// </returns>
        /// <example>
        /// POST /Players/Edit/1 -> Updates player with ID 1
        /// </example>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PlayerId,Name,Age,Role,TeamId")] Player player)
        {
            // Check if IDs match
            if (id != player.PlayerId)
            {
                return NotFound();
            }

            // Check if model is valid
            if (ModelState.IsValid)
            {
                try
                {
                    // Update the player in the database
                    _context.Update(player);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Check if player still exists
                    if (!PlayerExists(player.PlayerId))
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
            
            // Create a select list of teams for the dropdown
            ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "Name", player.TeamId);
            
            // Return the edit view with validation errors
            return View(player);
        }

        /// <summary>
        /// Shows the confirmation page to delete a player
        /// </summary>
        /// <param name="id">The unique identifier of the player to delete.</param>
        /// <returns>
        /// A view with the delete confirmation.
        /// </returns>
        /// <example>
        /// GET /Players/Delete/1 -> Shows confirmation to delete player with ID 1
        /// </example>
        public async Task<IActionResult> Delete(int? id)
        {
            // Check if ID is provided
            if (id == null || _context.Players == null)
            {
                return NotFound();
            }

            // Get player with related team data for confirmation
            var player = await _context.Players
                .Include(p => p.Team)
                .FirstOrDefaultAsync(m => m.PlayerId == id);
            
            // Check if player exists
            if (player == null)
            {
                return NotFound();
            }

            // Return the delete confirmation view
            return View(player);
        }

        /// <summary>
        /// Deletes a player from the database based on their ID
        /// </summary>
        /// <param name="id">The ID of the player to delete.</param>
        /// <returns>
        /// Redirects to index page after successful deletion.
        /// </returns>
        /// <example>
        /// POST /Players/Delete/1 -> Deletes player with ID 1
        /// </example>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Check if players table exists
            if (_context.Players == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Players'  is null.");
            }
            
            // Find the player to delete
            var player = await _context.Players.FindAsync(id);
            
            // Delete the player if it exists
            if (player != null)
            {
                _context.Players.Remove(player);
            }
            
            // Save changes to the database
            await _context.SaveChangesAsync();
            
            // Redirect to the index page
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Returns a list of all players as JSON with team information
        /// </summary>
        /// <example>
        /// GET /Players/ListPlayers -> [{"PlayerId":1, "Name":"John Doe", "Age":25, "Role":"Batsman", "TeamName":"Super Kings A"}, ...]
        /// </example>
        /// <returns>
        /// A JSON array of PlayerDto objects containing details of all players.
        /// </returns>
        [HttpGet]
        [Route("ListPlayers")]
        public async Task<IActionResult> ListPlayers()
        {
            // Get all players with their related team data
            var players = await _context.Players
                .Include(p => p.Team)
                .ToListAsync();

            // Create DTOs with team information
            var playerDtos = players.Select(p => new PlayerDto
            {
                PlayerId = p.PlayerId,
                FirstName = p.Name,
                LastName = "",
                DateOfBirth = DateTime.Now.AddYears(-p.Age),
                Position = p.Role,
                TeamId = p.TeamId,
                TeamName = p.Team?.Name ?? "Unknown"
            }).ToList();

            // Return the JSON response
            return Json(playerDtos);
        }

        /// <summary>
        /// Retrieves a specific player's details by their ID as JSON
        /// </summary>
        /// <param name="id">The unique identifier of the player.</param>
        /// <returns>
        /// A JSON PlayerDto object containing details of the player.
        /// </returns>
        /// <example>
        /// GET /Players/FindPlayer/1 -> {"PlayerId":1, "Name":"John Doe", "Age":25, "Role":"Batsman", "TeamName":"Super Kings A"}
        /// </example>
        [HttpGet]
        [Route("FindPlayer/{id}")]
        public async Task<IActionResult> FindPlayer(int id)
        {
            // Get player with related team data
            var player = await _context.Players
                .Include(p => p.Team)
                .FirstOrDefaultAsync(p => p.PlayerId == id);

            // Check if player exists
            if (player == null)
            {
                return NotFound();
            }

            // Create DTO with team information
            var playerDto = new PlayerDto
            {
                PlayerId = player.PlayerId,
                FirstName = player.Name,
                LastName = "",
                DateOfBirth = DateTime.Now.AddYears(-player.Age),
                Position = player.Role,
                TeamId = player.TeamId,
                TeamName = player.Team?.Name ?? "Unknown"
            };

            // Return the JSON response
            return Json(playerDto);
        }

        /// <summary>
        /// Adds a new player to the database via API
        /// </summary>
        /// <param name="playerDto">The player DTO object to add.</param>
        /// <returns>
        /// JSON response with the created player details.
        /// </returns>
        /// <example>
        /// POST /Players/CreatePlayer
        /// Request Body:
        /// {
        ///     "FirstName": "John",
        ///     "LastName": "Doe",
        ///     "DateOfBirth": "1990-01-01",
        ///     "Position": "Batsman",
        ///     "TeamId": 1
        /// }
        /// </example>
        [HttpPost]
        [Route("CreatePlayer")]
        public async Task<IActionResult> CreatePlayer([FromBody] PlayerDto playerDto)
        {
            // Check if model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Create new player from DTO
            var player = new Player
            {
                Name = playerDto.FirstName + " " + playerDto.LastName,
                Age = DateTime.Now.Year - playerDto.DateOfBirth.Year,
                Role = playerDto.Position,
                TeamId = playerDto.TeamId
            };

            // Add the new player to the database
            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            // Set the ID in the DTO for response
            playerDto.PlayerId = player.PlayerId;
            
            // Return the JSON response
            return Json(playerDto);
        }

        /// <summary>
        /// Updates an existing player in the database via API
        /// </summary>
        /// <param name="id">The unique identifier of the player.</param>
        /// <param name="playerDto">The updated player DTO object.</param>
        /// <returns>
        /// JSON response confirming the update operation.
        /// </returns>
        /// <example>
        /// PUT /Players/UpdatePlayer/1
        /// Request Body:
        /// {
        ///     "PlayerId": 1,
        ///     "FirstName": "John",
        ///     "LastName": "Smith",
        ///     "DateOfBirth": "1990-01-01",
        ///     "Position": "Bowler",
        ///     "TeamId": 1
        /// }
        /// </example>
        [HttpPut]
        [Route("UpdatePlayer/{id}")]
        public async Task<IActionResult> UpdatePlayer(int id, [FromBody] PlayerDto playerDto)
        {
            // Check if IDs match
            if (id != playerDto.PlayerId)
            {
                return BadRequest();
            }

            // Find the player to update
            var player = await _context.Players.FindAsync(id);
            
            // Check if player exists
            if (player == null)
            {
                return NotFound();
            }

            // Update player properties
            player.Name = playerDto.FirstName + " " + playerDto.LastName;
            player.Age = DateTime.Now.Year - playerDto.DateOfBirth.Year;
            player.Role = playerDto.Position;
            player.TeamId = playerDto.TeamId;

            try
            {
                // Update the player in the database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Check if player still exists
                if (!PlayerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Return success message
            return Json(new { message = "Player updated successfully" });
        }

        /// <summary>
        /// Deletes a player from the database based on their ID via API
        /// </summary>
        /// <param name="id">The ID of the player to delete.</param>
        /// <returns>
        /// JSON response confirming the deletion operation.
        /// </returns>
        /// <example>
        /// DELETE /Players/DeletePlayer/1 -> {"message": "Player deleted successfully"}
        /// </example>
        [HttpDelete]
        [Route("DeletePlayer/{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            // Find the player to delete
            var player = await _context.Players.FindAsync(id);
            
            // Check if player exists
            if (player == null)
            {
                return NotFound();
            }

            // Remove the player from the database
            _context.Players.Remove(player);
            await _context.SaveChangesAsync();

            // Return success message
            return Json(new { message = "Player deleted successfully" });
        }

        /// <summary>
        /// Checks if a player exists in the database
        /// </summary>
        /// <param name="id">The unique identifier of the player.</param>
        /// <returns>
        /// True if the player exists, false otherwise.
        /// </returns>
        private bool PlayerExists(int id)
        {
          return (_context.Players?.Any(e => e.PlayerId == id)).GetValueOrDefault();
        }
    }
}
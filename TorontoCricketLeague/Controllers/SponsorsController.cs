using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TorontoCricketLeague.Data;
using TorontoCricketLeague.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TorontoCricketLeague.Controllers
{
    /// <summary>
    /// Controller for managing Sponsor entities in the Toronto Cricket League.
    /// Handles CRUD operations for sponsors including create, read, update, and delete.
    /// Provides web interface for sponsor management and franchise assignments.
    /// </summary>
    /// <example>
    /// Web Interface:
    /// GET /Sponsors - Shows list of all sponsors
    /// GET /Sponsors/Create - Shows form to create a new sponsor
    /// POST /Sponsors/Create - Creates a new sponsor from form data
    /// GET /Sponsors/Edit/1 - Shows form to edit sponsor with ID 1
    /// POST /Sponsors/Edit/1 - Updates sponsor with ID 1
    /// GET /Sponsors/Delete/1 - Shows confirmation to delete sponsor with ID 1
    /// POST /Sponsors/Delete/1 - Deletes sponsor with ID 1
    /// </example>
    /// <returns>
    /// Provides complete sponsor management with:
    /// - Web interface for administrators
    /// - Franchise assignment functionality
    /// - Proper validation and error handling
    /// </returns>
    public class SponsorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Dependency injection of database context
        public SponsorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of all sponsors in the system.
        /// </summary>
        /// <example>
        /// GET /Sponsors -> Returns view with list of all sponsors
        /// </example>
        /// <returns>
        /// A view containing all sponsors.
        /// </returns>
        public async Task<IActionResult> Index()
        {
            // Get all sponsors from the database
            var sponsors = await _context.Sponsors.ToListAsync();
            
            // Return the view with sponsor data
            return View(sponsors);
        }

        /// <summary>
        /// Shows the form to create a new sponsor.
        /// </summary>
        /// <returns>
        /// A view with the create sponsor form including franchise selection.
        /// </returns>
        /// <example>
        /// GET /Sponsors/Create -> Shows form to create a new sponsor
        /// </example>
        public IActionResult Create()
        {
            // Create a select list of franchises for the dropdown
            ViewBag.FranchiseId = new SelectList(_context.Franchises, "FranchiseId", "Name");
            
            // Return the create view
            return View();
        }

        /// <summary>
        /// Creates a new sponsor in the database.
        /// </summary>
        /// <param name="sponsor">The sponsor object to add.</param>
        /// <returns>
        /// Redirects to franchise edit page if successful, or returns create view with errors.
        /// </returns>
        /// <example>
        /// POST /Sponsors/Create -> Creates a new sponsor from form data
        /// </example>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,LogoUrl,FranchiseId")] Sponsor sponsor)
        {
            // Check if model is valid
            if (ModelState.IsValid)
            {
                // Check if the franchise exists
                var franchise = await _context.Franchises.FindAsync(sponsor.FranchiseId);
                if (franchise == null)
                {
                    // Add error message if franchise doesn't exist
                    ModelState.AddModelError("FranchiseId", "Selected franchise does not exist.");
                    return View(sponsor);
                }

                // Add the new sponsor to the database
                _context.Sponsors.Add(sponsor);
                await _context.SaveChangesAsync();
                
                // Redirect to the franchise edit page
                return RedirectToAction("Edit", "Franchises", new { id = sponsor.FranchiseId });
            }
            
            // Return the create view with validation errors
            return View(sponsor);
        }

        /// <summary>
        /// Shows the form to edit an existing sponsor.
        /// </summary>
        /// <param name="id">The unique identifier of the sponsor to edit.</param>
        /// <returns>
        /// A view with the edit sponsor form.
        /// </returns>
        /// <example>
        /// GET /Sponsors/Edit/1 -> Shows form to edit sponsor with ID 1
        /// </example>
        public async Task<IActionResult> Edit(int? id)
        {
            // Check if ID is provided
            if (id == null)
            {
                return NotFound();
            }

            // Find the sponsor to edit
            var sponsor = await _context.Sponsors.FindAsync(id);
            
            // Check if sponsor exists
            if (sponsor == null)
            {
                return NotFound();
            }
            
            // Create a select list of franchises for the dropdown
            ViewBag.FranchiseId = new SelectList(_context.Franchises, "FranchiseId", "Name", sponsor.FranchiseId);
            
            // Return the edit view
            return View(sponsor);
        }

        /// <summary>
        /// Updates an existing sponsor in the database.
        /// </summary>
        /// <param name="id">The unique identifier of the sponsor.</param>
        /// <param name="sponsor">The updated sponsor object.</param>
        /// <returns>
        /// Redirects to index page if successful, or returns edit view with errors.
        /// </returns>
        /// <example>
        /// POST /Sponsors/Edit/1 -> Updates sponsor with ID 1
        /// </example>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SponsorId,Name,LogoUrl,FranchiseId")] Sponsor sponsor)
        {
            // Check if IDs match
            if (id != sponsor.SponsorId)
            {
                return NotFound();
            }

            // Check if model is valid
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if the franchise exists
                    var franchise = await _context.Franchises.FindAsync(sponsor.FranchiseId);
                    if (franchise == null)
                    {
                        // Add error message if franchise doesn't exist
                        ModelState.AddModelError("FranchiseId", "Selected franchise does not exist.");
                        ViewBag.FranchiseId = new SelectList(_context.Franchises, "FranchiseId", "Name", sponsor.FranchiseId);
                        return View(sponsor);
                    }

                    // Update the sponsor in the database
                    _context.Update(sponsor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Check if sponsor still exists
                    if (!_context.Sponsors.Any(e => e.SponsorId == id))
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
            ViewBag.FranchiseId = new SelectList(_context.Franchises, "FranchiseId", "Name", sponsor.FranchiseId);
            
            // Return the edit view with validation errors
            return View(sponsor);
        }

        /// <summary>
        /// Shows the confirmation page to delete a sponsor.
        /// </summary>
        /// <param name="id">The unique identifier of the sponsor to delete.</param>
        /// <returns>
        /// A view with the delete confirmation.
        /// </returns>
        /// <example>
        /// GET /Sponsors/Delete/1 -> Shows confirmation to delete sponsor with ID 1
        /// </example>
        public async Task<IActionResult> Delete(int? id)
        {
            // Check if ID is provided
            if (id == null)
            {
                return NotFound();
            }

            // Get sponsor for confirmation
            var sponsor = await _context.Sponsors
                .FirstOrDefaultAsync(m => m.SponsorId == id);
            
            // Check if sponsor exists
            if (sponsor == null)
            {
                return NotFound();
            }

            // Return the delete confirmation view
            return View(sponsor);
        }

        /// <summary>
        /// Deletes a sponsor from the database based on their ID.
        /// </summary>
        /// <param name="id">The ID of the sponsor to delete.</param>
        /// <returns>
        /// Redirects to index page after successful deletion.
        /// </returns>
        /// <example>
        /// POST /Sponsors/Delete/1 -> Deletes sponsor with ID 1
        /// </example>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Find the sponsor to delete
            var sponsor = await _context.Sponsors.FindAsync(id);
            
            // Delete the sponsor if it exists
            if (sponsor != null)
            {
                _context.Sponsors.Remove(sponsor);
                await _context.SaveChangesAsync();
            }
            
            // Redirect to the index page
            return RedirectToAction(nameof(Index));
        }
    }
} 
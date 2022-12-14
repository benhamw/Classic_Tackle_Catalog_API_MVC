using api_of_your_choice.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace api_of_your_choice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlyrodController : ControllerBase
    {
        private FlyrodContext _db;

        public FlyrodController(FlyrodContext db)
        {
            _db = db;
        }

        // GET/POST/UPDATE/DELETE STARTS HERE

        //Get all Flyrods
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Flyrod>>> GetFlyrods()
        {
            if (_db.Flyrods == null)
            {
                return NotFound();
            }
            return await _db.Flyrods.Include(m => m.Maker).OrderBy(m => m.Maker.Name).ToListAsync();
        }

        // GET: 1 flyrod from the database
        [HttpGet("{id}")]
        public async Task<ActionResult<Flyrod>> GetFlyrod(int id)
        {
            if (_db.Flyrods == null)
            {
                return NotFound();
            }
            var flyrod = await _db.Flyrods.Include(m => m.Maker).Include(f => f.Maker.flyrods).FirstOrDefaultAsync(i => i.Id == id);

            if (flyrod == null)
            {
                return NotFound();
            }

            return flyrod;
        }

        // PUT: Update a single record in the database
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFlyrod(int id, Flyrod flyrod)
        {
            if (id != flyrod.Id)
            {
                return BadRequest();
            }

            _db.Entry(flyrod).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlyrodExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: Create/Insert a Flyrod into the database
        [HttpPost]
        public async Task<ActionResult<Flyrod>> PostFlyrod(Flyrod flyrod)
        {
            if (_db.Flyrods == null)
            {
                return Problem("Entity set 'FlyrodContext.Flyrods'  is null.");
            }

            _db.Flyrods.Add(flyrod);
            await _db.SaveChangesAsync();

            return CreatedAtAction("GetFlyrod", new { id = flyrod.Id }, flyrod);
        }

        // DELETE: Delete a flyrod from the database
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlyrod(int id)
        {
            if (_db.Flyrods == null)
            {
                return NotFound();
            }
            var flyrod = await _db.Flyrods.FindAsync(id);
            if (flyrod == null)
            {
                return NotFound();
            }

            _db.Flyrods.Remove(flyrod);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        //Check whether Flyrod exists
        private bool FlyrodExists(int id)
        {
            return (_db.Flyrods?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

using api_of_your_choice.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_of_your_choice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MakerController : ControllerBase
    {
        private FlyrodContext _db;

        public MakerController(FlyrodContext db)
        {
            _db = db;
        }

        // GET/POST/UPDATE/DELETE STARTS HERE

        // GET: Read all makers from the database
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Maker>>> GetMakers()
        {
            if (_db.Makers == null)
            {
                return NotFound();
            }
            return await _db.Makers.Include(f => f.flyrods).OrderBy(m => m.Name).ToListAsync();
        }

        // GET: Read 1 maker from the database
        [HttpGet("{id}")]
        public async Task<ActionResult<Maker>> GetMaker(int? id)
        {
            if (id == null || _db.Makers == null)
            {
                return NotFound();
            }

            var maker = await _db.Makers.Include(f => f.flyrods).FirstOrDefaultAsync(i => i.Id == id);

            if (maker == null)
            {
                return NotFound();
            }

            return maker;
        }

        // PUT: Update a single record in the database
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMaker(int id, Maker maker)
        {
            if (id != maker.Id)
            {
                return BadRequest();
            }

            _db.Entry(maker).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MakerExists(id))
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

        // POST: Create/Insert a maker into the database
        [HttpPost]
        public async Task<ActionResult<Maker>> PostMaker(Maker maker)
        {
            if (_db.Makers == null)
            {
                return Problem("Entity set 'MakerContext.Makers'  is null.");
            }

            _db.Makers.Add(maker);
            await _db.SaveChangesAsync();

            return CreatedAtAction("GetMaker", new { id = maker.Id }, maker);
        }

        // DELETE: Delete a maker from the database
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaker(int id)
        {
            if (_db.Makers == null)
            {
                return NotFound();
            }
            var maker = await _db.Makers.FindAsync(id);
            if (maker == null)
            {
                return NotFound();
            }

            _db.Makers.Remove(maker);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        //Check whether a maker exists
        private bool MakerExists(int id)
        {
            return (_db.Makers?.Any(m => m.Id == id)).GetValueOrDefault();
        }
    }
}

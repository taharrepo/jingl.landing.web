using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using admin.jingl.net.Models;

namespace admin.jingl.net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LandingRegistrationsAPIController : ControllerBase
    {
        private readonly JINGLDBContext _context;

        public LandingRegistrationsAPIController(JINGLDBContext context)
        {
            _context = context;
        }

        // GET: api/LandingRegistrationsAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LandingRegistration>>> GetLandingRegistration()
        {
            return await _context.LandingRegistration.ToListAsync();
        }

        // GET: api/LandingRegistrationsAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LandingRegistration>> GetLandingRegistration(long id)
        {
            var landingRegistration = await _context.LandingRegistration.FindAsync(id);

            if (landingRegistration == null)
            {
                return NotFound();
            }

            return landingRegistration;
        }

        // PUT: api/LandingRegistrationsAPI/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLandingRegistration(long id, LandingRegistration landingRegistration)
        {
            if (id != landingRegistration.Id)
            {
                return BadRequest();
            }

            _context.Entry(landingRegistration).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LandingRegistrationExists(id))
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

        // POST: api/LandingRegistrationsAPI
        [HttpPost]
        public async Task<ActionResult<LandingRegistration>> PostLandingRegistration(LandingRegistration landingRegistration)
        {
            _context.LandingRegistration.Add(landingRegistration);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLandingRegistration", new { id = landingRegistration.Id }, landingRegistration);
        }

        // DELETE: api/LandingRegistrationsAPI/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<LandingRegistration>> DeleteLandingRegistration(long id)
        {
            var landingRegistration = await _context.LandingRegistration.FindAsync(id);
            if (landingRegistration == null)
            {
                return NotFound();
            }

            _context.LandingRegistration.Remove(landingRegistration);
            await _context.SaveChangesAsync();

            return landingRegistration;
        }

        private bool LandingRegistrationExists(long id)
        {
            return _context.LandingRegistration.Any(e => e.Id == id);
        }
    }
}

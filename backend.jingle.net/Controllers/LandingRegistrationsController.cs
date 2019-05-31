using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.jingle.net.Models;

namespace backend.jingle.net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LandingRegistrationsController : ControllerBase
    {
        private readonly JINGLDBContext _context;

        public LandingRegistrationsController(JINGLDBContext context)
        {
            _context = context;
        }

        // GET: api/LandingRegistrations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LandingRegistration>>> GetLandingRegistration()
        {
            return await _context.LandingRegistration.ToListAsync();
        }

        // GET: api/LandingRegistrations/5
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

        // PUT: api/LandingRegistrations/5
        [HttpPut("{id}")]
        public IActionResult PutLandingRegistration(long id, LandingRegistrationRequest landingRegistration)
        {
            if (id != landingRegistration.Id)
            {
                return BadRequest();
            }

            _context.Entry(landingRegistration).State = EntityState.Modified;

            try
            {
                //await _context.SaveChangesAsync();
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

        // POST: api/LandingRegistrations
        [HttpPost]
        public async Task<ActionResult<LandingRegistration>> PostLandingRegistration(LandingRegistrationRequest landingRegistration)
        {            
            LandingRegistration ld = new LandingRegistration();
            ld.Name = landingRegistration.Name;
            ld.Email = landingRegistration.Email;
            ld.SocialMedia = landingRegistration.SocialMedia;
            ld.VideoYes = landingRegistration.VideoYes;
            ld.FileUrl = landingRegistration.FileUrl;
            ld.SessionId = Guid.NewGuid().ToString();
            ld.RegistrationDate = DateTime.Now;
            ld.RegisteredIp = landingRegistration.RegisteredIp;
            ld.RegisteredUserAgent = landingRegistration.RegisteredUserAgent;
            ld.UniqueKey = Guid.NewGuid().ToString();
            ld.UniqueKeyConfirm = false;
            ld.InstagramUrl = landingRegistration.InstagramUrl;
            ld.FacebookUrl = landingRegistration.FacebookUrl;
            ld.TwitterUrl = landingRegistration.TwitterUrl;
            ld.GoogleUrl = landingRegistration.GoogleUrl;
            ld.LinkedInUrl = landingRegistration.LinkedInUrl;
            _context.LandingRegistration.Add(ld);

            UserAuth userauth = new UserAuth();
            userauth.Password = landingRegistration.Password;
            userauth.LastLogin = null;
            userauth.UniqueKey = ld.UniqueKey;
            userauth.Ipaddress = ld.RegisteredIp;
            userauth.UserAgent = ld.RegisteredUserAgent;
            userauth.LoginType = landingRegistration.LoginType;
            userauth.Email = ld.Email;
            _context.UserAuth.Add(userauth);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLandingRegistration", new { id = ld.Id }, ld);
        }

        // DELETE: api/LandingRegistrations/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<LandingRegistration>> DeleteLandingRegistration(long id)
        {
            var landingRegistration = await _context.LandingRegistration.FindAsync(id);
            if (landingRegistration == null)
            {
                return NotFound();
            }

            _context.LandingRegistration.Remove(landingRegistration);
            //await _context.SaveChangesAsync();

            return landingRegistration;
        }

        private bool LandingRegistrationExists(long id)
        {
            return _context.LandingRegistration.Any(e => e.Id == id);
        }
    }
}

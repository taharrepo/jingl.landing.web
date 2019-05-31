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
    public class UserAuthsController : ControllerBase
    {
        private readonly JINGLDBContext _context;

        public UserAuthsController(JINGLDBContext context)
        {
            _context = context;
        }

        // GET: api/UserAuths
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserAuth>>> GetUserAuth()
        {
            return await _context.UserAuth.ToListAsync();
        }

        // GET: api/UserAuths/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserAuth>> GetUserAuth(long id)
        {
            var userAuth = await _context.UserAuth.FindAsync(id);

            if (userAuth == null)
            {
                return NotFound();
            }

            return userAuth;
        }

        // PUT: api/UserAuths/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserAuth(long id, UserAuth userAuth)
        {
            if (id != userAuth.Id)
            {
                return BadRequest();
            }

            _context.Entry(userAuth).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserAuthExists(id))
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

        // POST: api/UserAuths
        [HttpPost]
        public async Task<ActionResult<UserAuth>> PostUserAuth(UserAuth userAuth)
        {
            _context.UserAuth.Add(userAuth);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserAuthExists(userAuth.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUserAuth", new { id = userAuth.Id }, userAuth);
        }

        // DELETE: api/UserAuths/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserAuth>> DeleteUserAuth(long id)
        {
            var userAuth = await _context.UserAuth.FindAsync(id);
            if (userAuth == null)
            {
                return NotFound();
            }

            _context.UserAuth.Remove(userAuth);
            await _context.SaveChangesAsync();

            return userAuth;
        }

        private bool UserAuthExists(long id)
        {
            return _context.UserAuth.Any(e => e.Id == id);
        }
    }
}

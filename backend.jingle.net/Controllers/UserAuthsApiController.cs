using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.jingle.net.Models;
using static backend.jingle.net.Logic.Helper;

namespace backend.jingle.net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthsApiController : ControllerBase
    {
        private readonly JINGLDBContext _context;

        public UserAuthsApiController(JINGLDBContext context)
        {
            _context = context;
        }

        // GET: api/UserAuthsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserAuth>>> GetUserAuth()
        {
            return await _context.UserAuth.ToListAsync();
        }

        // GET: api/UserAuthsApi/5
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

        // PUT: api/UserAuthsApi/5
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

        // POST: api/UserAuthsApi
        [HttpPost]
        public async Task<ActionResult<UserAuth>> PostUserAuth(UserAuth userAuth)
        {

            var xx = _context.UserAuth.FirstOrDefault(x => x.Id == userAuth.Id);

            var message = userAuth.Password;
            var salt = Salt.Create();
            var hash = Hash.Create(message, salt);

            xx.Salt = salt;
            xx.Password = hash;
           
                await _context.SaveChangesAsync();

            
                return CreatedAtAction("GetUserAuth", new { id = userAuth.Id }, userAuth);
           
            
        }

        // DELETE: api/UserAuthsApi/5
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

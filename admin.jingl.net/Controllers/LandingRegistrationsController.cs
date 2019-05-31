using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using admin.jingl.net.Models;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using static admin.jingl.net.Logic.Helper;
using Microsoft.AspNetCore.Identity;
using System.IO;
using Microsoft.AspNetCore.Routing;

namespace admin.jingl.net.Controllers
{
    public class LandingRegistrationsController : Controller
    {
        private readonly JINGLDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LandingRegistrationsController(JINGLDBContext context)
        {
            _context = context;
        }

        // GET: LandingRegistrations
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View(await _context.LandingRegistration.ToListAsync());
            }
            else
            {              
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: LandingRegistrations/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var landingRegistration = await _context.LandingRegistration
                .FirstOrDefaultAsync(m => m.Id == id);

            if (landingRegistration == null)
            {
                return NotFound();
            } else
            {

                string video = landingRegistration.FileUrl;
                if (video == null)
                {
                    landingRegistration.FileUrl = "";
                }
                else
                {
                    if (video.Length > 0)
                    {
                        video = video.Substring(video.IndexOf("wwwroot") + 8);
                        video = "http://www.jingl.com/" + video.Replace(" ", "%20");
                        landingRegistration.FileUrl = video;
                    }
                }
                
            }

            var admremarks = _context.AdminRemarks.Where(x => x.UserID == id).ToList();

            ViewData["Message"] = admremarks;

            return View(landingRegistration);
        }

        // GET: LandingRegistrations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LandingRegistrations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,SocialMedia,VideoYes,FileUrl,SessionId,RegistrationDate,RegisteredIp,RegisteredUserAgent,UniqueKey,UniqueKeyConfirm,InstagramUrl,FacebookUrl,TwitterUrl,GoogleUrl,LinkedInUrl")] LandingRegistration landingRegistration)
        {
            if (ModelState.IsValid)
            {
                _context.Add(landingRegistration);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(landingRegistration);
        }

        // GET: LandingRegistrations/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var landingRegistration = await _context.LandingRegistration.FindAsync(id);
            if (landingRegistration == null)
            {
                return NotFound();
            }
            return View(landingRegistration);
        }

        // POST: LandingRegistrations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, IFormCollection form, IFormFile file)
        {
            string messagetext = form["adminremarks"];
            var landingRegistration = await _context.LandingRegistration
               .FirstOrDefaultAsync(m => m.Id == id);
            if (messagetext.Length > 0)
            {
                AdminRemarks message = new AdminRemarks();
                message.CreatedDate = DateTime.Now;
                message.CreatedEmail = "";
                message.Remarks = messagetext;
                message.UpdatedDate = DateTime.Now;
                message.UpdatedEmail = "";
                message.UserEmail = "";
                message.UserID = id;
                _context.AdminRemarks.Add(message);
              
                var admremarks = _context.AdminRemarks.Where(x => x.UserID == id).ToList();

                ViewData["Message"] = admremarks;
            }

            if (file == null || file.Length == 0)
            {

            } else
            {
                var path = Path.Combine(
                       @"/home/ubuntu/jinglbackend/backend.landing/Jingl.Web/wwwroot",
                       landingRegistration.UniqueKey + "-" + file.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                landingRegistration.FileUrl = path;
                landingRegistration.UploadDate = DateTime.Now;
              
            }

            var routeValues = new RouteValueDictionary
                {
                        { "id", landingRegistration.Id }
                };

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), routeValues);
        }

        // GET: LandingRegistrations/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var landingRegistration = await _context.LandingRegistration
                .FirstOrDefaultAsync(m => m.Id == id);

            landingRegistration.FileUrl = null;

            var routeValues = new RouteValueDictionary
                {
                        { "id", landingRegistration.Id }
                };

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), routeValues);
        }

        // POST: LandingRegistrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var landingRegistration = await _context.LandingRegistration.FindAsync(id);
            _context.LandingRegistration.Remove(landingRegistration);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(IFormCollection collection)
        {
            var email = collection["email"];
            var password = collection["password"];

            var getsalt = _context.AdminAuth.FirstOrDefault(x => x.AdminEmail == email && x.IsActive != 0);
            var salt = getsalt.Salt;
            var hash = Hash.Create(password, salt);

            var match = Hash.Validate(password, salt, hash);

            if (!match)
            {
                //gagal sign
                return View();
            }
            else
            {
                await this.SignInUser(email, false);
                return RedirectToAction(nameof(Index));
            }


        }

        private async Task SignInUser(string username, bool isPersistent)
        {
            var claims = new List<Claim>
                {
                    new Claim("user", username),
                    new Claim("role", "Member")                    
                };

          await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", "user", "role")));
        }

        private bool LandingRegistrationExists(long id)
        {
            return _context.LandingRegistration.Any(e => e.Id == id);
        }
    }
}

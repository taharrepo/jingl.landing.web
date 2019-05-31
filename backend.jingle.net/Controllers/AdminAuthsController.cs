using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using backend.jingle.net.Models;
using Microsoft.AspNetCore.Http;
using static backend.jingle.net.Logic.Helper;

namespace backend.jingle.net.Controllers
{
    public class AdminAuthsController : Controller
    {
        private readonly JINGLDBContext _context;

        public AdminAuthsController(JINGLDBContext context)
        {
            _context = context;
        }

        public IActionResult LoginAdmin()
        {
            
                HttpContext.Session.SetString("_adminstatus", "OK");
                ViewBag.Admin = "OK";
           
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LoginAdmin(IFormCollection collection)
        {
            var masterkey = collection["masterkey"];
            var stringx = HttpContext.Session.GetString("_adminstatus");
            if (HttpContext.Session.GetString("_adminstatus") == "ZERO")
            {
                if (masterkey == "j1ngl:master")
                {
                    if (string.IsNullOrEmpty(HttpContext.Session.GetString("_logged")))
                    {
                        HttpContext.Session.SetString("_adminemail", "");
                        HttpContext.Session.SetString("_logged", "masuk");
                    }
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    if (string.IsNullOrEmpty(HttpContext.Session.GetString("_logged")))
                    {
                        HttpContext.Session.SetString("_adminemail", "");
                        HttpContext.Session.SetString("_logged", "error ZERO");
                    }
                    return RedirectToAction(nameof(Error));
                }
            } else
            {
                var email = collection["email"];
                var password = collection["password"];

                var getsalt = _context.AdminAuth.FirstOrDefault(x => x.AdminEmail == email && x.IsActive != 0);
                var salt = getsalt.Salt;
                var hash = Hash.Create(password, salt);

                var match = Hash.Validate(password, salt, hash);

                if (!match)
                {
                    if (string.IsNullOrEmpty(HttpContext.Session.GetString("_logged")))
                    {
                        HttpContext.Session.SetString("_adminemail", "");
                        HttpContext.Session.SetString("_logged", "error Auth");
                    }
                    return RedirectToAction(nameof(Error));
                } else
                {
                    if (string.IsNullOrEmpty(HttpContext.Session.GetString("_logged")))
                    {
                        HttpContext.Session.SetString("_adminemail", email);
                        HttpContext.Session.SetString("_logged", "masuk");
                    }
                    return RedirectToAction(nameof(Index));
                }                
            }
        }

        public IActionResult Error()
        {
            ViewBag.Error = HttpContext.Session.GetString("_logged");
            return View();
        }

        // GET: AdminAuths
        public async Task<IActionResult> Index()
        {
            var idsession = HttpContext.Session.GetString("_logged");

            if (idsession == null || idsession == "")
            {
                return RedirectToAction(nameof(LoginAdmin));

            }
            else
            {
                return View(await _context.AdminAuth.Where(x => x.IsActive != 0).ToListAsync());
            }

            // return View(await _context.AdminAuth.ToListAsync());
        }

        // GET: AdminAuths/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminAuth = await _context.AdminAuth
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adminAuth == null)
            {
                return NotFound();
            }

            return View(adminAuth);
        }

        // GET: AdminAuths/Create
        public IActionResult Create()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("_logged")))
            {
               
                HttpContext.Session.SetString("_logged", "masuk");
            }
            return View();
        }

        // POST: AdminAuths/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AdminEmail,AdminPassword,CreatedDate,LastLogin,IPAddress,UserAgent")] AdminAuth adminAuth)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("_logged")))
            {
                
                HttpContext.Session.SetString("_logged", "masuk");
            }
            if (ModelState.IsValid)
            {
                var insertdb = new AdminAuth();

                var message = adminAuth.AdminPassword;
                var salt = Salt.Create();
                var hash = Hash.Create(message, salt);

                insertdb.AdminEmail = adminAuth.AdminEmail;
                insertdb.Salt = salt;
                insertdb.AdminPassword = hash;
                insertdb.CreatedDate = DateTime.Now;
                insertdb.IPAddress = adminAuth.IPAddress;
                insertdb.IsActive = 1;
                insertdb.UserAgent = Request.Headers["User-Agent"];

                _context.Add(insertdb);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(adminAuth);
        }

        // GET: AdminAuths/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("_logged")))
            {
                
                HttpContext.Session.SetString("_logged", "masuk");
            }
            if (id == null)
            {
                return NotFound();
            }

            var adminAuth = await _context.AdminAuth.FindAsync(id);
            if (adminAuth == null)
            {
                return NotFound();
            }
            return View(adminAuth);
        }

        // POST: AdminAuths/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,AdminEmail,AdminPassword,CreatedDate,LastLogin,IPAddress,UserAgent")] AdminAuth adminAuth)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("_logged")))
            {           
                HttpContext.Session.SetString("_logged", "masuk");
            }
            if (id != adminAuth.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var updatedb = await _context.AdminAuth.FindAsync(id);

                    if (adminAuth.AdminPassword.Length > 0)
                    {
                        var message = adminAuth.AdminPassword;
                        var salt = Salt.Create();
                        var hash = Hash.Create(message, salt);

                        updatedb.Salt = salt;
                        updatedb.AdminPassword = hash;
                    }
                    updatedb.IPAddress = adminAuth.IPAddress;
                    updatedb.CreatedDate = DateTime.Now;
                    updatedb.UserAgent = Request.Headers["User-Agent"];                   

                    _context.Update(updatedb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminAuthExists(adminAuth.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(adminAuth);
        }

        // GET: AdminAuths/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminAuth = await _context.AdminAuth
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adminAuth == null)
            {
                return NotFound();
            }

            return View(adminAuth);
        }

        // POST: AdminAuths/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("_logged")))
            {

                HttpContext.Session.SetString("_logged", "masuk");
            }

            var adminAuth = await _context.AdminAuth.FindAsync(id);
            adminAuth.IsActive = 0;
            _context.Update(adminAuth);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        private bool AdminAuthExists(long id)
        {
            return _context.AdminAuth.Any(e => e.Id == id);
        }
    }
}

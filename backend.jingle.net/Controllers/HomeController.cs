﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using backend.jingle.net.Models;
using MailChimp.Net.Interfaces;
using MailChimp.Net.Models;
using MailChimp.Net;
using Microsoft.AspNetCore.Http;
using System.IO;

using Microsoft.AspNetCore.Routing;
using static backend.jingle.net.Logic.Helper;

namespace backend.jingle.net.Controllers
{
    public class HomeController : Controller
    {
        const string SessionName = "_ReffCode";
        private readonly JINGLDBContext _context;

        public HomeController(JINGLDBContext context)
        {
            _context = context;
        }
        public IActionResult Index(string reff)
        {
            if (reff != null)
            {
                HttpContext.Session.SetString(SessionName, reff);
            }
            return View();
        }

        public IActionResult About()
        {
            //ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            //ViewData["Message"] = "Your contact page.";

            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactMessage contact)
        {

            ContactMessage cm = new ContactMessage
            {
                CreatedDate = DateTime.Now,
                EmailUser = contact.EmailUser,
                IPAddress = contact.IPAddress,
                Message = contact.Message,
                Name = contact.Name,
                UserAgent = Request.Headers["User-Agent"]
            };

            _context.ContactMessage.Add(cm);

            try
            {
                _context.SaveChanges();
                ViewBag.Message = "Terima kasih telah menghubungi kami.";
            }
            catch
            {
                ViewBag.Message = "Mohon maaf, terdapat kesalahan di sistem. Silahkan input pesan kembali.";
            }
            return View();
        }

        public IActionResult Registration(string reff)
        {
            if (reff != null)
            {
                ViewBag.Reff = reff;
            }
            else
            {
                ViewBag.Reff = HttpContext.Session.GetString(SessionName);
            }
            return View();
        }

        [HttpPost]        
        public async Task<IActionResult> Registration(LandingRegistrationRequest landingRegistration, string reff)
        {
            var landingcheck = _context.LandingRegistration.FirstOrDefault(x => x.Email == landingRegistration.Email);

            if (landingcheck == null)
            {

                var userAgent = Request.Headers["User-Agent"];

                LandingRegistration ld = new LandingRegistration();
                ld.Name = landingRegistration.Name;
                ld.Email = landingRegistration.Email;
                ld.SocialMedia = landingRegistration.SocialMedia;
                ld.VideoYes = landingRegistration.VideoYes;
                ld.FileUrl = landingRegistration.FileUrl;
                ld.SessionId = Guid.NewGuid().ToString();
                ld.RegistrationDate = DateTime.Now;
                ld.RegisteredIp = landingRegistration.RegisteredIp;
                ld.RegisteredUserAgent = userAgent;
                ld.UniqueKey = Guid.NewGuid().ToString();
                ld.UniqueKeyConfirm = false;
                ld.InstagramUrl = landingRegistration.InstagramUrl;
                ld.FacebookUrl = landingRegistration.FacebookUrl;
                ld.TwitterUrl = landingRegistration.TwitterUrl;
                ld.GoogleUrl = landingRegistration.GoogleUrl;
                ld.LinkedInUrl = landingRegistration.LinkedInUrl;

                if (landingRegistration.Referral != null)
                {
                    if (landingRegistration.Referral.Length > 0)
                    {
                        ld.Referral = landingRegistration.Referral;
                    }
                }

                if (landingRegistration.Name.Length < 4)
                {
                    Random rdn = new Random();
                    int value = rdn.Next(1000);
                    string text = value.ToString("000");
                    ld.ReferralCode = landingRegistration.Name + text;
                } else
                {
                    Random rdn = new Random();
                    int value = rdn.Next(1000);
                    string text = value.ToString("000");
                    ld.ReferralCode = landingRegistration.Name.ToLower().Substring(0,4) + text;
                }
                
                _context.LandingRegistration.Add(ld);

                UserAuth userauth = new UserAuth();

                var message = landingRegistration.Password;
                var salt = Salt.Create();
                var hash = Hash.Create(message, salt);

                userauth.Salt = salt;
                userauth.Password = hash;
                userauth.LastLogin = null;
                userauth.UniqueKey = ld.UniqueKey;
                userauth.Ipaddress = ld.RegisteredIp;
                userauth.UserAgent = ld.RegisteredUserAgent;
                userauth.LoginType = landingRegistration.LoginType;
                userauth.Email = ld.Email;
                _context.UserAuth.Add(userauth);

                await _context.SaveChangesAsync();

                //try
                //{
                //    IMailChimpManager mailChimpManager = new MailChimpManager("5d37a4dcc7a09f5bb6b96efbebd41233-us20");
                //    var listId = "1006a7a0f5";
                //    // Use the Status property if updating an existing member
                //    var member = new Member { EmailAddress = landingRegistration.Email, StatusIfNew = Status.Subscribed };
                //    member.MergeFields.Add("FNAME", landingRegistration.Name);
                //    member.MergeFields.Add("LNAME", "");
                //    await mailChimpManager.Members.AddOrUpdateAsync(listId, member);

                //}
                //catch (Exception es)
                //{
                //    ViewBag.Error = es.Message;
                //}

                //send email here
                EmailHelper.SendEmail(ld.Email, ld.Name, ld.ReferralCode);


                if (string.IsNullOrEmpty(HttpContext.Session.GetString("_email")))
                {
                    HttpContext.Session.SetString("_email", landingRegistration.Email);
                    HttpContext.Session.SetString("_idunique", ld.UniqueKey);
                }

                var routeValues = new RouteValueDictionary
                {
                        { "id", ld.UniqueKey }
                };

                return RedirectToAction(nameof(RegistrationSuccess), routeValues);

            } else
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("_error")))
                {
                    HttpContext.Session.SetString("_error", "email Anda sudah pernah terdaftar.");
                }
                return RedirectToAction(nameof(Error));
            }
            
        }

        public IActionResult Error()
        {
            ViewBag.ErrorMessage = HttpContext.Session.GetString("_error");
            return View();
        }


        public IActionResult RegistrationSuccess()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RegistrationSuccess(string id)
        {
          
                var routeValues = new RouteValueDictionary
                {
                        { "id", id }
                }; 
    
            return RedirectToAction(nameof(VideoUpload), routeValues);
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgotPassword(UserAuth usr)
        {

            var getsalt = _context.UserAuth.FirstOrDefault(x => x.Email == usr.Email);

            if (getsalt == null)
            {
                ViewBag.Message = "Email yang Anda masukkan salah atau belum terdaftar!!";
            } else
            {
                string url = "http://jingl.net/Home/ResetPassword/" + getsalt.UniqueKey;
                EmailHelper.SendEmailForgot(getsalt.Email,url);
                ViewBag.Message = "Email untuk reset password sudah dikirim. Silahkan cek email Anda.";
            }
            return View();


        }

        public IActionResult ResetPassword(string uniquekey)
        {
            var routeValues = RouteData.Values;
            ViewBag.Id = routeValues["id"];
            string xx = ViewBag.Id;
            var getsalt = _context.UserAuth.FirstOrDefault(x => x.UniqueKey == xx);
            if (getsalt == null)
            {
                return NotFound();
            } else
            {
                return View(getsalt);
            }
            
        }
        [HttpPost]
        public IActionResult ResetPassword(UserAuth usr)
        {

            var getsalt = _context.UserAuth.FirstOrDefault(x => x.Email == usr.Email);          
            var userAgent = Request.Headers["User-Agent"];
            var salt = Salt.Create();
            var hash = Hash.Create(usr.Password, salt);

            getsalt.Salt = salt;
            getsalt.Password = hash;



            if (string.IsNullOrEmpty(HttpContext.Session.GetString("_email")))
                {
                    HttpContext.Session.SetString("_email", getsalt.Email);
                    HttpContext.Session.SetString("_idunique", getsalt.UniqueKey);
                }
                var routeValues = new RouteValueDictionary
                {
                        { "id", getsalt.UniqueKey }
                };


                getsalt.LastLogin = DateTime.Now;
                getsalt.UserAgent = userAgent;
                getsalt.Ipaddress = usr.Ipaddress;
                _context.SaveChanges();
                return RedirectToAction(nameof(VideoUpload), routeValues);
            


        }

        [HttpPost]
        public IActionResult Login(UserAuth usr)
        {

            var getsalt = _context.UserAuth.FirstOrDefault(x => x.Email == usr.Email);
            var salt = getsalt.Salt;
            var hash = getsalt.Password;
            var userAgent = Request.Headers["User-Agent"];
            var match = Hash.Validate(usr.Password, salt, hash);
            
            if (!match)
            {
                ViewBag.Message = "Email atau password Anda salah.";
                return View();

            } else
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("_email")))
                {
                    HttpContext.Session.SetString("_email", getsalt.Email);
                    HttpContext.Session.SetString("_idunique", getsalt.UniqueKey);
                }
                var routeValues = new RouteValueDictionary
                {
                        { "id", getsalt.UniqueKey }
                };


                getsalt.LastLogin = DateTime.Now;
                getsalt.UserAgent = userAgent;
                getsalt.Ipaddress = usr.Ipaddress;
                _context.SaveChanges();
                return RedirectToAction(nameof(VideoUpload), routeValues);
            }

            
        }

        public IActionResult UploadSuccess()
        {
            return View();
        }

        public IActionResult Video()
        {
            return View();
        }

        public IActionResult VideoUpload()
        {  
            var routeValues = RouteData.Values;
            ViewBag.Id = routeValues["id"];
            string unique = ViewBag.Id;
            var checkvideo = _context.LandingRegistration.FirstOrDefault(x => x.UniqueKey == unique);

            string video = checkvideo.FileUrl;
            if (video == null)
            {
                checkvideo.FileUrl = "";
            }
            else
            {
                if (video.Length > 0)
                {
                    video = video.Substring(video.IndexOf("wwwroot") + 8);
                    video = "http://www.jingl.net/" + video.Replace(" ", "%20");
                    checkvideo.FileUrl = video;
                }
            }

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("_idunique")))
            {
                ViewBag.Id = HttpContext.Session.GetString("_idunique");
            }

            return View(checkvideo);
        }

        [HttpPost]
        public async Task<IActionResult> VideoUpload(IFormFile file)
        {
            var routeValues = RouteData.Values;
            ViewBag.Id = routeValues["id"];
            string idsession = ViewBag.Id;

            //HttpContext.Session.GetString("_email");
            if (file == null || file.Length == 0)
                return Content("file not selected");

            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot",
                        idsession + "-" +file.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }            

            LandingRegistration ld = _context.LandingRegistration.FirstOrDefault(x => x.UniqueKey == idsession);
            ld.FileUrl = path;
            ld.UploadDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(UploadSuccess));
        }


        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}

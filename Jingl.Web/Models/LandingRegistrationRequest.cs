using System;
using System.Collections.Generic;

namespace Jingl.Web.Models
{
    public partial class LandingRegistrationRequest
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string SocialMedia { get; set; }
        public bool VideoYes { get; set; }
        public string FileUrl { get; set; }
        public string SessionId { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string RegisteredIp { get; set; }
        public string RegisteredUserAgent { get; set; }
        public string UniqueKey { get; set; }
        public bool? UniqueKeyConfirm { get; set; }
        public string InstagramUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string GoogleUrl { get; set; }
        public string LinkedInUrl { get; set; }
        public string Password { get; set; }
        public string LoginType { get; set; }
        public string Referral { get; set; }
    }
}

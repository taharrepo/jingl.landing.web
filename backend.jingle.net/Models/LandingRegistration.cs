using System;
using System.Collections.Generic;

namespace backend.jingle.net.Models
{
    public partial class LandingRegistration
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string SocialMedia { get; set; }
        public bool? VideoYes { get; set; }
        public string FileUrl { get; set; }
        public string SessionId { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime? UploadDate { get; set; }
        public string RegisteredIp { get; set; }
        public string RegisteredUserAgent { get; set; }
        public string UniqueKey { get; set; }
        public bool? UniqueKeyConfirm { get; set; }
        public string InstagramUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string GoogleUrl { get; set; }
        public string LinkedInUrl { get; set; }
        public string ReferralCode { get; set; }
        public string Referral { get; set; }
    }
}

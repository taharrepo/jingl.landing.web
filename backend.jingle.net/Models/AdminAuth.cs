using System;
using System.Collections.Generic;

namespace backend.jingle.net.Models
{
    public partial class AdminAuth
    {
        public long Id { get; set; }
        public string AdminEmail { get; set; }
        public string AdminPassword { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastLogin { get; set; }
        public string IPAddress { get; set; }
        public string UserAgent { get; set; }
        public int IsActive { get; set; }
        public string Salt { get; set; }
    }
}
using System;
using System.Collections.Generic;

namespace admin.jingl.net.Models
{
    public partial class UserAuth
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime? LastLogin { get; set; }
        public string UniqueKey { get; set; }
        public string Ipaddress { get; set; }
        public string UserAgent { get; set; }
        public string LoginType { get; set; }
        public string Salt { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace admin.jingl.net.Models
{
    public partial class ContactMessage
    {
        public long Id { get; set; }
        public string EmailUser { get; set; }
        public string Message { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Ipaddress { get; set; }
        public string UserAgent { get; set; }
        public string Name { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.jingle.net.Models
{
    public class ContactMessage
    {
        public long Id { get; set; }
        public string EmailUser { get; set; }
        public string Message { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string IPAddress { get; set; }
        public string UserAgent { get; set; }
        public string Name { get; set; }
    }
}

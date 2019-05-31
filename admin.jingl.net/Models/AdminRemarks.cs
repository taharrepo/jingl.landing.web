using System;
using System.Collections.Generic;

namespace admin.jingl.net.Models
{
    public partial class AdminRemarks
    {
        public long Id { get; set; }
        public long UserID { get; set; }
        public string UserEmail { get; set; }
        public string CreatedEmail { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedEmail { get; set; }
        public string Remarks { get; set; }
    }
}

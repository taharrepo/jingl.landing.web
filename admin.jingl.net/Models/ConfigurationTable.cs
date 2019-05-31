using System;
using System.Collections.Generic;

namespace admin.jingl.net.Models
{
    public partial class ConfigurationTable
    {
        public long Id { get; set; }
        public string ConfigName { get; set; }
        public string ConfigValue { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace Jingl.Web.Models
{
    public partial class ConfigurationTable
    {
        public long Id { get; set; }
        public string ConfigName { get; set; }
        public string ConfigValue { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}

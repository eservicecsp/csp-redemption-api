using System;
using System.Collections.Generic;

namespace CSP_Redemption_WebApi.Entities.Models
{
    public partial class Zone
    {
        public Zone()
        {
            Province = new HashSet<Province>();
        }

        public int Id { get; set; }
        public string NameTh { get; set; }
        public string NameEn { get; set; }

        public virtual ICollection<Province> Province { get; set; }
    }
}

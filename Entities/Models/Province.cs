using System;
using System.Collections.Generic;

namespace CSP_Redemption_WebApi.Entities.Models
{
    public partial class Province
    {
        public Province()
        {
            Amphur = new HashSet<Amphur>();
            Consumer = new HashSet<Consumer>();
        }

        public string Code { get; set; }
        public string NameTh { get; set; }
        public string NameEn { get; set; }
        public int ZoneId { get; set; }

        public virtual Zone Zone { get; set; }
        public virtual ICollection<Amphur> Amphur { get; set; }
        public virtual ICollection<Consumer> Consumer { get; set; }
    }
}

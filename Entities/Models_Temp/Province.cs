using System;
using System.Collections.Generic;

namespace CSP_Redemption_WebApi.Entities.Models_Temp
{
    public partial class Province
    {
        public Province()
        {
            Amphur = new HashSet<Amphur>();
            Consumer = new HashSet<Consumer>();
            Dealer = new HashSet<Dealer>();
        }

        public string Code { get; set; }
        public string NameTh { get; set; }
        public string NameEn { get; set; }
        public int ZoneId { get; set; }

        public virtual Zone Zone { get; set; }
        public virtual ICollection<Amphur> Amphur { get; set; }
        public virtual ICollection<Consumer> Consumer { get; set; }
        public virtual ICollection<Dealer> Dealer { get; set; }
    }
}

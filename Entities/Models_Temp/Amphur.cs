using System;
using System.Collections.Generic;

namespace CSP_Redemption_WebApi.Entities.Models_Temp
{
    public partial class Amphur
    {
        public Amphur()
        {
            Consumer = new HashSet<Consumer>();
            Tumbol = new HashSet<Tumbol>();
        }

        public string Code { get; set; }
        public string NameTh { get; set; }
        public string NameEn { get; set; }
        public string ProvinceCode { get; set; }

        public virtual Province ProvinceCodeNavigation { get; set; }
        public virtual ICollection<Consumer> Consumer { get; set; }
        public virtual ICollection<Tumbol> Tumbol { get; set; }
    }
}

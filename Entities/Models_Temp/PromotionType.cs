using System;
using System.Collections.Generic;

namespace CSP_Redemption_WebApi.Entities.Models_Temp
{
    public partial class PromotionType
    {
        public PromotionType()
        {
            Promotion = new HashSet<Promotion>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Promotion> Promotion { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Entities.Models
{
    public partial class PromotionSubType
    {
        public PromotionSubType()
        {
            Promotion = new HashSet<Promotion>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Promotion> Promotion { get; set; }
    }
}

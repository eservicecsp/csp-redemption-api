using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Entities.Models
{
    public partial class CampaignDealer
    {
        public int CampaignId { get; set; }
        public int DealerId { get; set; }

        public virtual Campaign Campaign { get; set; }
        public virtual Dealer Dealer { get; set; }
    }
}

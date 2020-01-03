using System;
using System.Collections.Generic;

namespace CSP_Redemption_WebApi.Entities.Models_Temp
{
    public partial class CampaignDealer
    {
        public int CampaignId { get; set; }
        public int DealerId { get; set; }

        public virtual Campaign Campaign { get; set; }
        public virtual Dealer Dealer { get; set; }
    }
}

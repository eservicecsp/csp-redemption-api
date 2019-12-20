using System;
using System.Collections.Generic;

namespace CSP_Redemption_WebApi.Entities.Models_Temp
{
    public partial class CampaignProduct
    {
        public int CampaignId { get; set; }
        public int ProductId { get; set; }

        public virtual Campaign Campaign { get; set; }
        public virtual Product Product { get; set; }
    }
}

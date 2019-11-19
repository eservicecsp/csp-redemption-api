using System;
using System.Collections.Generic;

namespace CSP_Redemption_WebApi.Entities.Models
{
    public partial class QrCode
    {
        public string QrCode1 { get; set; }
        public int CampaignId { get; set; }
        public int? ConsumerId { get; set; }
        public DateTime? ScanDate { get; set; }

        public virtual Campaign Campaign { get; set; }
        public virtual Consumer Consumer { get; set; }
    }
}

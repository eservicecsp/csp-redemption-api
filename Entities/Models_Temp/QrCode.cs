using System;
using System.Collections.Generic;

namespace CSP_Redemption_WebApi.Entities.Models_Temp
{
    public partial class QrCode
    {
        public string Token { get; set; }
        public int CampaignId { get; set; }
        public int? Peice { get; set; }
        public int? ConsumerId { get; set; }
        public int? TransactionId { get; set; }
        public int? Point { get; set; }
        public DateTime? ScanDate { get; set; }

        public virtual Campaign Campaign { get; set; }
        public virtual Consumer Consumer { get; set; }
        public virtual Transaction Transaction { get; set; }
    }
}

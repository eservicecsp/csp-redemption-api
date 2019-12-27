using System;
using System.Collections.Generic;

namespace CSP_Redemption_WebApi.Entities.Models
{
    public partial class QrCode
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public int CampaignId { get; set; }
        public string Code { get; set; }
        public int? Peice { get; set; }
        public int? ConsumerId { get; set; }
        public int? EnrollmentId { get; set; }
        public int? TransactionId { get; set; }
        public int? Point { get; set; }
        public DateTime? ScanDate { get; set; }

        public virtual Consumer Consumer { get; set; }
    }
}

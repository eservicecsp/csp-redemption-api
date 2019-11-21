using System;
using System.Collections.Generic;

namespace CSP_Redemption_WebApi.Entities.Models
{
    public partial class Transaction
    {
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public int ConsumerId { get; set; }
        public string ResponseMessage { get; set; }
        public string Code { get; set; }
        public string StatusTypeCode { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Company Campaign { get; set; }
        public virtual Consumer Consumer { get; set; }
    }
}

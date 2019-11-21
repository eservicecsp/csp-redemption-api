using System;
using System.Collections.Generic;

namespace CSP_Redemption_WebApi.Entities.Models
{
    public partial class Campaign
    {
        public Campaign()
        {
            Consumer = new HashSet<Consumer>();
            QrCode = new HashSet<QrCode>();
            Transaction = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CampaignTypeId { get; set; }
        public int BrandId { get; set; }
        public int Quantity { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual CampaignType CampaignType { get; set; }
        public virtual ICollection<Consumer> Consumer { get; set; }
        public virtual ICollection<QrCode> QrCode { get; set; }
        public virtual ICollection<Transaction> Transaction { get; set; }
    }
}

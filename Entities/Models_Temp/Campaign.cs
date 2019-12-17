using System;
using System.Collections.Generic;

namespace CSP_Redemption_WebApi.Entities.Models_Temp
{
    public partial class Campaign
    {
        public Campaign()
        {
            Transaction = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CampaignTypeId { get; set; }
        public int BrandId { get; set; }
        public string Url { get; set; }
        public int Quantity { get; set; }
        public int? TotalPeice { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string AlertMessage { get; set; }
        public string DuplicateMessage { get; set; }
        public string QrCodeNotExistMessage { get; set; }
        public string WinMessage { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual CampaignType CampaignType { get; set; }
        public virtual ICollection<Transaction> Transaction { get; set; }
    }
}

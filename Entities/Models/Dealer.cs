using System;
using System.Collections.Generic;

namespace CSP_Redemption_WebApi.Entities.Models
{
    public partial class Dealer
    {
        public Dealer()
        {
            Campaign = new HashSet<Campaign>();
            CampaignDealer = new HashSet<CampaignDealer>();
        }

        public int Id { get; set; }
        public string BranchNo { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string TaxNo { get; set; }
        public string Phone { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string TumbolCode { get; set; }
        public string AmphurCode { get; set; }
        public string ProvinceCode { get; set; }
        public string ZipCode { get; set; }
        public string Tel { get; set; }
        public int BrandId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Amphur AmphurCodeNavigation { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual Province ProvinceCodeNavigation { get; set; }
        public virtual Tumbol TumbolCodeNavigation { get; set; }
        public virtual ICollection<Campaign> Campaign { get; set; }
        public virtual ICollection<CampaignDealer> CampaignDealer { get; set; }
    }
}

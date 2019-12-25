using System;
using System.Collections.Generic;

namespace CSP_Redemption_WebApi.Entities.Models
{
    public partial class CampaignType
    {
        public CampaignType()
        {
            Campaign = new HashSet<Campaign>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Campaign> Campaign { get; set; }
    }
}

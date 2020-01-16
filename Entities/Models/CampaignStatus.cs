using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Entities.Models
{
    public class CampaignStatus
    {
        public CampaignStatus()
        {
            Campaign = new HashSet<Campaign>();
        }

        public int Id { get; set; }
        public string Status { get; set; }

        public virtual ICollection<Campaign> Campaign { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Entities.Models
{
    public class Collection
    {
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public int Quantity { get; set; }
        public int? WasteQuantity { get; set; }
        public int? TotalQuantity { get; set; }
        public int CollectionRow { get; set; }
        public int CollectionColumn { get; set; }
        public string CollectionName { get; set; }
        public string CollectionPath { get; set; }
        public string CollectionFile { get; set; }
        public string Extension { get; set; }

        public virtual Campaign Campaign { get; set; }
    }
}

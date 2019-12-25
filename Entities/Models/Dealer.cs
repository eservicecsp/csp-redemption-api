using System;
using System.Collections.Generic;

namespace CSP_Redemption_WebApi.Entities.Models
{
    public partial class Dealer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string TaxNo { get; set; }
        public string Phone { get; set; }
        public string Tel { get; set; }
        public int BrandId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Brand Brand { get; set; }
    }
}

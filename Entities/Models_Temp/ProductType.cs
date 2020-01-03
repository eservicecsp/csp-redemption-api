using System;
using System.Collections.Generic;

namespace CSP_Redemption_WebApi.Entities.Models_Temp
{
    public partial class ProductType
    {
        public ProductType()
        {
            ConsumerProductType = new HashSet<ConsumerProductType>();
            Product = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int BrandId { get; set; }
        public bool IsActived { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual Staff CreatedByNavigation { get; set; }
        public virtual ICollection<ConsumerProductType> ConsumerProductType { get; set; }
        public virtual ICollection<Product> Product { get; set; }
    }
}

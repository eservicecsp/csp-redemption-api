﻿using System;
using System.Collections.Generic;

namespace CSP_Redemption_WebApi.Entities.Models
{
    public partial class Product
    {
        public Product()
        {
            CampaignProduct = new HashSet<CampaignProduct>();
            ProductAttachment = new HashSet<ProductAttachment>();
            Promotion = new HashSet<Promotion>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? BrandId { get; set; }
        public int? ProductTypeId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual Staff CreatedByNavigation { get; set; }
        public virtual ProductType ProductType { get; set; }
        public virtual ICollection<CampaignProduct> CampaignProduct { get; set; }
        public virtual ICollection<ProductAttachment> ProductAttachment { get; set; }
        public virtual ICollection<Promotion> Promotion { get; set; }
    }
}

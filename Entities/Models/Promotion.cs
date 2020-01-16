using System;
using System.Collections.Generic;

namespace CSP_Redemption_WebApi.Entities.Models
{
    public partial class Promotion
    {
        public Promotion()
        {
            Tracking = new HashSet<Tracking>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int BrandId { get; set; }
        public int PromotionTypeId { get; set; }
        public int? PromotionSubTypeId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? MemberDiscount { get; set; }
        public int? BirthDateDiscount { get; set; }
        public int? ProductGroupDiscount { get; set; }
        public string ImagePath1 { get; set; }
        public string ImageExtension1 { get; set; }
        public string ImageUrl1 { get; set; }
        public string ImagePath2 { get; set; }
        public string ImageExtension2 { get; set; }
        public string ImageUrl2 { get; set; }
        public string ImagePath3 { get; set; }
        public string ImageExtension3 { get; set; }
        public string ImageUrl3 { get; set; }
        public string ImageBackground { get; set; }
        public string ImageBackgroundExtention { get; set; }
        public int? ProductId { get; set; }
        public string Tel { get; set; }
        public string Facebook { get; set; }
        public string Line { get; set; }
        public string Web { get; set; }
        public bool IsActived { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual Staff CreatedByNavigation { get; set; }
        public virtual Product Product { get; set; }
        public virtual PromotionSubType PromotionSubType { get; set; }
        public virtual PromotionType PromotionType { get; set; }
        public virtual ICollection<Tracking> Tracking { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace CSP_Redemption_WebApi.Entities.Models
{
    public partial class Staff
    {
        public Staff()
        {
            Product = new HashSet<Product>();
            ProductType = new HashSet<ProductType>();
            Promotion = new HashSet<Promotion>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public int BrandId { get; set; }
        public int RoleId { get; set; }
        public bool IsActived { get; set; }
        public string ResetPasswordToken { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<Product> Product { get; set; }
        public virtual ICollection<ProductType> ProductType { get; set; }
        public virtual ICollection<Promotion> Promotion { get; set; }
    }
}

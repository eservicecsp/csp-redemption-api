using System;
using System.Collections.Generic;

namespace CSP_Redemption_WebApi.Entities.Models
{
    public partial class Consumer
    {
        public Consumer()
        {
            QrCode = new HashSet<QrCode>();
            Transaction = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string TumbolCode { get; set; }
        public string AmphurCode { get; set; }
        public string ProvinceCode { get; set; }
        public string ZipCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Location { get; set; }
        public int CompanyId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int ConsumerTypeId { get; set; }

        public virtual Amphur AmphurCodeNavigation { get; set; }
        public virtual Company Company { get; set; }
        public virtual ConsumerType ConsumerType { get; set; }
        public virtual Province ProvinceCodeNavigation { get; set; }
        public virtual Tumbol TumbolCodeNavigation { get; set; }
        public virtual ICollection<QrCode> QrCode { get; set; }
        public virtual ICollection<Transaction> Transaction { get; set; }
    }
}

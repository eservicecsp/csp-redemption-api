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
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string TumbolCode { get; set; }
        public string AmphurCode { get; set; }
        public string ProvinceCode { get; set; }
        public string ZipCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Location { get; set; }
        public int ConsumerSourceId { get; set; }
        public int BrandId { get; set; }
        public int? CampaignId { get; set; }
        public int? Point { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual Campaign Campaign { get; set; }
        public virtual ConsumerSource ConsumerSource { get; set; }
        public virtual ICollection<QrCode> QrCode { get; set; }
        public virtual ICollection<Transaction> Transaction { get; set; }
    }
}

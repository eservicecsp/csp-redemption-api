using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Entities.Models
{
    public class ContactUs
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public string Tel { get; set; }
        public string Facebook { get; set; }
        public string Line { get; set; }
        public string Web { get; set; }
        public string ShopOnline { get; set; }

        public virtual Brand Brand { get; set; }
    }
}

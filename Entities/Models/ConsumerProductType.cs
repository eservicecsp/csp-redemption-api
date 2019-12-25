using System;
using System.Collections.Generic;

namespace CSP_Redemption_WebApi.Entities.Models
{
    public partial class ConsumerProductType
    {
        public int ProductTypeId { get; set; }
        public int ConsumerId { get; set; }

        public virtual Consumer Consumer { get; set; }
        public virtual ProductType ProductType { get; set; }
    }
}

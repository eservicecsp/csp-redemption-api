using System;
using System.Collections.Generic;

namespace CSP_Redemption_WebApi.Entities.Models
{
    public partial class ConsumerType
    {
        public ConsumerType()
        {
            Consumer = new HashSet<Consumer>();
        }

        public int Id { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Consumer> Consumer { get; set; }
    }
}

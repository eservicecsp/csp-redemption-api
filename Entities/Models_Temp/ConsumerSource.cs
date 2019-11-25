using System;
using System.Collections.Generic;

namespace CSP_Redemption_WebApi.Entities.Models_Temp
{
    public partial class ConsumerSource
    {
        public ConsumerSource()
        {
            Consumer = new HashSet<Consumer>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Consumer> Consumer { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace CSP_Redemption_WebApi.Entities.Models_Temp
{
    public partial class Tracking
    {
        public int PromitionId { get; set; }
        public int ConsumerId { get; set; }
        public string SendType { get; set; }
        public int SendBy { get; set; }
        public DateTime SendDate { get; set; }

        public virtual Consumer Promition { get; set; }
        public virtual Promotion PromitionNavigation { get; set; }
        public virtual Staff SendByNavigation { get; set; }
    }
}

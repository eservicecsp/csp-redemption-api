using System;
using System.Collections.Generic;

namespace CSP_Redemption_WebApi.Entities.Models
{
    public partial class RoleMenu
    {
        public int RoleId { get; set; }
        public int MenuId { get; set; }
        public bool IsReadOnly { get; set; }

        public virtual Menu Menu { get; set; }
        public virtual Role Role { get; set; }
    }
}

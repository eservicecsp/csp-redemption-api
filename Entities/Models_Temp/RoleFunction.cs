using System;
using System.Collections.Generic;

namespace CSP_Redemption_WebApi.Entities.Models_Temp
{
    public partial class RoleFunction
    {
        public int RoleId { get; set; }
        public int FunctionId { get; set; }
        public bool IsReadOnly { get; set; }

        public virtual Function Function { get; set; }
        public virtual Role Role { get; set; }
    }
}

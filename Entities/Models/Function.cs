using System;
using System.Collections.Generic;

namespace CSP_Redemption_WebApi.Entities.Models
{
    public partial class Function
    {
        public Function()
        {
            RoleFunction = new HashSet<RoleFunction>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }
        public int ParentId { get; set; }
        public string Path { get; set; }
        public string Icon { get; set; }
        public bool IsInternal { get; set; }
        public bool IsExternal { get; set; }
        public bool IsActived { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<RoleFunction> RoleFunction { get; set; }
    }
}

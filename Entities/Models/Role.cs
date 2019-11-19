using System;
using System.Collections.Generic;

namespace CSP_Redemption_WebApi.Entities.Models
{
    public partial class Role
    {
        public Role()
        {
            RoleMenu = new HashSet<RoleMenu>();
            Staff = new HashSet<Staff>();
        }

        public int Id { get; set; }
        public int? CompanyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<RoleMenu> RoleMenu { get; set; }
        public virtual ICollection<Staff> Staff { get; set; }
    }
}

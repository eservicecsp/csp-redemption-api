using System;
using System.Collections.Generic;

namespace CSP_Redemption_WebApi.Entities.Models
{
    public partial class Menu
    {
        public Menu()
        {
            RoleMenu = new HashSet<RoleMenu>();
        }

        public int Id { get; set; }
        public bool IsInternal { get; set; }
        public bool IsExternal { get; set; }

        public virtual ICollection<RoleMenu> RoleMenu { get; set; }
    }
}

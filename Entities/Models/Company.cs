using System;
using System.Collections.Generic;

namespace CSP_Redemption_WebApi.Entities.Models
{
    public partial class Company
    {
        public Company()
        {
            Campaign = new HashSet<Campaign>();
            Role = new HashSet<Role>();
            Staff = new HashSet<Staff>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActived { get; set; }

        public virtual ICollection<Campaign> Campaign { get; set; }
        public virtual ICollection<Role> Role { get; set; }
        public virtual ICollection<Staff> Staff { get; set; }
    }
}

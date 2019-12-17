﻿using System;
using System.Collections.Generic;

namespace CSP_Redemption_WebApi.Entities.Models_Temp
{
    public partial class Brand
    {
        public Brand()
        {
            Campaign = new HashSet<Campaign>();
            Dealer = new HashSet<Dealer>();
            Product = new HashSet<Product>();
            Staff = new HashSet<Staff>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsOwner { get; set; }

        public virtual ICollection<Campaign> Campaign { get; set; }
        public virtual ICollection<Dealer> Dealer { get; set; }
        public virtual ICollection<Product> Product { get; set; }
        public virtual ICollection<Staff> Staff { get; set; }
    }
}
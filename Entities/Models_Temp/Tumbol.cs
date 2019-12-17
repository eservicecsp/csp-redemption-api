﻿using System;
using System.Collections.Generic;

namespace CSP_Redemption_WebApi.Entities.Models_Temp
{
    public partial class Tumbol
    {
        public string Code { get; set; }
        public string NameTh { get; set; }
        public string NameEn { get; set; }
        public string ZipCode { get; set; }
        public string AmphurCode { get; set; }

        public virtual Amphur AmphurCodeNavigation { get; set; }
    }
}
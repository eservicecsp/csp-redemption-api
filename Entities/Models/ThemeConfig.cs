using System;
using System.Collections.Generic;

namespace CSP_Redemption_WebApi.Entities.Models
{
    public partial class ThemeConfig
    {
        public int BrandId { get; set; }
        public int ThemeId { get; set; }
        public string ImagesHeader { get; set; }
        public string ImageBackground { get; set; }
        public string TextValue01 { get; set; }
        public string TextValue02 { get; set; }
        public string TextValue03 { get; set; }
        public string Facebook { get; set; }
        public string Line { get; set; }
        public string WebSite { get; set; }
        public string Twitter { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual Theme Theme { get; set; }
    }
}

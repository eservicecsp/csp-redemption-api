using System;
using System.Collections.Generic;

namespace CSP_Redemption_WebApi.Entities.Models
{
    public partial class Theme
    {
        public Theme()
        {
            Promotion = new HashSet<Promotion>();
            ThemeConfig = new HashSet<ThemeConfig>();
        }

        public int Id { get; set; }
        public string ThemeName { get; set; }
        public string HtmlCode { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<Promotion> Promotion { get; set; }
        public virtual ICollection<ThemeConfig> ThemeConfig { get; set; }
    }
}

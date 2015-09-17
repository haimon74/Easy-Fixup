using System;
using System.Collections.Generic;

namespace ezFixUp.Model.Models
{
    public class AdsCategory
    {
        public AdsCategory()
        {
            this.Ads = new List<Ad>();
            this.AdsCategories1 = new List<AdsCategory>();
        }

        public int ac_id { get; set; }
        public Nullable<int> ac_parentid { get; set; }
        public string ac_title { get; set; }
        public virtual ICollection<Ad> Ads { get; set; }
        public virtual ICollection<AdsCategory> AdsCategories1 { get; set; }
        public virtual AdsCategory AdsCategory1 { get; set; }
    }
}

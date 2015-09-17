using System;
using System.Collections.Generic;

namespace ezFixUp.Model.Models
{
    public class WinkType
    {
        public WinkType()
        {
            this.WinkMessaages = new List<WinkMessaage>();
        }

        public int wt_id { get; set; }
        public string wt_name { get; set; }
        public string wt_phrase { get; set; }
        public bool wt_active { get; set; }
        public Nullable<int> wt_category_id { get; set; }
        public virtual WinkCategory WinkCategory { get; set; }
        public virtual ICollection<WinkMessaage> WinkMessaages { get; set; }
    }
}

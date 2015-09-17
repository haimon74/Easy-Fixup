using System.Collections.Generic;

namespace ezFixUp.Model.Models
{
    public class WinkCategory
    {
        public WinkCategory()
        {
            this.WinkTypes = new List<WinkType>();
        }

        public int wc_id { get; set; }
        public string wc_name { get; set; }
        public int wc_language_id { get; set; }
        public bool wc_active { get; set; }
        public virtual Language Language { get; set; }
        public virtual ICollection<WinkType> WinkTypes { get; set; }
    }
}

using System.Collections.Generic;

namespace ezFixUp.Model.Models
{
    public class GiftCategory
    {
        public GiftCategory()
        {
            this.EcardTypes = new List<EcardType>();
            this.GiftTypes = new List<GiftType>();
        }

        public int gc_id { get; set; }
        public string gc_name { get; set; }
        public bool gc_active { get; set; }
        public virtual ICollection<EcardType> EcardTypes { get; set; }
        public virtual ICollection<GiftType> GiftTypes { get; set; }
    }
}

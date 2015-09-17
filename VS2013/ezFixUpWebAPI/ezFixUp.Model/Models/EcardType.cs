using System;
using System.Collections.Generic;

namespace ezFixUp.Model.Models
{
    public class EcardType
    {
        public EcardType()
        {
            this.Ecards = new List<Ecard>();
        }

        public int ect_id { get; set; }
        public string ect_name { get; set; }
        public string ect_phrase { get; set; }
        public byte[] ect_content { get; set; }
        public int ect_type { get; set; }
        public bool ect_active { get; set; }
        public Nullable<int> ect_category_id { get; set; }
        public virtual ECardCategory ECardCategory { get; set; }
        public virtual ICollection<Ecard> Ecards { get; set; }
        public virtual GiftCategory GiftCategory { get; set; }
    }
}

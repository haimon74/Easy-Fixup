using System;
using System.Collections.Generic;

namespace ezFixUp.Model.Models
{
    public class GiftType
    {
        public GiftType()
        {
            this.GiftsMessages = new List<GiftsMessage>();
        }

        public int gt_id { get; set; }
        public string gt_name { get; set; }
        public string gt_phrase { get; set; }
        public byte[] gt_content { get; set; }
        public int gt_type { get; set; }
        public bool gt_active { get; set; }
        public Nullable<int> gt_category_id { get; set; }
        public Nullable<int> gt_price { get; set; }
        public virtual GiftCategory GiftCategory { get; set; }
        public virtual ICollection<GiftsMessage> GiftsMessages { get; set; }
    }
}

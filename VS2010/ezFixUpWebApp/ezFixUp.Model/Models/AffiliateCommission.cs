using System;

namespace ezFixUp.Model.Models
{
    public class AffiliateCommission
    {
        public int ac_id { get; set; }
        public int a_id { get; set; }
        public string u_username { get; set; }
        public Nullable<int> ph_id { get; set; }
        public System.DateTime ac_timestamp { get; set; }
        public string ac_notes { get; set; }
        public decimal ac_amount { get; set; }
        public virtual Affiliate Affiliate { get; set; }
        public virtual PaymentHistory PaymentHistory { get; set; }
        public virtual User User { get; set; }
    }
}

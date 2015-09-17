using System.Collections.Generic;

namespace ezFixUp.Model.Models
{
    public class PaymentHistory
    {
        public PaymentHistory()
        {
            this.AffiliateCommissions = new List<AffiliateCommission>();
        }

        public int ph_id { get; set; }
        public string u_username { get; set; }
        public string ph_paymentprocessor { get; set; }
        public decimal ph_amount { get; set; }
        public string ph_description { get; set; }
        public string ph_notes { get; set; }
        public int ph_status { get; set; }
        public System.DateTime ph_timestamp { get; set; }
        public virtual ICollection<AffiliateCommission> AffiliateCommissions { get; set; }
        public virtual User User { get; set; }
    }
}

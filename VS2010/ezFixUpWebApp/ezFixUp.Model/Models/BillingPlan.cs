using System.Collections.Generic;

namespace ezFixUp.Model.Models
{
    public class BillingPlan
    {
        public BillingPlan()
        {
            this.Subscriptions = new List<Subscription>();
        }

        public int p_id { get; set; }
        public string p_title { get; set; }
        public decimal p_amount { get; set; }
        public int p_billing_cycle { get; set; }
        public byte p_billing_cycle_unit { get; set; }
        public bool p_deleted { get; set; }
        public string p_options { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
    }
}

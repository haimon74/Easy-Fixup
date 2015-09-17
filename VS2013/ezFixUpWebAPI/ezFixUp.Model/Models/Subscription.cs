
namespace ezFixUp.Model.Models
{
    public class Subscription
    {
        public int s_id { get; set; }
        public string u_username { get; set; }
        public int p_id { get; set; }
        public System.DateTime s_order_date { get; set; }
        public System.DateTime s_renew_date { get; set; }
        public bool s_confirmed { get; set; }
        public bool s_cancelled { get; set; }
        public bool s_cancellation_requested { get; set; }
        public string s_paymentprocessor { get; set; }
        public string s_custom { get; set; }
        public virtual BillingPlan BillingPlan { get; set; }
        public virtual User User { get; set; }
    }
}

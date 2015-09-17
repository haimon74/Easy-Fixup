using System;
using System.Collections.Generic;

namespace ezFixUp.Model.Models
{
    public class Affiliate
    {
        public Affiliate()
        {
            this.AffiliateCommissions = new List<AffiliateCommission>();
            this.AffiliatesHistories = new List<AffiliatesHistory>();
        }

        public int a_id { get; set; }
        public string a_username { get; set; }
        public string a_password { get; set; }
        public string a_name { get; set; }
        public string a_email { get; set; }
        public string a_siteurl { get; set; }
        public string a_payment_details { get; set; }
        public bool a_deleted { get; set; }
        public bool a_active { get; set; }
        public Nullable<int> a_percentage { get; set; }
        public Nullable<decimal> a_fixed_amount { get; set; }
        public bool a_recurrent { get; set; }
        public decimal a_balance { get; set; }
        public bool a_request_payment { get; set; }
        public virtual ICollection<AffiliateCommission> AffiliateCommissions { get; set; }
        public virtual ICollection<AffiliatesHistory> AffiliatesHistories { get; set; }
    }
}

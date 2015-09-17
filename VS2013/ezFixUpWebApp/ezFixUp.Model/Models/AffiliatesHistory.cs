
namespace ezFixUp.Model.Models
{
    public class AffiliatesHistory
    {
        public int ah_id { get; set; }
        public int a_id { get; set; }
        public decimal ah_amount { get; set; }
        public System.DateTime ah_date_paid { get; set; }
        public string ah_notes { get; set; }
        public string ah_private_notes { get; set; }
        public virtual Affiliate Affiliate { get; set; }
    }
}

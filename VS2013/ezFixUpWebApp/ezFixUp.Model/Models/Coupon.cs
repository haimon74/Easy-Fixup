
namespace ezFixUp.Model.Models
{
    public class Coupon
    {
        public int c_id { get; set; }
        public string c_code { get; set; }
        public string c_username { get; set; }
        public System.DateTime c_start_date { get; set; }
        public System.DateTime c_due_date { get; set; }
        public int c_max_use { get; set; }
        public int c_type_id { get; set; }
        public int c_used { get; set; }
        public bool c_only_new_user { get; set; }
    }
}

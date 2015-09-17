
namespace ezFixUp.Model.Models
{
    public class UserTipsStatu
    {
        public int uts_id { get; set; }
        public int uts_tip_id { get; set; }
        public string uts_username { get; set; }
        public int uts_viewed_times { get; set; }
        public bool uts_is_blocked { get; set; }
        public System.DateTime uts_last_viewed_date { get; set; }
    }
}

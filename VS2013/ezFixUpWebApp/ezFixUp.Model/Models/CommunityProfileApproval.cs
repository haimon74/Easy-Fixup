
namespace ezFixUp.Model.Models
{
    public class CommunityProfileApproval
    {
        public int cpa_id { get; set; }
        public string u_username { get; set; }
        public string cpa_approvedby { get; set; }
        public bool cpa_approved { get; set; }
        public System.DateTime cpa_timestamp { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}

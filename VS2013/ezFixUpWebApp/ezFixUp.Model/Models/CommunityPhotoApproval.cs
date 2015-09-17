
namespace ezFixUp.Model.Models
{
    public class CommunityPhotoApproval
    {
        public int cpa_id { get; set; }
        public string u_username { get; set; }
        public int p_id { get; set; }
        public bool cpa_approved { get; set; }
        public System.DateTime cpa_timestamp { get; set; }
        public virtual Photo Photo { get; set; }
        public virtual User User { get; set; }
    }
}

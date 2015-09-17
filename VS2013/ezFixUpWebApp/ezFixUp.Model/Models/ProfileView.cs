
namespace ezFixUp.Model.Models
{
    public class ProfileView
    {
        public string pv_viewer { get; set; }
        public string pv_viewed { get; set; }
        public System.DateTime pv_timestamp { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}

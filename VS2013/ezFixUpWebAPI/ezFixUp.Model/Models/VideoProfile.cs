
namespace ezFixUp.Model.Models
{
    public class VideoProfile
    {
        public string u_username { get; set; }
        public bool vp_approved { get; set; }
        public bool vp_private { get; set; }
        public virtual User User { get; set; }
    }
}

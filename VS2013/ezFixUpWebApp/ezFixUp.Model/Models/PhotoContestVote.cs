
namespace ezFixUp.Model.Models
{
    public class PhotoContestVote
    {
        public string u_username { get; set; }
        public int pce_id_picked { get; set; }
        public int pce_id_nonpicked { get; set; }
        public virtual PhotoContestEntry PhotoContestEntry { get; set; }
        public virtual PhotoContestEntry PhotoContestEntry1 { get; set; }
        public virtual User User { get; set; }
    }
}

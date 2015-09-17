
namespace ezFixUp.Model.Models
{
    public class PhotoContestRank
    {
        public string u_username { get; set; }
        public int pc_id { get; set; }
        public int pce_id { get; set; }
        public int pcr_value { get; set; }
        public virtual PhotoContestEntry PhotoContestEntry { get; set; }
        public virtual PhotoContest PhotoContest { get; set; }
        public virtual User User { get; set; }
    }
}

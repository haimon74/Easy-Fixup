using System.Collections.Generic;

namespace ezFixUp.Model.Models
{
    public class PhotoContestEntry
    {
        public PhotoContestEntry()
        {
            this.PhotoContestRanks = new List<PhotoContestRank>();
            this.PhotoContestVotes = new List<PhotoContestVote>();
            this.PhotoContestVotes1 = new List<PhotoContestVote>();
        }

        public int pce_id { get; set; }
        public int pc_id { get; set; }
        public string u_username { get; set; }
        public int p_id { get; set; }
        public virtual PhotoContest PhotoContest { get; set; }
        public virtual ICollection<PhotoContestRank> PhotoContestRanks { get; set; }
        public virtual ICollection<PhotoContestVote> PhotoContestVotes { get; set; }
        public virtual ICollection<PhotoContestVote> PhotoContestVotes1 { get; set; }
    }
}

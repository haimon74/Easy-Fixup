using System;
using System.Collections.Generic;

namespace ezFixUp.Model.Models
{
    public class PhotoContest
    {
        public PhotoContest()
        {
            this.PhotoContestEntries = new List<PhotoContestEntry>();
            this.PhotoContestRanks = new List<PhotoContestRank>();
        }

        public int pc_id { get; set; }
        public string pc_name { get; set; }
        public Nullable<int> pc_gender { get; set; }
        public string pc_description { get; set; }
        public string pc_terms { get; set; }
        public Nullable<int> pc_minage { get; set; }
        public Nullable<int> pc_maxage { get; set; }
        public System.DateTime pc_datecreated { get; set; }
        public Nullable<System.DateTime> pc_dateends { get; set; }
        public virtual ICollection<PhotoContestEntry> PhotoContestEntries { get; set; }
        public virtual ICollection<PhotoContestRank> PhotoContestRanks { get; set; }
    }
}

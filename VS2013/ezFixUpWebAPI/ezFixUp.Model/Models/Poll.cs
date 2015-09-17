using System.Collections.Generic;

namespace ezFixUp.Model.Models
{
    public class Poll
    {
        public Poll()
        {
            this.PollChoices = new List<PollChoice>();
        }

        public int p_id { get; set; }
        public string p_title { get; set; }
        public System.DateTime p_startdate { get; set; }
        public System.DateTime p_enddate { get; set; }
        public System.DateTime p_showresultsuntil { get; set; }
        public virtual ICollection<PollChoice> PollChoices { get; set; }
    }
}

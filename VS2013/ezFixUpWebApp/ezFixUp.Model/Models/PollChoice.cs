using System.Collections.Generic;

namespace ezFixUp.Model.Models
{
    public class PollChoice
    {
        public PollChoice()
        {
            this.PollAnswers = new List<PollAnswer>();
        }

        public int pc_id { get; set; }
        public int p_id { get; set; }
        public string pc_answer { get; set; }
        public virtual ICollection<PollAnswer> PollAnswers { get; set; }
        public virtual Poll Poll { get; set; }
    }
}

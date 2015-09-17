using System.Collections.Generic;

namespace ezFixUp.Model.Models
{
    public class GroupPollsChoice
    {
        public GroupPollsChoice()
        {
            this.GroupPollsAnswers = new List<GroupPollsAnswer>();
        }

        public int gpc_id { get; set; }
        public int gt_id { get; set; }
        public string gpc_answer { get; set; }
        public virtual ICollection<GroupPollsAnswer> GroupPollsAnswers { get; set; }
        public virtual GroupTopic GroupTopic { get; set; }
    }
}

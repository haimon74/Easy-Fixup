
namespace ezFixUp.Model.Models
{
    public class GroupPollsAnswer
    {
        public int gt_id { get; set; }
        public string u_username { get; set; }
        public int gpc_id { get; set; }
        public virtual GroupPollsChoice GroupPollsChoice { get; set; }
        public virtual GroupTopic GroupTopic { get; set; }
        public virtual User User { get; set; }
    }
}

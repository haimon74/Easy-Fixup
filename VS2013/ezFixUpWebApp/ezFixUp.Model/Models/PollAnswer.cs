
namespace ezFixUp.Model.Models
{
    public class PollAnswer
    {
        public int p_id { get; set; }
        public string u_username { get; set; }
        public int pc_id { get; set; }
        public virtual PollChoice PollChoice { get; set; }
        public virtual User User { get; set; }
    }
}

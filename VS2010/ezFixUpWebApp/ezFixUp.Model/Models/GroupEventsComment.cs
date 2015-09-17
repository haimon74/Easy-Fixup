
namespace ezFixUp.Model.Models
{
    public class GroupEventsComment
    {
        public int gec_id { get; set; }
        public int ge_id { get; set; }
        public string u_username { get; set; }
        public string gec_comment { get; set; }
        public System.DateTime gec_date { get; set; }
        public virtual GroupEvent GroupEvent { get; set; }
        public virtual User User { get; set; }
    }
}

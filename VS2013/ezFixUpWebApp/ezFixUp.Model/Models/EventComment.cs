
namespace ezFixUp.Model.Models
{
    public class EventComment
    {
        public int ec_id { get; set; }
        public int e_id { get; set; }
        public string u_username { get; set; }
        public System.DateTime ec_date { get; set; }
        public string ec_comment { get; set; }
        public virtual Event Event { get; set; }
        public virtual User User { get; set; }
    }
}


namespace ezFixUp.Model.Models
{
    public class UserScoresLog
    {
        public int usl_id { get; set; }
        public string u_username { get; set; }
        public System.DateTime usl_timestamp { get; set; }
        public int usl_score { get; set; }
        public string usl_notes { get; set; }
        public virtual User User { get; set; }
    }
}

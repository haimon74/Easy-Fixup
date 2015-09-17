
namespace ezFixUp.Model.Models
{
    public class CreditsHistory
    {
        public int ch_id { get; set; }
        public string u_username { get; set; }
        public System.DateTime ch_date { get; set; }
        public int ch_service { get; set; }
        public int ch_amount { get; set; }
        public virtual User User { get; set; }
    }
}

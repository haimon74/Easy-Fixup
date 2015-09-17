
namespace ezFixUp.Model.Models
{
    public class IPLog
    {
        public int ipl_id { get; set; }
        public string u_username { get; set; }
        public string ipl_ip { get; set; }
        public int ipl_action { get; set; }
        public System.DateTime ipl_timestamp { get; set; }
    }
}

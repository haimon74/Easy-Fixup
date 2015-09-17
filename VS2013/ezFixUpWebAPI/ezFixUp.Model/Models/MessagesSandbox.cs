
namespace ezFixUp.Model.Models
{
    public class MessagesSandbox
    {
        public string u_username { get; set; }
        public string ms_message { get; set; }
        public int ms_count { get; set; }
        public System.DateTime ms_datetime { get; set; }
        public virtual User User { get; set; }
    }
}

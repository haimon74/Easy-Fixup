
namespace ezFixUp.Model.Models
{
    public class FacebookInvitation
    {
        public int fi_id { get; set; }
        public string fi_request_id { get; set; }
        public string fi_sender_id { get; set; }
        public string fi_recipient_id { get; set; }
        public System.DateTime fi_created_time { get; set; }
        public bool fi_is_credited { get; set; }
    }
}

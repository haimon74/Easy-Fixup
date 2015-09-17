
namespace ezFixUp.Model.Models
{
    public class archive_Messages
    {
        public int m_id { get; set; }
        public string m_from_username { get; set; }
        public int m_from_folder { get; set; }
        public string m_to_username { get; set; }
        public int m_to_folder { get; set; }
        public string m_body { get; set; }
        public System.DateTime m_timestamp { get; set; }
        public int m_replied_to { get; set; }
        public bool m_is_read { get; set; }
        public bool m_pending_approval { get; set; }
        public bool m_new { get; set; }
    }
}

using System;

namespace ezFixUp.Model.Models
{
    public class Message
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
        public Nullable<int> m_type_id { get; set; }
        public string m_subject { get; set; }
        public string m_about_username { get; set; }
        public virtual MessagesType MessagesType { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}

using System;

namespace ezFixUp.Model.Models
{
    public class ChatMessage
    {
        public int cm_id { get; set; }
        public int cm_type { get; set; }
        public System.DateTime cm_time { get; set; }
        public Nullable<int> cr_id { get; set; }
        public Nullable<int> cu_id_sender { get; set; }
        public string cm_senderdisplayname { get; set; }
        public Nullable<int> cu_id_target { get; set; }
        public string cm_text { get; set; }
        public string cm_texthtml { get; set; }
    }
}

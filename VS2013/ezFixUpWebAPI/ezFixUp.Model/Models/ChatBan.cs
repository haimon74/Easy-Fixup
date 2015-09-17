using System;

namespace ezFixUp.Model.Models
{
    public class ChatBan
    {
        public int cb_id { get; set; }
        public Nullable<int> cr_id { get; set; }
        public Nullable<int> cu_id { get; set; }
        public string cb_ip { get; set; }
        public System.DateTime cb_date { get; set; }
        public Nullable<System.DateTime> cb_dateexpires { get; set; }
    }
}

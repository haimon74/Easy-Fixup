using System;

namespace ezFixUp.Model.Models
{
    public class ChatRoom
    {
        public int cr_id { get; set; }
        public string cr_name { get; set; }
        public string cr_topic { get; set; }
        public string cr_password { get; set; }
        public Nullable<int> cr_maxusers { get; set; }
        public bool cr_visible { get; set; }
    }
}

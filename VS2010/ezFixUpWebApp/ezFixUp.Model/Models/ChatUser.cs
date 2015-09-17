using System;
using System.Collections.Generic;

namespace ezFixUp.Model.Models
{
    public class ChatUser
    {
        public ChatUser()
        {
            this.ChatIgnoredUsers = new List<ChatIgnoredUser>();
            this.ChatIgnoredUsers1 = new List<ChatIgnoredUser>();
        }

        public int cu_id { get; set; }
        public string cu_username { get; set; }
        public string cu_password { get; set; }
        public string cu_displayname { get; set; }
        public Nullable<int> cu_gender { get; set; }
        public System.DateTime cu_begintime { get; set; }
        public System.DateTime cu_lastactive { get; set; }
        public string cu_ip { get; set; }
        public string cu_lastactiveguid { get; set; }
        public string cu_thumbimage { get; set; }
        public virtual ICollection<ChatIgnoredUser> ChatIgnoredUsers { get; set; }
        public virtual ICollection<ChatIgnoredUser> ChatIgnoredUsers1 { get; set; }
    }
}

using System;

namespace ezFixUp.Model.Models
{
    public class Friend
    {
        public string u_username { get; set; }
        public string f_username { get; set; }
        public System.DateTime f_timestamp { get; set; }
        public bool f_accepted { get; set; }
        public string fb_mutualy_friends { get; set; }
        public Nullable<bool> f_is_facebook_friend { get; set; }
        public int f_denied_counter { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}

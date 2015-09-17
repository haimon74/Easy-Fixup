using System;

namespace ezFixUp.Model.Models
{
    public class View_FemaleInterestedInFemaleSingles
    {
        public string u_username { get; set; }
        public string u_name { get; set; }
        public int u_gender { get; set; }
        public System.DateTime u_birthdate { get; set; }
        public System.DateTime u_usersince { get; set; }
        public string u_state { get; set; }
        public string u_city { get; set; }
        public System.DateTime u_lastonline { get; set; }
        public Nullable<int> u_interested_in { get; set; }
        public int u_score { get; set; }
        public int u_credits { get; set; }
        public string u_status_text { get; set; }
        public Nullable<long> u_facebookid { get; set; }
        public string u_country { get; set; }
        public int u_profileviews { get; set; }
        public string u_myspaceid { get; set; }
        public bool u_paid_member { get; set; }
        public string u_email { get; set; }
        public Nullable<bool> p_primary { get; set; }
        public Nullable<bool> p_approved { get; set; }
        public Nullable<bool> p_private { get; set; }
        public Nullable<bool> p_explicit { get; set; }
        public byte[] p_image { get; set; }
        public Nullable<int> p_id { get; set; }
        public string p_name { get; set; }
        public string u_signup_ip { get; set; }
    }
}

using System;

namespace ezFixUp.Model.Models
{
    public class GroupMember
    {
        public int g_id { get; set; }
        public string u_username { get; set; }
        public int gm_type { get; set; }
        public System.DateTime gm_joindate { get; set; }
        public string gm_invitedby { get; set; }
        public bool gm_active { get; set; }
        public string gm_joinanswer { get; set; }
        public bool gm_warned { get; set; }
        public string gm_warn_reason { get; set; }
        public Nullable<System.DateTime> gm_warn_expirationdate { get; set; }
        public virtual Group Group { get; set; }
        public virtual User User { get; set; }
    }
}

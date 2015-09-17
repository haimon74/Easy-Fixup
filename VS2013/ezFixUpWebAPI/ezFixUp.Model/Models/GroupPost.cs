using System;

namespace ezFixUp.Model.Models
{
    public class GroupPost
    {
        public int gp_id { get; set; }
        public int gt_id { get; set; }
        public string u_username { get; set; }
        public System.DateTime gp_dateposted { get; set; }
        public Nullable<System.DateTime> gp_dateedited { get; set; }
        public string gp_editnotes { get; set; }
        public string gp_post { get; set; }
        public virtual GroupTopic GroupTopic { get; set; }
        public virtual User User { get; set; }
    }
}

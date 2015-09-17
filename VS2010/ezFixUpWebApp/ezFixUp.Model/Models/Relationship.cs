using System;

namespace ezFixUp.Model.Models
{
    public class Relationship
    {
        public int r_id { get; set; }
        public string u_username { get; set; }
        public string r_username { get; set; }
        public int r_type { get; set; }
        public Nullable<int> r_pendingtype { get; set; }
        public bool r_accepted { get; set; }
        public System.DateTime r_timestamp { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}

using System;

namespace ezFixUp.Model.Models
{
    public class UnlockedSection
    {
        public int us_id { get; set; }
        public string u_username { get; set; }
        public int us_type { get; set; }
        public string us_targetusername { get; set; }
        public Nullable<int> us_targetid { get; set; }
        public System.DateTime us_unlockeduntil { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}

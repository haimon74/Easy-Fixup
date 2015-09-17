using System;

namespace ezFixUp.Model.Models
{
    public class AbuseReport
    {
        public int ar_id { get; set; }
        public string ar_reportedby { get; set; }
        public int ar_type { get; set; }
        public string ar_report { get; set; }
        public string ar_reporteduser { get; set; }
        public Nullable<int> ar_targetid { get; set; }
        public bool ar_reviewed { get; set; }
        public System.DateTime ar_datereported { get; set; }
    }
}

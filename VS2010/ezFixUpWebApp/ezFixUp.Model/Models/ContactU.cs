using System;

namespace ezFixUp.Model.Models
{
    public class ContactU
    {
        public int cu_id { get; set; }
        public string cu_reportedby { get; set; }
        public int cu_type { get; set; }
        public string cu_report { get; set; }
        public string cu_reporteduser { get; set; }
        public Nullable<int> cu_targetid { get; set; }
        public bool cu_reviewed { get; set; }
        public System.DateTime cu_datereported { get; set; }
    }
}

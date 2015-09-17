using System;

namespace ezFixUp.Model.Models
{
    public class EmailQueue
    {
        public int eq_id { get; set; }
        public string eq_from { get; set; }
        public string eq_to { get; set; }
        public string eq_fromname { get; set; }
        public string eq_toname { get; set; }
        public string eq_cc { get; set; }
        public string eq_bcc { get; set; }
        public string eq_subject { get; set; }
        public string eq_body { get; set; }
        public int eq_tries { get; set; }
        public Nullable<System.DateTime> eq_last_try { get; set; }
        public string eq_last_error { get; set; }
        public Nullable<System.DateTime> eq_next_try { get; set; }
    }
}

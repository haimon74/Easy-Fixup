using System;

namespace ezFixUp.Model.Models
{
    public class ScheduledAnnouncement
    {
        public int sa_id { get; set; }
        public string sa_name { get; set; }
        public string sa_subject { get; set; }
        public string sa_body { get; set; }
        public Nullable<int> sa_gender { get; set; }
        public Nullable<bool> sa_paidmember { get; set; }
        public Nullable<bool> sa_hasphotos { get; set; }
        public Nullable<bool> sa_hasprofile { get; set; }
        public Nullable<int> sa_languageid { get; set; }
        public string sa_country { get; set; }
        public string sa_region { get; set; }
        public int sa_type { get; set; }
        public Nullable<System.DateTime> sa_date { get; set; }
        public Nullable<int> sa_days { get; set; }
    }
}

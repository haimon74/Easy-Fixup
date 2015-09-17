using System;

namespace ezFixUp.Model.Models
{
    public class SavedSearch
    {
        public int ss_id { get; set; }
        public string u_username { get; set; }
        public string ss_name { get; set; }
        public int ss_gender { get; set; }
        public string ss_country { get; set; }
        public string ss_state { get; set; }
        public string ss_zip { get; set; }
        public string ss_city { get; set; }
        public int ss_agefrom { get; set; }
        public int ss_ageto { get; set; }
        public bool ss_photoreq { get; set; }
        public string ss_choiceids { get; set; }
        public bool ss_emailmatches { get; set; }
        public Nullable<System.DateTime> ss_nextemaildate { get; set; }
        public int ss_emailfrequency { get; set; }
        public virtual User User { get; set; }
    }
}

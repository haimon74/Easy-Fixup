using System;

namespace ezFixUp.Model.Models
{
    public class TipsPopup
    {
        public int tp_id { get; set; }
        public Nullable<int> tp_language_id { get; set; }
        public Nullable<int> tp_show_times { get; set; }
        public Nullable<int> tp_login_count_trigger { get; set; }
        public Nullable<int> tp_days_count_trigger { get; set; }
        public string tp_page_name { get; set; }
        public Nullable<int> tp_min_credits_trigger { get; set; }
        public Nullable<int> tp_no_spending_days_trigger { get; set; }
        public string tp_content { get; set; }
        public string tp_title { get; set; }
        public string tp_name_key { get; set; }
        public Nullable<bool> tp_matchmaker_view { get; set; }
    }
}

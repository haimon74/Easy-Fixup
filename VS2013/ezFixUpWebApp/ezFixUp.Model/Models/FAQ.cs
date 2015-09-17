using System;

namespace ezFixUp.Model.Models
{
    public class FAQ
    {
        public int q_id { get; set; }
        public string q_name { get; set; }
        public bool q_active { get; set; }
        public int q_order { get; set; }
        public string q_question { get; set; }
        public string q_answer { get; set; }
        public Nullable<int> q_category_id { get; set; }
    }
}

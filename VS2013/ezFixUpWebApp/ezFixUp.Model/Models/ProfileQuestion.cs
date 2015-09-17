using System;
using System.Collections.Generic;

namespace ezFixUp.Model.Models
{
    public class ProfileQuestion
    {
        public ProfileQuestion()
        {
            this.ProfileAnswers = new List<ProfileAnswer>();
            this.ProfileChoices = new List<ProfileChoice>();
        }

        public int pq_id { get; set; }
        public int pt_id { get; set; }
        public string pq_name { get; set; }
        public string pq_altname { get; set; }
        public string pq_description { get; set; }
        public string pq_hint { get; set; }
        public int pq_edit_style { get; set; }
        public int pq_show_style { get; set; }
        public int pq_search_style { get; set; }
        public bool pq_required { get; set; }
        public int pq_priority { get; set; }
        public bool pq_requires_approval { get; set; }
        public bool pq_visible_male { get; set; }
        public bool pq_visible_female { get; set; }
        public bool pq_visible_couple { get; set; }
        public Nullable<int> pq_match_field { get; set; }
        public bool pq_viewpaidonly { get; set; }
        public bool pq_editpaidonly { get; set; }
        public Nullable<int> pq_parent_question_id { get; set; }
        public string pq_parent_question_choices { get; set; }
        public virtual ICollection<ProfileAnswer> ProfileAnswers { get; set; }
        public virtual ICollection<ProfileChoice> ProfileChoices { get; set; }
        public virtual ProfileTopic ProfileTopic { get; set; }
    }
}

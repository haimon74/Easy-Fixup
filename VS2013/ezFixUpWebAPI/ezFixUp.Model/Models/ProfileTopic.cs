using System.Collections.Generic;

namespace ezFixUp.Model.Models
{
    public class ProfileTopic
    {
        public ProfileTopic()
        {
            this.ProfileQuestions = new List<ProfileQuestion>();
        }

        public int pt_id { get; set; }
        public string pt_name { get; set; }
        public int pt_priority { get; set; }
        public int pt_editcolumns { get; set; }
        public int pt_viewcolumns { get; set; }
        public virtual ICollection<ProfileQuestion> ProfileQuestions { get; set; }
    }
}

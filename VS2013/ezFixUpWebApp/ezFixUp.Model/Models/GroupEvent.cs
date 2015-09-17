using System.Collections.Generic;

namespace ezFixUp.Model.Models
{
    public class GroupEvent
    {
        public GroupEvent()
        {
            this.GroupEventsComments = new List<GroupEventsComment>();
            this.Users = new List<User>();
        }

        public int ge_id { get; set; }
        public int g_id { get; set; }
        public string u_username { get; set; }
        public string ge_title { get; set; }
        public string ge_description { get; set; }
        public byte[] ge_image { get; set; }
        public System.DateTime ge_date { get; set; }
        public string ge_location { get; set; }
        public virtual Group Group { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<GroupEventsComment> GroupEventsComments { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}

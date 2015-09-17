using System;
using System.Collections.Generic;

namespace ezFixUp.Model.Models
{
    public class GroupTopic
    {
        public GroupTopic()
        {
            this.GroupPollsAnswers = new List<GroupPollsAnswer>();
            this.GroupPollsChoices = new List<GroupPollsChoice>();
            this.GroupPosts = new List<GroupPost>();
            this.GroupTopicSubscriptions = new List<GroupTopicSubscription>();
        }

        public int gt_id { get; set; }
        public int g_id { get; set; }
        public string gt_name { get; set; }
        public System.DateTime gt_datecreated { get; set; }
        public System.DateTime gt_dateupdated { get; set; }
        public string u_username { get; set; }
        public int gt_posts { get; set; }
        public bool gt_locked { get; set; }
        public Nullable<System.DateTime> gt_stickyuntil { get; set; }
        public bool gt_poll { get; set; }
        public int gt_views { get; set; }
        public virtual ICollection<GroupPollsAnswer> GroupPollsAnswers { get; set; }
        public virtual ICollection<GroupPollsChoice> GroupPollsChoices { get; set; }
        public virtual ICollection<GroupPost> GroupPosts { get; set; }
        public virtual Group Group { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<GroupTopicSubscription> GroupTopicSubscriptions { get; set; }
    }
}

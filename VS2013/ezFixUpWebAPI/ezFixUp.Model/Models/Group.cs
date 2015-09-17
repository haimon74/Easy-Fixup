using System;
using System.Collections.Generic;

namespace ezFixUp.Model.Models
{
    public class Group
    {
        public Group()
        {
            this.Events = new List<Event>();
            this.GroupBans = new List<GroupBan>();
            this.GroupEvents = new List<GroupEvent>();
            this.GroupMembers = new List<GroupMember>();
            this.GroupPhotos = new List<GroupPhoto>();
            this.GroupTopics = new List<GroupTopic>();
            this.GroupTopicSubscriptions = new List<GroupTopicSubscription>();
            this.Categories = new List<Category>();
        }

        public int g_id { get; set; }
        public string g_name { get; set; }
        public string g_description { get; set; }
        public byte[] g_icon { get; set; }
        public System.DateTime g_datecreated { get; set; }
        public bool g_approved { get; set; }
        public int g_accesslevel { get; set; }
        public string g_owner { get; set; }
        public int g_activemembers { get; set; }
        public string g_jointerms { get; set; }
        public string g_joinquestion { get; set; }
        public long g_permissions { get; set; }
        public Nullable<int> g_minage { get; set; }
        public bool g_autojoin { get; set; }
        public string g_autojoincountry { get; set; }
        public string g_autojoinregion { get; set; }
        public string g_autojoincity { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<GroupBan> GroupBans { get; set; }
        public virtual ICollection<GroupEvent> GroupEvents { get; set; }
        public virtual ICollection<GroupMember> GroupMembers { get; set; }
        public virtual ICollection<GroupPhoto> GroupPhotos { get; set; }
        public virtual ICollection<GroupTopic> GroupTopics { get; set; }
        public virtual ICollection<GroupTopicSubscription> GroupTopicSubscriptions { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace ezFixUp.Model.Models
{
    public class Event
    {
        public Event()
        {
            this.EventComments = new List<EventComment>();
        }

        public int e_id { get; set; }
        public string e_fromusername { get; set; }
        public Nullable<int> e_fromgroup { get; set; }
        public long e_type { get; set; }
        public string e_details { get; set; }
        public System.DateTime e_date { get; set; }
        public virtual ICollection<EventComment> EventComments { get; set; }
        public virtual Group Group { get; set; }
        public virtual User User { get; set; }
    }
}

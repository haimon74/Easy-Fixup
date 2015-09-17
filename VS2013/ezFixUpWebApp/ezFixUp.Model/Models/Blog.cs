using System.Collections.Generic;

namespace ezFixUp.Model.Models
{
    public class Blog
    {
        public Blog()
        {
            this.BlogPosts = new List<BlogPost>();
        }

        public int b_id { get; set; }
        public string u_username { get; set; }
        public string b_name { get; set; }
        public string b_description { get; set; }
        public System.DateTime b_datecreated { get; set; }
        public string b_settings { get; set; }
        public virtual ICollection<BlogPost> BlogPosts { get; set; }
        public virtual User User { get; set; }
    }
}

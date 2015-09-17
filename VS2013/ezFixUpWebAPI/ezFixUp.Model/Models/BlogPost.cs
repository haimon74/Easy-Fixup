using System.Collections.Generic;

namespace ezFixUp.Model.Models
{
    public class BlogPost
    {
        public BlogPost()
        {
            this.BlogPostComments = new List<BlogPostComment>();
        }

        public int bp_id { get; set; }
        public int b_id { get; set; }
        public string bp_title { get; set; }
        public string bp_content { get; set; }
        public System.DateTime bp_dateposted { get; set; }
        public int bp_reads { get; set; }
        public bool bp_approved { get; set; }
        public virtual ICollection<BlogPostComment> BlogPostComments { get; set; }
        public virtual Blog Blog { get; set; }
    }
}

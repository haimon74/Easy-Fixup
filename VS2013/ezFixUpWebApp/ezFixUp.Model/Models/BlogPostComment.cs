
namespace ezFixUp.Model.Models
{
    public class BlogPostComment
    {
        public int bpc_id { get; set; }
        public int bp_id { get; set; }
        public string u_username { get; set; }
        public string bpc_text { get; set; }
        public System.DateTime bpc_dateposted { get; set; }
        public bool bpc_approved { get; set; }
        public virtual BlogPost BlogPost { get; set; }
        public virtual User User { get; set; }
    }
}

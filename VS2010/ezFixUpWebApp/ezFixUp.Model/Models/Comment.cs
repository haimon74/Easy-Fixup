
namespace ezFixUp.Model.Models
{
    public class Comment
    {
        public int c_id { get; set; }
        public string c_from_username { get; set; }
        public string c_to_username { get; set; }
        public string c_comment_text { get; set; }
        public System.DateTime c_date_posted { get; set; }
        public bool c_approved { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}

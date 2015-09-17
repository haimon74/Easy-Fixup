
namespace ezFixUp.Model.Models
{
    public class ChatIgnoredUser
    {
        public int cu_id { get; set; }
        public int ciu_ignoreduserid { get; set; }
        public System.DateTime ciu_ignoredate { get; set; }
        public virtual ChatUser ChatUser { get; set; }
        public virtual ChatUser ChatUser1 { get; set; }
    }
}

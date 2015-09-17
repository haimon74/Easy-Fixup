
namespace ezFixUp.Model.Models
{
    public class GroupBan
    {
        public int gb_id { get; set; }
        public int g_id { get; set; }
        public string u_username { get; set; }
        public System.DateTime gb_expires { get; set; }
        public System.DateTime gb_date { get; set; }
        public virtual Group Group { get; set; }
        public virtual User User { get; set; }
    }
}

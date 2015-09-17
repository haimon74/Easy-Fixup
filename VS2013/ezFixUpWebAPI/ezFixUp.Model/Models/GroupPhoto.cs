
namespace ezFixUp.Model.Models
{
    public class GroupPhoto
    {
        public int gp_id { get; set; }
        public int g_id { get; set; }
        public string u_username { get; set; }
        public string gp_name { get; set; }
        public string gp_description { get; set; }
        public byte[] gp_image { get; set; }
        public System.DateTime gp_date { get; set; }
        public virtual Group Group { get; set; }
        public virtual User User { get; set; }
    }
}

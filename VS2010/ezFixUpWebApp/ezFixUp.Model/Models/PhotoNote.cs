
namespace ezFixUp.Model.Models
{
    public class PhotoNote
    {
        public int pn_id { get; set; }
        public int p_id { get; set; }
        public string u_username { get; set; }
        public string pn_notes { get; set; }
        public System.DateTime pn_timestamp { get; set; }
        public int pn_x { get; set; }
        public int pn_y { get; set; }
        public int pn_width { get; set; }
        public int pn_height { get; set; }
        public virtual Photo Photo { get; set; }
        public virtual User User { get; set; }
    }
}


namespace ezFixUp.Model.Models
{
    public class archive_Photos
    {
        public int p_id { get; set; }
        public string u_username { get; set; }
        public byte[] p_image { get; set; }
        public string p_name { get; set; }
        public string p_description { get; set; }
        public bool p_approved { get; set; }
        public bool p_primary { get; set; }
        public bool p_explicit { get; set; }
        public bool p_private { get; set; }
    }
}

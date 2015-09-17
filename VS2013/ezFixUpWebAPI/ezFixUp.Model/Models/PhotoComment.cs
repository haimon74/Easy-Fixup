
namespace ezFixUp.Model.Models
{
    public class PhotoComment
    {
        public int pc_id { get; set; }
        public int p_id { get; set; }
        public string u_username { get; set; }
        public string pc_comment { get; set; }
        public System.DateTime pc_date { get; set; }
        public virtual Photo Photo { get; set; }
        public virtual User User { get; set; }
    }
}

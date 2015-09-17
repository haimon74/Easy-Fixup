
namespace ezFixUp.Model.Models
{
    public class AdComment
    {
        public int ac_id { get; set; }
        public int a_id { get; set; }
        public string u_username { get; set; }
        public string ac_comment { get; set; }
        public System.DateTime ac_date { get; set; }
        public virtual Ad Ad { get; set; }
        public virtual User User { get; set; }
    }
}

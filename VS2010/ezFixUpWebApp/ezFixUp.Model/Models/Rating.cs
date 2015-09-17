
namespace ezFixUp.Model.Models
{
    public class Rating
    {
        public string r_fromusername { get; set; }
        public string r_tousername { get; set; }
        public int r_rating { get; set; }
        public System.DateTime r_timestamp { get; set; }
        public string r_ip { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}

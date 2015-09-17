
namespace ezFixUp.Model.Models
{
    public class Favourite
    {
        public string u_username { get; set; }
        public string f_username { get; set; }
        public System.DateTime f_timestamp { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}

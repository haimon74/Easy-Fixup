
namespace ezFixUp.Model.Models
{
    public class EstablishedCommunication
    {
        public string ec_from_username { get; set; }
        public string ec_to_username { get; set; }
        public System.DateTime ec_date { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}

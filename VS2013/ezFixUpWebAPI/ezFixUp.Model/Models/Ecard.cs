
namespace ezFixUp.Model.Models
{
    public class Ecard
    {
        public int ec_id { get; set; }
        public int ect_id { get; set; }
        public string ec_from_username { get; set; }
        public string ec_to_username { get; set; }
        public System.DateTime ec_date { get; set; }
        public string ec_message { get; set; }
        public bool ec_deleted_by_fromuser { get; set; }
        public bool ec_deleted_by_touser { get; set; }
        public bool ec_is_opened { get; set; }
        public virtual EcardType EcardType { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}

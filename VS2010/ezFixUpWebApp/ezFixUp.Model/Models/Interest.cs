
namespace ezFixUp.Model.Models
{
    public class Interest
    {
        public int i_id { get; set; }
        public string i_from_username { get; set; }
        public string i_to_username { get; set; }
        public System.DateTime i_date_posted { get; set; }
        public bool i_deleted_by_fromuser { get; set; }
        public bool i_deleted_by_touser { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}

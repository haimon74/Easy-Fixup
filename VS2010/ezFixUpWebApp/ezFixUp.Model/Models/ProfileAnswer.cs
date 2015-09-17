
namespace ezFixUp.Model.Models
{
    public class ProfileAnswer
    {
        public string u_username { get; set; }
        public int pq_id { get; set; }
        public string pa_value { get; set; }
        public bool pa_approved { get; set; }
        public virtual User User { get; set; }
        public virtual ProfileQuestion ProfileQuestion { get; set; }
    }
}

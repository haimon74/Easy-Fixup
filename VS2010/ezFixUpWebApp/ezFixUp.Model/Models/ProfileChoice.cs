
namespace ezFixUp.Model.Models
{
    public class ProfileChoice
    {
        public int pc_id { get; set; }
        public int pq_id { get; set; }
        public string pc_value { get; set; }
        public virtual ProfileQuestion ProfileQuestion { get; set; }
    }
}

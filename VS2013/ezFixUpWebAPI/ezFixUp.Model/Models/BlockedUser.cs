
namespace ezFixUp.Model.Models
{
    public class BlockedUser
    {
        public string bu_userblocker { get; set; }
        public string bu_blockeduser { get; set; }
        public System.DateTime bu_blockdate { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}


namespace ezFixUp.Model.Models
{
    public class VerifiedUser
    {
        public string vu_verifiedby { get; set; }
        public string vu_verifieduser { get; set; }
        public System.DateTime vu_timestamp { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}

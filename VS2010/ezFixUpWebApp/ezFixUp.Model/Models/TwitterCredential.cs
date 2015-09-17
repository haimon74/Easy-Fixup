
namespace ezFixUp.Model.Models
{
    public class TwitterCredential
    {
        public string u_username { get; set; }
        public string tc_username { get; set; }
        public string tc_password { get; set; }
        public virtual User User { get; set; }
    }
}

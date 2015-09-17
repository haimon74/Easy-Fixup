
namespace ezFixUp.Model.Models
{
    public class AudioUpload
    {
        public int au_id { get; set; }
        public string u_username { get; set; }
        public string au_title { get; set; }
        public bool au_approved { get; set; }
        public bool au_private { get; set; }
        public virtual User User { get; set; }
    }
}


namespace ezFixUp.Model.Models
{
    public class AudioAccess
    {
        public string aa_audioowner { get; set; }
        public string aa_audioviewer { get; set; }
        public System.DateTime aa_dateaccessgranted { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}

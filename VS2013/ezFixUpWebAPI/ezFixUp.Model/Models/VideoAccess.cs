
namespace ezFixUp.Model.Models
{
    public class VideoAccess
    {
        public string va_videoviewer { get; set; }
        public string va_videoowner { get; set; }
        public System.DateTime va_dateaccessgranted { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}

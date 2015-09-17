
namespace ezFixUp.Model.Models
{
    public class VideoEmbed
    {
        public int ve_id { get; set; }
        public string u_username { get; set; }
        public string ve_thumburl { get; set; }
        public string ve_videourl { get; set; }
        public string ve_title { get; set; }
        public int ve_sourcetype { get; set; }
        public virtual User User { get; set; }
    }
}

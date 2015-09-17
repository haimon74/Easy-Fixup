
namespace ezFixUp.Model.Models
{
    public class VideoUpload
    {
        public int vu_id { get; set; }
        public string u_username { get; set; }
        public bool vu_processed { get; set; }
        public bool vu_approved { get; set; }
        public bool vu_private { get; set; }
    }
}


namespace ezFixUp.Model.Models
{
    public class AdPhoto
    {
        public int ap_id { get; set; }
        public int a_id { get; set; }
        public string ap_description { get; set; }
        public byte[] ap_image { get; set; }
        public virtual Ad Ad { get; set; }
    }
}

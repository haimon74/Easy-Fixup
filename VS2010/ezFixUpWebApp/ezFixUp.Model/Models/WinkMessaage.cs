
namespace ezFixUp.Model.Models
{
    public class WinkMessaage
    {
        public int wm_id { get; set; }
        public int wm_message_id { get; set; }
        public int wm_wink_id { get; set; }
        public virtual WinkType WinkType { get; set; }
    }
}

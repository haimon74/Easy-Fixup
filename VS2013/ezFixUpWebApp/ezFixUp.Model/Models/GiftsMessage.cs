
namespace ezFixUp.Model.Models
{
    public class GiftsMessage
    {
        public int gm_id { get; set; }
        public int gm_gift_id { get; set; }
        public int gm_message_id { get; set; }
        public virtual GiftType GiftType { get; set; }
    }
}

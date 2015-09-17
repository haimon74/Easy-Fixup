
namespace ezFixUp.Model.Models
{
    public class GroupTopicSubscription
    {
        public int gts_id { get; set; }
        public int gt_id { get; set; }
        public int g_id { get; set; }
        public string u_username { get; set; }
        public System.DateTime gts_dateupdated { get; set; }
        public virtual Group Group { get; set; }
        public virtual GroupTopic GroupTopic { get; set; }
        public virtual User User { get; set; }
    }
}

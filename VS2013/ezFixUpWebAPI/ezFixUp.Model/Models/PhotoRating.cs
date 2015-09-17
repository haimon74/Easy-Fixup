
namespace ezFixUp.Model.Models
{
    public class PhotoRating
    {
        public string pr_fromusername { get; set; }
        public int p_id { get; set; }
        public int pr_rating { get; set; }
        public System.DateTime pr_timestamp { get; set; }
        public virtual Photo Photo { get; set; }
        public virtual User User { get; set; }
    }
}

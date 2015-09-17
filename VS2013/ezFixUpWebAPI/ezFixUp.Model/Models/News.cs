
namespace ezFixUp.Model.Models
{
    public class News
    {
        public int n_id { get; set; }
        public System.DateTime n_date { get; set; }
        public string n_text { get; set; }
        public string n_title { get; set; }
        public int l_id { get; set; }
        public virtual Language Language { get; set; }
    }
}

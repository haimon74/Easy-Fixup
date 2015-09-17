
namespace ezFixUp.Model.Models
{
    public class Translation
    {
        public int l_id { get; set; }
        public string t_key { get; set; }
        public int t_adminpanel { get; set; }
        public string t_value { get; set; }
        public virtual Language Language { get; set; }
    }
}

using System;

namespace ezFixUp.Model.Models
{
    public class ContentPage
    {
        public int cp_id { get; set; }
        public string cp_title { get; set; }
        public string cp_content { get; set; }
        public Nullable<int> cp_header_position { get; set; }
        public Nullable<int> cp_footer_position { get; set; }
        public int l_id { get; set; }
        public int cp_visiblefor { get; set; }
        public string cp_url { get; set; }
        public string cp_metadescription { get; set; }
        public string cp_metakeyword { get; set; }
        public string cp_urlrewrite { get; set; }
        public virtual Language Language { get; set; }
    }
}

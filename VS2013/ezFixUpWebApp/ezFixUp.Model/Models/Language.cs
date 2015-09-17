using System.Collections.Generic;

namespace ezFixUp.Model.Models
{
    public class Language
    {
        public Language()
        {
            this.ContentPages = new List<ContentPage>();
            this.News = new List<News>();
            this.Translations = new List<Translation>();
            this.WinkCategories = new List<WinkCategory>();
        }

        public int l_id { get; set; }
        public string l_language { get; set; }
        public bool l_active { get; set; }
        public bool l_predefined { get; set; }
        public int l_order { get; set; }
        public string l_browser_languages { get; set; }
        public string l_ipcountries { get; set; }
        public string l_theme { get; set; }
        public string l_dir { get; set; }
        public virtual ICollection<ContentPage> ContentPages { get; set; }
        public virtual ICollection<News> News { get; set; }
        public virtual ICollection<Translation> Translations { get; set; }
        public virtual ICollection<WinkCategory> WinkCategories { get; set; }
    }
}

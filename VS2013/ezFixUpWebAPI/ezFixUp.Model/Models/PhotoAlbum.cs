using System;
using System.Collections.Generic;

namespace ezFixUp.Model.Models
{
    public class PhotoAlbum
    {
        public PhotoAlbum()
        {
            this.Photos = new List<Photo>();
        }

        public int pa_id { get; set; }
        public string u_username { get; set; }
        public string pa_name { get; set; }
        public int pa_access { get; set; }
        public Nullable<int> pa_cover_photoid { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
    }
}

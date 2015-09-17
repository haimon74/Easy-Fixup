using System;
using System.Collections.Generic;

namespace ezFixUp.Model.Models
{
    public class Photo
    {
        public Photo()
        {
            this.CommunityPhotoApprovals = new List<CommunityPhotoApproval>();
            this.PhotoComments = new List<PhotoComment>();
            this.PhotoNotes = new List<PhotoNote>();
            this.PhotoRatings = new List<PhotoRating>();
        }

        public int p_id { get; set; }
        public string u_username { get; set; }
        public Nullable<int> pa_id { get; set; }
        public byte[] p_image { get; set; }
        public string p_name { get; set; }
        public string p_description { get; set; }
        public bool p_approved { get; set; }
        public bool p_primary { get; set; }
        public bool p_explicit { get; set; }
        public bool p_private { get; set; }
        public string p_approvedby { get; set; }
        public Nullable<System.DateTime> p_approvedtimestamp { get; set; }
        public string p_facecrop { get; set; }
        public bool p_manual_approval { get; set; }
        public bool p_salute { get; set; }
        public virtual ICollection<CommunityPhotoApproval> CommunityPhotoApprovals { get; set; }
        public virtual PhotoAlbum PhotoAlbum { get; set; }
        public virtual ICollection<PhotoComment> PhotoComments { get; set; }
        public virtual ICollection<PhotoNote> PhotoNotes { get; set; }
        public virtual ICollection<PhotoRating> PhotoRatings { get; set; }
        public virtual User User { get; set; }
    }
}

using System.Collections.Generic;

namespace ezFixUp.Model.Models
{
    public class Ad
    {
        public Ad()
        {
            this.AdComments = new List<AdComment>();
            this.AdPhotos = new List<AdPhoto>();
        }

        public int a_id { get; set; }
        public int ac_id { get; set; }
        public string a_postedby { get; set; }
        public System.DateTime a_date { get; set; }
        public System.DateTime a_expirationdate { get; set; }
        public string a_location { get; set; }
        public string a_subject { get; set; }
        public string a_description { get; set; }
        public bool a_approved { get; set; }
        public virtual ICollection<AdComment> AdComments { get; set; }
        public virtual ICollection<AdPhoto> AdPhotos { get; set; }
        public virtual AdsCategory AdsCategory { get; set; }
        public virtual User User { get; set; }
    }
}

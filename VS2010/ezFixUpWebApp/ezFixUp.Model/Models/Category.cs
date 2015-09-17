using System.Collections.Generic;

namespace ezFixUp.Model.Models
{
    public class Category
    {
        public Category()
        {
            this.Groups = new List<Group>();
        }

        public int c_id { get; set; }
        public string c_name { get; set; }
        public int c_order { get; set; }
        public bool c_userscancreategroups { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
    }
}

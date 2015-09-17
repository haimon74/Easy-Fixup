using System.Collections.Generic;

namespace ezFixUp.Model.Models
{
    public class ECardCategory
    {
        public ECardCategory()
        {
            this.EcardTypes = new List<EcardType>();
        }

        public int ecc_id { get; set; }
        public string ecc_name { get; set; }
        public virtual ICollection<EcardType> EcardTypes { get; set; }
    }
}

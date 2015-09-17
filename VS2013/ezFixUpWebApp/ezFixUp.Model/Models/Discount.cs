
namespace ezFixUp.Model.Models
{
    public class Discount
    {
        public int d_id { get; set; }
        public decimal d_amount { get; set; }
        public int d_type { get; set; }
        public int d_field { get; set; }
        public string d_match { get; set; }
    }
}

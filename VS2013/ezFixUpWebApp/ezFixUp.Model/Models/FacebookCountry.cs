using System;

namespace ezFixUp.Model.Models
{
    public class FacebookCountry
    {
        public int fc_id { get; set; }
        public string fc_country_code { get; set; }
        public string fc_name { get; set; }
        public Nullable<bool> fc_supports_region { get; set; }
    }
}

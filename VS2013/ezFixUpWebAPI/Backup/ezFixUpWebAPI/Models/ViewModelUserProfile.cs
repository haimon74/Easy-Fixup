using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ezFixUpWebAPI.Models
{
    public class ViewModelUserProfile
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string PrimaryImageUrl { get; set; }
        public IList<string> ImagesUrl { get; set; }
        
        
    }
}
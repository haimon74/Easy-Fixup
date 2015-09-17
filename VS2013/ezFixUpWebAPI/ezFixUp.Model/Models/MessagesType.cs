using System.Collections.Generic;

namespace ezFixUp.Model.Models
{
    public class MessagesType
    {
        public MessagesType()
        {
            this.Messages = new List<Message>();
        }

        public int mt_id { get; set; }
        public string mt_name { get; set; }
        public bool mt_active { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}

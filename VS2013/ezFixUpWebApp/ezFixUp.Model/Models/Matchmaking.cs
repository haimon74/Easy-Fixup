using System;

namespace ezFixUp.Model.Models
{
    public class Matchmaking
    {
        public int mm_id { get; set; }
        public string mm_matchmaker_username { get; set; }
        public string mm_to_friend_username { get; set; }
        public string mm_with_friend_username { get; set; }
        public System.DateTime mm_timestamp { get; set; }
        public bool mm_to_friend_ack { get; set; }
        public Nullable<bool> mm_with_friend_ack { get; set; }
        public string mm_status { get; set; }
    }
}

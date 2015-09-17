using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace ezFixUp.Classes
{
    public class MatchMaking
    {
        private int? _id;
        private string _matchmakerUsername;
        private string _friend1Username;
        private string _friend2Username;
        private bool _friend1Ack;
        private bool _friend2Ack;
        private DateTime _timestamp;
        private eMatchingStatus _matchingStatus;

        public eMatchingStatus MatchingStatus
        {
            get { return _matchingStatus; }
            set { _matchingStatus = value; }
        }

        public int? Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public enum eMatchingStatus
        {
            eRequested = 1,
            eForwarded = 2,
            eApproved = 3,
            eDenied = 4,
            eMatched = 5
        };

        public string MatchmakerUsername
        {
            get { return _matchmakerUsername; }
            set { _matchmakerUsername = value; }
        }

        public string Friend1Username
        {
            get { return _friend1Username; }
            set { _friend1Username = value; }
        }

        public string Friend2Username
        {
            get { return _friend2Username; }
            set { _friend2Username = value; }
        }

        public bool Friend1Ack
        {
            get { return _friend1Ack; }
            set { _friend1Ack = value; }
        }

        public bool Friend2Ack
        {
            get { return _friend2Ack; }
            set { _friend2Ack = value; }
        }

        public DateTime Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }

        public void Save()
        {
            //using (var conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "SaveMatchmakings", Id, 
                                          MatchmakerUsername, (int)MatchingStatus, 
                                          Friend1Username, Friend2Username,
                                          Friend1Ack, Friend2Ack,
                                          Timestamp);
            }
        }

        public static MatchMaking[] FetchMatchMakings(string mmUsername)
        {
            var lMatchings = new List<MatchMaking>();

            //using (var conn = Config.DB.Open())
            {
                using (
                    var reader = SqlHelper.GetDB()
                                          .ExecuteReader("FetchMatchmakings", 0, mmUsername, null, null, null, null,
                                                         null))
                {

                    while (reader.Read())
                    {
                        lMatchings.Add(new MatchMaking
                            {
                                MatchmakerUsername = (string) reader["MatchmakerUsername"],
                                Friend1Username = (string) reader["ToFriendUsername"],
                                Friend2Username = (string) reader["WithFriendUsername"],
                                Timestamp = (DateTime) reader["MatchDateTime"],
                                Friend1Ack = (bool) reader["ToFriendAck"],
                                Friend2Ack = (bool) reader["WithFriendAck"]
                            });
                    }
                    reader.Close();
                }
            }
            MatchMaking[] result = lMatchings.ToArray();
            return result;
        }
        public static MatchMaking[] FetchMatchMakingsInProgress(string mmUsername)
        {
            var lMatchings = FetchMatchMakings(mmUsername).ToList();
            MatchMaking[] result = lMatchings.Where(m => !(m.Friend1Ack && m.Friend2Ack) && m.Timestamp > DateTime.Now.AddMonths(-1)).ToArray();
            return result;
        }
        
    }
}

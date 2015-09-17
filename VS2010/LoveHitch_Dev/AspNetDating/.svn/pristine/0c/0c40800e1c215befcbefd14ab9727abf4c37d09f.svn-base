using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace AspNetDating.Classes
{
    public class PhotoContestRank
    {
        #region Fields

        private string username;
        private int contestId;
        private int entryId;
        private int value;

        #endregion

        #region Properties

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public int ContestId
        {
            get { return contestId; }
            set { contestId = value; }
        }

        public int EntryId
        {
            get { return entryId; }
            set { entryId = value; }
        }

        public int Value
        {
            get { return value; }
            set { this.value = value; }
        }

        #endregion

        private PhotoContestRank()
        {
        }

        public PhotoContestRank(string username, int contestId, int entryId, int value)
        {
            this.username = username;
            this.contestId = contestId;
            this.entryId = entryId;
            this.value = value;
        }

        public static PhotoContestRank[] Load(string username, int contestId)
        {
            return Load(username, contestId, null, null);
        }

        public static PhotoContestRank[] Load(string username, int? contestId, int? entryId, int? value)
        {
            List<PhotoContestRank> ranks = new List<PhotoContestRank>();
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = SqlHelper.ExecuteReader(conn, "LoadPhotoContestRanks", username,
                    contestId, entryId, value);

                while (reader.Read())
                {
                    PhotoContestRank rank = new PhotoContestRank();

                    rank.username = (string) reader["Username"];
                    rank.contestId = (int) reader["ContestId"];
                    rank.entryId = (int)reader["EntryId"];
                    rank.value = (int)reader["Value"];

                    ranks.Add(rank);
                }
            }

            return ranks.ToArray();
        }

        public void Save()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "SavePhotoContestRank", username, contestId,
                    entryId, value);
            }
        }
    }
}
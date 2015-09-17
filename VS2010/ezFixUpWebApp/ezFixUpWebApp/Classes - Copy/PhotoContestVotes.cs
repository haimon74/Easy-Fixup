using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ezFixUp.Data
{
    /// <summary>
    /// This class handles the photo contest votes
    /// </summary>
    public static class PhotoContestVotes
    {
        /// <summary>
        /// Saves the vote.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="pickedEntryId">The picked entry id.</param>
        /// <param name="nonpickedEntryId">The nonpicked entry id.</param>
        public static void SaveVote(string username, int pickedEntryId, int nonpickedEntryId)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "SavePhotoContestVote", username, pickedEntryId, nonpickedEntryId);
            }
        }

        /// <summary>
        /// Fetches the percentage.
        /// </summary>
        /// <param name="entry1Id">The entry1 id.</param>
        /// <param name="entry2Id">The entry2 id.</param>
        /// <returns></returns>
        public static decimal? FetchPercentage(int entry1Id, int entry2Id)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.GetDB().ExecuteScalar( "FetchPhotoContestVotePercentage", entry1Id, entry2Id);

                return result is decimal ? (decimal) result : (decimal?) null;
            }
        }
    }
}
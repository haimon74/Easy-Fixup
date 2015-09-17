using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.SqlClient;

namespace ezFixUp.Classes
{
    public class UnlockedSection
    {
        public enum SectionType
        {
            Photos,
            Video,
            IM,
            VideoStream
        }

        #region fields

        //private int? id;
        //private string username;
        //private string targetUsername;
        //private SectionType type;
        //private int? targetId;
        //private DateTime unlockedUntil;

        #endregion

        #region Constructors

        private UnlockedSection()
        { }

        //public UnlockedSection(string username, string targetUsername, SectionType type)
        //{
        //    this.username = username;
        //    this.targetUsername = targetUsername;
        //    this.type = type;
        //}

        #endregion

        #region Properties

        //public int ID
        //{
        //    get
        //    {
        //        if (id.HasValue)
        //            return id.Value;
        //        else throw new Exception("ID is not set");
        //    }
        //}

        //public string Username
        //{
        //    get { return username; }
        //    set { username = value; }
        //}

        //public string TargetUsername
        //{
        //    get { return targetUsername; }
        //    set { targetUsername = value; }
        //}

        //public SectionType Type
        //{
        //    get { return type; }
        //    set { type = value; }
        //}

        //public int? TargetID
        //{
        //    get { return targetId; }
        //    set { targetId = value; }
        //}

        //public DateTime UnlockedUntil
        //{
        //    get { return unlockedUntil; }
        //    set { unlockedUntil = value; }
        //}
        #endregion

        #region Methods

        public static void UnlockSection(string username, string targetUsername, SectionType type, int? targetId, DateTime unlockedUntil)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "UnlockSection",
                                                        username, targetUsername, type, targetId, unlockedUntil);
            }
        }

        public static bool IsSectionUnlocked(string username, string targetUsername, SectionType type, int? targetId)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.GetDB().ExecuteScalar( "IsSectionUnlocked",
                                                        username, targetUsername, type, targetId);

                return Convert.ToBoolean(result);
            }
        }



        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ezFixUp.Classes
{
    public class AudioUpload
    {
        #region Fields

        private int? id;
        private string username;
        private string title;
        private bool approved = false;
        private bool isPrivate = false;

        #endregion

        #region Properties

        public int Id
        {
            get { return id.Value; }
            set { id = value; }
        }

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public bool Approved
        {
            get { return approved; }
            set { approved = value; }
        }

        public bool IsPrivate
        {
            get { return isPrivate; }
            set { isPrivate = value; }
        }

        #endregion

        #region Constructors

        private AudioUpload()
        {
        }

        public AudioUpload(string username)
        {
            this.username = username;
        }

        #endregion

        #region Methods

        public void Save()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.GetDB().ExecuteScalar( "SaveAudioUpload", id, username, title, approved, isPrivate);

                if (!id.HasValue)
                    id = Convert.ToInt32(result);
            }
        }

        public static AudioUpload Load(int id)
        {
            AudioUpload[] lAudioUpload = Load(id, null, null, null);
            return lAudioUpload.Length > 0 ? lAudioUpload[0] : null;
        }

        public static AudioUpload[] Load(int? id, string username, bool? approved, bool? isPrivate)
        {
            List<AudioUpload> lAudioUpload = new List<AudioUpload>();
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = (SqlDataReader) SqlHelper.GetDB().ExecuteReader( "LoadAudioUploads", id,
                                                               username, approved, isPrivate);

                while (reader.Read())
                {
                    AudioUpload audioUpload = new AudioUpload();

                    audioUpload.id = (int)reader["Id"];
                    audioUpload.username = (string)reader["Username"];
                    audioUpload.title = (string)reader["Title"];
                    audioUpload.approved = (bool)reader["Approved"];
                    audioUpload.isPrivate = (bool)reader["Private"];
                    lAudioUpload.Add(audioUpload);
                }
            }

            return lAudioUpload.ToArray();
        }

        public void Delete()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "DeleteAudioUpload", id);
            }

            string userFilesDir = Config.Directories.Home + @"\UserFiles\" + username;
            string audioUpload = String.Format(@"{0}\audio_{1}.mp3", userFilesDir, id);

            try
            {
                File.Delete(audioUpload);
            }
            catch (Exception err)
            {
                Global.Logger.LogInfo("DeleteAudioUpload", err);
            }
        }

        public static void Delete(int id)
        {
            AudioUpload audioUpload = Load(id);
            audioUpload.Delete();
        }

        /// <summary>
        /// Determines whether the specified username has approved audio upload.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>
        /// 	<c>true</c> if [has audio upload] [the specified username]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasAudioUpload(string username)
        {
            AudioUpload[] uploads = Load(null, username, true, null);
            return uploads.Length > 0;
        }

        #endregion
    }
}

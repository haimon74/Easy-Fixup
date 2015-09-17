using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.ApplicationBlocks.Data;

namespace AspNetDating.Classes
{
    public class VideoEmbed
    {
        #region Types & enumbs

        public enum EmbedSourceType
        {
            YouTube = 1
        }

        #endregion

        #region Fields

        private int? id;
        private string username;
        private string thumbUrl;
        private string videoUrl;
        private string title;
        private EmbedSourceType sourceType = EmbedSourceType.YouTube;

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

        public string ThumbUrl
        {
            get { return thumbUrl; }
            set { thumbUrl = value; }
        }

        public string VideoUrl
        {
            get { return videoUrl; }
            set { videoUrl = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public EmbedSourceType SourceType
        {
            get { return sourceType; }
            set { sourceType = value; }
        }

        #endregion

        private VideoEmbed()
        {
        }

        public VideoEmbed(string username, string videoUrl)
        {
            this.username = username;
            this.videoUrl = videoUrl;
        }

        public void Save()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.ExecuteScalar(conn, "SaveVideoEmbed", id, username, 
                    thumbUrl, videoUrl, title, sourceType);

                if (!id.HasValue)
                    id = Convert.ToInt32(result);
            }
        }

        public static VideoEmbed Load(int id)
        {
            List<VideoEmbed> videoEmbeds = Load(id, null, null);
            return videoEmbeds.Count > 0 ? videoEmbeds[0] : null;
        }

        public static List<VideoEmbed> Load(int? id, string username, int? numberOfVideos)
        {
            List<VideoEmbed> videoEmbeds = new List<VideoEmbed>();
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = SqlHelper.ExecuteReader(conn, "LoadVideoEmbeds", id, username, numberOfVideos);

                while (reader.Read())
                {
                    VideoEmbed videoEmbed = new VideoEmbed();

                    videoEmbed.id = (int)reader["Id"];
                    videoEmbed.username = (string)reader["Username"];
                    videoEmbed.thumbUrl = (string)reader["ThumbUrl"];
                    videoEmbed.videoUrl = (string)reader["VideoUrl"];
                    videoEmbed.title = (string)reader["Title"];
                    videoEmbed.sourceType = (EmbedSourceType) reader["SourceType"];
                    videoEmbeds.Add(videoEmbed);
                }
            }

            return videoEmbeds;
        }

        public void Delete()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "DeleteVideoEmbed", id);
            }
        }

        public static void Delete(int id)
        {
            VideoEmbed videoEmbed = Load(id);
            videoEmbed.Delete();
        }
    }
}

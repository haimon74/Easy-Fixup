using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Linq;
using AspNetDating.Classes;

namespace AspNetDating.Components.Profile
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "https://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class UserAudioData : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string username = context.Request.Params["uid"];
            string viewerUsername = context.Request.Params["vid"];

            context.Response.ContentType = "text/xml";
            XmlWriter xml = XmlWriter.Create(context.Response.Output);
            xml.WriteStartDocument();
            xml.WriteStartElement("playlist");
            xml.WriteStartElement("trackList");

            AudioUpload[] audioUploads = AudioUpload.Load(null, username, true, null);
            if (audioUploads != null && audioUploads.Length > 0)
            {
                foreach (AudioUpload audioUpload in audioUploads)
                {
                    if (audioUpload.IsPrivate && username != viewerUsername 
                        // viewerUsername is "" if the viewer is not logged in
                        && (viewerUsername == String.Empty || !User.HasUserAccessToPrivateAudio(username, viewerUsername)))
                        continue;

                    string location = String.Format("{0}/UserFiles/{1}/audio_{2}.mp3", Config.Urls.Home, username, audioUpload.Id);
                    xml.WriteStartElement("track");
                    xml.WriteElementString("title", audioUpload.Title);
                    xml.WriteElementString("annotation", audioUpload.Title);
                    xml.WriteElementString("creator", audioUpload.Username);
                    xml.WriteElementString("location", location);
                    xml.WriteEndElement();    
                }
            }

            xml.WriteEndElement();
            xml.WriteEndElement();
            xml.WriteEndDocument();
            xml.Close();
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}

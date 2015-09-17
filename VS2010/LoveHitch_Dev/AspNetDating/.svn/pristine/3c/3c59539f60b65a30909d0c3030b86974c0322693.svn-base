using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Xml;
using AspNetDating.Classes;

namespace AspNetDating
{
    public class CoolIrisHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Params["feed"] == null) return;

            switch (context.Request.Params["feed"].ToLower())
            {
                case "newusers":
                    GenerateNewUsersFeed(context);
                    break;
                case "userphotos":
                    GenerateUserPhotosFeed(context);
                    break;
                case "groupphotos":
                    GenerateGroupPhotosFeed(context);
                    break;
            }
        }

        private static void GenerateNewUsersFeed(HttpContext context)
        {
            byte[] xmlResponse;
            int minAge, maxAge;
            User.eGender? gender = null;
            if (!int.TryParse(context.Request.Params["minage"], out minAge))
                minAge = Config.Users.MinAge;
            if (!int.TryParse(context.Request.Params["maxage"], out maxAge))
                maxAge = Config.Users.MaxAge;
            try
            {
                gender = (User.eGender?) Enum.Parse(typeof (User.eGender), context.Request.Params["gender"]);
            }
            catch (ArgumentNullException) {}
            catch (ArgumentException) {}
            

            string cacheKey = String.Format("RssHandler_GenerateNewUsersFeed_{0}_{1}_{2}",
                gender, minAge, maxAge);
            if (context.Cache[cacheKey] != null)
            {
                xmlResponse = context.Cache[cacheKey] as byte[];
            }
            else
            {
                var nuSearch = new NewUsersSearch
                                   {
                                       PhotoReq = true,
                                       UsersCount = 20,
                                       MinAge = minAge,
                                       MaxAge = maxAge
                                   };
                if (gender.HasValue)
                    nuSearch.Gender = gender.Value;

                UserSearchResults nuResults = nuSearch.GetResults();

                var ms = new MemoryStream();
                var objX = new XmlTextWriter(ms, Encoding.UTF8);

                objX.WriteStartDocument();
                objX.WriteStartElement("rss");
                objX.WriteAttributeString("version", "2.0");
                objX.WriteAttributeString("xmlns:media", "https://search.yahoo.com/mrss");
                objX.WriteStartElement("channel");

                if (nuResults != null && nuResults.Usernames != null)
                {
                    foreach (string username in nuResults.Usernames)
                    {
                        User user = User.Load(username);
                        var age = (int) (DateTime.Now.Subtract(user.Birthdate).TotalDays/365.25);
                        var sloganText = "";
                        try
                        {
                            var slogan = user.FetchSlogan();
                            if (slogan.Approved)
                                sloganText = slogan.Value;
                        }
                        catch (NotFoundException) {}
                        string descr = String.Format(Lang.Trans("{0} ({1}/{2})\n{3}"),
                                                     username, user.Gender, age, sloganText);

                        objX.WriteStartElement("item");
                        objX.WriteElementString("title", username);
                        objX.WriteElementString("description", descr);
                        objX.WriteElementString("link", Config.Urls.Home + "/ShowUser.aspx?uid=" + username);
                        objX.WriteStartElement("media:thumbnail");
                        objX.WriteAttributeString("url", ImageHandler.CreateImageUrl(
                            Photo.GetPrimary(username).Id, 150, 150, false, true, true));
                        objX.WriteEndElement();
                        objX.WriteStartElement("media:content");
                        objX.WriteAttributeString("url", ImageHandler.CreateImageUrl(
                            Photo.GetPrimary(username).Id, Config.Photos.PhotoMaxWidth, 
                            Config.Photos.PhotoMaxHeight, false, true, true));
                        objX.WriteEndElement();
                        objX.WriteEndElement();
                    }
                }

                objX.WriteEndElement();
                objX.WriteEndElement();
                objX.WriteEndDocument();
                objX.Flush();
                objX.Close();

                xmlResponse = ms.ToArray();

                context.Cache.Insert(cacheKey, xmlResponse, null, DateTime.Now.AddMinutes(10),
                                     Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
            }

            context.Response.Clear();
            context.Response.ContentType = "application/xml";
            if (xmlResponse != null) context.Response.BinaryWrite(xmlResponse);
            context.Response.End();
        }

        private static void GenerateUserPhotosFeed(HttpContext context)
        {
            byte[] xmlResponse;

            if (context.Request.Params["username"] == null) return;
            string username = context.Request.Params["username"];

            string cacheKey = String.Format("RssHandler_GenerateUserPhotosFeed_{0}",
                username);
            if (context.Cache[cacheKey] != null)
            {
                xmlResponse = context.Cache[cacheKey] as byte[];
            }
            else
            {
                User user = User.Load(username);
                if (user == null) return;

                Photo[] photos = Photo.Fetch(username);

                var ms = new MemoryStream();
                var objX = new XmlTextWriter(ms, Encoding.UTF8);

                objX.WriteStartDocument();
                objX.WriteStartElement("rss");
                objX.WriteAttributeString("version", "2.0");
                objX.WriteAttributeString("xmlns:media", "https://search.yahoo.com/mrss");
                objX.WriteStartElement("channel");

                if (photos != null && photos.Length > 0)
                {
                    foreach (Photo photo in photos)
                    {
                        if (!photo.Approved || photo.PrivatePhoto) continue;

                        objX.WriteStartElement("item");
                        objX.WriteElementString("title", photo.Name ?? "");
                        objX.WriteElementString("description", photo.Description ?? "");
                        objX.WriteStartElement("media:thumbnail");
                        objX.WriteAttributeString("url", ImageHandler.CreateImageUrl(
                            photo.Id, 150, 150, false, true, true));
                        objX.WriteEndElement();
                        objX.WriteStartElement("media:content");
                        objX.WriteAttributeString("url", ImageHandler.CreateImageUrl(
                            photo.Id, Config.Photos.PhotoMaxWidth,
                            Config.Photos.PhotoMaxHeight, false, true, true));
                        objX.WriteEndElement();
                        objX.WriteEndElement();
                    }
                }

                objX.WriteEndElement();
                objX.WriteEndElement();
                objX.WriteEndDocument();
                objX.Flush();
                objX.Close();

                xmlResponse = ms.ToArray();

                context.Cache.Insert(cacheKey, xmlResponse, null, DateTime.Now.AddMinutes(10),
                                     Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
            }

            context.Response.Clear();
            context.Response.ContentType = "application/xml";
            if (xmlResponse != null) context.Response.BinaryWrite(xmlResponse);
            context.Response.End();
        }

        private static void GenerateGroupPhotosFeed(HttpContext context)
        {
            byte[] xmlResponse;

            if (context.Request.Params["groupid"] == null) return;
            int groupId;
            if (!int.TryParse(context.Request.Params["groupid"], out groupId)) return;

            string cacheKey = String.Format("RssHandler_GenerateGroupPhotosFeed_{0}",
                groupId);
            if (context.Cache[cacheKey] != null)
            {
                xmlResponse = context.Cache[cacheKey] as byte[];
            }
            else
            {
                Group group = Group.Fetch(groupId);
                if (group == null) return;

                GroupPhoto[] groupPhotos = GroupPhoto.Fetch(groupId, 100, 
                    GroupPhoto.eSortColumn.DateUploaded);

                var ms = new MemoryStream();
                var objX = new XmlTextWriter(ms, Encoding.UTF8);

                objX.WriteStartDocument();
                objX.WriteStartElement("rss");
                objX.WriteAttributeString("version", "2.0");
                objX.WriteAttributeString("xmlns:media", "https://search.yahoo.com/mrss");
                objX.WriteStartElement("channel");

                if (groupPhotos != null && groupPhotos.Length > 0)
                {
                    foreach (GroupPhoto photo in groupPhotos)
                    {
                        objX.WriteStartElement("item");
                        objX.WriteElementString("title", photo.Name ?? "");
                        objX.WriteElementString("description", photo.Description ?? "");
                        objX.WriteStartElement("media:thumbnail");
                        objX.WriteAttributeString("url", String.Format(
                            "GroupImage.ashx?gpid={0}&width=150&height=150&diskCache=1", photo.ID));
                        objX.WriteEndElement();
                        objX.WriteStartElement("media:content");
                        objX.WriteAttributeString("url", String.Format(
                            "GroupImage.ashx?gpid={0}&width={1}&height={2}&diskCache=1", photo.ID,
                            Config.Groups.GroupPhotoMaxWidth, Config.Groups.GroupPhotoMaxHeight));
                        objX.WriteEndElement();
                        objX.WriteEndElement();
                    }
                }

                objX.WriteEndElement();
                objX.WriteEndElement();
                objX.WriteEndDocument();
                objX.Flush();
                objX.Close();

                xmlResponse = ms.ToArray();

                context.Cache.Insert(cacheKey, xmlResponse, null, DateTime.Now.AddMinutes(10),
                                     Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
            }

            context.Response.Clear();
            context.Response.ContentType = "application/xml";
            if (xmlResponse != null) context.Response.BinaryWrite(xmlResponse);
            context.Response.End();
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}
/* ezFixUp 
 * Copyright (C) 2011 eStream 
 * https://ezFixUp.com
 *  
 */
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Xml;
using System.Xml.Serialization;
using ezFixUp.Classes.ezFixUpParallel;
using ezFixUp.Services;
using Facebook;


namespace ezFixUp.Classes
{
    [Serializable]
    public class BaseFacebookSearchResults : SearchResults<string, User>
    {
        public string[] Usernames
        {
            get { return Results; }
            set { Results = value; }
        }
        protected override User LoadResult(string id)
        {
            try
            {
                if (Usernames.Contains(id))
                {
                    return Classes.User.Load(id);
                }
                else
                {
                    long longId;
                    return long.TryParse(id, out longId)
                               ? FacebookHelper.LoadUser(longId)
                               : FacebookHelper.LoadUser(null);
                }
            }
            catch (NotFoundException)
            {
                return null;
            }
        }
        protected override User[] LoadResults(string[] ids)
        {
            List<string> keysList = ids.ToList();
            List<User> resultList = new List<User>();
            if (keysList.Count() > 0)
            {
                var parameterizedThreadsList = new ParameterizedThreadsList<string, User>(
                                                    keysList, resultList,
                                                    (new LoadUserResultThreadClass(keysList, resultList)).DoWorkThread,
                                                    HttpContext.Current);
                parameterizedThreadsList.RunAsBackgroundThreads = true;
                parameterizedThreadsList.ExecuteParallelWork(5);
            }
            return resultList.ToArray();
        }
        public int GetTotalPages()
        {
            return GetTotalPages(Config.Search.UsersPerPage);
        }
        public new int GetTotalPages(int usersPerPage)
        {
            return base.GetTotalPages(usersPerPage);
        }
        /// <summary>
        /// Use this method to get the search results
        /// Number of users per page is defined in Config.Search
        /// </summary>
        /// <param name="Page">Page number</param>
        /// <returns>Array of usernames</returns>
        public User[] GetPage(int Page)
        {
            return GetPage(Page, Config.Search.UsersPerPage);
        }

        /// <summary>
        /// Use this method to get the search results
        /// </summary>
        /// <param name="Page">Page number</param>
        /// <param name="usersPerPage">usersPerPage</param>
        /// <returns>Array of usernames</returns>
        public new User[] GetPage(int Page, int usersPerPage)
        {
            return base.GetPage(Page, usersPerPage);
        }

        public User[] Get()
        {
            return GetPage(1, Int32.MaxValue);
        }
    }
    
    [Serializable]
    public class FacebookSearchResults : BaseFacebookSearchResults 
    {
    }

    [Serializable]
    public class FacebookAndUsersMergeResults : BaseFacebookSearchResults
    {
        private string[] _usernames;
        public string[] Usernames
        {
            get { return _usernames; }
            set
            {
                _usernames = value;
                if (value != null && value.Length > 0)
                    Results = (Results == null) ? value : Results.Concat(value).ToArray(); 
            }
        }

        private string[] _facebookIds;
        public string[] FacebookIds
        {
            get { return _facebookIds; }
            set
            {
                _facebookIds = value;
                if (value != null && value.Length > 0)
                    Results = (Results == null) ? value : Results.Concat(value).ToArray(); 
            }
        }
    }

    [Serializable]
    public class FacebookFriendsSearch
    {
        public FacebookSearchResults GetResults()
        {
            UserSession userSession = PageBase.GetCurrentUserSession();

            ezFixUpFacebookApp facebook = FacebookHelper.GetSessionFacebookApp(userSession.FacebookID.HasValue);

            Facebook.JsonObject user = null;
            if (userSession.FacebookID.HasValue) 
                user = FacebookHelper.LoadFacebookUser(null);

            long id;

            if (user != null && user["id"] != null &&
                long.TryParse((String)user["id"], out id) && id > 0 )
            {
                JsonObject friends = facebook.Api("/me/friends") as JsonObject;
                var lResults = new List<string>();
                if (friends != null)
                {
                    JsonArray friendsArr = friends["data"] as JsonArray;
                    if (friendsArr != null)
                    {
                        long[] fbFriendsIDs = new long[friendsArr.Count];
                        for (int i = 0; i < friendsArr.Count; i++)
                        {
                            long.TryParse((string)((JsonObject)friendsArr[i])["id"], out fbFriendsIDs[i]);
                            lResults.Add(fbFriendsIDs[i].ToString());
                        }
                    }
                }
                if (lResults.Count > 0)
                {
                    var fbResults = new FacebookSearchResults { Usernames = lResults.ToArray() };
                    return fbResults;
                }
            }
            return null;
        }
    }
    [Serializable]
    public class FacebookMutualFriendsSearch
    {
        #region Properties

        private string viewed;
        private string viewer;
        private UserSession userSession;
        
        public string Viewer
        {
            get { return viewer; }
            set { viewer = value; }
        }

        public string Viewed
        {
            get { return viewed; }
            set { viewed = value; }
        }

        public UserSession ItsUserSession
        {
            get { return userSession; }
            set { userSession = value; }
        }

        private ezFixUpFacebookApp fbAppSession;

        public ezFixUpFacebookApp FacebookAppSession
        {
            get { return fbAppSession; }
            set { fbAppSession = value; }
        }

        public FacebookMutualFriendsSearch(string viewer, string viewed, UserSession userSession, ezFixUpFacebookApp facebookApp)
        {
            this.Viewed = viewed;
            this.Viewer = viewer;
            this.ItsUserSession = userSession;
            this.FacebookAppSession = facebookApp;
        }
        #endregion
        public List<SearchService.MutualFriendsClass> GetSavedResults()
        {
            if (Viewed.IsNullOrEmpty() || Viewer.IsNullOrEmpty())
                return null;

            string cacheKey = String.Format("User_FetchFacebookMutualFriends_{0}_{1}", Viewer, Viewed);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as List<SearchService.MutualFriendsClass>;
            }

            var lstMutualFriends = new List<SearchService.MutualFriendsClass>();

            //using (var conn = Config.DB.Open())
            {
                try
                {
                    XmlReader reader =
                        (XmlReader)SqlHelper.GetDB().ExecuteReader("FetchFacebookMutualFriends", Viewer, Viewed);
                    StringWriter writer = new StringWriter(CultureInfo.InvariantCulture);
                    XmlSerializer serializer = new XmlSerializer(new List<SearchService.MutualFriendsClass>().GetType());
                    lstMutualFriends = (List<SearchService.MutualFriendsClass>)serializer.Deserialize(reader);
                }
                catch (Exception ex)
                {
                }
            }

            if (HttpContext.Current != null)
            {
                HttpContext.Current.Cache.Insert(cacheKey, lstMutualFriends, null, Cache.NoAbsoluteExpiration,
                                                 TimeSpan.FromHours(1), CacheItemPriority.NotRemovable, null);
            }
            return lstMutualFriends;
        }

        public FacebookSearchResults GetResults()
        {
            if (Viewed.IsNullOrEmpty() || Viewer.IsNullOrEmpty())
                return null;

            User viewerUser = Classes.User.Load(Viewer);
            User viewedUser = Classes.User.Load(Viewed);
            long? fbViewerIdNullable = null;
            long? fbViewedIdNullable = null;
            long fbViewedId;
            if (viewerUser != null && viewedUser != null && viewedUser.LoginCount > 0)
            {
                
            }
            else if (viewerUser != null && Viewed.IsNotNullOrEmpty() && long.TryParse(Viewed, out fbViewedId))
            {
                Facebook.JsonObject user = null;
                //if (ItsUserSession.FacebookID.HasValue)
                //    user = FacebookHelper.LoadFacebookUser(null);
                //long id;

                //if (user != null && user["id"] != null &&
                //    long.TryParse((String) user["id"], out id) &&
                //    id > 0 && viewerUser .FacebookID == id)
                //{
                DateTime[] time = new DateTime[3];
                try
                {
                    //JsonObject fbUser = FacebookHelper.LoadFacebookUser(fbViewedId);
                    //object param = fbUser.Keys.Contains("username") ? fbUser["username"] : fbUser["id"];
                    time[0] = DateTime.Now;
                        //JsonObject mutualFriends = FacebookAppSession.Api("/me/mutualfriends",
                        //                                        new Dictionary<string, object>
                        //                                            {
                        //                                                {"user", param}
                        //                                            }) as JsonObject;
                        var mfDic = HttpContext.Current.GetFacebookMutualFriendsDic();
                        var allSessionMutualFriends = (List<MutualFriendItem>)mfDic[viewerUser.FacebookID.Value];

                        //var allSessionMutualFriends = HttpContext.Current.GetSession()["FacebookMutualFriends"] as List<MutualFriendItem>;

                        List<MutualFriendItem> friendMutualFriends =
                            allSessionMutualFriends
                                .Where(mf => mf.FriendID == fbViewedId /*long.Parse((string)fbUser["id"])*/)
                                .ToList();
                        var lResults = new List<string>();
                        if (friendMutualFriends != null)
                        {
                            lResults = friendMutualFriends.Select(f => f.MutualFriendID.ToString()).Distinct().ToList();
                            
                            //JsonArray mutualFriendsArr = mutualFriends["data"] as JsonArray;
                            //if (mutualFriendsArr != null)
                            {
                                long[] fbMutualFriendsIDs = new long[lResults.Count];
                                for (int i = 0; i < fbMutualFriendsIDs.Length; i++)
                                {
                                    //long.TryParse((string)((JsonObject)friendMutualFriends[i])["uid2"],
                                    fbMutualFriendsIDs[i] = long.Parse(lResults[i]);// friendMutualFriends[i].MutualFriendID;
                                    //lResults.Add(fbMutualFriendsIDs[i].ToString());
                                }
                            }
                        }
                        if (lResults.Count > 0)
                        {
                            var fbResults = new FacebookSearchResults {Usernames = lResults.ToArray()};
                            return fbResults;
                        }
                    }
                    catch(Exception ex)
                    {
                    }
                }
            //}
            return null;
        }
    }
    /*
    [Serializable]
    public class FacebookFriendsConnectionSearch
    {
        #region Properties

        public string Viewer { get; set; }

        public string Viewed { get; set; }

        #endregion

        public UserSearchResults GetResults()
        {
            List<string> usersChain;
            bool success = Classes.User.FindFriendsConnection(Viewer, Viewed, Config.Users.MaxFriendsHops,
                                                      out usersChain);
            if (!success) return null;
            var results = new UserSearchResults {Usernames = usersChain.ToArray()};
            return results;
        }
    }
     */
}

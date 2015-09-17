using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using System.Xml.Serialization;
using ezFixUp.Handlers;
using ezFixUp.Model;
using ezFixUp.Services;
using Facebook;


namespace ezFixUp.Classes
{

    [Serializable]
    public class ezFixUpFacebookApp : FacebookApp, ISerializable
    {
        public ezFixUpFacebookApp()
            : base()
        {
        }
        public ezFixUpFacebookApp(string accessToken)
            : base(accessToken)
        {
        }
        public ezFixUpFacebookApp(IFacebookSettings settings)
            : base(settings)
        {
        }
        public ezFixUpFacebookApp(SerializationInfo info, StreamingContext context)
        {
            this.Session = new FacebookSession();
            this.Session.AccessToken = (string)info.GetValue("AccessToken", typeof(string));
            this.Session.Secret = (string)info.GetValue("AppSecret", typeof(string));
            //this.Session.SessionKey = (string)info.GetValue("SessionKey", typeof(string));
            SessionLoaded = (bool)info.GetValue("SessionLoaded", typeof(bool));
            //SignedRequest = (bool)info.GetValue("SessionLoaded", typeof(bool));
            this.Session.Secret = (string)info.GetValue("AppSecret", typeof(string));
            this.Session.UserId = (long)info.GetValue("UserId", typeof(long));
            this.Session.Expires = DateTime.Now.AddDays(1);// (DateTime)info.GetDateTime("SessionExpires");
            //this.Session.Signature = (string)info.GetValue("Signature", typeof(string));
        }


        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("AccessToken", AccessToken);
            info.AddValue("AppId", AppId);
            info.AddValue("AppSecret", AppSecret);
            info.AddValue("SessionKey", Session.SessionKey);
            info.AddValue("SessionLoaded", SessionLoaded);
            //info.AddValue("SessionExpires", Session.Expires);
            //info.AddValue("SignedRequest", SignedRequest);
            info.AddValue("UserId", UserId);
            //info.AddValue("Signature", Session.Signature);
        }
        #endregion
    }

    public class FacebookHelper
    {
        SortedDictionary<string, string> FacebookParams
        {
            get;
            set;
        }

        public FacebookHelper()
        {
            FacebookParams = GetValidFacebookParams();
        }
        public class AddNewFacebookFriendClass
        {
            public void AddNewFacebookFriend(object fbFriendData)
            {
                //var fbUser = LoadFacebookUser((long)friendId);
                FacebookFriendData fbUser = (FacebookFriendData)fbFriendData;
                if (fbUser == null || fbUser.IsMinor) return;

                if (fbUser.Birthdate.Year == DateTime.Now.Year)
                    fbUser.Birthdate = fbUser.Birthdate.AddYears(-30);
                else if ((fbUser.Birthdate > DateTime.Now.AddYears(-18)) ||
                         (fbUser.Birthdate < DateTime.Now.AddYears(-80)))
                    fbUser.Birthdate = new DateTime(DateTime.Now.Year - 30, 1, 1);

                var friendId = fbUser.UserId;
                var pass = (-DateTime.Now.Ticks).ToString();
                if (pass.Length > 20) pass = pass.Substring(0, 20);
                try
                {
                    string[] tokens = fbUser.Location.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    string cityName = tokens.Length > 0 ? tokens[0] : "",
                        regionName = tokens.Length > 1 ? tokens[1] : "",
                        countryName = tokens.Length > 2 ? tokens[2] : "";

                    //var cityName = Config.Users.MapFacebookCityToezFixUpCity( HttpContext.Current,
                    //                                                            fbUser.Location,
                    //                                                            out regionName,
                    //                                                            out countryName);
                    //if (cityName.IsNotNullOrEmpty())
                    //    cityName = Config.Users.MapFacebookCityToezFixUpCity( HttpContext.Current,
                    //                                                            fbUser.HomeTown,
                    //                                                            out regionName,
                    //                                                            out countryName);
                    var newUser = new User(friendId.ToString())
                                      {
                                          Active = true,
                                          FacebookID = friendId,
                                          Password = pass,
                                          //InterestedIn =  Classes.User.eGender.Matchmaker,
                                          Birthdate = fbUser.Birthdate,
                                          Email = String.Format("{0}@ezFixUp.com", fbUser.Username),
                                          Name = fbUser.Name,
                                          ProfileVisible = true,
                                          FaceControlApproved = false,
                                          StatusText = fbUser.RelationshipStatus ?? "Single",
                                          Country = countryName,
                                          State = regionName,
                                          City = cityName
                                      };

                    Classes.User.eGender? gender = null;
                    Classes.User.eGender? interestedIn = null;
                    switch (fbUser.GenderStr.ToLower(CultureInfo.InvariantCulture))
                    {
                        case "male":
                            gender = Classes.User.eGender.Male;
                            interestedIn = Classes.User.eGender.Female;
                            break;
                        case "female":
                            gender = Classes.User.eGender.Female;
                            interestedIn = Classes.User.eGender.Male;
                            break;
                        default:
                            gender = Classes.User.eGender.Matchmaker;
                            interestedIn = Classes.User.eGender.Matchmaker;
                            break;
                    }
                    newUser.Gender = gender.Value;

                    switch (fbUser.InterestedInStr.ToLower(CultureInfo.InvariantCulture))
                    {
                        case "male":
                            interestedIn = Classes.User.eGender.Male;
                            break;
                        case "female":
                            interestedIn = Classes.User.eGender.Female;
                            break;
                        default:
                            break;
                    }
                    newUser.InterestedIn = interestedIn.Value;

                    try
                    {
                        //todo: transaction
                        newUser.Create("127.0.0.1");
                        if (fbUser.IsVerified) newUser.SetAsVerified(newUser.Username);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                finally
                {
                }
            }
        }
        public class FetchAndSaveSaveFacebookFriendImageClass
        {
            public void FetchAndSaveFacebookImageByFacebookId(object facebookId)
            {
                string imgUrl = ImageHandler.CreateFacebookImageUrl((long)facebookId,
                                                                        ImageHandler.eFacebookImageType.Large);
                FetchAndSaveFacebookImageBySource(imgUrl, (long)facebookId);
            }

            public void FetchAndSaveFacebookImageBySource(string imgUrl, long facebookId)
            {
                //foreach (var facebookId in (long[])facebookIds)
                try
                {
                    Image img = DownloadImage(imgUrl);
                    User user = Classes.User.Load(facebookId.ToString());
                    var newPhoto = new Photo
                                       {
                                           Approved = true,
                                           Image = img,
                                           Primary = true,
                                           ExplicitPhoto = false,
                                           PrivatePhoto = false,
                                           ManualApproval = false,
                                           ApprovedDate = DateTime.Now,
                                           Name = "",
                                           Description = imgUrl,
                                           User = user
                                       };
                    string tempFileName;

                    if (!Misc.GetTempFileName(out tempFileName))
                        tempFileName = Path.GetTempFileName();
                    newPhoto.Image.Save(tempFileName);
                    newPhoto.Save(true);
                    Photo.SetPrimary(user.Username, newPhoto);
                    user.FaceControlApproved = true;
                    user.Update();
                    newPhoto.ApprovePhoto("facebook");
                }
                catch (Exception ex)
                {
                }
            }
        }
        public static string SerializeToXml<T>(T value)
        {
            StringWriter writer = new StringWriter(CultureInfo.InvariantCulture);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(writer, value);
            return writer.ToString();
        }
        public static void AddFriends(User user, int maxFriendsToAdd, HttpContext context, ezFixUpFacebookApp facebook)
        {
            if (facebook == null)
                facebook = GetSessionFacebookApp();

            if (facebook == null) return;

            IList<FacebookFriendData> allFriendsData = null;
            List<long> lstFriendsIds = null;
            IList<FacebookFriendData> friendsData = null;
            var oldUsernames = new List<string>();
            IList<Thread> thredsList = new List<Thread>();
            var timer = new TimeMeasure();
            int maxThreadsInPool = Config.DebugSettings.MaxParallelThreads;
            long fbId = facebook.UserId;

            try
            {
                timer.AddTimestamp("FacebookHelper.AddFriends before GetFriendsDataList()");
                if (fbId == 0 && user != null && user.FacebookID.HasValue)
                    fbId = user.FacebookID.Value;

                allFriendsData = GetFriendsDataList(context, facebook, fbId);
                if (allFriendsData == null || allFriendsData.Count == 0) return;

                var fbFriendsIds = allFriendsData.Select(f => f.UserId);

                timer.AddTimestamp("FacebookHelper.AddFriends after GetFriendsDataList()");

                var mfDic = context.GetFacebookMutualFriendsDic();
                var allMutualFriends = (List<MutualFriendItem>)mfDic[fbId];

                var friendsIDsWithMutualFriends = maxFriendsToAdd >= 0
                    ? allMutualFriends.Select(f => f.FriendID).Distinct().ToList()
                    : allMutualFriends.Select(f => f.FriendID).Take(60).ToList();

                bool addOnlyWithMutualFriends = maxFriendsToAdd <= 0;

                lstFriendsIds = (friendsIDsWithMutualFriends.Count >= 20 &&
                                     addOnlyWithMutualFriends)
                                        ? friendsIDsWithMutualFriends
                                        : fbFriendsIds.ToList();

                friendsData = allFriendsData.Where(f => lstFriendsIds.Contains(f.UserId)).ToList();

                for (int i = 0; i < friendsData.Count; i++)
                {
                    var fbFriendData = friendsData[i];
                    fbFriendData.MutualFriendsCount =
                        allMutualFriends.Count(f => f.FriendID == fbFriendData.UserId);
                }

                friendsData = friendsData.OrderByDescending(f => f.MutualFriendsCount).ToList();

                if (context.Session != null)
                {
                    if (friendsData.Count > 60)
                    {
                        if (Global.GetSessionState()["SessionRefreshState"] == null)
                        {
                            Global.GetSessionState()["SessionRefreshState"] =
                                RefreshSessionState.SessionRefreshStateEnum.AddFriendsWithMutualFriends;
                            friendsData = friendsData.Take(60).ToList();
                        }
                        else
                        {
                            friendsData = friendsData.Skip(60).ToList();
                        }
                    }
                    else
                    {
                        Global.GetSessionState()["SessionRefreshState"] =
                            RefreshSessionState.SessionRefreshStateEnum.CacheNormalImagesOfFriendsWithMutualFriends;
                    }
                }
                maxThreadsInPool = (int)Math.Min(Math.Sqrt((double)friendsData.Count), Config.DebugSettings.MaxParallelThreads);

                var ftu1 = new FetchTakenUsernames { Usernames = friendsData.Select(f => f.Username).ToList() };
                var takenUsernameThreadsList =
                    new ezFixUpParallel.ParameterizedThreadsList<string>(ftu1.Usernames, ftu1.TakenUsernames,
                                                                           ftu1.AddUsernameIfTaken, context) { RunAsBackgroundThreads = true };
                takenUsernameThreadsList.ExecuteParallelWork(maxThreadsInPool);
                oldUsernames = ftu1.TakenUsernames;

                timer.AddTimestamp("FacebookHelper.AddFriends after AddUsernameIfTaken");
            }
            catch (Exception ex)
            {
            }
            List<FacebookFriendData> fbFriendsDataList = null;
            try
            {
                if (lstFriendsIds != null && lstFriendsIds.Any())
                {
                    fbFriendsDataList =
                        friendsData.Where(f => !oldUsernames.Contains(f.UserId.ToString())).ToList();
                    var parameterizedThreadsList =
                        new ezFixUpParallel.ParameterizedThreadsList<FacebookFriendData>(fbFriendsDataList,
                                                                         (new AddNewFacebookFriendClass()).AddNewFacebookFriend,
                                                                         context);
                    parameterizedThreadsList.RunAsBackgroundThreads = true;
                    parameterizedThreadsList.ExecuteParallelWork(maxThreadsInPool);
                }
                timer.AddTimestamp("FacebookHelper.AddFriends after AddNewFacebookFriend");

            }
            catch (Exception ex)
            {
            }
            finally
            {
                timer.AddTimestamp("FacebookHelper.AddFriends start finally");
                List<Friend> dbFriends = null;
                List<string> oldUsernamesNotFriends = null;
                using (var db = new ezFixUpDataContext())
                {
                    dbFriends = CompiledQueries.FetchActiveFriendsForUsername(db, user.Username).ToList();
                }
                if (dbFriends != null && dbFriends.Count > 0)
                {
                    oldUsernamesNotFriends = oldUsernames.Where(u => !dbFriends.Any(f => f.f_username == u)).ToList();
                }
                else
                {
                    oldUsernamesNotFriends = oldUsernames;
                }
                List<string> lstAddUsernamesToFriends = fbFriendsDataList.Select(f => f.UserId.ToString()).ToList();
                if (oldUsernamesNotFriends != null && oldUsernamesNotFriends.Count > 0)
                {
                    var allFbFriendsIds = friendsData.Select(f => f.UserId.ToString()).ToList();
                    var toUnionFriendsIds = allFbFriendsIds.Intersect(oldUsernamesNotFriends);
                    lstAddUsernamesToFriends = lstAddUsernamesToFriends.Union(toUnionFriendsIds).ToList();
                }

                //var ftu = new FetchTakenUsernames { Usernames = lstFriendsIds.Select(f => f.ToString()).ToList() };
                //var parameterizedThreadsList =
                //    new ezFixUpParallel.ParameterizedThreadsList<string>(ftu.Usernames, ftu.TakenUsernames, ftu.AddUsernameIfTaken, context);
                //parameterizedThreadsList.RunAsBackgroundThreads = true;
                //parameterizedThreadsList.ExecuteParallelWork(maxThreadsInPool);

                //time[7] = DateTime.Now;
                //time[7] = DateTime.Now;

                //var friendsUsernames = friendsData.Select(f => f.UserId.ToString());
                //var notTakenUsernames = ftu.TakenUsernames.Where(u => !friendsUsernames.Contains(u)).ToString();
                //List<FacebookFriendDataClass> inTakenFriendsList =
                //    ftu.TakenUsernames.Select(f => new FacebookFriendDataClass
                //                                       {
                //                                           ViewerUsernme = user.Username,
                //                                           FriendUsernme = f.ToString(),
                //                                           FacebookAppSession = facebook
                //                                       }).ToList();
                List<FacebookFriendDataClass> addNewFriendsList =
                    lstAddUsernamesToFriends.Select(f => new FacebookFriendDataClass
                                                       {
                                                           ViewerUsernme = user.Username,
                                                           FriendUsernme = f.ToString(),
                                                           FacebookAppSession = facebook
                                                       }).ToList();
                timer.AddTimestamp("FacebookHelper.AddFriends after FetchActiveFriendsForUsername");

                if (lstAddUsernamesToFriends.Any())
                {
                    var addFriendsDataThreadsList =
                        new ezFixUpParallel.ParameterizedThreadsList<FacebookFriendDataClass>(
                                    addNewFriendsList, null, context);

                    addFriendsDataThreadsList.ExecuteParallelWork(1);

                    timer.AddTimestamp("FacebookHelper.AddFriends after FacebookFriendDataClass.ExecuteParallelWork");
                }
                var dic = (Dictionary<string, ezFixUpBackgroundWorker>)Global.AppBackgroundWorkersDic;
                if (context.Session != null && CurrentSessionUserID > 0)
                {
                    if (dic != null)
                    {
                        PageBase.GetCurrentUserSession().SetFacebookApp(facebook);
                        //var bgWorker1 =
                        //    (AddNewFacebookFriendsBackgroundWorker)
                        //       dic["ezFixUp.Classes.AddNewFacebookFriendsBackgroundWorker"];

                        //if (bgWorker1 != null)
                        //{
                        //    facebook.Session.UserId = fbId;
                        //    lock (bgWorker1.FacebookAppList)
                        //    {
                        //        if (!bgWorker1.FacebookAppList.Contains(facebook))
                        //            bgWorker1.FacebookAppList.Add(facebook);
                        //    }
                        //}
                    }
                }

                //var bgWorker2 = (LoadAndSaveFacebookImageBackgroundWorker)
                //                        dic["ezFixUp.Classes.LoadAndSaveFacebookImageBackgroundWorker"];
                //if (bgWorker2 != null && !bgWorker2.FacebookAppList.Contains(facebook))
                //    bgWorker2.FacebookAppList.Add(facebook);
            }
        }
        public class FetchTakenUsernames
        {
            public List<string> Usernames = new List<string>();
            public List<string> TakenUsernames = new List<string>();

            public void AddUsernameIfTaken(object username)
            {
                var strUsername = username.ToString();
                if (User.IsUsernameTaken(strUsername))
                    lock (this.TakenUsernames)
                    {
                        if (User.Load(strUsername) != null)
                            this.TakenUsernames.Add(strUsername);
                        else
                        {
                            User user = LoadUserFromDbByFacebookId(long.Parse(strUsername));
                            if (user != null)
                                this.TakenUsernames.Add(user.Username);
                        }
                    }
            }
        }
        public class FacebookFriendDataClass : ezFixUpParallel.WorkThreadClass
        {
            public string ViewerUsernme;
            public string FriendUsernme;
            public List<SearchService.MutualFriendsClass> MutualFriendsList;
            public ezFixUpFacebookApp FacebookAppSession;

            public override void DoWorkThread(object parameter)
            {
                if (parameter == null) return;
                var frData = parameter as FacebookFriendDataClass;

                //TODO: should be a transaction section for each friend which add as new user ~ till mutual friends
                try
                {
                    if (frData.ViewerUsernme.IsNullOrEmpty()) return;
                    if (frData.FriendUsernme.IsNullOrEmpty()) return;

                    using (User user = Classes.User.Load(frData.ViewerUsernme))
                    {
                        if (!user.IsUserInFriendList(frData.FriendUsernme))
                            user.AddToFriends(frData.FriendUsernme, true);
                    }
                    using (User friend = Classes.User.Load(frData.FriendUsernme))
                    {
                        if (!friend.IsUserInFriendList(frData.ViewerUsernme))
                            friend.AddToFriends(frData.ViewerUsernme, true);
                    }
                    DateTime[] time = new DateTime[5];
                    time[0] = DateTime.Now;
                    //var searchFacebookMF = new FacebookMutualFriendsSearch(user.Username, friend.Username, PageBase.GetCurrentUserSession(), FacebookAppSession);
                    //var searchFacebookResults = searchFacebookMF.GetResults();
                    //User[] fbMF = (searchFacebookResults != null) ? searchFacebookResults.Get() : new User[] { };
                    //frData.MutualFriendsList = fbMF.Select(u =>
                    //                    new SearchService.MutualFriendsClass
                    //                    {
                    //                        Username = u.Username,
                    //                        PhotoId = 0,
                    //                        PhotoUrl = String.Format("https://graph.facebook.com/{0}/picture?type=square", user.FacebookID),
                    //                        AlreadyRequested = false
                    //                    }).ToList();

                    ////using (var conn = Config.DB.Open())
                    //{
                    //    SqlHelper.GetDB().ExecuteNonQuery( "UpdateFacebookMutualFriends", user.Username, friend.Username, SerializeToXml(frData.MutualFriendsList));
                    //}
                    //time[2] = DateTime.Now;
                    //time[2] = DateTime.Now;
                }
                catch (NotFoundException ex)
                {
                }
                catch (Exception ex)
                {
                }
            }
        }
        /// <summary>
        /// Function to download Image from website
        /// </summary>
        /// <param name="url">URL address to download image</param>
        /// <returns>Image</returns>
        public static Image DownloadImage(string url)
        {
            Image tmpImage = null;

            try
            {
                // Open a connection
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

                httpWebRequest.AllowWriteStreamBuffering = true;

                // You can also specify additional header values like the user agent or the referer: (Optional)
                //_HttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";
                //_HttpWebRequest.Referer = "https://www.google.com/";

                // set timeout for 20 seconds (Optional)
                httpWebRequest.Timeout = 80000;

                // Request response:
                WebResponse webResponse = httpWebRequest.GetResponse();

                // Open data stream:
                Stream webStream = webResponse.GetResponseStream();

                // convert webstream to image
                if (webStream != null) tmpImage = Image.FromStream(webStream);

                // Cleanup
                webResponse.Close();
                webResponse.Close();
            }
            catch (Exception exception)
            {
                // Error
                Console.WriteLine("Exception caught in process: {0}", exception.ToString());
                return null;
            }

            return tmpImage;
        }
        public static byte[] GetBytesFromUrl(string url)
        {
            byte[] b = null;
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
            WebResponse myResp = myReq.GetResponse();

            Stream stream = myResp.GetResponseStream();

            if (stream != null)
                using (BinaryReader br = new BinaryReader(stream))
                {
                    //i = (int)(stream.Length);
                    b = br.ReadBytes(500000);
                    br.Close();
                }
            myResp.Close();
            return b;
        }
        //public static bool IsAuthenticated()
        //{

        //}
        public static long CurrentSessionUserID
        {
            get
            {
                long retVal = 0;
                if (Global.GetSessionState().ContainsKey("CurrentFacebookID"))
                    retVal = (long)Global.GetSessionState()["CurrentFacebookID"];
                return retVal;
            }
            set { Global.GetSessionState()["CurrentFacebookID"] = value; }
        }
        public static bool IsCurrentSessionConnected
        {
            get
            {
                bool retVal = false;
                if (Global.GetSessionState().ContainsKey("IsFacebookConnected"))
                    retVal = (bool)Global.GetSessionState()["IsFacebookConnected"];
                return retVal; 
            }
            set { Global.GetSessionState()["IsFacebookConnected"] = value; }
        }
        public static int SessionConnectTriesElapsed
        {
            get
            {
                string retVal = "0";
                if (Global.GetSessionState().ContainsKey("FacebookConnectTriesElapsed"))
                    retVal = (string)Global.GetSessionState()["FacebookConnectTriesElapsed"];
                return Convert.ToInt32(retVal); 
            }
            set { Global.GetSessionState()["FacebookConnectTriesElapsed"] = value.ToString(); }
        }
        public static string CurrentSessionAccessToken
        {
            get
            {
                string retVal = null;
                if (Global.GetSessionState().ContainsKey("facebookAccessToken"))
                    retVal = (string)Global.GetSessionState()["facebookAccessToken"];
                return retVal; 
            }
            set { Global.GetSessionState()["facebookAccessToken"] = value; }
        }
        public static void SessionClear(string backUrl)
        {
            CurrentSessionAccessToken = null;
            SessionConnectTriesElapsed = 2;
            IsCurrentSessionConnected = false;
            CurrentSessionUserID = 0;

            //if (GetSessionFacebookApp() != null)
            //{
            //    var fbHelper = new FacebookHelper();
            //    fbHelper.RemoveFacebookCookies();
            //    fbHelper.Logout(backUrl);
            //}
        }
        private bool VerifySignature(SortedDictionary<string, string> facebookParams, string signature)
        {
            return GenerateSignature(facebookParams) == signature;
        }

        private string GenerateSignature(SortedDictionary<string, string> facebookParams)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var keyValuePair in facebookParams)
            {
                sb.Append(String.Format("{0}={1}", keyValuePair.Key, keyValuePair.Value));
            }

            sb.Append(SecretKey);

            return GetMd5Hash(sb.ToString());
        }

        public string Signature
        {
            get
            {
                if (HttpContext.Current == null)
                    throw new ApplicationException("HttpContext cannot be null.");

                if (HttpContext.Current.Request.Cookies[ApiKey] == null)
                    throw new ApplicationException("Could not find facebook cookie named " + ApiKey);

                var httpCookie = HttpContext.Current.Request.Cookies[ApiKey];
                return httpCookie != null ? httpCookie.Value : null;
            }
        }

        private SortedDictionary<string, string> GetValidFacebookParams()
        {
            if (HttpContext.Current == null)
                throw new ApplicationException("HttpContext cannot be null.");

            string prefix = ApiKey + "_";

            var facebookParams = new SortedDictionary<string, string>();

            foreach (string cookieKey in HttpContext.Current.Request.Cookies.AllKeys)
            {
                if (cookieKey.StartsWith(prefix))
                {
                    var httpCookie = HttpContext.Current.Request.Cookies[cookieKey];
                    if (httpCookie != null)
                        facebookParams.Add(cookieKey.Substring(prefix.Length),
                                           httpCookie.Value);
                }
            }

            if (facebookParams.Count > 0 && !VerifySignature(facebookParams, Signature))
                throw new SecurityException("The provided signature does not match generated one!");

            return facebookParams;
        }

        public static string AppID
        {
            get { return Properties.Settings.Default.Facebook_APP_ID; }
        }

        public static string ApiKey
        {
            get { return Properties.Settings.Default.Facebook_API_Key; }
        }

        public static string SecretKey
        {
            get { return Properties.Settings.Default.Facebook_Secret_Key; }
        }

        public string SessionKey
        {
            get
            {
                if (FacebookParams.ContainsKey("session_key"))
                    return FacebookParams["session_key"];

                return null;
            }
        }

        public long UserID
        {
            get
            {
                if (FacebookParams.ContainsKey("user"))
                    return Int64.Parse(FacebookParams["user"]);

                return 0L;
            }
        }

        // Hash an input string and return the hash as
        // a 32 character hexadecimal string.
        static string GetMd5Hash(string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5 md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        private bool RemoveFacebookCookies()
        {
            if (HttpContext.Current == null)
                throw new ApplicationException("HttpContext cannot be null.");

            var response = HttpContext.Current.Response;

            string prefix = ApiKey + "_";

            bool facebookCookiesExist = false;

            foreach (string cookieKey in HttpContext.Current.Request.Cookies.AllKeys)
            {
                if (cookieKey.StartsWith(prefix) || cookieKey == ApiKey)
                {
                    facebookCookiesExist = true;
                    HttpCookie cookie = new HttpCookie(cookieKey);
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    response.Cookies.Add(cookie);
                }
            }

            return facebookCookiesExist;
        }

        public void Logout(string redirectURL)
        {
            if (HttpContext.Current == null)
                throw new ApplicationException("HttpContext cannot be null.");

            if (RemoveFacebookCookies())
            {
                var response = HttpContext.Current.Response;
                response.Redirect(
                    String.Format("https://www.facebook.com/logout.php?app_key={0}&session_key={1}&next={2}",
                                  ApiKey, SessionKey, HttpContext.Current.Server.UrlDecode(redirectURL)));
            }
        }

        //private static string GetFacebookCookie(string name)
        //{
        //    if (HttpContext.Current == null)
        //        throw new ApplicationException("HttpContext cannot be null.");

        //    string fullName = ApiKey + "_" + name;
        //    if (HttpContext.Current.Request.Cookies[fullName] == null)
        //        throw new ApplicationException("Could not find facebook cookie named " + fullName);

        //    return HttpContext.Current.Request.Cookies[fullName].Value;
        //}

        public static ezFixUpFacebookApp GetSessionFacebookApp(bool redirectToLogin)
        {
            ezFixUpFacebookApp facebook = GetSessionFacebookApp();

            if (facebook == null)
            {
                if (redirectToLogin && FacebookHelper.SessionConnectTriesElapsed > 0)
                    HttpContext.Current.Response.Redirect("~/LoginThroughFacebook.aspx");
                else
                    return null;
            }
            return facebook;
        }

        public static ezFixUpFacebookApp SessionFacebookApp
        {
            get { return GetSessionFacebookApp(); }
            set { if (HttpContext.Current != null) Global.GetSessionState()["facebook"] = value; }
        }

        public static ezFixUpFacebookApp GetSessionFacebookApp()
        {
            ezFixUpFacebookApp facebook = null;
            if (HttpContext.Current != null && HttpContext.Current.Session != null && Global.GetSessionState()["facebook"] != null)
            {
                facebook = (ezFixUpFacebookApp)Global.GetSessionState()["facebook"];
            }
            else if (CurrentSessionAccessToken.IsNotNullOrEmpty())
            {
                facebook = new ezFixUpFacebookApp(CurrentSessionAccessToken);
            }
            else if (HttpContext.Current != null && HttpContext.Current.Request.QueryString["access_token"] != null && HttpContext.Current.Request.QueryString["access_token"].Length > 0)
            {
                facebook = new ezFixUpFacebookApp(HttpContext.Current.Request.QueryString["access_token"]);
            }
            else
            {
                facebook = new ezFixUpFacebookApp();
            }
            if (facebook.AppSecret.IsNotNullOrEmpty())
            {
                facebook.AppSecret = SecretKey;
            }

            facebook = (facebook.SessionLoaded) ? facebook : null;
            SessionFacebookApp = facebook;
            return facebook;
        }
        public static JsonObject LoadFacebookUser(long? id)
        {
            ezFixUpFacebookApp facebook = GetSessionFacebookApp();
            Facebook.JsonObject user = null;
            try
            {
                if (facebook.AppSecret == null)
                {
                    facebook.AppSecret = Properties.Settings.Default.Facebook_Secret_Key;
                }
                if (id == null)
                    user = facebook.Api("/me") as JsonObject;
                else
                    user = facebook.Api("/" + id.Value) as JsonObject;
            }
            catch
            {
            }
            return user;
        }
        public static User LoadUserFromDbByFacebookId(long? id)
        {
            User user = null;
            string[] usernames = Classes.User.FetchUsernamesByFacebookID(new[] { (long)(id.HasValue ? id : 0) });
            if (usernames != null && usernames.Length > 0)
                user = Classes.User.Load(usernames[0]);
            return user;
        }

        public static User LoadUser(long? id)
        {
            //FacebookApp facebook = GetSessionFacebookApp();
            Facebook.JsonObject fbUser = LoadFacebookUser(id);
            User user = new User();
            string[] usernames = Classes.User.FetchUsernamesByFacebookID(new[] { (long)(id.HasValue ? id : 0) });
            if (usernames != null && usernames.Length > 0)
                user = Classes.User.Load(usernames[0]);
            try
            {
                if (fbUser != null)
                {
                    user.Name = fbUser.ContainsKey("name") ? (string)fbUser["name"] : "";
                    user.Username =
                        fbUser.ContainsKey("username")
                            ? (string)fbUser["username"]
                            : (string)fbUser["id"];
                    user.FacebookID = long.Parse((string)fbUser["id"]);
                    if (fbUser.ContainsKey("gender"))
                        if ((string)fbUser["gender"] == "male")
                            user.Gender = Classes.User.eGender.Male;
                        else if ((string)fbUser["gender"] == "female")
                            user.Gender = Classes.User.eGender.Female;

                    if (fbUser.ContainsKey("interested_in"))
                    {
                        string interestedIn = "";
                        if (fbUser["interested_in"] is JsonArray)
                            interestedIn = (string)((JsonArray)fbUser["interested_in"])[0];
                        else interestedIn = (string)(fbUser["interested_in"]);

                        if (interestedIn == "male")
                            user.InterestedIn = Classes.User.eGender.Male;
                        else if (interestedIn == "female")
                            user.InterestedIn = Classes.User.eGender.Female;
                    }

                    string regionName, countryName;
                    if (fbUser.ContainsKey("location"))
                    {
                        JsonObject location = (JsonObject)fbUser["location"];
                        if (location.ContainsKey("name"))
                        {
                            var locationName = (string)location["name"];
                            user.City = Config.Users.MapFacebookCityToezFixUpCity(HttpContext.Current,
                                                                                locationName, out regionName, out countryName);
                            user.State = regionName;
                            user.Country = countryName;
                        }
                    }
                    if (user.Country.IsNullOrEmpty() && fbUser.ContainsKey("hometown"))
                    {
                        JsonObject hometown = (JsonObject)fbUser["hometown"];
                        if (hometown.ContainsKey("name"))
                        {
                            user.City = Config.Users.MapFacebookCityToezFixUpCity(HttpContext.Current,
                                                                                (string)hometown["name"], out regionName, out countryName);
                            user.State = regionName;
                            user.Country = countryName;
                        }
                    }
                    DateTime parsedBirthday = new DateTime();
                    if (fbUser.ContainsKey("birthday") &&
                        DateTime.TryParseExact((string)fbUser["birthday"], "MM/dd/yyyy", null,
                                               DateTimeStyles.None, out parsedBirthday))
                        user.Birthdate = parsedBirthday;
                }
                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static JsonArray GetFqlResult(string qry)
        {
            var fb = GetSessionFacebookApp();

            JsonArray result = (JsonArray)fb.Query(qry);
            return result;
        }
        public static JsonArray GetFqlResult(ezFixUpFacebookApp facebook, string qry)
        {
            try
            {
                JsonArray result = (JsonArray)facebook.Query(qry);
                return result;
            }
            catch (FacebookOAuthException ex)
            {
                facebook.Session.AccessToken = "AAAAAJWZAHraYBAAf5KYN4HWhcedgiuUsJcdmUkNUXD9T8VhQfH2ipwFtMPwSUkjnN8zZA07DvsMe4QVFLgNiOiljbtY3zWvvfFPZCfawgZDZD";
                JsonArray result = (JsonArray)facebook.Query(qry);
                return result;

            }
            catch (Exception ex)
            {
                ExceptionLogger.Log("public static JsonArray GetFqlResult(ezFixUpFacebookApp facebook, string qry)", ex);
                return null;
            }
        }
        public static void GetAsyncFqlResult(ezFixUpFacebookApp facebook,
            string qry, FacebookAsyncCallback callback, object objectState)
        {
            var args = new Dictionary<string, object>();
            args["q"] = qry;
            facebook.GetAsync("fql", args, callback, objectState);
        }
        public static IList<FacebookFriendData> GetFriendsDataList(HttpContext context)
        {
            ezFixUpFacebookApp fb = GetSessionFacebookApp();
            return GetFriendsDataList(context, fb);
        }
        public static IList<FacebookFriendData> GetFriendsDataList(HttpContext context, ezFixUpFacebookApp fb)
        {
            return GetFriendsDataList(context, fb, CurrentSessionUserID);
        }
        class FriendsData
        {
            public List<JsonObject> FriendsDataJsonList;
            public List<JsonObject> MutualFriendsCount;
        }
        private static FriendsData GetFriendsDataAsyncQueries()
        {
            var fb = GetSessionFacebookApp();
            if (fb == null || CurrentSessionUserID <= 0)
                return null;

            return GetFriendsDataAsyncQueries(fb, CurrentSessionUserID);
        }

        private static FriendsData GetFriendsDataAsyncQueries(ezFixUpFacebookApp fb, long id)
        {
            var result = new FriendsData();

            if (fb == null)
                fb = GetSessionFacebookApp();

            if (fb == null || id <= 0)
                return null;

            FriendsDataCallbackClass friendsAsyncCallbackWorker = new FriendsDataCallbackClass(fb);

            var limit1 = 500;
            for (int i = 0; i < 5000; i = i + limit1)
            {
                string query1 = string.Format("select uid, name, is_app_user, sex, birthday_date, meeting_sex, relationship_status, "
                                         + "verified, is_minor, is_app_user, pic, current_location, hometown_location "
                                         + "from user where uid in "
                                         + "(select uid2 from friend where uid1 = {0}  limit {1} offset {2} ) ",
                                         id, limit1, i);
                try
                {
                    lock (friendsAsyncCallbackWorker.ItsSyncObject)
                    {
                        friendsAsyncCallbackWorker.ItsQueryCounter++;
                    }
                    FacebookHelper.GetAsyncFqlResult(fb, query1, friendsAsyncCallbackWorker.Callback, null);
                }
                catch (Exception ex)
                {
                    //return null;
                }
            }

            MutualFriendsCountCallbackClass mfAsyncCallbackWorker = new MutualFriendsCountCallbackClass(fb);
            var limit = 1000;
            for (int i = 0; i < 5000; i = i + limit)
            {
                string query2 = string.Format("select uid, mutual_friend_count "
                                              + "from user where uid in "
                                              + "(select uid2 from friend where uid1 = {0} limit {1} offset {2} ) ",
                                              id, limit, i);
                try
                {
                    lock (mfAsyncCallbackWorker.ItsSyncObject)
                    {
                        mfAsyncCallbackWorker.ItsQueryCounter++;
                    }
                    FacebookHelper.GetAsyncFqlResult(fb, query2, mfAsyncCallbackWorker.Callback, null);
                }
                catch (Exception ex)
                {
                    //return null;
                }
            }
            while (mfAsyncCallbackWorker.ItsQueryCounter > 0) { }
            result.MutualFriendsCount = mfAsyncCallbackWorker.Result;

            while (friendsAsyncCallbackWorker.ItsQueryCounter > 0) { }
            result.FriendsDataJsonList = friendsAsyncCallbackWorker.Result;

            return result;
        }

        public static IList<FacebookFriendData> GetFriendsDataList(HttpContext context, ezFixUpFacebookApp fb, long fbUserId)
        {
            var dic = context.GetFacebookFriendsDic();
            if (dic == null) return null;

            var sessionFriendsData = (dic.ContainsKey(fbUserId)) ? dic[fbUserId] : null;

            if (sessionFriendsData != null)
                return sessionFriendsData as List<FacebookFriendData>;

            User user = FacebookHelper.LoadUserFromDbByFacebookId(fbUserId);

            TimeMeasure timestamps = new TimeMeasure();

            var friendsDataContainer = GetFriendsDataAsyncQueries(fb, fbUserId);
            var fbFriendsDataList = new List<FacebookFriendData>();
            var arr = friendsDataContainer.FriendsDataJsonList.ToArray();
            var mfCountArr = friendsDataContainer.MutualFriendsCount.ToArray();

            timestamps.AddTimestamp("FacebookHelper.GetFriendsDataList after GetFriendsDataAsyncQueries()");

            Dictionary<JsonObject, long> friend2mfCountDic = new Dictionary<JsonObject, long>();

            var mfCountObjectList = mfCountArr.Select(x => new { ID = x["uid"].ToString(), Count = (long)x["mutual_friend_count"] }).ToList();

            //var tmpDic = new Dictionary<string,long>();            
            //mfCountObjectList.ForEach(x => tmpDic.Add(x.ID, x.Count));


            var tmpList = (from m in mfCountObjectList
                           join f in arr
                           on m.ID equals ((JsonObject)f)["uid"].ToString()
                           select new { MutualFriendsCount = m.Count, FriendObj = (JsonObject)f }).Distinct().ToList();

            tmpList.ForEach(x => friend2mfCountDic.Add(x.FriendObj, x.MutualFriendsCount));

            timestamps.AddTimestamp("FacebookHelper.GetFriendsDataList after merging friends and mf-count lists");

            //foreach (JsonObject o in arr)
            //{
            //    try
            //    {
            //        JsonObject mfCountJsonObject = (JsonObject)mfCountArr.Where(f => f["uid"].ToString() == o["uid"].ToString()).FirstOrDefault();
            //        friend2mfCountDic.Add(o, (long)(mfCountJsonObject["mutual_friend_count"] ?? 0));


            //        //DateTime parsedBirthday = new DateTime();
            //        //if (o.ContainsKey("birthday_date") && o["birthday_date"] != null)
            //        //{
            //        //    if (!DateTime.TryParseExact((string)o["birthday_date"], "MM/dd/yyyy", null,
            //        //                                DateTimeStyles.None, out parsedBirthday))
            //        //        if (!DateTime.TryParseExact((string)o["birthday_date"], "dd/MM/yyyy", null,
            //        //                                DateTimeStyles.None, out parsedBirthday))
            //        //            if (DateTime.TryParseExact((string)o["birthday_date"], "MM/dd", null,
            //        //                                DateTimeStyles.None, out parsedBirthday)
            //        //                || DateTime.TryParseExact((string)o["birthday_date"], "dd/MM", null,
            //        //                                DateTimeStyles.None, out parsedBirthday))
            //        //                parsedBirthday = parsedBirthday.AddYears(-user.Birthdate.Year);
            //        //}
            //        //else parsedBirthday = user.Birthdate;
            //    }catch{}
            //}

            //timestamps.AddTimestamp();

            foreach (JsonObject o in friend2mfCountDic.Keys)
            {
                try
                {
                    //JsonObject mfCountJsonObject = (JsonObject)mfCountArr.Where(f => f["uid"].ToString() == (string)o["uid"]).FirstOrDefault();
                    DateTime parsedBirthday = new DateTime();
                    if (o.ContainsKey("birthday_date") && o["birthday_date"] != null)
                    {
                        if (!DateTime.TryParseExact((string)o["birthday_date"], "MM/dd/yyyy", null,
                                                    DateTimeStyles.None, out parsedBirthday))
                            if (!DateTime.TryParseExact((string)o["birthday_date"], "dd/MM/yyyy", null,
                                                    DateTimeStyles.None, out parsedBirthday))
                                if (DateTime.TryParseExact((string)o["birthday_date"], "MM/dd", null,
                                                    DateTimeStyles.None, out parsedBirthday)
                                    || DateTime.TryParseExact((string)o["birthday_date"], "dd/MM", null,
                                                    DateTimeStyles.None, out parsedBirthday))
                                    parsedBirthday = parsedBirthday.AddYears(-user.Birthdate.Year);
                    }
                    else parsedBirthday = user.Birthdate;

                    string locationName = null, locationCity = null, locationRegion = null, locationCountry = null;
                    if (o.ContainsKey("current_location"))
                    {
                        JsonObject location = (JsonObject)o["current_location"];
                        if (location != null && location.ContainsKey("name"))
                        {
                            locationName = (string)location["name"];
                            //locationCity = Config.Users.MapFacebookCityToezFixUpCity(context, locationName, out locationRegion, out locationCountry);
                        }
                    }
                    //locationName = (locationCountry.IsNullOrEmpty())
                    //                    ? String.Format("{0}, {1}, {2}", user.City, user.State, user.Country)
                    //                    : String.Format("{0}, {1}, {2}", locationCity, locationRegion, locationCountry);
                    locationName = locationName ?? String.Format("{0}, {1}, {2}", user.City, user.State, user.Country);

                    string hometownName = null, hometownCity = null, hometownRegion = null, hometownCountry = null;
                    if (o.ContainsKey("hometown_location"))
                    {
                        JsonObject hometown = (JsonObject)o["hometown_location"];
                        if (hometown != null && hometown.ContainsKey("name"))
                        {
                            hometownName = (string)hometown["name"];
                            //hometownCity = Config.Users.MapFacebookCityToezFixUpCity(context, hometownName, out hometownRegion, out hometownCountry);
                        }
                    }
                    //hometownName = (hometownCountry.IsNullOrEmpty())
                    //                    ? String.Format("{0}, {1}, {2}", user.City, user.State, user.Country)
                    //                    : String.Format("{0}, {1}, {2}", hometownCity, hometownRegion, hometownCountry);
                    hometownName = hometownName ?? String.Format("{0}, {1}, {2}", user.City, user.State, user.Country);

                    string meetingSex = "";
                    JsonArray meetingSexes = null;
                    if (o.ContainsKey("meeting_sex"))
                    {
                        meetingSexes = o.ContainsKey("meeting_sex") ? (JsonArray)o["meeting_sex"] : null;
                    }
                    else if (o.ContainsKey("interested_in"))
                    {
                        meetingSexes = o.ContainsKey("interested_in") ? (JsonArray)o["interested_in"] : null;
                    }
                    if (meetingSexes != null && meetingSexes.Count > 0)
                    {
                        meetingSex = (string)meetingSexes[0];
                    }

                    string genderStr = "matchmaker";
                    if (o.ContainsKey("sex"))
                        genderStr = (string)o["sex"];
                    else if (o.ContainsKey("gender"))
                        genderStr = (string)o["gender"];

                    fbFriendsDataList.Add(
                        new FacebookFriendData
                            {
                                UserId = long.Parse(o["uid"].ToString()),
                                Birthdate = parsedBirthday,
                                GenderStr = genderStr,
                                InterestedInStr = meetingSex,
                                IsAppUser = (bool)(o["is_app_user"] ?? false),
                                IsMinor = (bool)(o["is_minor"] ?? false),
                                IsVerified = (bool)(o["verified"] ?? true),
                                MutualFriendsCount = Convert.ToInt32(friend2mfCountDic[o]),
                                PicUrl =
                                    (string)
                                    (o["pic"] ??
                                     ImageHandler.CreateFacebookImageUrl(long.Parse((string)o["uid"]),
                                                                         ImageHandler.eFacebookImageType.Large)),
                                //Status = (string)o["relationship_status"],
                                Username = o["uid"].ToString(),
                                Name = (string)o["name"],
                                RelationshipStatus = (string)o["relationship_status"],
                                Location = locationName,
                                HomeTown = hometownName
                            });
                }
                catch (Exception ex)
                {
                }
            }
            timestamps.AddTimestamp("FacebookHelper.GetFriendsDataList after parsing friends fields from json");
            //HttpContext.Current.GetSession()["SessionFacebookFriends"] = fbFriendsDataList;
            fbFriendsDataList = fbFriendsDataList.OrderByDescending(f => f.MutualFriendsCount).ToList();
            dic[fbUserId] = fbFriendsDataList;
            return fbFriendsDataList;
            //return fbFriendsDataList.Where(u=> !(u.IsMinor || u.IsVerified || u.IsAppUser)).ToList();
        }
        public static IList<FacebookFriendData> GetFriendsDataList(ezFixUpFacebookApp fb, long fbUserId)
        {
            DateTime time1 = DateTime.Now;
            var fbFriendsDataList = new List<FacebookFriendData>();
            var arr = GetFriendsData(fb, fbUserId).ToArray();
            //var mfCountArr = GetMutualFriendsCount(fb, fbUserId).ToArray();
            DateTime time2 = DateTime.Now;
            foreach (JsonObject o in arr)
            {
                try
                {
                    DateTime parsedBirthday = new DateTime();
                    if (!(bool)(o["is_minor"] ?? false))
                    {
                        if (o.ContainsKey("birthday_date"))
                        {
                            if (!DateTime.TryParseExact((string)o["birthday_date"], "MM/dd/yyyy", null,
                                                        DateTimeStyles.None, out parsedBirthday))
                                if (!DateTime.TryParseExact((string)o["birthday_date"], "dd/MM/yyyy", null,
                                                        DateTimeStyles.None, out parsedBirthday))
                                    if (DateTime.TryParseExact((string)o["birthday_date"], "MM/dd", null,
                                                        DateTimeStyles.None, out parsedBirthday)
                                        || DateTime.TryParseExact((string)o["birthday_date"], "dd/MM", null,
                                                        DateTimeStyles.None, out parsedBirthday))
                                        parsedBirthday = parsedBirthday.AddYears(-PageBase.GetCurrentUserSession().Birthdate.Year);
                        }
                        else parsedBirthday = PageBase.GetCurrentUserSession().Birthdate;

                        string locationName = null;
                        if (o.ContainsKey("current_location"))
                        {
                            JsonObject location = (JsonObject)o["current_location"];
                            if (location != null && location.ContainsKey("name"))
                                locationName = (string)location["name"];
                        }

                        string hometownName = null;
                        if (o.ContainsKey("hometown_location"))
                        {
                            JsonObject hometown = (JsonObject)o["hometown_location"];
                            if (hometown != null && hometown.ContainsKey("name"))
                                hometownName = (string)hometown["name"];
                        }

                        fbFriendsDataList.Add(
                            new FacebookFriendData
                                {
                                    UserId = long.Parse((string)o["uid"]),
                                    Birthdate = parsedBirthday,
                                    GenderStr = (string)(o["sex"] ?? "matchmaker"),
                                    //InterestedInStr = (string)(o["meeting_sex"] ?? "matchmaker"),
                                    IsAppUser = (bool)(o["is_app_user"] ?? false),
                                    IsMinor = (bool)(o["is_minor"] ?? false),
                                    IsVerified = (bool)(o["verified"] ?? true),
                                    //MutualFriendsCount = (int)(o["mutual_friend_count"] ?? 0),
                                    PicUrl =
                                        (string)
                                        (o["pic"] ??
                                         ImageHandler.CreateFacebookImageUrl((long)o["uid"],
                                                                             ImageHandler.eFacebookImageType.Large)),
                                    //Status = (string)o["relationship_status"],
                                    Username = (string)o["uid"],
                                    Name = (string)o["name"],
                                    RelationshipStatus = (string)o["relationship_status"],
                                    Location = locationName,
                                    HomeTown = hometownName
                                });
                    }
                    else
                    {

                    }
                }
                catch (Exception ex)
                {
                }
            }
            return fbFriendsDataList.OrderByDescending(f => f.MutualFriendsCount).ToList();
        }

        public static List<JsonObject> GetMutualFriendsCount(int friendsCount)
        {
            var fb = GetSessionFacebookApp();
            if (fb == null || CurrentSessionUserID <= 0)
                return null;

            MutualFriendsCountCallbackClass asyncCallbackWorker = new MutualFriendsCountCallbackClass(fb);
            var limit = 1000;
            for (int i = 0; i < friendsCount; i = i + limit)
            {
                string query1 = string.Format("select uid, mutual_friend_count "
                                              + "from user where uid in "
                                              + "(select uid2 from friend where uid1 = {0} limit {1} offset {2} ) ",
                                              CurrentSessionUserID, limit, i);
                try
                {
                    //JsonArray result = (JsonArray)fb.Query(query1);
                    //returnResult = returnResult.Union(result.Select(r => (JsonObject)r).ToList()).ToList();
                    lock (asyncCallbackWorker.ItsSyncObject)
                    {
                        asyncCallbackWorker.ItsQueryCounter++;
                    }
                    FacebookHelper.GetAsyncFqlResult(fb, query1, asyncCallbackWorker.Callback, null);
                }
                catch (Exception ex)
                {
                    //return null;
                }
            }
            while (asyncCallbackWorker.ItsQueryCounter > 0) { }
            return asyncCallbackWorker.Result;
        }

        class MutualFriendsCountCallbackClass
        {
            public MutualFriendsCountCallbackClass(ezFixUpFacebookApp facebookApp)
            {
                ItsQueryCounter = 0;
                ItsFacebookApp = facebookApp;
                ItsSyncObject = "";
                Result = new List<JsonObject>();
            }
            public string ItsSyncObject;
            public ezFixUpFacebookApp ItsFacebookApp;
            public List<JsonObject> Result;
            public int ItsQueryCounter;

            public void Callback(FacebookAsyncResult result)
            {
                lock (ItsSyncObject)
                {
                    if (result.IsCompleted && result.Error == null && result.Result != null)
                    {
                        var jsonArrayResult = (JsonArray)((JsonObject)result.Result)["data"];
                        Result = Result.Union(jsonArrayResult.Select(r => (JsonObject)r).ToList()).ToList();
                    }
                    ItsQueryCounter--;
                }
            }
        }
        class FriendsDataCallbackClass
        {
            public FriendsDataCallbackClass(ezFixUpFacebookApp facebookApp)
            {
                ItsQueryCounter = 0;
                ItsFacebookApp = facebookApp;
                ItsSyncObject = "";
                Result = new List<JsonObject>();
            }
            public string ItsSyncObject;
            public ezFixUpFacebookApp ItsFacebookApp;
            public List<JsonObject> Result;
            public int ItsQueryCounter;

            public void Callback(FacebookAsyncResult result)
            {
                lock (ItsSyncObject)
                {
                    if (result.IsCompleted && result.Error == null && result.Result != null)
                    {
                        var jsonArrayResult = (JsonArray)((JsonObject)result.Result)["data"];
                        Result = Result.Union(jsonArrayResult.Select(r => (JsonObject)r).ToList()).ToList();
                    }
                    ItsQueryCounter--;
                }
            }
        }

        public static JsonArray GetFriendsData()
        {
            var fb = GetSessionFacebookApp();
            if (fb == null || CurrentSessionUserID <= 0)
                return null;

            string query = string.Format("select uid, name, is_app_user, sex, birthday_date, meeting_sex, relationship_status, "
                                         + "verified, is_minor, is_app_user, pic, current_location, hometown_location "
                                         + "from user where uid in "
                                         + "(select uid2 from friend where uid1 = {0}) ",
                                         CurrentSessionUserID);
            try
            {
                JsonArray result = (JsonArray)fb.Query(query);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static string GetProfilePictureSource(ezFixUpFacebookApp facebook, long fbUserId)
        {
            string source = String.Format("https://graph.facebook.com/{0}/picture?type={1}",
                                              fbUserId, ImageHandler.eFacebookImageType.Large);

            string graphAlbums = String.Format("/{0}/albums", fbUserId);
            var albumsObj = facebook.Api(graphAlbums) as JsonObject;
            if (albumsObj != null)
            {
                var albums = albumsObj["data"] as JsonArray;
                if (albums != null)
                {
                    JsonObject profilePictures =
                        albums.Cast<JsonObject>()
                            .Where(album => album.ContainsKey("name"))
                            .FirstOrDefault(album => ((string)(album["name"])) == "Profile Pictures");

                    if (profilePictures != null)
                    {
                        string graphProfilePhoto = String.Format("/{0}", profilePictures["cover_photo"]);
                        var profilePhotos = facebook.Api(graphProfilePhoto) as JsonObject;
                        if (profilePhotos != null && profilePhotos.ContainsKey("source"))
                            source = (string)profilePhotos["source"];
                    }
                }
            }
            return source;
        }
        public static JsonArray GetFriendsData(ezFixUpFacebookApp fb, long fbUserId)
        {
            if (fb == null || fbUserId <= 0)
                return null;

            string query = string.Format("select uid, name, is_app_user, sex, birthday_date, meeting_sex, relationship_status, "
                                         + "verified, is_minor, is_app_user, pic, current_location, hometown_location  "
                                         + "from user where uid in "
                                         + "(select uid2 from friend where uid1 = {0})",
                                         fbUserId);
            try
            {
                JsonArray result = (JsonArray)fb.Query(query);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public class FacebookFriendData
        {
            public long UserId;
            public int MutualFriendsCount;
            public string Username;
            public string Status;
            public bool IsAppUser;
            public bool IsMinor;
            public bool IsVerified;
            public string GenderStr;
            public DateTime Birthdate;
            public string InterestedInStr;
            public string PicUrl;
            public string Name;
            public string RelationshipStatus;
            public string Location;
            public string HomeTown;
        }
        public static void FetchSessionFriends(object curContext)
        {
            var context = (HttpContext)curContext;
            lock (context)
            {
                var session = context.GetSession();
                lock (session.SyncRoot)
                {
                    session["FacebookFriendsJsonArray"] = context.GetFriendsData();
                }
            }
        }

        public static void LoginezFixUpAppThroughFacebook()
        {
            if (Config.Misc.EnableFacebookIntegration)
            {
                ezFixUpFacebookApp facebook = null;
                long id = 0;
                String accessToken = "";
                string backUrl = HttpContext.Current.Request.QueryString["back_url"]
                               ?? ((HttpContext.Current.Request.UrlReferrer != null)
                                        ? HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.Url.PathAndQuery)
                                        : MatchmakerHelper.CurrentHomePage);
                if ((HttpContext.Current.Request.QueryString["access_token"] != null) && (HttpContext.Current.Request.QueryString["access_token"].Length > 0))
                {
                    accessToken = HttpContext.Current.Request.QueryString["access_token"];
                    facebook = new ezFixUpFacebookApp(accessToken);
                    JsonObject user = facebook.Api("/me") as JsonObject;
                    if (user != null) id = long.Parse((String)user["id"]);
                }
                else
                {
                    facebook = new ezFixUpFacebookApp();
                    id = facebook.UserId;
                }
                if (id == 0)
                {
                    if (FacebookHelper.SessionConnectTriesElapsed-- > 0)
                        HttpContext.Current.Response.Redirect(MatchmakerHelper.CurrentHomePage);
                }
                else
                {
                    //context.GetSession()["facebook"] = facebook;
                    SessionFacebookApp = facebook;
                    //context.GetSession()["facebookAccessToken"] = accessToken;
                    CurrentSessionAccessToken = accessToken;

                    CurrentSessionUserID = id;
                    string[] usernames = Classes.User.FetchUsernamesByFacebookID(new[] { id });

                    if (usernames.Length == 0)
                    {
                        HttpContext.Current.Response.Redirect("Login.aspx?facebook=1&login=1&access_token=" + accessToken + "&back_url=" + backUrl);
                        //Context.Response.Redirect("ShowStatus.aspx");
                    }
                    else
                    {
                        UserSession userSession;

                        try
                        {
                            userSession = new UserSession(usernames[0]);
                            Classes.User.AuthorizeByFacebookID(id);
                            userSession.Authorize(HttpContext.Current.Session.SessionID);
                            PageBase.SetCurrentUserSession(userSession);
                        }
                        catch (NotFoundException)
                        {
                            if (accessToken.Length > 0)
                            {
                                HttpContext.Current.Response.Redirect("Login.aspx?facebook=1&login=1&access_token=" + accessToken + "&back_url=" + backUrl);
                            }
                            else
                            {
                                HttpContext.Current.Response.Redirect("Login.aspx?facebook=1&login=1&back_url=" + backUrl);
                            }

                            return;
                        }
                        catch (AccessDeniedException err)
                        {
                            IDictionary<string, object> parameters = new Dictionary<string, object>();
                            //StatusPageMessage = err.Message;
                            parameters["next"] = Config.Urls.Home + "/ShowStatus.aspx";
                            HttpContext.Current.Response.Redirect(facebook.GetLogoutUrl(parameters).ToString());
                            return;
                        }
                        catch (SmsNotConfirmedException)
                        {
                            HttpContext.Current.Response.Redirect("SmsConfirm.aspx?username=" + usernames[0]);
                            return;
                        }
                        catch (ArgumentException err)
                        {
                            IDictionary<string, object> parameters = new Dictionary<string, object>();
                            //StatusPageMessage = err.Message;
                            parameters["next"] = Config.Urls.Home + "/ShowStatus.aspx";
                            HttpContext.Current.Response.Redirect(facebook.GetLogoutUrl(parameters).ToString());
                            return;
                        }
                        catch (Exception err)
                        {
                            IDictionary<string, object> parameters = new Dictionary<string, object>();
                            Global.Logger.LogWarning(err);
                            //StatusPageMessage = err.Message;
                            parameters["next"] = Config.Urls.Home + "/ShowStatus.aspx";
                            HttpContext.Current.Response.Redirect(facebook.GetLogoutUrl(parameters).ToString());
                            return;
                        }

                        userSession.LoggedInThroughFacebook = true;
                        PageBase.SetCurrentUserSession(userSession);
                        HttpContext.Current.Response.Redirect(backUrl);
                    }
                }
            }
        }
    }
    public static class FacebookHelperExtensions
    {
        public static long GetCurrentSessionUserID(this HttpContext context)
        {
            long res;
            lock (context)
            {
                if (long.TryParse((string)context.GetSession()["CurrentFacebookID"] ?? "0", out res))
                {
                    return res;
                }
            }
            return 0;
        }
        public static void SetCurrentSessionUserID(this HttpContext context, long id)
        {
            lock (context)
            {
                context.GetSession()["CurrentFacebookID"] = id;
            }
        }
        public static void SetIsCurrentSessionConnected(this HttpContext context, bool isConnected)
        {
            lock (context)
            {
                context.GetSession()["IsFacebookConnected"] = isConnected;
            }
        }
        public static bool GetIsCurrentSessionConnected(this HttpContext context)
        {
            bool res;
            lock (context)
            {
                res = (bool)(context.GetSession()["IsFacebookConnected"] ?? false);
            }
            return res;
        }
        public static void SetSessionConnectTriesElapsed(this HttpContext context, int tries)
        {
            lock (context)
            {
                context.GetSession()["FacebookConnectTriesElapsed"] = tries.ToString();
            }
        }
        public static int GetSessionConnectTriesElapsed(this HttpContext context)
        {
            int res;
            lock (context)
            {
                res = int.Parse((string)(context.GetSession()["FacebookConnectTriesElapsed"] ?? "0"));
            }
            return res;
        }
        public static string GetCurrentSessionAccessToken(this HttpContext context)
        {
            string res;
            lock (context)
            {
                res = (string)context.GetSession()["facebookAccessToken"];
            }
            return res;
        }

        public static void SetCurrentSessionAccessToken(this HttpContext context, string token)
        {
            lock (context)
            {
                context.GetSession()["facebookAccessToken"] = token;
            }
        }
        public static void SessionClear(this HttpContext context, string backUrl)
        {
            lock (context)
            {
                context.SetCurrentSessionAccessToken(null);
                context.SetSessionConnectTriesElapsed(1);
                context.SetIsCurrentSessionConnected(false);
                context.SetCurrentSessionUserID(0);
            }
        }
        public static void FetchSessionFriends(this HttpContext context)
        {
            lock (context)
            {
                context.GetSession()["FacebookFriendsJsonArray"] = context.GetFriendsData();
            }
        }
        public static JsonArray GetFriendsData(this HttpContext context)
        {
            var fb = context.GetSessionFacebookApp();
            if (fb == null || fb.Session == null || fb.Session.UserId <= 0)
                return null;

            //string query  =  string.Format("select uid, name, username, mutual_friend_count, is_app_user, sex, birthday_date, meeting_sex, meeting_for, relationship_status, can_post, "
            //                             + "relationship_status, current_location, email, verified, is_minor, languages, pic_with_logo, pic_square_with_logo, pic "
            //                             + "from user where uid in "
            //                             + "(select uid2 from friend where uid1 = {0})",
            //                             CurrentSessionUserID);
            string query = string.Format("select uid, name, mutual_friend_count, is_app_user, sex, birthday_date, meeting_sex, relationship_status, "
                                         + "verified, is_minor, pic, current_location, hometown_location "
                                         + "from user where uid in "
                                         + "(select uid2 from friend where uid1 = {0})",
                                         fb.Session.UserId);
            try
            {
                JsonArray result = (JsonArray)fb.Query(query);
                return result;
            }
            catch
            {
                return null;
            }
        }

        public class AppInvitation
        {
            public string RequestId;
            public string SenderId;
            public string RecipientId;
            public DateTime CreatedTime;
        }

        public static List<AppInvitation> GetAppInvitationsForCurrentUser(this HttpContext context)
        {
            var fb = context.GetSessionFacebookApp();
            if (fb == null || fb.Session == null || fb.Session.UserId <= 0)
                return null;

            string qry = String.Format(
                "SELECT sender_uid, recipient_uid, request_id, created_time FROM apprequest " +
                "WHERE recipient_uid = me() AND app_id = {0}", fb.AppId);

            List<AppInvitation> list = new List<AppInvitation>();

            try
            {
                JsonArray array = (JsonArray)fb.Query(qry);
                foreach (JsonObject item in array)
                {
                    var invitation = new AppInvitation();
                    if (item.ContainsKey("sender_uid") && item.ContainsKey("recipient_uid") &&
                        item.ContainsKey("request_id") && item.ContainsKey("created_time"))
                    {
                        try
                        {
                            invitation.CreatedTime = DateTime.Parse("created_time");
                        }
                        catch (Exception ex) { }
                        invitation.SenderId = (string)item["sender_uid"];
                        invitation.RecipientId = (string)item["recipient_uid"];
                        invitation.RequestId = (string)item["request_id"];
                        list.Add(invitation);
                    }
                }
                return list;
            }
            catch
            {
                return null;
            }
        }

        public static ezFixUpFacebookApp GetSessionFacebookApp(this HttpContext context, bool redirectToLogin)
        {
            ezFixUpFacebookApp facebook = context.GetSessionFacebookApp();

            if (facebook == null)
            {
                if (redirectToLogin && context.GetSessionConnectTriesElapsed() > 0)
                    context.Response.Redirect("~/LoginThroughFacebook.aspx");
                else
                    return null;
            }
            return facebook;
        }

        public static void SetSessionFacebookApp(this HttpContext context, ezFixUpFacebookApp facebookApp)
        {
            lock (context)
            {
                context.GetSession()["facebook"] = facebookApp;
            }
        }
        private static readonly object _sessionLock = new object();

        public static HttpSessionState GetSession(this HttpContext context)
        {
            lock (_sessionLock)
            {
                try
                {
                    //var context = HttpContext.Current;
                    var result = SessionStateUtility.GetHttpSessionStateFromContext(context);
                    lock (result.SyncRoot)
                    {
                        return context.Session;
                    }
                }
                catch (Exception ex)
                {
                    //ex.Handle();
                    return null;
                }
            }
        }
        public static ezFixUpFacebookApp GetSessionFacebookApp(this HttpContext context)
        {
            ezFixUpFacebookApp facebook = null;

            lock (context)
            {
                if (context.GetSession()["facebook"] != null)
                {
                    facebook = (ezFixUpFacebookApp)context.GetSession()["facebook"];
                }
                else if (context.GetCurrentSessionAccessToken().IsNotNullOrEmpty())
                {
                    facebook = new ezFixUpFacebookApp(context.GetCurrentSessionAccessToken());
                }
                else if (context.Request.QueryString["access_token"] != null &&
                         context.Request.QueryString["access_token"].Length > 0)
                {
                    facebook = new ezFixUpFacebookApp(context.Request.QueryString["access_token"]);
                }
                else
                {
                    facebook = new ezFixUpFacebookApp();
                }
                if (facebook.AppSecret.IsNotNullOrEmpty())
                {
                    facebook.AppSecret = FacebookHelper.SecretKey;
                }

                facebook = (facebook.SessionLoaded) ? facebook : null;
                context.SetSessionFacebookApp(facebook);
            }
            return facebook;
        }

        public static JsonArray GetFqlResult(this ezFixUpFacebookApp facebook, string qry)
        {
            JsonArray result = (JsonArray)facebook.Query(qry);
            return result;
        }


        public static JsonArray GetFqlResult(this HttpContext context, string qry)
        {
            JsonArray result;
            lock (context)
            {
                var fb = context.GetSessionFacebookApp();
                result = (JsonArray)fb.Query(qry);
            }
            return result;
        }
        public static void StartFetchFacebookFriendsThread(this HttpContext context)
        {
            ParameterizedThreadStart threadMethod1 = FacebookHelper.FetchSessionFriends;
            Thread newThread1 = new Thread(threadMethod1);
            newThread1.Start(context);
        }
        public static Dictionary<long, List<MutualFriendItem>> GetFacebookMutualFriendsDic(this HttpContext context)
        {
            var retValue = Global.AppFacebookMutualFriendsDic;
            //(Dictionary<long, List<MutualFriendItem>>)
            //    context.Application["FacebookMutualFriendsDic"];
            if (retValue == null)
            {
                retValue = new Dictionary<long, List<MutualFriendItem>>();
                //context.Application["FacebookMutualFriendsDic"] = retValue;
                Global.AppFacebookMutualFriendsDic = retValue;
            }
            return retValue;
        }
        public static Dictionary<long, List<FacebookHelper.FacebookFriendData>> GetFacebookFriendsDic(this HttpContext context)
        {
            var retValue = Global.AppFacebookFriendsDic;
            //(Dictionary<long, List<FacebookHelper.FacebookFriendData>>)
            //    context.Application["FacebookFriendsDic"];
            if (retValue == null)
            {
                retValue = new Dictionary<long, List<FacebookHelper.FacebookFriendData>>();
                //context.Application["FacebookFriendsDic"] = retValue;
                Global.AppFacebookFriendsDic = retValue;
            }
            return retValue;
        }
    }
    public class TimeMeasure
    {
        public TimeMeasure()
        {
            Timestamps = new Dictionary<string, DateTime>();
            AddTimestamp("constructor");
        }

        public Dictionary<string, DateTime> Timestamps;

        public void AddTimestamp(string key)
        {
            key = String.Format("{0}_{1}", key, Timestamps.Keys.Count);
            Timestamps.Add(key, DateTime.Now);
        }
    }
    public class MutualFriendItem
    {
        public long FriendID;
        public long MutualFriendID;
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Web;
using System.Web.Caching;
using AspNetDating.Model;
using Facebook;
using Microsoft.ApplicationBlocks.Data;

namespace AspNetDating.Classes
{
    public class LovehitchBackgroundWorker : BackgroundWorker
    {
        private HttpContext Context;
        
        public LovehitchBackgroundWorker(HttpContext context)
        {
            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;
            Context = context;
        }
        /// <summary>
        /// This routine creates a background worker and starts it working on another thread to 
        /// periodically poll SQL server and refresh this applications cached data as needed.
        /// </summary>
        public static void StartBackgroundWorker(LovehitchBackgroundWorker backgroundWorker)
        {
            var worker = backgroundWorker;
            //worker.DoWork += new DoWorkEventHandler(SqlPollingWork);
            //worker.WorkerReportsProgress = false;
            //worker.WorkerSupportsCancellation = true;
            //worker.RunWorkerCompleted +=
            //       new RunWorkerCompletedEventHandler(SqlPollingWorkCompleted);
            worker.RunWorkerAsync(backgroundWorker.Context); //Pass HttpContext to background worker

            //Add this BackgroundWorker object instance to the application variables 
            //so it can be cleared when the Application_End event fires.
            //HttpContext.Current.Application.GetVariables().SqlPollingBackgroundWorker = worker;

            //((Dictionary<string, LovehitchBackgroundWorker>)
            //    backgroundWorker.Context.Application["BackgroundWorkersDic"])
            //        .Add(worker.GetType().ToString(), worker);
            Global.AppBackgroundWorkersDic.Add(worker.GetType().ToString(), worker);
        }
        public static void StopBackgroundWorker(LovehitchBackgroundWorker backgroundWorker)
        {
            var worker = backgroundWorker;
            if (worker != null)
                worker.CancelAsync();
        }
    }
    public class LoadAndSaveFacebookImageBackgroundWorker : LovehitchBackgroundWorker
    {
        //public bool Result;
        public List<LoveHitchFacebookApp> FacebookAppList;

        public LoadAndSaveFacebookImageBackgroundWorker(HttpContext context) :
            base(context)
        {
            WorkerReportsProgress = false;
            WorkerSupportsCancellation = true;
            FacebookAppList = new List<LoveHitchFacebookApp>();
        }

        //protected override void OnDoWork(DoWorkEventArgs e)
        public new static void DoWork(LoveHitchFacebookApp facebook, HttpContext context)
        {
            //ReportProgress(0, "Working hard on this report...");
            //var msWait = 1000 * 120;

            //while (true)
            {
                //Thread.Sleep(msWait);

                //if (this.FacebookAppList.Count == 0)
                //{
                //    Thread.Sleep(msWait);
                //}
                //else
                {
                    //var facebook = FacebookAppList[0];
                    var user = FacebookHelper.LoadUserFromDbByFacebookId(facebook.UserId);
                    if (user == null) return;

                    //for (int i = 0; i < 2; i++)
                    {
                        DateTime[] time = new DateTime[10];
                        time[0] = DateTime.Now;

                        IList<FacebookHelper.FacebookFriendData> allFriendsData =
                            FacebookHelper.GetFriendsDataList(facebook, facebook.UserId);
                        var mfDic = context.GetFacebookMutualFriendsDic();// (Dictionary<long, List<MutualFriendItem>>)context.Application["FacebookMutualFriendsDic"];
                        var allMutualFriends = (List<MutualFriendItem>)mfDic[facebook.UserId];
                        if (allMutualFriends != null)
                        {
                            for (int i = 0; i < allFriendsData.Count; i++)
                            {
                                var fbFriendData = allFriendsData[i];
                                fbFriendData.MutualFriendsCount =
                                    allMutualFriends.Count(f => f.FriendID == fbFriendData.UserId);
                            }

                            allFriendsData = allFriendsData.OrderByDescending(f => f.MutualFriendsCount).ToList();
                        }
                        var lstFriendsIds = new List<long>();
                        try
                        {
                            using (var db = new AspNetDatingDataContext())
                            {
                                var tmpList =
                                    CompiledQueries.FetchActiveFriendsForUsername(db, user.Username).
                                        ToList();
                                var tmpArray =
                                    tmpList.Where(
                                        f =>
                                        f.User1.u_logincount == 0 && f.User1.u_facebookid.HasValue &&
                                        !f.User1.u_face_control_approved)
                                        .OrderBy(f => f.User1.u_name).Select(f => f.User1.u_facebookid.Value).ToArray();

                                foreach (var friendData in allFriendsData)
                                {
                                    var un = friendData.UserId;

                                    if (!tmpArray.Contains(un)) continue;

                                    int imgId = Photo.GetPrimaryOrDefaultId(un.ToString());
                                    if (imgId < 0)
                                        lstFriendsIds.Add(un);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        if (lstFriendsIds.Count > 0)
                        {
                            try
                            {
                                var parameterizedThreadsList =
                                    new LoveHitchParallel.ParameterizedThreadsList<long>(
                                        lstFriendsIds,
                                        (new FacebookHelper.FetchAndSaveSaveFacebookFriendImageClass()).
                                            FetchAndSaveFacebookImageByFacebookId,
                                        null);
                                parameterizedThreadsList.RunAsBackgroundThreads = true;
                                parameterizedThreadsList.ExecuteParallelWork(1);
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                    //FacebookAppList.RemoveAt(0);
                }
            }
        }
    }

    public class FetchDbImageToCacheDirectoryBackgroundWorker : LovehitchBackgroundWorker
    {
        //public bool Result;
        public static List<int> PhotosIdsList = new List<int>();
        public HttpContext Context;
        private int _size1 = 117;
        private int _size2 = 113;

        public FetchDbImageToCacheDirectoryBackgroundWorker(HttpContext context) :
            base(context)
        {
            WorkerReportsProgress = false;
            WorkerSupportsCancellation = true;
            Context = context;
        }
        private void SaveImageToCachDir(int size, int photoId)
        {
            if (photoId <= 0) return;

            Photo photo = Photo.Fetch(photoId);
            Image image = photo.Image;
            int cacheDir = Math.Abs(photoId % 10);
            var format = image.RawFormat;
            string cacheUrl = Config.Urls.ImagesCacheHome + String.Format("/{3}/photo{4}_{0}_{1}_{2}.jpg",
                                        photoId, size, size, cacheDir, "face");
            string cacheFile = Config.Directories.ImagesCacheDirectory + String.Format(@"\{3}\photo{4}_{0}_{1}_{2}.jpg",
                                            photoId, size, size, cacheDir, "face");
            string cacheKey = String.Format("Photos_Exists_{0}", cacheUrl);

            if (Context.Cache[cacheKey] == null)
            {
                try
                {
                    if (!File.Exists(cacheFile))
                    {
                        image = Photo.ResizeRectangleImage(image, size, size);
                        image.Save(cacheFile, format);
                    }
                }
                catch (Exception err)
                {
                }
                finally
                {
                    image.Dispose();
                }
            }
            if (File.Exists(cacheFile))
            {
                Context.Cache.Add(cacheKey, true, new CacheDependency(cacheFile),
                                  Cache.NoAbsoluteExpiration, TimeSpan.FromHours(1), CacheItemPriority.NotRemovable,
                                  null);
            }
        }
        protected override void OnDoWork(DoWorkEventArgs e)
        {
            while (true)
            {
                if (PhotosIdsList.Count > 0)
                {
                    SaveImageToCachDir(_size1, PhotosIdsList[0]);
                    SaveImageToCachDir(_size2, PhotosIdsList[0]);

                    lock (PhotosIdsList)
                    {
                        PhotosIdsList.RemoveAt(0);
                    }
                }
                else
                {
                    Thread.Sleep(10000);
                }
            }
        }
    }

    public class UpdateFacebookFriendsDataBackgroundWorker : LovehitchBackgroundWorker
    {
        //public bool Result;
        public static List<FacebookFriendsDataBackgroundWorkerClass> WorkersList = new List<FacebookFriendsDataBackgroundWorkerClass>();
        public HttpContext Context;
        //public static object SyncRoot = new object();

        public UpdateFacebookFriendsDataBackgroundWorker(HttpContext context) :
            base(context)
        {
            WorkerReportsProgress = false;
            WorkerSupportsCancellation = true;
            Context = context;
        }
        protected override void OnDoWork(DoWorkEventArgs e)
        {
            while (true)
            {
                if (WorkersList.Count > 0)
                {
                    WorkersList[0].UpdateFriendsData();
                    lock (WorkersList)
                    {
                        WorkersList.RemoveAt(0);
                    }
                }
                else
                {
                    Thread.Sleep(10000);
                }
            }
        }
    }

    public class AddNewFacebookFriendsBackgroundWorker : LovehitchBackgroundWorker
    {
        //public bool Result;
        public List<FacebookApp> FacebookAppList;
        public HttpContext Context;
        public AddNewFacebookFriendsBackgroundWorker(HttpContext context) :
            base(context)
        {
            WorkerReportsProgress = false;
            WorkerSupportsCancellation = true;
            FacebookAppList = new List<FacebookApp>();
            Context = context;
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            var msWait = 1000 * 90;

            var p = System.Diagnostics.Process.GetCurrentProcess();
            p.PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime;

            while (true)
            {
                if (CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                TimeSpan timeout = TimeSpan.FromSeconds(10);
                Thread.Sleep(timeout);

                if (this.FacebookAppList.Count == 0)
                {
                    timeout = TimeSpan.FromMinutes(2);
                    Thread.Sleep(timeout);
                }
                else
                {
                    var facebook = FacebookAppList[0];
                    //AddFriendsWithMutualFriends(facebook, Context);
                    //AddFriendsWithoutMutualFriends(facebook, Context);
                    //FetchAndSaveFacebookMutualFriends(facebook, Context);

                    ProcessFacebookFriendsAsync(Context, facebook);



                    //LoadAndSaveFacebookImageBackgroundWorker.DoWork(facebook, Context);
                    //LoadAndSaveFacebookImageBackgroundWorker.DoWork(facebook, Context);

                    //DateTime[] time = new DateTime[10];
                    //time[0] = DateTime.Now;

                    //IList<FacebookHelper.FacebookFriendData> allFriendsData =
                    //    FacebookHelper.GetFriendsDataList(facebook, facebook.UserId);

                    //if (allFriendsData == null || allFriendsData.Count == 0) return;

                    //var fbFriendsIds = allFriendsData.Select(f => f.UserId);

                    //time[1] = DateTime.Now;
                    //time[1] = DateTime.Now;

                    //var lstFriendsIds = fbFriendsIds.ToList();

                    //int maxThreadsInPool = (int) Math.Min(Math.Sqrt((double) allFriendsData.Count), 3);
                    //var oldUsernames = new List<string>();

                    //if (CancellationPending)
                    //{
                    //    e.Cancel = true;
                    //    return;
                    //}

                    //var ftu1 = new FacebookHelper.FetchTakenUsernames
                    //               {Usernames = allFriendsData.Select(f => f.Username).ToList()};
                    //var takenUsernameThreadsList =
                    //    new LoveHitchParallel.ParameterizedThreadsList<string>(ftu1.Usernames, ftu1.TakenUsernames,
                    //                                                           ftu1.AddUsernameIfTaken, null);
                    //takenUsernameThreadsList.RunAsBackgroundThreads = true;
                    //takenUsernameThreadsList.ExecuteParallelWork(maxThreadsInPool);
                    //oldUsernames = ftu1.TakenUsernames;

                    //if (CancellationPending)
                    //{
                    //    e.Cancel = true;
                    //    return;
                    //}
                    //time[4] = DateTime.Now;
                    //time[4] = DateTime.Now;
                    //try
                    //{
                    //    if (lstFriendsIds.Count() > 0)
                    //    {
                    //        List<FacebookHelper.FacebookFriendData> fbFriendsDataList =
                    //            allFriendsData.Where(f => !oldUsernames.Contains(f.UserId.ToString())).ToList();
                    //        var parameterizedThreadsList =
                    //            new LoveHitchParallel.ParameterizedThreadsList<FacebookHelper.FacebookFriendData>(
                    //                fbFriendsDataList,
                    //                (new FacebookHelper.AddNewFacebookFriendClass()).AddNewFacebookFriend,
                    //                null);
                    //        parameterizedThreadsList.RunAsBackgroundThreads = true;
                    //        parameterizedThreadsList.ExecuteParallelWork(maxThreadsInPool);
                    //    }
                    //    time[5] = DateTime.Now;
                    //    if (CancellationPending)
                    //    {
                    //        e.Cancel = true;
                    //        return;
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //}
                    //finally
                    //{
                    //    time[6] = DateTime.Now;

                    //    var ftu = new FacebookHelper.FetchTakenUsernames
                    //                  {Usernames = lstFriendsIds.Select(f => f.ToString()).ToList()};
                    //    var parameterizedThreadsList =
                    //        new LoveHitchParallel.ParameterizedThreadsList<string>(ftu.Usernames, ftu.TakenUsernames,
                    //                                                               ftu.AddUsernameIfTaken, null);
                    //    parameterizedThreadsList.RunAsBackgroundThreads = true;
                    //    parameterizedThreadsList.ExecuteParallelWork(maxThreadsInPool);

                    //    time[7] = DateTime.Now;
                    //    time[7] = DateTime.Now;

                    //    var friendsUsernames = allFriendsData.Select(f => f.UserId.ToString());
                    //    var notTakenUsernames = ftu.TakenUsernames.Where(u => !friendsUsernames.Contains(u)).ToString();
                    //    var user = FacebookHelper.LoadUserFromDbByFacebookId(facebook.UserId);
                    //    List<FacebookHelper.FacebookFriendDataClass> inTakenFriendsList =
                    //        ftu.TakenUsernames.Select(f => new FacebookHelper.FacebookFriendDataClass
                    //                                           {
                    //                                               ViewerUsernme = user.Username,
                    //                                               FriendUsernme = f.ToString(),
                    //                                               FacebookAppSession = facebook
                    //                                           }).ToList();
                    //    time[8] = DateTime.Now;
                    //    time[8] = DateTime.Now;

                    //    if (ftu.TakenUsernames.Count() > 0)
                    //    {
                    //        var addFriendsDataThreadsList =
                    //            new LoveHitchParallel.ParameterizedThreadsList<FacebookHelper.FacebookFriendDataClass>(
                    //                inTakenFriendsList, null, null);

                    //        addFriendsDataThreadsList.ExecuteParallelWork(maxThreadsInPool);

                    //        time[9] = DateTime.Now;
                    //        time[9] = DateTime.Now;
                    //    }
                    //    //LoadAndSaveFacebookImageBackgroundWorker.DoWork(facebook,Context);
                    //}
                    lock (FacebookAppList)
                    {
                        FacebookAppList.RemoveAt(0);
                    }
                }
            }
            //e.Result = Result = true;
        }

        private static HybridDictionary userStateToLifetime = new HybridDictionary();

        private delegate void FacebookFriendsWorkerEventHandler(LoveHitchFacebookApp facebook, HttpContext context, AsyncOperation asyncOp);

        private static void ProcessFacebookFriendsAsync(HttpContext context, object taskId)
        {
            // Create an AsyncOperation for taskId.
            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(taskId);

            LoveHitchFacebookApp facebook = taskId as LoveHitchFacebookApp;
            bool toInvoke = true;

            // Multiple threads will access the task dictionary,
            // so it must be locked to serialize access.
            lock (userStateToLifetime.SyncRoot)
            {
                if (userStateToLifetime.Contains(taskId))
                {
                    toInvoke = false;
                    //throw new ArgumentException("Task ID parameter must be unique","taskId");
                }
                var values = userStateToLifetime.Values as Collection<AsyncOperation>;

                if (values != null && values.Contains(asyncOp))
                {
                    toInvoke = false;
                    //throw new ArgumentException("This Async Operation is Already Exist","asyncOp");
                }
                if (toInvoke)
                    userStateToLifetime[taskId] = asyncOp;
            }

            if (toInvoke)
            {
                // Start the asynchronous operation.
                FacebookFriendsWorkerEventHandler workerDelegate = new FacebookFriendsWorkerEventHandler(FacebookFriendsWorker);
                workerDelegate.BeginInvoke(
                    facebook, context,
                    asyncOp,
                    null,
                    null);
            }
        }

        private static void FacebookFriendsWorker(LoveHitchFacebookApp facebook, HttpContext context, AsyncOperation asyncOp)
        {
            AddFriendsWithMutualFriends(facebook, context);
            AddFriendsWithoutMutualFriends(facebook, context);
            FetchAndSaveFacebookMutualFriends(facebook, context);


        }

        private static void FetchAndSaveFacebookMutualFriends(LoveHitchFacebookApp facebook, HttpContext context)
        {
            DateTime[] time = new DateTime[5];
            time[0] = DateTime.Now;

            var user = FacebookHelper.LoadUserFromDbByFacebookId(facebook.UserId);
            var dic = context.GetFacebookFriendsDic();
            if (user == null || dic == null) return;

            var sessionFriendsData = (dic.ContainsKey(facebook.UserId)) ? dic[facebook.UserId] : null;
            if (sessionFriendsData == null) return;

            var mfDic = context.GetFacebookMutualFriendsDic();
            if (mfDic == null || mfDic[facebook.UserId] == null) return;

            var lstMutualFriends = (List<MutualFriendItem>)mfDic[facebook.UserId];
            if (lstMutualFriends == null) return;

            var totalFriendsCount = sessionFriendsData.Count;
            int limit = 40;
            var lstFriendsMutualCount = new List<MutualFriendItem>();

            for (int offset = 0; offset < totalFriendsCount; offset += limit)
            {
                var qry1 = "SELECT uid2 FROM friend WHERE uid1=me()";
                var qry2tmpl =
                    "select uid, mutual_friend_count from user where mutual_friend_count > 00 and mutual_friend_count <= 30 and uid in " +
                    "( SELECT uid2 FROM friend WHERE uid1=me() limit {0} offset {1})";
                var qry2 = String.Format(qry2tmpl, limit, offset);

                JsonArray res = FacebookHelper.GetFqlResult(facebook, qry2);
                lstFriendsMutualCount =
                    lstFriendsMutualCount.Union(
                                          res.Select(o => new MutualFriendItem
                                          {
                                              FriendID = long.Parse((string)((JsonObject)o)["uid"]),
                                              MutualFriendID = ((JsonObject)o)["mutual_friend_count"] == null ? 0 : long.Parse((string)((JsonObject)o)["mutual_friend_count"])
                                          })).ToList();
            }
            //var groups = lstFriendsMutualCount.GroupBy(o => o.FriendID);
            //int groupsCount = groups.Count();
            //long x = lstFriendsMutualCount.Sum(o => o.MutualFriendID);

            time[1] = DateTime.Now;

            var lstFacebookSessionMutualFriends = new List<MutualFriendItem>();
            //long counter = 0;
            const long limitCount = 20;
            //long skipCount = 0;
            int qryCount = 0;
            for (long i = 0; i < totalFriendsCount; i += limitCount)
            {
                //if (counter + lstFriendsMutualCount[i].MutualFriendID < 400 && limitCount < 80)
                //if (counter + lstFriendsMutualCount[i].MutualFriendID < 300 && limitCount < 30)
                //{
                //    limitCount++;
                //    counter += lstFriendsMutualCount[i].MutualFriendID;
                //}
                //else
                {
                    qryCount++;
                    //counter = lstFriendsMutualCount[i].MutualFriendID;
                    var qry1tmpl =
                        "select uid from user where mutual_friend_count > 30 and mutual_friend_count <= 200 and uid in (SELECT uid2 FROM friend WHERE uid1=me()  limit {0} offset {1}) ";
                    var qry1 = String.Format(qry1tmpl, limitCount, i);
                    //for (long j = 0; j < totalFriendsCount; j += 1260)
                    {
                        //var qry2tmpl = "SELECT uid2 FROM friend WHERE uid1=me() limit 1260 offset {0}";
                        //var qry2 = String.Format(qry2tmpl, j);
                        var qry2 = "SELECT uid2 FROM friend WHERE uid1=me()";
                        //limitCount = 1;
                        //skipCount = i-1;
                        var qry3tmpl = "select uid1,uid2 from friend where uid1 in ({0}) and uid2 in ({1})";
                        var qry3 = String.Format(qry3tmpl, qry1, qry2);
                        JsonArray res = FacebookHelper.GetFqlResult(facebook, qry3);
                        var res1 = res.Select(o => new MutualFriendItem
                        {
                            FriendID = long.Parse((string)((JsonObject)o)["uid1"]),
                            MutualFriendID =
                                long.Parse((string)((JsonObject)o)["uid2"])
                        });
                        var res2 = res1.Select(o => new MutualFriendItem
                        {
                            FriendID = o.MutualFriendID,
                            MutualFriendID = o.FriendID
                        });
                        lstFacebookSessionMutualFriends = lstFacebookSessionMutualFriends.Union(res1).Union(res2).ToList();
                    }
                }
            }
            time[2] = DateTime.Now;
            //time[2] = DateTime.Now;

            //var qryFriendsCountMF =
            //    //"SELECT uid FROM user ORDER BY mutual_friend_count DESC limit 5000";
            //    //"SELECT uid,mutual_friend_count FROM user WHERE uid IN (SELECT uid2 FROM friend WHERE uid1=me()) order by mutual_friend_count DESC limit 5000";
            //        "SELECT uid2 FROM friend WHERE uid1=me()";

            //JsonArray friendsByQuery = facebook.GetFqlResult(qryFriendsCountMF);

            //var lstFriends = friendsByQuery
            //        .Select(o => new
            //        {
            //            FriendID = long.Parse((string)((JsonObject)o)["uid2"])
            //            //,MutualFriendsCount = long.Parse((string) ((JsonObject) o)["mutual_friend_count"])
            //        }).ToList();

            //var qryMutualFriendsTemplate = "SELECT uid1, uid2 FROM friend " +
            //                               " WHERE uid1 IN (SELECT uid2 FROM friend WHERE uid1=me() limit {0} offset {1}) " +
            //                               " AND   uid2 IN (SELECT uid2 FROM friend WHERE uid1=me())";

            //var mutualFriendItems = new List<MutualFriendItem>();
            //int limit = 60;
            //for (int offset = 0; offset < lstFriends.Count; offset += limit)
            //{
            //    JsonArray mutualFriendsPerFriendByQuery =
            //        facebook.GetFqlResult(String.Format(qryMutualFriendsTemplate, limit, offset));

            //    mutualFriendItems = mutualFriendItems.Union(
            //        mutualFriendsPerFriendByQuery.Select(o => new MutualFriendItem
            //        {
            //            FriendID = long.Parse((string)((JsonObject)o)["uid1"]),
            //            MutualFriendID = long.Parse((string)((JsonObject)o)["uid2"])
            //        })
            //                                            ).ToList();
            //}
            //var mfDic = context.GetFacebookMutualFriendsDic();
            //if (mfDic == null) return;
            //var lstMutualFriends = (List<MutualFriendItem>)mfDic[facebook.UserId];
            //if (lstMutualFriends == null) return;

            lstMutualFriends = lstMutualFriends.Union(lstFacebookSessionMutualFriends).ToList();
            var quickSessionMutualFriendsList = new List<MutualFriendItem>();

            //var user = FacebookHelper.LoadUserFromDbByFacebookId(facebook.UserId);
            var mfGroups = lstMutualFriends.GroupBy(f => f.FriendID);
            foreach (var mfGroup in mfGroups)
            {
                var friendUsername = mfGroup.Key.ToString();
                if (user.IsUserInFriendList(friendUsername))
                {
                    string mfData = string.Join(",", mfGroup.Select(g => g.MutualFriendID.ToString()).ToArray());
                    using (SqlConnection conn = Config.DB.Open())
                    {
                        object scalar =
                            SqlHelper.ExecuteScalar(conn, "UpdateFacebookMutualFriends",
                                                    user.Username, friendUsername, mfData);
                    }
                    quickSessionMutualFriendsList.AddRange(mfGroup.Take(5));
                }
            }
            time[3] = DateTime.Now;
            time[3] = DateTime.Now;
            mfDic[facebook.UserId] = quickSessionMutualFriendsList;
        }

        public static void AddFriendsWithMutualFriends(LoveHitchFacebookApp facebook, HttpContext context)
        {
            try
            {
                var mfDic = context.GetFacebookMutualFriendsDic();
                if (mfDic == null) return;
                var lstMutualFriends = (List<MutualFriendItem>)mfDic[facebook.UserId];
                if (lstMutualFriends == null) return;
                var user = FacebookHelper.LoadUserFromDbByFacebookId(facebook.UserId);

                var friendsIDsWithMutualFriends =
                    lstMutualFriends.Select(f => f.FriendID).Distinct().ToList();

                //if (friendsIDsWithMutualFriends != null)
                {
                    if (friendsIDsWithMutualFriends.Count > 0)
                    {
                        FacebookHelper.AddFriends(user, 0, context, facebook);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        public static void AddFriendsWithoutMutualFriends(LoveHitchFacebookApp facebook, HttpContext context)
        {
            try
            {
                var mfDic = context.GetFacebookMutualFriendsDic();
                if (mfDic == null) return;
                var lstMutualFriends = (List<MutualFriendItem>)mfDic[facebook.UserId];
                if (lstMutualFriends == null) return;
                var user = FacebookHelper.LoadUserFromDbByFacebookId(facebook.UserId);

                var friendsIDsWithMutualFriends =
                    lstMutualFriends.Select(f => f.FriendID).Distinct().ToList();

                if (User.IsUsernameTaken(friendsIDsWithMutualFriends.Last().ToString()))
                {
                    FacebookHelper.AddFriends(user, 5000, context, facebook);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }

    public class FacebookPhotosWorker
    {
        static FacebookPhotosWorker()
        {
        }

        private static System.Timers.Timer _tWorker;
        private static bool _workerLock = false;

        public static void InitializeWorkerTimer()
        {
            _tWorker = new System.Timers.Timer
                           {
                               AutoReset = true,
                               Interval = TimeSpan.FromHours(1).TotalMilliseconds
                           };
            _tWorker.Elapsed += new ElapsedEventHandler(tWorker_Elapsed);
            _tWorker.Start();

            // Run payment processing the 1st time
            tWorker_Elapsed(null, null);
        }

        private static void tWorker_Elapsed(object sender, ElapsedEventArgs e)
        {
            DateTime dtLastTick = DBSettings.Get("FacebookPhotosWorker_LastTimerTick", DateTime.Now);

            if (DateTime.Now.Subtract(dtLastTick) >= TimeSpan.FromMinutes(1))
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(AsyncProcessFacebookPhotos), null);
                DBSettings.Set("FacebookPhotosWorker_LastTimerTick", DateTime.Now);
            }
        }

        private static void AsyncProcessFacebookPhotos(object data)
        {
            int curHour = DateTime.Now.Hour;
            if (_workerLock || curHour < 2 || curHour > 5)
            {
                return;
            }

            try
            {
                _workerLock = true;
                List<string> lstWithoutPhotosUsernames = null;
                List<long> lstWithoutPhotosFacebookIds = null;

                using (var db = new AspNetDatingDataContext())
                {
                    try
                    {
                        //var lstWithPhotosUsernames = CompiledQueries.FetchUsernamesWithPhotos(db).ToList();
                        //var lstNewUsernames = CompiledQueries.FetchNew24HourFacebookFriendsUsernames(db).ToList();

                        //lstWithoutPhotosUsernames = lstNewUsernames.Except(lstWithPhotosUsernames).ToList();
                        lstWithoutPhotosUsernames = CompiledQueries.FetchFacebookUsernamesWithoutPhoto(db).ToList();
                        //foreach (var un in lstWithoutPhotosUsernames)
                        //{
                        //    int imgId = Photo.GetPrimaryOrDefaultId(un.ToString());
                        //    if (imgId < 0)
                        //        lstFriendsIds.Add(un);
                        //}
                    }
                    catch (Exception ex)
                    {
                    }
                    if (lstWithoutPhotosUsernames != null && lstWithoutPhotosUsernames.Count > 0)
                    {
                        try
                        {
                            lstWithoutPhotosFacebookIds =
                                lstWithoutPhotosUsernames.Select(un => long.Parse(un)).ToList();
                            var parameterizedThreadsList =
                                new LoveHitchParallel.ParameterizedThreadsList<long>(
                                    lstWithoutPhotosFacebookIds,
                                    (new FacebookHelper.FetchAndSaveSaveFacebookFriendImageClass()).
                                        FetchAndSaveFacebookImageByFacebookId,
                                    null);
                            parameterizedThreadsList.RunAsBackgroundThreads = true;
                            parameterizedThreadsList.ExecuteParallelWork(1);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
            catch (Exception err)
            {
                Global.Logger.LogError("FacebookPhotosWorker", err);
            }
            finally
            {
                _workerLock = false;
            }
        }
    }
}
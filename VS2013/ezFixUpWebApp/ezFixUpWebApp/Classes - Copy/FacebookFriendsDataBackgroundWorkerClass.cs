using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading;
using Facebook;

namespace ezFixUp.Classes
{
    public class FacebookFriendsDataBackgroundWorkerClass
    {
        private PageBase _pageBase;
        private HttpContext _context;
        private TimeMeasure timestamps;
        private LoveHitchFacebookApp _facebook;
        private long _id;
        private string _accessToken;

        public FacebookFriendsDataBackgroundWorkerClass(PageBase pageBase, HttpContext context,
                                                   LoveHitchFacebookApp facebook, long id, string accessToken)
        {
            _pageBase = pageBase;
            _context = context;
            timestamps = new TimeMeasure();
            _facebook = facebook;
            _id = id;
            _accessToken = accessToken;
        }
        public void UpdateFriendsData()
        {
            IList<FacebookHelper.FacebookFriendData> friendsData = FacebookHelper.GetFriendsDataList(_context, _facebook, _id);
            timestamps.AddTimestamp("PageBase.UpdateFacebookFriendsdata");
            #region obsolete 1
            //int totalFriendsCount = friendsData.Count;
            //int limit = 40;
            //var lstFriendsMutualCount = new List<MutualFriendItem>();
            //for (int offset = 0; offset < totalFriendsCount; offset += limit)
            //{
            //    var qry1 = "SELECT uid2 FROM friend WHERE uid1=me()";
            //    var qry2tmpl =
            //        "select uid, mutual_friend_count from user where mutual_friend_count > 00 and mutual_friend_count <= 10 and uid in " +
            //        "( SELECT uid2 FROM friend WHERE uid1=me() limit {0} offset {1})";
            //    var qry2 = String.Format(qry2tmpl, limit, offset);
            //    JsonArray res = FacebookHelper.GetFqlResult(qry2);
            //    lstFriendsMutualCount =
            //        lstFriendsMutualCount.Union(
            //                              res.Select(o => new MutualFriendItem
            //                                        {
            //                                            FriendID = long.Parse((string)((JsonObject)o)["uid"]),
            //                                            MutualFriendID = ((JsonObject)o)["mutual_friend_count"] == null ? 0 : long.Parse((string)((JsonObject)o)["mutual_friend_count"])
            //                                        })).ToList();
            //}
            //var groups = lstFriendsMutualCount.GroupBy(o => o.FriendID);
            //int groupsCount = groups.Count();
            //long x = lstFriendsMutualCount.Sum(o => o.MutualFriendID);

            //time[1] = DateTime.Now;

            //var lstFacebookSessionMutualFriends = new List<MutualFriendItem>();
            //long counter = 0;
            //long limitCount = 20;
            ////long skipCount = 0;
            //int qryCount = 0;
            //for (long i = 0; i < totalFriendsCount; i += limitCount)
            //{
            //    //if (counter + lstFriendsMutualCount[i].MutualFriendID < 400 && limitCount < 80)
            //    //if (counter + lstFriendsMutualCount[i].MutualFriendID < 300 && limitCount < 30)
            //    //{
            //    //    limitCount++;
            //    //    counter += lstFriendsMutualCount[i].MutualFriendID;
            //    //}
            //    //else
            //    {
            //        qryCount++;
            //        //counter = lstFriendsMutualCount[i].MutualFriendID;
            //        var qry1tmpl =
            //            "select uid from user where mutual_friend_count > 00 and mutual_friend_count <= 10 and uid in (SELECT uid2 FROM friend WHERE uid1=me()  limit {0} offset {1}) ";
            //        var qry1 = String.Format(qry1tmpl, limitCount, i);
            //        //for (long j = 0; j < totalFriendsCount; j += 1260)
            //        {
            //            //var qry2tmpl = "SELECT uid2 FROM friend WHERE uid1=me() limit 1260 offset {0}";
            //            //var qry2 = String.Format(qry2tmpl, j);
            //            var qry2 = "SELECT uid2 FROM friend WHERE uid1=me()";
            //            //limitCount = 1;
            //            //skipCount = i-1;
            //            var qry3tmpl = "select uid1,uid2 from friend where uid1 in ({0}) and uid2 in ({1})";
            //            var qry3 = String.Format(qry3tmpl, qry1, qry2);
            //            JsonArray res = FacebookHelper.GetFqlResult(qry3);
            //            var res1 = res.Select(o => new MutualFriendItem
            //                                           {
            //                                               FriendID = long.Parse((string) ((JsonObject) o)["uid1"]),
            //                                               MutualFriendID =
            //                                                   long.Parse((string) ((JsonObject) o)["uid2"])
            //                                           });
            //            var res2 = res1.Select(o => new MutualFriendItem
            //                                            {
            //                                                FriendID = o.MutualFriendID,
            //                                                MutualFriendID = o.FriendID
            //                                            });
            //            lstFacebookSessionMutualFriends = lstFacebookSessionMutualFriends.Union(res1).Union(res2).ToList();
            //        }
            //    }
            //}
            //time[2] = DateTime.Now;
            //time[2] = DateTime.Now;

            ////int maxIter = totalFriendsCount/500 + (totalFriendsCount%500 != 0 ? 1 : 0);
            //var lstFacebookSessionMutualFriends = new List<MutualFriendItem>();
            //var qry0 = "SELECT uid2 FROM friend WHERE uid1=me()";
            //int limit = 600;
            //int step = 10;
            //for (int i = 1; i < 140; i += step)
            //{
            //    for (int offset = 0; offset < totalFriendsCount; offset += limit)
            //    {
            //        var qry1tmpl = "SELECT uid2 FROM friend WHERE uid1=me() limit {0} offset {1}";
            //        var qry1 = String.Format(qry1tmpl, limit, offset);
            //        var qry2tmpl = "select uid from user where mutual_friend_count >= {0} and mutual_friend_count < {1} and uid in " +
            //                       "( SELECT uid1 FROM friend WHERE uid1 IN ({2}) )";
            //        var qry2_a = String.Format(qry2tmpl, i, i+step, qry1);
            //        var qry2_b = "select uid from user where mutual_friend_count >= 1 and mutual_friend_count < 100 and uid in " +
            //                       "( SELECT uid2 FROM friend WHERE uid1=me() )";
            //        var qry3tmpl = "select uid1,uid2 from friend where uid1 in ({0}) and uid2 in ({1})";
            //        var qry3 = String.Format(qry3tmpl, qry2_a, qry2_b);
            //        JsonArray res = FacebookHelper.GetFqlResult(qry3);
            //        lstFacebookSessionMutualFriends =
            //            lstFacebookSessionMutualFriends.Union(
            //                res.Select(o => new MutualFriendItem
            //                                    {
            //                                        FriendID = long.Parse((string) ((JsonObject) o)["uid1"]),
            //                                        MutualFriendID = long.Parse((string) ((JsonObject) o)["uid2"])
            //                                    })).ToList();
            //    }
            //    step += 10;
            //    limit -= 100;
            //}
            //var qry00 = String.Format(qry0tmpl,1000,0);
            //var qry01 = String.Format(qry0tmpl,1000,1000);
            //var qry02 = String.Format(qry0tmpl,1000,2000);
            //var qry03 = String.Format(qry0tmpl,1000,3000);
            //var qry04 = String.Format(qry0tmpl,1000,4000);
            //var qry5000 = String.Format(qry0tmpl,5000,0);
            //var qry2tmpl = String.Format(
            //    "select uid,mutual_friend_count from user where mutual_friend_count > 0 and mutual_friend_count < 20 and uid in " +
            //           "( SELECT uid1 FROM friend WHERE uid1 IN ({0}) )");
            //var qry20 = String.Format(qry2tmpl, qry00);
            //var qry21 = String.Format(qry2tmpl, qry01);
            //var qry22 = String.Format(qry2tmpl, qry02);
            //var qry23 = String.Format(qry2tmpl, qry03);
            //var qry24 = String.Format(qry2tmpl, qry04);
            //var qry5 = String.Format("select uid1,uid2 from friend where uid1 in ({0}) and uid2 in ({1})", qry20, qry5000);
            //JsonArray res1 = FacebookHelper.GetFqlResult(qry5);
            //var qry6 = String.Format("select uid1,uid2 from friend where uid1 in ({0}) and uid2 in ({1})", qry21, qry5000);
            //JsonArray res2 = FacebookHelper.GetFqlResult(qry6);
            //var qry7 = String.Format("select uid1,uid2 from friend where uid1 in ({0}) and uid2 in ({1})", qry22, qry5000);
            //JsonArray res3 = FacebookHelper.GetFqlResult(qry7);
            //var qry8 = String.Format("select uid1,uid2 from friend where uid1 in ({0}) and uid2 in ({1})", qry23, qry5000);
            //JsonArray res4 = FacebookHelper.GetFqlResult(qry8);
            //var qry9 = String.Format("select uid1,uid2 from friend where uid1 in ({0}) and uid2 in ({1})", qry24, qry5000);
            //JsonArray res5 = FacebookHelper.GetFqlResult(qry9);
            //var qry10 = String.Format("select uid1,uid2 from friend where uid1 in ({0}) and uid2 in ({1})", qry1, qry0);
            //JsonArray res6 = FacebookHelper.GetFqlResult(qry10);
            //var qry11 = String.Format("select uid1,uid2 from friend where uid1 in ({0}) and uid2 in ({1})", qry1, qry1);
            //JsonArray res7 = FacebookHelper.GetFqlResult(qry11);

            //var qry3 = String.Format(
            //    "select uid from user where friend_count > 0 and friend_count < 1000 and uid in " +
            //           "(SELECT uid1 FROM friend " +
            //           " WHERE uid1 IN ({0}) " +
            //           " AND   uid2 IN ({0})" +
            //           ")", qry0);
            //var qry2000 = String.Format(
            //    "select uid from user where friend_count >= 1000 and friend_count < 2000 and uid in " +
            //           "(SELECT uid1 FROM friend " +
            //           " WHERE uid1 IN ({0}) " +
            //           " AND   uid2 IN ({0})" +
            //           ")", qry0);
            //var qry3000 = String.Format(
            //    "select uid from user where friend_count >= 2000 and friend_count < 3000 and uid in " +
            //           "(SELECT uid1 FROM friend " +
            //           " WHERE uid1 IN ({0}) " +
            //           " AND   uid2 IN ({0})" +
            //           ")", qry0);
            //var qry4000 = String.Format(
            //    "select uid from user where friend_count >= 3000 and friend_count < 4000 and uid in " +
            //           "(SELECT uid1 FROM friend " +
            //           " WHERE uid1 IN ({0}) " +
            //           " AND   uid2 IN ({0})" +
            //           ")", qry0);
            //var qry5000 = String.Format(
            //    "select uid from user where friend_count >= 4000 and friend_count < 6000 and uid in " +
            //           "(SELECT uid1 FROM friend " +
            //           " WHERE uid1 IN ({0}) " +
            //           " AND   uid2 IN ({0})" +
            //           ")", qry0);
            //var qry5 = String.Format("select uid1,uid2 from friend where uid1 in ({0}) and uid2 in ({1})", qry1000, qry0);
            //JsonArray res1 = FacebookHelper.GetFqlResult(qry5);
            //var qry6 = String.Format("select uid1,uid2 from friend where uid1 in ({0}) and uid2 in ({1})", qry2000, qry0);
            //JsonArray res2 = FacebookHelper.GetFqlResult(qry6);
            //var qry7 = String.Format("select uid1,uid2 from friend where uid1 in ({0}) and uid2 in ({1})", qry3000, qry0);
            //JsonArray res3 = FacebookHelper.GetFqlResult(qry7);
            //var qry8 = String.Format("select uid1,uid2 from friend where uid1 in ({0}) and uid2 in ({1})", qry4000, qry0);
            //JsonArray res4 = FacebookHelper.GetFqlResult(qry8);
            //var qry9 = String.Format("select uid1,uid2 from friend where uid1 in ({0}) and uid2 in ({1})", qry5000, qry0);
            //JsonArray res5 = FacebookHelper.GetFqlResult(qry9);
            //var qry10 = String.Format("select uid1,uid2 from friend where uid1 in ({0}) and uid2 in ({1})", qry1, qry0);
            //JsonArray res6 = FacebookHelper.GetFqlResult(qry10);
            //var qry11 = String.Format("select uid1,uid2 from friend where uid1 in ({0}) and uid2 in ({1})", qry1, qry1);
            //JsonArray res7 = FacebookHelper.GetFqlResult(qry11);
            //int count = res1.Count + res2.Count + res3.Count + res4.Count + res5.Count;
            //JsonArray friendsWithMutualFriends = FacebookHelper.GetFqlResult(qry1);

            //int itr = 0;
            //var qryTemplate = "SELECT uid1, uid2 FROM friend " +
            //                  " WHERE (uid1 IN (SELECT uid2 FROM friend WHERE uid1=me()) or uid1 IN (SELECT uid1 FROM friend WHERE uid2=me())" +
            //                  " AND   (uid2 IN (SELECT uid2 FROM friend WHERE uid1=me()) or uid2 IN (SELECT uid1 FROM friend WHERE uid2=me())";
            //                  //" limit 5000 offset {0} ";
            //var qry = String.Format(qryTemplate, 0);
            #endregion obsolete 1

            string[] fbFriendsIds =
                friendsData.Select(f => f.UserId.ToString())
                           .Take(300).ToArray();

            bool loadOnlyWithMutualFriends = true;
            #region obsolete 2
            //try
            //{
            //    var qry = "SELECT uid1, uid2 FROM friend " +
            //                " WHERE uid1 IN (SELECT uid2 FROM friend WHERE uid1=me()) " +
            //                " AND   uid2 IN (SELECT uid2 FROM friend WHERE uid1=me())";
            //    //JsonArray mutualFriendsByQuery = FacebookHelper.GetFqlResult(facebook, qry);

            //    //lstFacebookSessionMutualFriends = mutualFriendsByQuery
            //    //                                        .Select(o => new MutualFriendItem
            //    //                                        {
            //    //                                            FriendID = long.Parse((string)((JsonObject)o)["uid1"]),
            //    //                                            MutualFriendID = long.Parse((string)((JsonObject)o)["uid2"])
            //    //                                        })
            //    //                                    .ToList();
            //    FacebookHelper.GetAsyncFqlResult(facebook, qry, FetchFriends_FacebookResultCallback, null);
            //    while (mutualFriendsQueryCount.Counter > 0) { }
            //    loadOnlyWithMutualFriends = !Data.User.IsUsernameTaken(lstFacebookSessionMutualFriends.Last().FriendID.ToString());
            //}
            //catch (FacebookApiException exc)
            #endregion obsolete 2
            {
                var maxFriends = fbFriendsIds.Length;
                var limit = 200 / (2 + (friendsData.Count / 500));
                var qryTmpl = "SELECT uid1, uid2 FROM friend " +
                    //" WHERE uid1 IN (SELECT uid2 FROM friend WHERE uid1=me() limit {0} offset  {1} ) " +
                            " WHERE uid1 IN ( {0} ) " +
                            " AND   uid2 IN (SELECT uid2 FROM friend WHERE uid1=me())";
                for (int offset = 0; offset < maxFriends; offset += limit)
                {
                    var friendsList = fbFriendsIds.Skip(offset).Take(limit).ToList();
                    var friendsSeperated = String.Join(",", fbFriendsIds, offset, fbFriendsIds.Length - offset);
                    var qry = String.Format(qryTmpl, friendsSeperated);

                    lock (mutualFriendsQueryCount)
                    {
                        mutualFriendsQueryCount.Counter++;
                    }
                    FacebookHelper.GetAsyncFqlResult(_facebook, qry, FetchFriends_FacebookResultCallback, null);
                    #region obsolete 3
                    //JsonArray mutualFriendsByQuery = FacebookHelper.GetFqlResult(facebook, qry);

                    //lstFacebookSessionMutualFriends = lstFacebookSessionMutualFriends.Union(
                    //                                      mutualFriendsByQuery
                    //                                            .Select(o => new MutualFriendItem
                    //                                            {
                    //                                                FriendID = long.Parse((string)((JsonObject)o)["uid1"]),
                    //                                                MutualFriendID = long.Parse((string)((JsonObject)o)["uid2"])
                    //                                            })).ToList();
                    #endregion obsolete 3
                }
                timestamps.AddTimestamp("PageBase.UpdateFacebookFriendsdata start FacebookHelper.GetAsyncFqlResult");
                while (mutualFriendsQueryCount.Counter > 0) { Thread.Sleep(100); }
                timestamps.AddTimestamp("PageBase.UpdateFacebookFriendsdata end FacebookHelper.GetAsyncFqlResult");
                loadOnlyWithMutualFriends = !Data.User.IsUsernameTaken(lstFacebookSessionMutualFriends.Last().FriendID.ToString());
                #region obsolete 4
                //IDictionary<string, object> parameters = new Dictionary<string, object>();
                //Global.Logger.LogWarning(exc);
                //StatusPageMessage = exc.Message;
                //parameters["next"] = Config.Urls.Home + "/ShowStatus.aspx";
                //Response.Redirect("ShowStatus.aspx");
                //return;
                #endregion obsolete 4
            }
            #region obsolete 5
            //while (mutualFriendsByQuery.Count > 4999)
            //{
            //    itr++;
            //    qry = String.Format(qryTemplate, 5000*itr);
            //    mutualFriendsByQuery = FacebookHelper.GetFqlResult(qry);
            //    lstFacebookSessionMutualFriends =
            //        lstFacebookSessionMutualFriends
            //            .Union(mutualFriendsByQuery
            //                    .Select(o => new MutualFriendItem
            //                    {
            //                        FriendID = long.Parse((string)((JsonObject)o)["uid1"]),
            //                        MutualFriendID = long.Parse((string)((JsonObject)o)["uid2"])
            //                    }))
            //                    .ToList();
            //}
            #endregion obsolete 5
            var dic1 = _context.GetFacebookMutualFriendsDic(); //(Dictionary<long, List<MutualFriendItem>>)Application["FacebookMutualFriendsDic"];
            dic1[_id] = lstFacebookSessionMutualFriends;
            //FacebookHelper.AddFriends(CurrentUserSession, loadOnlyWithMutualFriends ? 0 : 40, HttpContext.Current, null);
            FacebookHelper.AddFriends(_pageBase.CurrentUserSession, -1, _context, null);
            timestamps.AddTimestamp("PageBase.UpdateFacebookFriendsdata after FacebookHelper.AddFriends");
            var dic2 = (Dictionary<string, LovehitchBackgroundWorker>)Global.AppBackgroundWorkersDic;
            if (_context.Session != null && _pageBase.CurrentUserSession.FacebookID.HasValue)
            {
                if (dic2 != null)
                {
                    //var facebook = CurrentUserSession.GetFacebookApp();
                    var bgWorker1 =
                        (AddNewFacebookFriendsBackgroundWorker)
                           dic2["ezFixUp.Data.AddNewFacebookFriendsBackgroundWorker"];

                    if (bgWorker1 != null && _facebook != null && _pageBase.CurrentUserSession.FacebookID.HasValue)
                    {
                        _facebook.Session.UserId = _pageBase.CurrentUserSession.FacebookID.Value;
                        lock (bgWorker1.FacebookAppList)
                        {
                            if (!bgWorker1.FacebookAppList.Contains(_facebook))
                                bgWorker1.FacebookAppList.Add(_facebook);
                        }
                    }
                }
            }
            timestamps.AddTimestamp("PageBase.UpdateFacebookFriendsdata after bgWorker1.FacebookAppList.Add(facebook)");
            //_pageBase.CurrentUserSession.ToUpdateFacebookFriendsData = false;
        }
        public void FetchFriends_FacebookResultCallback(FacebookAsyncResult asyncResult)
        {
            if (asyncResult.IsCompleted && asyncResult.Error == null && asyncResult.Result != null)
            {
                lock (lstFacebookSessionMutualFriends)
                {
                    lstFacebookSessionMutualFriends =
                        lstFacebookSessionMutualFriends.Union(
                            ((JsonArray)((JsonObject)asyncResult.Result)["data"])
                                .Select(o => new MutualFriendItem
                                {
                                    FriendID = long.Parse((string)((JsonObject)o)["uid1"]),
                                    MutualFriendID = long.Parse((string)((JsonObject)o)["uid2"])
                                })).ToList();
                }
            }

            lock (mutualFriendsQueryCount)
            {
                mutualFriendsQueryCount.Counter--;
            }

            //System.Diagnostics.Debug.WriteLine(asyncResult.Result);
        }
        
        public List<MutualFriendItem> lstFacebookSessionMutualFriends = new List<MutualFriendItem>();

        class MutualFriendsQueryCounterClass
        {
            public MutualFriendsQueryCounterClass()
            {
                Counter = 0;
            }
            public int Counter;
        }

        private MutualFriendsQueryCounterClass mutualFriendsQueryCount = new MutualFriendsQueryCounterClass();

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using AspNetDating.Classes;
using AspNetDating.Model;
using Photo = AspNetDating.Classes.Photo;


namespace AspNetDating.Handlers
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "https://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class RefreshSessionState : LoveHitchBaseHandler
    {
        public enum SessionRefreshStateEnum
        {
            AddFriendsWithMutualFriends = 0,
            CacheSmallImagesOfFriendsWithMutualFriends = 1,
            CacheNormalImagesOfFriendsWithMutualFriends = 2,
            LoadPhotosOfFriendsWithMutualFriends = 3,
            AddFriendsWithoutMutualFriends = 4,
            LoadPhotosOfFriendsWithoutMutualFriends = 5,
            Idle = 6
        }
        public override void ProcessRequest(HttpContext context)
        {
            CacheProcessRequest(context,0,0,0);
            // Set some value in session
            if ((int)context.Session["SessionTimeout"] > 4)
                context.Session["SessionTimeout"] = (int)context.Session["SessionTimeout"] - 1; 
            // Set session timeout to small value, in this case 
            // 2 minutes, to see quickly if Timer will keep session alive
            context.Session.Timeout = (int)context.Session["SessionTimeout"];
            return;

            if (!FacebookHelper.IsCurrentSessionConnected) return;

            if (context.Session["SessionRefreshState"] == null)
                context.Session["SessionRefreshState"] = SessionRefreshStateEnum.AddFriendsWithMutualFriends;
            DateTime[] time = new DateTime[3];
            time[0] = DateTime.Now;
            var refreshState = (SessionRefreshStateEnum) context.Session["SessionRefreshState"];
            switch (refreshState)
            {
                case SessionRefreshStateEnum.AddFriendsWithMutualFriends:
                    AddFriendsWithMutualFriends(context);
                    CacheImagesOfFriends(context, ImageHandler.eFacebookImageType.Square);
                    time[1] = DateTime.Now;
                    break;

                case SessionRefreshStateEnum.CacheSmallImagesOfFriendsWithMutualFriends:
                    CacheImagesOfFriends(context, ImageHandler.eFacebookImageType.Square);
                    CacheImagesOfFriends(context, ImageHandler.eFacebookImageType.Normal);
                    time[1] = DateTime.Now;
                    break;
                
                case SessionRefreshStateEnum.CacheNormalImagesOfFriendsWithMutualFriends:
                    CacheImagesOfFriends(context, ImageHandler.eFacebookImageType.Normal);
                    CacheImagesOfFriends(context, ImageHandler.eFacebookImageType.Square);
                    time[1] = DateTime.Now;
                    break;

                case SessionRefreshStateEnum.LoadPhotosOfFriendsWithMutualFriends:
                    LoadPhotosOfFriendsWithMutualFriends(context);
                    CacheLoadedImagesOfFriendsWithMutualFriends(context);
                    time[1] = DateTime.Now;
                    break;

                case SessionRefreshStateEnum.AddFriendsWithoutMutualFriends:
                    CacheLoadedImagesOfFriendsWithMutualFriends(context);
                    AddFriendsWithoutMutualFriends(context);
                    time[1] = DateTime.Now;
                    break;

                case SessionRefreshStateEnum.LoadPhotosOfFriendsWithoutMutualFriends:
                    LoadPhotosOfFriendsWithoutMutualFriends(context);
                    time[1] = DateTime.Now;
                    break;

                case SessionRefreshStateEnum.Idle:
                    return;
                    break;
            }
            context.Session["SessionRefreshState"] = (SessionRefreshStateEnum)(++refreshState);
        }
        private void CacheLoadedImagesOfFriendsWithMutualFriends(HttpContext context)
        {
            var friendsIDsWithMutualFriends =
                ((List<MutualFriendItem>) HttpContext.Current.GetSession()["FacebookMutualFriends"])
                    .Select(f => f.FriendID).Distinct().ToList();
            List<int> loadedPhotosIds = friendsIDsWithMutualFriends
                .Select(fbId => Photo.GetPrimaryOrDefaultId(fbId.ToString())).ToList();
            List<string> urlList = loadedPhotosIds
                .Where(id => id > 0)
                .Select(id => ImageHandler.CreateImageUrl(id, 117,117,false,true,true))
                .ToList();
            foreach (string url in urlList)
            {
                context.Response.Write(String.Format("<img src='{0}' style='display:none;'></img>", url));
            }
        }

        private void CacheImagesOfFriends(HttpContext context, ImageHandler.eFacebookImageType imgSize)
        {
            var friendsIDsWithMutualFriends =
                ((List<MutualFriendItem>)HttpContext.Current.GetSession()["FacebookMutualFriends"])
                    .Select(f => f.FriendID).Distinct().ToList();
            List<string> urlList = friendsIDsWithMutualFriends
                    .Select(fbId => ImageHandler.CreateFacebookImageUrl(fbId, imgSize))
                    .ToList();
            foreach (string url in urlList)
            {
                context.Response.Write(String.Format("<img src='{0}' style='display:none;'></img>", url));
            }
            //var lstImagesTags = new List<string>();
            //List<Friend> fbFriends;
            //using (var db = new AspNetDatingDataContext())
            //{

            //    fbFriends = CompiledQueries
            //        .FetchActiveFriendsForUsername(db, PageBase.GetCurrentUserSession().Username)
            //        .Where(u => u.f_is_facebook_friend.HasValue && u.f_is_facebook_friend.Value)
            //        .Skip(lastFriendsPage * 20).Take(40).ToList();
            //}
            //foreach (var fbFriend in fbFriends)
            //{
            //    if (!fbFriend.User1.u_facebookid.HasValue) continue;
            //    long fbId = fbFriend.User1.u_facebookid.Value;
            //    string url = ImageHandler.CreateFacebookImageUrl(fbId, imgSize);
            //    lstImagesTags.Add(String.Format("<img src='{0}' style='display:none;'></img>", url));
            //}
            //lastFriendsPage++;
            //context.Session["LastFriendsViewedPage"] = (lstImagesTags.Count > 0) ? lastFriendsPage : 0;
        }
        private void AddFriendsWithMutualFriends(HttpContext context)
        {
            var session = HttpContext.Current.GetSession();
            if (session == null) return;
            var lstMutualFriends = ((List<MutualFriendItem>) session["FacebookMutualFriends"]);
            if (lstMutualFriends == null) return;

            var friendsIDsWithMutualFriends = 
                lstMutualFriends.Select(f => f.FriendID).Distinct().ToList();

            if (friendsIDsWithMutualFriends != null)
            {
                if (friendsIDsWithMutualFriends.Count > 0)
                {
                    FacebookHelper.AddFriends(PageBase.GetCurrentUserSession(), 0, context, null);
                }
                else
                {
                    context.Session["SessionRefreshState"] =
                        SessionRefreshStateEnum.LoadPhotosOfFriendsWithMutualFriends;
                    LoadPhotosOfFriendsWithMutualFriends(context);
                }
            }
            else
            {
                context.Session["SessionRefreshState"] =
                        SessionRefreshStateEnum.AddFriendsWithoutMutualFriends;
                AddFriendsWithoutMutualFriends(context);
            }
        }
        private void AddFriendsWithoutMutualFriends(HttpContext context)
        {
            var friendsIDsWithMutualFriends =
                ((List<MutualFriendItem>)HttpContext.Current.GetSession()["FacebookMutualFriends"])
                    .Select(f => f.FriendID).Distinct().ToList();

            if (friendsIDsWithMutualFriends != null && Classes.User.IsUsernameTaken(friendsIDsWithMutualFriends.Last().ToString()))
            {
                FacebookHelper.AddFriends(PageBase.GetCurrentUserSession(), 10, context, null);
            }
            else
            {
                context.Session["SessionRefreshState"] = SessionRefreshStateEnum.LoadPhotosOfFriendsWithMutualFriends;
                LoadPhotosOfFriendsWithMutualFriends(context);
            }
        }
        private void LoadPhotosOfFriendsWithMutualFriends(HttpContext context)
        {
            CacheImagesOfFriends(context, ImageHandler.eFacebookImageType.Large);
            var friendsIDsWithMutualFriends =
                ((List<MutualFriendItem>)HttpContext.Current.GetSession()["FacebookMutualFriends"])
                    .Select(f => f.FriendID).Distinct().ToList();
            DateTime[] time = new DateTime[5];
            time[0] = DateTime.Now;
            if (friendsIDsWithMutualFriends != null && Classes.User.IsUsernameTaken(friendsIDsWithMutualFriends.Last().ToString()))
            {
                var lstFriendsIds = new List<long>();
                try
                {
                    // fetch facebook friends without images
                    using (var db = new AspNetDatingDataContext())
                    {
                        var tmpList =
                            CompiledQueries.FetchActiveFriendsForUsername(db, MatchmakerHelper.CurrentUsername).ToList();
                        var tmpArray =
                            tmpList.Where(
                                f =>
                                f.User1.u_logincount == 0 && f.User1.u_facebookid.HasValue &&
                                !f.User1.u_face_control_approved)
                                .OrderBy(f => f.User1.u_name).Select(f => f.User1.u_facebookid.Value).ToArray();

                        foreach (var un in tmpArray)
                        {
                            int imgId = Photo.GetPrimaryOrDefaultId(un.ToString());
                            if (imgId < 0)
                                lstFriendsIds.Add(un);
                            if (lstFriendsIds.Count > 60) break;
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
                            new Classes.LoveHitchParallel.ParameterizedThreadsList<long>(
                                lstFriendsIds,
                                (new FacebookHelper.FetchAndSaveSaveFacebookFriendImageClass()).
                                    FetchAndSaveFacebookImageByFacebookId,
                                context);
                        parameterizedThreadsList.RunAsBackgroundThreads = true;
                        parameterizedThreadsList.ExecuteParallelWork(10);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                else
                {
                    context.Session["SessionRefreshState"] = SessionRefreshStateEnum.AddFriendsWithoutMutualFriends;
                    FacebookHelper.AddFriends(PageBase.GetCurrentUserSession(), 100, context, null);
                }
            }
            else
            {
                context.Session["SessionRefreshState"] = SessionRefreshStateEnum.AddFriendsWithMutualFriends;
                FacebookHelper.AddFriends(PageBase.GetCurrentUserSession(), 0, context, null);
            }
        }
        private void LoadPhotosOfFriendsWithoutMutualFriends(HttpContext context)
        {
            IList<FacebookHelper.FacebookFriendData> friendsData = FacebookHelper.GetFriendsDataList(context);
            if (Classes.User.IsUsernameTaken(friendsData.Last().Username))
            {
                var lstFriendsIds = new List<long>();
                try
                {
                    // fetch facebook friends without images
                    using (var db = new AspNetDatingDataContext())
                    {
                        var tmpList =
                            CompiledQueries.FetchActiveFriendsForUsername(db, MatchmakerHelper.CurrentUsername).ToList();
                        var tmpArray =
                            tmpList.Where(
                                f =>
                                f.User1.u_logincount == 0 && f.User1.u_facebookid.HasValue &&
                                !f.User1.u_face_control_approved)
                                .OrderBy(f => f.User1.u_name).Select(f => f.User1.u_facebookid.Value).ToArray();
                        foreach (var un in tmpArray)
                        {
                            int imgId = Photo.GetPrimaryOrDefaultId(un.ToString());
                            if (imgId < 0)
                                lstFriendsIds.Add(un);
                            if (lstFriendsIds.Count > 100) break;
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                if (!FacebookHelper.IsCurrentSessionConnected || lstFriendsIds.Count <= 0)
                    return;

                try
                {
                    var parameterizedThreadsList =
                        new Classes.LoveHitchParallel.ParameterizedThreadsList<long>(
                            lstFriendsIds,
                            (new FacebookHelper.FetchAndSaveSaveFacebookFriendImageClass()).FetchAndSaveFacebookImageByFacebookId,
                            context);
                    parameterizedThreadsList.RunAsBackgroundThreads = true;
                    parameterizedThreadsList.ExecuteParallelWork(10);
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                context.Session["SessionRefreshState"] = SessionRefreshStateEnum.AddFriendsWithoutMutualFriends;
            }
        }
    }
}

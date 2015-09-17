using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ezFixUp.Services;

namespace ezFixUp.Classes
{
    public class UsersRelationsState
    {
        public string CurrentUsername { get; set; }
        public string QryUsername { get; set; }
        public string QryUserDisplayName { get; set; }
        public bool IsFriend { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsBlocked { get; set; }
        public bool InFriendshipRequest { get; set; }
        public bool InRelationshipRequest { get; set; }
        public IList<SearchService.MutualFriendsClass> MutualFriends { get; set; }

        public static IList<SearchService.MutualFriendsClass> GetMutualFriendsList(string viewer, string viewed, ImageHandler.eFacebookImageType fbImageSize)
        {
            if (HttpContext.Current.Session == null)
            {
                
            }
            var dic = (Dictionary<string, List<string>>)Global.GetSessionState()["MatchmakingRequestedDic"];
            if (dic == null)
            {
                
            }
            if (!dic.ContainsKey(viewed))
                dic.Add(viewed, new List<string>());
            var reqList = (List<string>)dic[viewed];
            DateTime[] t = new DateTime[5];
            t[0] = DateTime.Now;
            var searchAppMF = new MutualFriendsSearch { Viewer = viewer, Viewed = viewed };
            var searchAppResults = searchAppMF.GetResults();
            User[] appMF = (searchAppResults != null) ? searchAppResults.Get() : new User[] { };
            t[1] = DateTime.Now;
            IList<SearchService.MutualFriendsClass> mfList = null;
            //using (var db = new ezFixUpDataContext())
            {
                mfList = appMF.Select(user =>
                                 new SearchService.MutualFriendsClass
                                     {
                                         Name = UserSession.GetViewedUserDisplayedNameByUsername(viewer, user.Username),
                                         Username = user.Username,
                                         PhotoId = Photo.GetPrimaryOrDefaultId(user.Username),
                                         PhotoUrl = Photo.GetPrimaryOrDefaultId(user.Username) == 0 && user.FacebookID.HasValue
                                             ? ImageHandler.CreateFacebookImageUrl(user.FacebookID.Value,ImageHandler.eFacebookImageType.Square)
                                             : ImageHandler.CreateImageUrl(Photo.GetPrimaryOrDefaultId(user.Username),
                                                                         50, 50, false, true, true),
                                         AlreadyRequested = reqList.Contains(user.Username)
                                     }).ToList();
            }
            t[2] = DateTime.Now;
            if (FacebookHelper.IsCurrentSessionConnected)
            {
                var searchFacebookMF = new FacebookMutualFriendsSearch(viewer, viewed, PageBase.GetCurrentUserSession(),
                                                                       FacebookHelper.GetSessionFacebookApp());
                var searchFacebookResults = searchFacebookMF.GetResults();
                User[] fbMF = (searchFacebookResults != null) ? searchFacebookResults.Get() : new User[] { };
                t[3] = DateTime.Now;
                //using (var db = new ezFixUpDataContext())
                {
                    IList<SearchService.MutualFriendsClass> lstFacebookMF =
                        fbMF.Where(user => mfList.All(u => u.Username != user.Username))
                            .Select(user =>
                                    new SearchService.MutualFriendsClass
                                        {
                                            Name = UserSession.GetViewedUserDisplayedNameByUsername(viewer, user.Username),
                                            Username = user.Username,
                                            PhotoId = 0,
                                            PhotoUrl = ImageHandler.CreateFacebookImageUrl(user.FacebookID.Value, fbImageSize),
                                                //String.Format("https://graph.facebook.com/{0}/picture?type=square",user.FacebookID),
                                            AlreadyRequested = reqList.Contains(user.Username)
                                        }).ToList();
                    if (lstFacebookMF != null && lstFacebookMF.Count > 0)
                    {
                        mfList = mfList.Union(lstFacebookMF).ToList();
                    }
                }
                t[4] = DateTime.Now;
            }
            return mfList;

            //List<SearchService.MutualFriendsClass> lstFacebookMF = searchFacebookMF.GetSavedResults();
            //if (lstFacebookMF != null && lstFacebookMF.Count > 0)
            //{
            //    lstFacebookMF = lstFacebookMF.Select(f => new SearchService.MutualFriendsClass
            //                                                {
            //                                                    Username = f.Username,
            //                                                    PhotoId = f.PhotoId,
            //                                                    PhotoUrl = f.PhotoUrl,
            //                                                    AlreadyRequested = reqList.Contains(f.Username)
            //                                                }).ToList();
            //    mfList = lstAppMF.Concat(lstFacebookMF).ToList();
            //}
            //return mfList;
        }
        public static UsersRelationsState GetRelationState(string viewer, string viewed)
        {
            var mfList = GetMutualFriendsList(viewer, viewed, ImageHandler.eFacebookImageType.Square);

            User currentUser = PageBase.GetCurrentUserSession();//User.Load(viewer);
            if (currentUser == null) return null;

            UsersRelationsState relationState = new UsersRelationsState
            {
                CurrentUsername = viewer,
                QryUsername = viewed,
                IsFriend = currentUser.IsUserInFriendList(viewed),
                IsBlocked = currentUser.IsUserBlocked(viewed),
                IsFavorite = currentUser.IsUserInFavouriteList(viewed),
                InFriendshipRequest = (User.FetchOpenFriendshipRequestsToUsernme(viewer)
                                           .Where(r => r == viewed)
                                           .FirstOrDefault() != null),
                MutualFriends = mfList
            };
            //using (var db = new ezFixUpDataContext())
            {
                string displayName = UserSession.GetViewedUserDisplayedNameByUsername(viewer, viewed);
                relationState.QryUserDisplayName = displayName;
            }
            return relationState;
        }
    }
}

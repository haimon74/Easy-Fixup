using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using ezFixUp.Classes;
using ezFixUp.Services;
using Newtonsoft.Json;

namespace ezFixUp.Handlers
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "https://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GetJsonMutualFriends : ezFixUpBaseHandler
    {
        public class UserRelationState
        {
            public string CurrentUsername { get; set; }
            public string QryUsername { get; set; }
            public bool IsFriend { get; set; }
            public bool IsFavorite { get; set; }
            public bool IsBlocked { get; set; }
            public bool InFriendshipRequest { get; set; }
            public bool InRelationshipRequest { get; set; }
            public IList<SearchService.MutualFriendsClass> MutualFriends { get; set; }
        }

        public override void ProcessRequest(HttpContext context)
        {
            string viewer = context.Request["viewer"];
            string viewed = context.Request["viewed"];
            //var dic = (Dictionary<string, List<string>>)Global.GetSessionState()["MatchmakingRequestedDic"];
            //if (!dic.ContainsKey(viewed)) 
            //    dic.Add(viewed, new List<string>());
            //var reqList = (List<string>)dic[viewed];
            //var searchAppMF = new MutualFriendsSearch{ Viewer = viewer, Viewed = viewed };
            //var searchAppResults = searchAppMF.GetResults();
            //User[] appMF = (searchAppResults != null) ? searchAppResults.Get() : new User[] { };
            //IList<SearchService.MutualFriendsClass> lstAppMF =
            //    appMF.Select(user =>
            //        new SearchService.MutualFriendsClass
            //            {
            //                Username = user.Username, 
            //                PhotoId = Photo.GetPrimaryOrDefaultId(user.Username),
            //                PhotoUrl = ImageHandler.CreateImageUrl(Photo.GetPrimaryOrDefaultId(user.Username), 
            //                                                       50, 50, false, true, true),
            //                AlreadyRequested = reqList.Contains(user.Username)
            //            }).ToList();
            //var searchFacebookMF = new FacebookMutualFriendsSearch{ Viewer = viewer, Viewed = viewed };
            //var searchFacebookResults = searchFacebookMF.GetResults();
            //User[] fbMF = (searchFacebookResults != null) ? searchFacebookResults.Get() : new User[] { };
            //IList<SearchService.MutualFriendsClass> lstFacebookMF =
            //    fbMF.Select(user =>
            //        new SearchService.MutualFriendsClass
            //            {
            //                Username = user.Username, 
            //                PhotoId = 0,
            //                PhotoUrl = String.Format("https://graph.facebook.com/{0}/picture?type=square", user.FacebookID),
            //                AlreadyRequested = reqList.Contains(user.Username)
            //            }).ToList();
            //var mfList = (lstFacebookMF != null && lstFacebookMF.Count > 0) 
            //    ? lstAppMF.Concat(lstFacebookMF).ToList() : lstAppMF;

            //User currentUser = PageBase.GetCurrentUserSession();//User.Load(viewer);
            //UserRelationState relationState = new UserRelationState
            //{
            //    CurrentUsername = viewer,
            //    QryUsername = viewed,
            //    IsFriend = currentUser.IsUserInFriendList(viewed),
            //    IsBlocked = currentUser.IsUserBlocked(viewed),
            //    IsFavorite = currentUser.IsUserInFavouriteList(viewed),
            //    InFriendshipRequest = (User.FetchOpenFriendshipRequestsToUsernme(viewer)
            //                               .Where(r => r == viewed)
            //                               .FirstOrDefault() != null),
            //    MutualFriends = mfList
            //};
            var relationState = UsersRelationsState.GetRelationState(viewer, viewed);
            string retval = JsonConvert.SerializeObject(relationState);
            context.Response.ContentType = "application/json";
            //context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.Write(retval);
        }

        public override bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

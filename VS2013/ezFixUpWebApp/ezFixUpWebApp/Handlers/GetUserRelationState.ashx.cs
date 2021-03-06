﻿using System;
using System.Linq;
using System.Web;
using System.Web.Services;
using ezFixUp.Classes;
using Newtonsoft.Json;

namespace ezFixUp.Handlers
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
    }
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "https://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GetUserRelationState : ezFixUpBaseHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
            //string qry = context.Request["qry"];
            string username1 = context.Request["un1"];
            string username2 = context.Request["un2"];
            if (string.IsNullOrEmpty(username1) || string.IsNullOrEmpty(username2))
                return;

            User currentUser = User.Load(username1);
            User qryAboutUser = User.Load(username2);
            if (currentUser == null || qryAboutUser == null) return;

            var relationState = new UserRelationState
            {
                CurrentUsername = username1,
                QryUsername = username2,
                IsFriend = currentUser.IsUserInFriendList(username2),
                IsBlocked = currentUser.IsUserBlocked(username2),
                IsFavorite = currentUser.IsUserInFavouriteList(username2),
                InFriendshipRequest = (User.FetchOpenFriendshipRequestsToUsernme(username1)
                                           .FirstOrDefault(r => r == username2) != null)
            };
            string retval = JsonConvert.SerializeObject(relationState);
            context.Response.ContentType = "application/json";
            context.Response.Write(retval);
        }

        public override bool IsReusable
        {
            get
            {
                return false;
            }
        }
        /*public class GetUserRelationState : ezFixUpBaseAsyncHandler
    {
        private HttpContext _context;
        public override void EndProcessRequest(IAsyncResult result)
        {
            var context = ItsAsynchOperation.Context;
            base.EndProcessRequest(result);
            //string qry = context.Request["qry"];
            string username1 = context.Request["un1"];
            string username2 = context.Request["un2"];
            if (string.IsNullOrEmpty(username1) || string.IsNullOrEmpty(username2)) 
                return;
            
            User currentUser = User.Load(username1);
            User qryAboutUser = User.Load(username2);
            if (currentUser == null || qryAboutUser == null) return;
            
            UserRelationState relationState = new UserRelationState
                                                  {
                                                      CurrentUsername = username1,
                                                      QryUsername = username2,
                                                      IsFriend = currentUser.IsUserInFriendList(username2),
                                                      IsBlocked = currentUser.IsUserBlocked(username2),
                                                      IsFavorite = currentUser.IsUserInFavouriteList(username2),
                                                      InFriendshipRequest = (User.FetchOpenFriendshipRequestsToUsernme(username1)
                                                                                 .Where(r => r == username2)
                                                                                 .FirstOrDefault() != null)
                                                  };
            string retval = JsonConvert.SerializeObject(relationState);
            context.Response.ContentType = "application/json";
            context.Response.Write(retval);
        }

        public override bool IsReusable
        {
            get
            {
                return false;
            }
        }*/
    }
}

/* ASPnetDating 
 * Copyright (C) 2003-2010 eStream 
 * ezFixUp.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at ezFixUp.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Data;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using ezFixUp.Classes;

namespace ezFixUp.Components.WebParts
{
    [Themeable(true), Editable]
    public partial class MatchmakingInProcessWebPart : WebPartUserControl
    {
        //[Personalizable(PersonalizationScope.User), WebBrowsable(true), WebDisplayName("RepeatColumns")]
        public int RepeatColumns { get; set; }
        
        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Number of rows")]
        public int LimitRows
        {
            get
            {
                return (int)(ViewState["MatchmakingInProcess_LimitRows"] ?? Config.Photos.MaxRowsPhotosOnMatchMakerPage);
            }
            set
            {
                ViewState["MatchmakingInProcess_LimitRows"] = value < 10 ? value : 10;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = "Matchmaking In Process".Translate();
        }

        //protected void Page_PreRenderComplete(object sender, EventArgs e)
        protected override void OnPreRender(EventArgs e)
        {
            dlMatchmakings.RepeatColumns = RepeatColumns;
        //    base.OnPreRender(e);

            #region Load New Users (thumbnails)
            UserSession matchmaker = PageBase.GetCurrentUserSession();
            DataTable dtMatchmaking;

            string cacheKey = String.Format("MatchmakingInProcessWebPart_{0}_{1}", matchmaker.Username, LimitRows);
            if (Cache[cacheKey] != null && Cache[cacheKey] is DataTable && Global.GetSessionState()["theme"] == null)
            {
                dtMatchmaking = (DataTable)Cache[cacheKey];
            }
            else
            {
                var mmSearch = new MatchmakingSearch{MatchmakerUsername = matchmaker.Username};
                UserSearchResults mmResults = mmSearch.GetResults();
                dtMatchmaking = new DataTable(); 
                dtMatchmaking.Columns.Add("MatchToUsername");
                dtMatchmaking.Columns.Add("MatchToImageId", typeof(int));
                dtMatchmaking.Columns.Add("MatchToAge");
                dtMatchmaking.Columns.Add("MatchToGender");
                dtMatchmaking.Columns.Add("MatchToInterestedIn");
                dtMatchmaking.Columns.Add("MatchToCountry");
                dtMatchmaking.Columns.Add("MatchToCity");
                dtMatchmaking.Columns.Add("MatchWithUsername");
                dtMatchmaking.Columns.Add("MatchWithImageId", typeof(int));
                dtMatchmaking.Columns.Add("MatchWithAge");
                dtMatchmaking.Columns.Add("MatchWithGender");
                dtMatchmaking.Columns.Add("MatchWithInterestedIn");
                dtMatchmaking.Columns.Add("MatchWithCountry");
                dtMatchmaking.Columns.Add("MatchWithCity");
                if (mmResults != null && mmResults.Usernames != null)
                {
                    for (int index = 0; index < mmResults.Usernames.Length; )
                    {
                        UserInfoForMatchmaker info1;
                        UserInfoForMatchmaker info2;
                        string username1 = mmResults.Usernames[index++];
                        string username2 = mmResults.Usernames[index++];
                        GetUserInfoByUsername(username1, out info1);
                        GetUserInfoByUsername(username2, out info2);
                        
                        if (info1 == null || info2 == null)
                            continue;

                        string user1Country = (info1.TheUser != null) ? info1.TheUser.Country : String.Empty;
                        string user1City = (info1.TheUser != null) ? info1.TheUser.City : String.Empty;
                        string user2Country = (info2.TheUser != null) ? info2.TheUser.Country : String.Empty;
                        string user2City = (info2.TheUser != null) ? info2.TheUser.City : String.Empty;
                        dtMatchmaking.Rows.Add(
                            new object[]
                                {
                                    username1, info1.ImageId, info1.Age, info1.Gender, info1.InterestedIn, 
                                    user1Country, user1City,
                                    username2, info2.ImageId, info2.Age, info2.Gender, info2.InterestedIn, 
                                    user2Country, user2City,
                                });
                    }

                    Cache.Add(cacheKey, dtMatchmaking, null, DateTime.Now.Add(TimeSpan.FromMinutes(1)),
                              Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
                }
            }

            dlMatchmakings.DataSource = dtMatchmaking;
            dlMatchmakings.DataBind();

            base.OnPreRender(e);
            
            #endregion
        }
        protected class UserInfoForMatchmaker
        {
            public int ImageId;
            public string Age;
            public string Gender;
            public string InterestedIn;
            public User TheUser;
        }
        protected void GetUserInfoByUsername(string username, out UserInfoForMatchmaker userInfoForMatchmaker)
        {
            int imageId = 0;
            User user = null;

            try
            {
                user = User.Load(username);
            }
            catch (NotFoundException)
            {
                userInfoForMatchmaker = null;
                return;
            }
            if (user == null)
            {
                userInfoForMatchmaker = null;
                return;
            }
            userInfoForMatchmaker = new UserInfoForMatchmaker
                                        {
                                            ImageId = Photo.GetTopOrPrimaryOrDefaultId(username),
                                            Age = !Config.Users.DisableAgeInformation 
                                                        ? user.Age.ToString() 
                                                        : String.Empty,
                                            Gender = !Config.Users.DisableGenderInformation
                                                            ? user.Gender.ToString()
                                                            : String.Empty,
                                            InterestedIn = !Config.Users.DisableGenderInformation
                                                               ? user.InterestedIn.ToString()
                                                               : String.Empty
                                        };

        }

        protected void dlGenderFriends_ItemCreated(object sender, DataListItemEventArgs e)
        {
            if (Config.Users.DisableAgeInformation && Config.Users.DisableGenderInformation)
            {
                HtmlGenericControl pnlMatchToGenderAge = (HtmlGenericControl)e.Item.FindControl("pnlMatchToGenderAge");
                pnlMatchToGenderAge.Visible = false;
                HtmlGenericControl pnlMatchWithGenderAge = (HtmlGenericControl)e.Item.FindControl("pnlMatchWithGenderAge");
                pnlMatchWithGenderAge.Visible = false;
            }
            else if (Config.Users.DisableAgeInformation || Config.Users.DisableGenderInformation)
            {
                HtmlGenericControl pnlMatchToDelimiter = (HtmlGenericControl)e.Item.FindControl("pnlMatchToDelimiter");
                pnlMatchToDelimiter.Visible = false;
                HtmlGenericControl pnlMatchWithDelimiter = (HtmlGenericControl)e.Item.FindControl("pnlMatchWithDelimiter");
                pnlMatchWithDelimiter.Visible = false;
            }
            var lbMatchToMsg = (LinkButton)e.Item.FindControl("lbMatchToMsg");
            lbMatchToMsg.PostBackUrl = Config.Urls.Home + "/SendMessage_to_" +
                    DataBinder.Eval(e.Item.DataItem, "MatchToUsername") + ".aspx";
            var lbMatchWithMsg = (LinkButton)e.Item.FindControl("lbMatchWithMsg");
            lbMatchWithMsg.PostBackUrl = Config.Urls.Home + "/SendMessage_to_" +
                    DataBinder.Eval(e.Item.DataItem, "MatchWithUsername") + ".aspx";
            var lbSendBothMsg = (LinkButton)e.Item.FindControl("lbSendBothMsg");
            lbSendBothMsg.PostBackUrl = Config.Urls.Home + 
                String.Format("/Matchmaking_{0}_With_{1}.aspx",
                              DataBinder.Eval(e.Item.DataItem, "MatchToUsername"),
                              DataBinder.Eval(e.Item.DataItem, "MatchWithUsername"));
        }
        protected override void OnUnload(EventArgs e)
        {
            //HttpContext.Current.Response.Flush();
            base.OnUnload(e);
        }
    }
}

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
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ezFixUp.Classes;

namespace ezFixUp.Components.WebParts
{
    //[Themeable(true), Editable]
    public partial class GenderFriendsWebPart : WebPartUserControl
    {
        private DataList _dlGenderFriends = new DataList();
        protected DataList DataListGenderFriends
        {
            get { return _dlGenderFriends; }
            set { _dlGenderFriends = value; }
        }

        //[Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Gender"), GenderProperty]
        public User.eGenderSearch SearchGender
        {
            get
            {
                UserSession user = PageBase.GetCurrentUserSession();

                if (user != null)
                {
                    if (ViewState["SearchGender"] == null)
                    {
                        if (Config.Users.DisableGenderInformation)
                            return User.eGenderSearch.All;
                        if (Config.Users.InterestedInFieldEnabled)
                        {
                            return (User.eGenderSearch)user.InterestedIn;
                        }
                        switch (user.Gender)
                        {
                            case User.eGender.Male:
                                return (User.eGenderSearch)User.eGender.Female;
                            case User.eGender.Female:
                                return (User.eGenderSearch)User.eGender.Male;
                            case User.eGender.Couple:
                                return User.eGenderSearch.All;
                        }
                        return User.eGenderSearch.All;
                    }
                    return (User.eGenderSearch)ViewState["SearchGender"];
                }
                return User.eGenderSearch.All;
            }
            set
            {
                ViewState["SearchGender"] = value;
            }
        }

        //[Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Minimum Age"), AgeProperty]
        public int MinAge
        {
            get
            {
                UserSession user = PageBase.GetCurrentUserSession();
                if (user != null)
                {
                    if (ViewState["MatchFriends_MinAge"] == null)
                        return Math.Max(user.IncomingMessagesRestrictions.AgeFrom, Config.Users.MinAge);

                    return (int)(ViewState["MatchFriends_MinAge"] ?? Config.Users.MinAge);
                }
                else return Config.Users.MinAge;
            }
            set
            {
                ViewState["MatchFriends_MinAge"] = value;
            }
        }

        //[Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Maximum Age"), AgeProperty]
        public int MaxAge
        {
            get
            {
                UserSession user = PageBase.GetCurrentUserSession();

                if (user != null)
                {
                    if (ViewState["MatchFriends_MaxAge"] == null)
                        return Math.Min(user.IncomingMessagesRestrictions.AgeTo, Config.Users.MaxAge);

                    return (int)(ViewState["MatchFriends_MaxAge"] ?? Config.Users.MaxAge);
                }
                else return Config.Users.MinAge;
            }
            set
            {
                ViewState["MatchFriends_MaxAge"] = value;
            }
        }

        // //[Personalizable(PersonalizationScope.User), WebBrowsable(true), WebDisplayName("RepeatColumns")]
        public int RepeatColumns { get; set; }
        
        //[Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Number of rows")]
        public int LimitRows
        {
            get
            {
                return (int)(ViewState["MatchFriends_LimitRows"] ?? Config.Photos.MaxRowsPhotosOnMatchMakerPage);
            }
            set
            {
                ViewState["MatchFriends_LimitRows"] = value < 10 ? value : 10;
            }
        }

        protected virtual void Page_Load(object sender, EventArgs e)
        {
            //Page.PreRenderComplete += Page_PreRenderComplete;
        }
        //protected void Page_PreRenderComplete(object sender, EventArgs e)
        protected override void OnPreRender(EventArgs e)
        {
            DataListGenderFriends.RepeatColumns = RepeatColumns;
        //}

        //protected override void OnPreRender(EventArgs e)
        //{
        //    base.OnPreRender(e);

            #region Load New Users (thumbnails)
            UserSession matchmaker = PageBase.GetCurrentUserSession();
            DataTable dtGenderFriends;

            string cacheKey = String.Format("GenderFriendsWebPart_{0}_{1}_{2}_{3}_{4}", matchmaker.Username,
                SearchGender == User.eGenderSearch.All ? "null" : SearchGender.ToString(), MinAge, MaxAge, LimitRows);
            if (Cache[cacheKey] != null && Cache[cacheKey] is DataTable && Global.GetSessionState()["theme"] == null)
            {
                dtGenderFriends = (DataTable)Cache[cacheKey];
            }
            else
            {
                var genderFriendsSearch = new QuickSingleFriendsSearch
                                   {
                                       UsersCount = RepeatColumns * LimitRows,
                                       PageNumber = 0,
                                       MinAge = MinAge,
                                       MaxAge = MaxAge,
                                       MatchmakerUsername = matchmaker.Username
                                   };
                if (SearchGender != User.eGenderSearch.All)
                    genderFriendsSearch.FriendGender = SearchGender;
                UserSearchResults friendsResults = genderFriendsSearch.GetResults();
                dtGenderFriends = new DataTable();
                dtGenderFriends.Columns.Add("Username");
                dtGenderFriends.Columns.Add("ImageId", typeof(int));
                dtGenderFriends.Columns.Add("Age");
                dtGenderFriends.Columns.Add("Gender");
                dtGenderFriends.Columns.Add("InterestedIn");
                dtGenderFriends.Columns.Add("Country");
                dtGenderFriends.Columns.Add("City");
                
                if (friendsResults != null && friendsResults.Usernames != null)
                {
                    foreach (string username in friendsResults.Usernames)
                    {
                        int imageId = 0;
                        User user = null;

                        try { user = User.Load(username); }
                        catch (NotFoundException){continue;}

                        imageId = Photo.GetTopOrPrimaryOrDefaultId(username);

                        string age = !Config.Users.DisableAgeInformation ? user.Age.ToString() : String.Empty;
                        string gender = !Config.Users.DisableGenderInformation ? user.Gender.ToString() : String.Empty;
                        string interestedIn = !Config.Users.DisableGenderInformation ? user.InterestedIn.ToString() : String.Empty;
                        dtGenderFriends.Rows.Add(
                            new object[]
                                {
                                    username, imageId, age, gender, interestedIn, user.Country, user.City
                                });
                    }

                    Cache.Add(cacheKey, dtGenderFriends, null, DateTime.Now.Add(TimeSpan.FromMinutes(10)),
                              Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
                }
            }

            DataListGenderFriends.DataSource = dtGenderFriends;
            DataListGenderFriends.DataBind();
            //base.Page_PreRenderComplete(sender, e);
            base.OnPreRender(e);
            #endregion
        }

        protected void dlGenderFriends_ItemCreated(object sender, DataListItemEventArgs e)
        {
            if (Config.Users.DisableAgeInformation && Config.Users.DisableGenderInformation)
            {
                HtmlGenericControl pnlGenderAge = (HtmlGenericControl)e.Item.FindControl("pnlGenderAge");
                pnlGenderAge.Visible = false;
            }
            else if (Config.Users.DisableAgeInformation || Config.Users.DisableGenderInformation)
            {
                HtmlGenericControl pnlDelimiter = (HtmlGenericControl)e.Item.FindControl("pnlDelimiter");
                pnlDelimiter.Visible = false;
            }
        }

        protected void dlGenderFriends_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            string username = (string)DataBinder.Eval(e.Item.DataItem, "Username");
            var lbWink = (LinkButton)e.Item.FindControl("lbWink");
            lbWink.PostBackUrl = String.Format("~/SendWink_to_{0}.aspx", username);
            var lbMsg = (LinkButton)e.Item.FindControl("lbMsg");
            lbMsg.PostBackUrl = String.Format("~/SendMessage_to_{0}.aspx", username);
            var lbFavorite = (LinkButton)e.Item.FindControl("lbFavorite");
            lbFavorite.PostBackUrl = String.Format("~/AddRemoveFavorite.aspx?cmd=add&src=profile&uid={0}", username);
            var lbMatch = (LinkButton)e.Item.FindControl("lbMatch");
            lbMatch.Attributes.Add("image_id", DataBinder.Eval(e.Item.DataItem, "ImageId").ToString());
            lbMatch.Attributes.Add("username", username);
            var lbEnvelope = (LinkButton)e.Item.FindControl("lbSendMsg");
            lbEnvelope.PostBackUrl = "~/SendMessage_to_" + username + ".aspx";
            //lbMatch.Click += lbMatch_Clicked;
            //lbMatch.CommandArgument = username;
            //var trigger = new AsyncPostBackTrigger
            //                  {
            //                      ControlID = lbMatch.UniqueID,
            //                      EventName = "Click"
            //                  };
            //ItsUpdatePanel.Triggers.Add(trigger);
            //if (Page.Master != null) ((Site)Page.Master).ScriptManagerMaster.RegisterAsyncPostBackControl(lbMatch);
        }
    }
}
/* ASPnetDating 
 * Copyright (C) 2003-2010 eStream 
 * lovehitch.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at lovehitch.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Data;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;
using AspNetDating.Components.Buttons;

namespace AspNetDating.Components
{
    [Themeable(true)]
    public partial class NewUsers : UserControl
    {
        private User.eGender gender;
        private bool genderSearch;

        public User.eGender Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                genderSearch = true;
            }
        }

        public int MinAge
        {
            get
            {
                return (int) (ViewState["NewUsers_MinAge"] ?? Config.Users.MinAge);
            }
            set
            {
                ViewState["NewUsers_MinAge"] = value;
            }
        }

        public int MaxAge
        {
            get
            {
                return (int)(ViewState["NewUsers_MaxAge"] ?? Config.Users.MaxAge);
            }
            set
            {
                ViewState["NewUsers_MaxAge"] = value;
            }
        }

        public int RepeatColumns
        {
            get
            {
                return dlNewMembers.RepeatColumns;
            }
            set
            {
                dlNewMembers.RepeatColumns = value;
            }
        }

        public int LimitRows
        {
            get
            {
                return (int)(ViewState["NewUsers_LimitRows"] ?? Config.Photos.MaxRowsPhotosOnMainPage);
            }
            set
            {
                ViewState["NewUsers_LimitRows"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblNewUsersBoxHeader.Text = Config.Users.ShowFeaturedMemberOnHomePage
                               ? "featured members".Translate()
                               : "new members".Translate();
            lblNewUsersBoxTitle.Text = Config.Users.ShowFeaturedMemberOnHomePage
                                ? String.Format("Featured members on {0}".Translate(), Config.Misc.SiteTitle)
                                : String.Format("New members on {0}".Translate(), Config.Misc.SiteTitle);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            
            #region Load New Users (thumbnails)

            DataTable dtNewUsers;

            string cacheKey = String.Format("NewUsers.ascx_{0}_{1}_{2}_{3}", 
                genderSearch ? gender.ToString() : "null", MinAge, MaxAge, Config.Users.ShowFeaturedMemberOnHomePage);
            if (Cache[cacheKey] != null && Cache[cacheKey] is DataTable && Session["theme"] == null)
            {
                dtNewUsers = (DataTable)Cache[cacheKey];
            }
            else
            {
                var nuSearch = new NewUsersSearch
                                   {
                                       PhotoReq = Config.Users.RequirePhotoToShowInNewUsers,
                                       ProfileReq = Config.Users.RequireProfileToShowInNewUsers,
                                       UsersCount = (dlNewMembers.RepeatColumns*LimitRows),
                                       MinAge = MinAge,
                                       MaxAge = MaxAge
                                   };
                if (genderSearch) nuSearch.Gender = Gender;
                if (Config.Users.ShowFeaturedMemberOnHomePage) nuSearch.IsFeaturedMember = true;
                UserSearchResults nuResults = nuSearch.GetResults();
                dtNewUsers = new DataTable();
                dtNewUsers.Columns.Add("Username");
                dtNewUsers.Columns.Add("ImageId", typeof (int));
                dtNewUsers.Columns.Add("Age");
                dtNewUsers.Columns.Add("Age2");
                dtNewUsers.Columns.Add("Gender");
                dtNewUsers.Columns.Add("Country");
                dtNewUsers.Columns.Add("City");
                dtNewUsers.Columns.Add("StatusText");
                if (nuResults != null && nuResults.Usernames != null)
                {
                    foreach (string username in nuResults.Usernames)
                    {
                        int imageId;
                        User user;

                        try
                        {
                            user = User.Load(username);
                        }
                        catch (NotFoundException)
                        {
                            continue;
                        }

                        try
                        {
                            imageId = Photo.GetPrimary(username).Id;
                        }
                        catch (NotFoundException)
                        {
                            imageId = ImageHandler.GetPhotoIdByGender(user.Gender);
                        }
                        string age = !Config.Users.DisableAgeInformation ? user.Age.ToString() : String.Empty;
                        string age2 = !Config.Users.DisableAgeInformation ? user.Age2.ToString() : String.Empty;
                        string g = !Config.Users.DisableGenderInformation ? user.Gender.ToString() : String.Empty;
                        dtNewUsers.Rows.Add(
                            new object[]
                                {
                                    username, imageId, age, user.Gender == User.eGender.Couple ? age2 : String.Empty, g, user.Country, user.City, user.StatusText
                                });
                    }

                    Cache.Add(cacheKey, dtNewUsers, null, DateTime.Now.Add(TimeSpan.FromMinutes(10)),
                              Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
                }
            }

            if (dtNewUsers.Rows.Count > 0)
            {
                dlNewMembers.DataSource = dtNewUsers;
                dlNewMembers.DataBind();
            }

            #endregion

            #region Prepare CoolIris link

            if (Config.Misc.EnableCoolIris)
            {
                var rssNews = new HtmlLink();
                rssNews.Attributes.Add("rel", "alternate");
                rssNews.Attributes.Add("type", "application/rss+xml");
                rssNews.Attributes.Add("title", "");
                rssNews.Attributes.Add("id", "gallery");
                var feedUrl = String.Format("CoolIris.ashx?feed=newusers&gender={0}&minage={1}&maxage={2}",
                    genderSearch ? Gender.ToString() : "", MinAge, MaxAge);
                rssNews.Attributes.Add("href", feedUrl);
                Page.Header.Controls.Add(rssNews);
            }

            #endregion
        }

        protected void dlNewMembers_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            string username = (string)DataBinder.Eval(e.Item.DataItem, "Username");
            ((SendWinkIcon)e.Item.FindControl("btnSendWinkIcon")).Username = username;
            ((SendMsgIcon)e.Item.FindControl("btnSendMsgIcon")).Username = username;
            ((SendGiftIcon)e.Item.FindControl("btnSendGiftIcon")).Username = username;
            ((AddFriendIcon)e.Item.FindControl("btnAddFriendIcon")).Username = username;
            ((AddFavoriteIcon)e.Item.FindControl("btnAddFavoriteIcon")).Username = username;
        }

        protected void dlNewMembers_ItemCreated(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
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
    }
}
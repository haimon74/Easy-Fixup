using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating
{
    public partial class BlockedUsers : PageBase
    {
        #region properties

        protected bool showSlogan = false;

        public bool ShowSlogan
        {
            set { showSlogan = value; }
        }

        protected bool showLastOnline = true;

        public bool ShowLastOnline
        {
            set { showLastOnline = value; }
        }

        protected bool showRating = false;

        public bool ShowRating
        {
            set { showRating = value; }
        }

        #endregion

        protected void Page_PreRender(object sender, EventArgs e)
        {
            lnkBack.OnClientClick = String.Format("window.location='https://{0}';return false;", Session["RefererPage"]);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                loadStrings();
                loadBlockedUsers();
            }
        }

        private void loadStrings()
        {
            SideMenuStart1.Title = Lang.Trans("Actions");
            LargeBoxStart1.Title = Lang.Trans("Blocked Users");
            lnkBack.Text = Lang.Trans("<< Previous Page");
            pnlBackLink.Visible = true;
        }

        private void loadBlockedUsers()
        {
            DataTable dtFavourites = new DataTable();

            dtFavourites.Columns.Add("Username");
            dtFavourites.Columns.Add("PhotoId", typeof(int));
            dtFavourites.Columns.Add("Slogan");
            dtFavourites.Columns.Add("Age");
            dtFavourites.Columns.Add("LastOnlineString");
            dtFavourites.Columns.Add("Rating");
            dtFavourites.Columns.Add("BlockedOn");

            Dictionary<string, DateTime> blockedUsers = Classes.User.FetchBlockedUsers(CurrentUserSession.Username);

            if (blockedUsers.Count > 0)
            {
                foreach (KeyValuePair<string, DateTime> blockedUser in blockedUsers)
                {
                    User user;
                    try
                    {
                        user = Classes.User.Load(blockedUser.Key);
                    }
                    catch (NotFoundException)
                    {
                        continue;
                    }

                    Photo primaryPhoto = null;
                    try
                    {
                        primaryPhoto = user.GetPrimaryPhoto();
                    }
                    catch (NotFoundException)
                    {
                    }
                    string slogan = String.Empty;
                    try
                    {
                        ProfileAnswer sloganAnswer = user.FetchSlogan();
                        if (sloganAnswer.Approved)
                            slogan = sloganAnswer.Value;
                        else
                            slogan = Lang.Trans("-- pending approval --");
                    }
                    catch (NotFoundException)
                    {
                    }

                    string ratingString = String.Empty;
                    if (showRating)
                    {
                        try
                        {
                            UserRating userRating = UserRating.FetchRating(user.Username);
                            ratingString = String.Format(
                                Lang.Trans("{0} ({1} votes)"),
                                userRating.AverageVote.ToString("0.00"), userRating.Votes);
                        }
                        catch (NotFoundException)
                        {
                            ratingString = Lang.Trans("no rating");
                        }
                    }

                    DateTime blockedOn = blockedUser.Value;

                    #region Check user.Gender and set photoId

                    int photoId = 0;

                    if (primaryPhoto == null || !primaryPhoto.Approved)
                    {
                        photoId = ImageHandler.GetPhotoIdByGender(user.Gender);
                    }
                    else
                    {
                        photoId = primaryPhoto.Id;
                    }

                    #endregion

                    dtFavourites.Rows.Add(new object[]
                                              {
                                                  user.Username,
                                                  photoId,
                                                  slogan,
                                                  user.Age,
                                                  user.LastOnlineString,
                                                  ratingString,
                                                  blockedOn.ToShortDateString()
                                              });
                }
            }
            else
            {
                lblMessage.Text = Lang.Trans("There are no blocked users!");
            }

            dlBlockedUsers.DataSource = dtFavourites;
            dlBlockedUsers.DataBind();
        }

        protected void dlBlockedUsers_ItemCreated(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            LinkButton lnkUnblock = (LinkButton)e.Item.FindControl("lnkUnblock");
            HtmlGenericControl pnlAge = (HtmlGenericControl) e.Item.FindControl("pnlAge");
            lnkUnblock.Text = Lang.Trans("Unblock");
            pnlAge.Visible = !Config.Users.DisableAgeInformation;
        }

        protected void dlBlockedUsers_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "Unblock")
            {
                CurrentUserSession.UnblockUser((string) e.CommandArgument);
                loadBlockedUsers();
            }
        }
    }
}

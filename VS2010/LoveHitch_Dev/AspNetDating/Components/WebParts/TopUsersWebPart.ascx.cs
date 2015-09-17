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
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using AspNetDating.Classes;

namespace AspNetDating.Components.WebParts
{
    [Themeable(true), Editable]
    public partial class TopUsersWebPart : WebPartUserControl
    {
        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Gender"), GenderProperty]
        public User.eGender Gender
        {
            get
            {
                UserSession user = PageBase.GetCurrentUserSession();

                if (user != null)
                {
                    if (ViewState["Gender"] == null)
                    {
                        if (Config.Users.InterestedInFieldEnabled)
                        {
                            return user.InterestedIn;
                        }
                        switch (user.Gender)
                        {
                            case User.eGender.Couple:
                                return User.eGender.Couple;
                            case User.eGender.Female:
                                return User.eGender.Male;
                            default:
                                return User.eGender.Female;
                        }
                    }
                    return (User.eGender) ViewState["Gender"];
                }
                return User.eGender.Female;
            }
            set
            {
                ViewState["Gender"] = value;
            }
        }

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Minimum Age"), AgeProperty]
        public int MinAge
        {
            get
            {
                UserSession user = PageBase.GetCurrentUserSession();
                if (user != null)
                {
                    if (ViewState["NewUsers_MinAge"] == null)
                        return Math.Max(user.IncomingMessagesRestrictions.AgeFrom, Config.Users.MinAge);

                    return (int)(ViewState["NewUsers_MinAge"] ?? Config.Users.MinAge);
                }
                return Config.Users.MinAge;
            }
            set
            {
                ViewState["NewUsers_MinAge"] = value;
            }
        }

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Maximum Age"), AgeProperty]
        public int MaxAge
        {
            get
            {
                UserSession user = PageBase.GetCurrentUserSession();

                if (user != null)
                {
                    if (ViewState["NewUsers_MaxAge"] == null)
                        return Math.Min(user.IncomingMessagesRestrictions.AgeTo, Config.Users.MaxAge);

                    return (int)(ViewState["NewUsers_MaxAge"] ?? Config.Users.MaxAge);
                }
                return Config.Users.MinAge;
            }
            set
            {
                ViewState["NewUsers_MaxAge"] = value;
            }
        }

        //[Personalizable(PersonalizationScope.User), WebBrowsable(true), WebDisplayName("RepeatColumns")]
        public int RepeatColumns
        {
            get
            {
                return dlTopUsersDaily.RepeatColumns;
            }
            set
            {
                dlTopUsersDaily.RepeatColumns = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = Lang.Translate("Top Voted Users");
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            var topUsers = UserVotes.FetchTopUsers(Gender, MinAge, MaxAge, TimeSpan.FromDays(1), 6);
            if (topUsers.Count > 0)
                PrepareUserList(topUsers, dlTopUsersDaily);
            else
                h4Daily.Visible = false;

            topUsers = UserVotes.FetchTopUsers(Gender, MinAge, MaxAge, TimeSpan.FromDays(7), 6);
            if (topUsers.Count > 0)
                PrepareUserList(topUsers, dlTopUsersWeekly);
            else
                h4Weekly.Visible = false;

            topUsers = UserVotes.FetchTopUsers(Gender, MinAge, MaxAge, TimeSpan.FromDays(30), 6);
            if (topUsers.Count > 0)
                PrepareUserList(topUsers, dlTopUsersMonthly);
            else
                h4Monthly.Visible = false;

            topUsers = UserVotes.FetchTopUsers(Gender, MinAge, MaxAge, TimeSpan.FromDays(365), 6);
            if (topUsers.Count > 0)
                PrepareUserList(topUsers, dlTopUsersYearly);
            else
                h4Yearly.Visible = false;
        }

        private void PrepareUserList(ICollection<KeyValuePair<string, double>> topUsers, 
            BaseDataList dlTopUsers)
        {
            var dtTopUsers = new DataTable();
            dtTopUsers.Columns.Add("Username");
            dtTopUsers.Columns.Add("ImageId", typeof(int));
            dtTopUsers.Columns.Add("Rating", typeof(double));
            if (topUsers != null && topUsers.Count > 0)
            {
                foreach (var topUser in topUsers)
                {
                    int imageId;
                    try
                    {
                        imageId = Photo.GetPrimary(topUser.Key).Id;
                    }
                    catch (NotFoundException)
                    {
                        imageId = ImageHandler.GetPhotoIdByGender(Gender);
                    }

                    dtTopUsers.Rows.Add(
                        new object[]
                            {
                                topUser.Key, imageId
                            });
                }
            }

            dlTopUsers.DataSource = dtTopUsers;
            dlTopUsers.DataBind();
        }
    }
}
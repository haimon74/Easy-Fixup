using System;
using System.Data;
using System.Web;
using System.Web.Caching;
using System.Web.UI.WebControls.WebParts;
using ezFixUp.Classes;
using User = ezFixUp.Classes.User;

namespace ezFixUp.Components.WebParts
{
    [Editable]
    public partial class BirthdayBoxWebPart : WebPartUserControl
    {
        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Gender"), GenderProperty]
        public User.eGenderSearch Gender
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
                            return (User.eGenderSearch)user.InterestedIn;
                        }
                        else
                        {
                            switch (user.Gender)
                            {
                                case User.eGender.Male:
                                    return (User.eGenderSearch)User.eGender.Female;
                                case User.eGender.Female:
                                    return (User.eGenderSearch)User.eGender.Male;
                                case User.eGender.Couple:
                                    return User.eGenderSearch.All;
                            }
                        }
                        return User.eGenderSearch.All;
                    }
                    else
                    {
                        return (User.eGenderSearch)ViewState["Gender"];
                    }
                }
                else return User.eGenderSearch.All;
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
                else return Config.Users.MinAge;
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
                else return Config.Users.MinAge;
            }
            set
            {
                ViewState["NewUsers_MaxAge"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = Lang.Translate("Birthdays");
//            LoadBirthdays();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            
            if (!Config.Users.DisableAgeInformation)
                LoadBirthdays();
        }

        private void LoadBirthdays()
        {
            DataTable dtBirthdays = null;

            string cacheKey =
                String.Format("Components_LoadBirthdays_{0}_{1}_{2}",
                              Gender == User.eGenderSearch.All ? "null" : Gender.ToString(), MinAge, MaxAge);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                dtBirthdays = Cache[cacheKey] as DataTable;
            }
            else
            {
                dtBirthdays = new DataTable();
                dtBirthdays.Columns.Add("Date", typeof (DateTime));
                dtBirthdays.Columns.Add("Username", typeof (string));
                dtBirthdays.Columns.Add("DisplayName", typeof (string));
                
                UserSession user = PageBase.GetCurrentUserSession();
                
                int count = 0;
                for (int i = 0; i < 7; i++)
                {
                    if (count >= 12) break;
                    BirthdaySearch search = new BirthdaySearch();
                    search.Birthdate = DateTime.Now.AddDays(i).Add(Config.Misc.TimeOffset);
                    search.MinAge = Math.Max(MinAge, user.Age -10);
                    search.MaxAge = Math.Min(MaxAge, user.Age + 10);
                    if (Gender != User.eGenderSearch.All)
                        search.Gender = (User.eGender)Gender;
                    if (search.Birthdate.Month == 1 && search.Birthdate.Day == 1) continue; // skip fake birthdays
                    UserSearchResults results = search.GetResults();
                    if (results == null || results.Usernames == null) continue;
                    foreach (string username in results.Usernames)
                    {
                        //using (var db = new ezFixUpDataContext())
                        {
                            string displayName = user.GetUserDisplayedNameByUsername(username);
                            dtBirthdays.Rows.Add(new object[] { search.Birthdate, username, displayName });
                        }
                        ++count;
                    }
                }

                Cache.Insert(cacheKey, dtBirthdays, null, DateTime.Now.AddHours(1),
                             Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
            }

            if (dtBirthdays.Rows.Count != 0)
            {
                rptBirthdays.DataSource = dtBirthdays;
                rptBirthdays.DataBind();
                mvBirthdays.SetActiveView(vBirthdays);
            }
            else
                mvBirthdays.SetActiveView(vNoBirthdays);
        }
    }
}
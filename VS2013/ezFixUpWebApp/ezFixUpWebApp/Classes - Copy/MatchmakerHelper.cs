using System;
using System.Web;
using ezFixUp.Model;

namespace ezFixUp.Classes
{
    public class MatchmakerHelper
    {
        public static void ToggleState(string username)
        {
            CurrentUsername = username;
            ToggleState();
        }
        public static void ToggleState()
        {
            bool toEnterMatchmakerMode = !IsMatchmakerState;
            SetMatchmakerState(toEnterMatchmakerMode);
        }
        public static bool IsOnlyMatchmakerSessionUser()
        {
            var userSession = PageBase.GetCurrentUserSession();
            return ( userSession != null && userSession.InterestedIn == User.eGender.Matchmaker );
        }
        public static void SetMatchmakerState(bool enterMatchmakerMode)
        {
            bool hasUser = CurrentUsername.IsNotNullOrEmpty();

            if ( enterMatchmakerMode )
            {
                if ( !hasUser )
                {
                    SetMatchmakerState(false);
                    HttpContext.Current.Response.Redirect("~/Registration.aspx");
                }
                SiteTheme = Config.Misc.SiteTheme + "_MM";
                IsMatchmakerState = true;
                MenuItemName = IsOnlyMatchmakerSessionUser() ? "" : "Exit Matchmaker";
                CurrentHomePage = "Home_mm.aspx";
            }
            else if ( !IsOnlyMatchmakerSessionUser() )
            {
                HttpContext.Current.Session["MatchToFriendImageId"] = null;
                HttpContext.Current.Session["MatchToUsername"] = null;
                HttpContext.Current.Session["ToUsername"] = null;
                SiteTheme = Config.Misc.SiteTheme;
                IsMatchmakerState = false;
                MenuItemName = "Play Matchmaker";
                CurrentHomePage = "Home.aspx";
            }
        }
        public static string SiteTheme
        {
            get { return HttpContext.Current.Session["theme"] as string; }
            set { HttpContext.Current.Session["theme"] = value; }
        }
        public static bool IsMatchmakerState
        {
            get { return HttpContext.Current.Session["MatchmakerState"].ToString() == "ON"; }
            set { HttpContext.Current.Session["MatchmakerState"] = value ? "ON" : "OFF"; }
        }
        public static string MenuItemName
        {
            get { return (string)HttpContext.Current.Session["MatchmakerMenuItemName"]; }
            set { HttpContext.Current.Session["MatchmakerMenuItemName"] = value; }
        }
        public static string CurrentHomePage
        {
            get { return (string)(HttpContext.Current.Session["CurrentHomePage"] ?? "Home.aspx"); }
            set { HttpContext.Current.Session["CurrentHomePage"] = value; }
        }
        public static int MatchToFriendImageId
        {
            get
            {
                int result;
                if ( HttpContext.Current.Session["MatchToFriendImageId"] != null &&
                    int.TryParse(HttpContext.Current.Session["MatchToFriendImageId"] as string, out result) )
                    return result;
                bool isMale = PageBase.GetCurrentUserSession().Gender == User.eGender.Male;
                return ( isMale ) ? -1 : -2;
            }
            set { HttpContext.Current.Session["MatchToFriendImageId"] = value.ToString(); }
        }
        
        public static string MatchToDisplayName
        {
            get { return (string)(HttpContext.Current.Session["MatchToDisplayName"] ?? string.Empty); }
            set { HttpContext.Current.Session["MatchToDisplayName"] = value; }
        }

        public static string MatchToUsername
        {
            get { return (string)( HttpContext.Current.Session["MatchToUsername"] ?? string.Empty ); }
            set 
            { 
                HttpContext.Current.Session["MatchToUsername"] = value;
                if (value == null)
                {
                    MatchToDisplayName = null;
                }
                else
                {
                    MatchToDisplayName = UserSession.GetViewedUserDisplayedNameByUsername(CurrentUsername, value);
                }
            }
        }

        public static bool IsMatchToFriendAlredySelected
        {
            get
            {
                int id;
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    return HttpContext.Current.Session["MatchToFriendImageId"] != null &&
                           int.TryParse(HttpContext.Current.Session["MatchToFriendImageId"].ToString(), out id);
                }
                else return false;
            }
        }
        public static void CreateMatchmakingWithUsername(string withUsername)
        {
            if ( !IsMatchToFriendAlredySelected || !withUsername.IsNotNullOrEmpty() ||
                !CurrentUsername.IsNotNullOrEmpty() ) return;

            string toUsername = MatchToUsername;
            var matchMaking = new MatchMaking
                                  {
                                      Friend1Ack = false,
                                      Friend1Username = toUsername,
                                      Friend2Ack = false,
                                      Friend2Username = withUsername,
                                      MatchmakerUsername = CurrentUsername,
                                      Timestamp = DateTime.Now,
                                      MatchingStatus = MatchMaking.eMatchingStatus.eMatched
                                  };
            matchMaking.Save();
            MatchToUsername = null;
            HttpContext.Current.Session["MatchToFriendImageId"] = null;
        }
        public static string CurrentUsername
        {
            get { return (string)HttpContext.Current.Session["CurrentUsername"] ?? String.Empty; }
            set { HttpContext.Current.Session["CurrentUsername"] = value; }
        }
    }
}

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
            return ( userSession != null && userSession.InterestedIn == Classes.User.eGender.Matchmaker );
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
                Global.GetSessionState()["MatchToFriendImageId"] = null;
                Global.GetSessionState()["MatchToUsername"] = null;
                Global.GetSessionState()["ToUsername"] = null;
                SiteTheme = Config.Misc.SiteTheme;
                IsMatchmakerState = false;
                MenuItemName = "Play Matchmaker";
                CurrentHomePage = "Home.aspx";
            }
        }
        public static string SiteTheme
        {
            get { return Global.GetSessionState().ContainsKey("theme") ? Global.GetSessionState()["theme"] as string : null; }
            set { Global.GetSessionState()["theme"] = value; }
        }
        public static bool IsMatchmakerState
        {
            get
            {
                var mmState = Global.GetSessionState().ContainsKey("MatchmakerState") ?  Global.GetSessionState()["MatchmakerState"] : null;
                return  mmState!= null && mmState.ToString() == "ON";
            }
            set { Global.GetSessionState()["MatchmakerState"] = (value ? "ON" : "OFF"); }
        }
        public static string MenuItemName
        {
            get { return Global.GetSessionState().ContainsKey("MatchmakerMenuItemName") ? (string)Global.GetSessionState()["MatchmakerMenuItemName"] : null; }
            set { Global.GetSessionState()["MatchmakerMenuItemName"] = value; }
        }
        public static string CurrentHomePage
        {
            get { return Global.GetSessionState().ContainsKey("CurrentHomePage") ? (string)Global.GetSessionState()["CurrentHomePage"] : "Home.aspx"; }
            set { Global.GetSessionState()["CurrentHomePage"] = value; }
        }
        public static int MatchToFriendImageId
        {
            get
            {
                int result;
                if ( Global.GetSessionState().ContainsKey("MatchToFriendImageId") &&
                    int.TryParse(Global.GetSessionState()["MatchToFriendImageId"] as string, out result) )
                    return result;
                bool isMale = PageBase.GetCurrentUserSession().Gender == Classes.User.eGender.Male;
                return ( isMale ) ? -1 : -2;
            }
            set { Global.GetSessionState()["MatchToFriendImageId"] = value.ToString(); }
        }
        
        public static string MatchToDisplayName
        {
            get { return Global.GetSessionState().ContainsKey("MatchToDisplayName") ? (string)Global.GetSessionState()["MatchToDisplayName"] : string.Empty; }
            set { Global.GetSessionState()["MatchToDisplayName"] = value; }
        }

        public static string MatchToUsername
        {
            get { return Global.GetSessionState().ContainsKey("MatchToUsername") ? (string)Global.GetSessionState()["MatchToUsername"] : string.Empty; }
            set 
            { 
                Global.GetSessionState()["MatchToUsername"] = value;
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
                if (Global.GetSessionState().ContainsKey("MatchToFriendImageId"))
                {
                    return Global.GetSessionState()["MatchToFriendImageId"] != null &&
                           int.TryParse(Global.GetSessionState()["MatchToFriendImageId"].ToString(), out id);
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
            Global.GetSessionState()["MatchToFriendImageId"] = null;
        }
        public static string CurrentUsername
        {
            get { return (Global.GetSessionState().ContainsKey("CurrentUsername")) 
                        ? (string)Global.GetSessionState()["CurrentUsername"] 
                        : String.Empty; }
            set { Global.GetSessionState()["CurrentUsername"] = value; }
        }
    }
}

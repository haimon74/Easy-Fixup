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
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Caching;
using ezFixUp.Model;


namespace ezFixUp.Classes
{
    [Serializable]
    public class UserSearchResults : SearchResults<string, User>
    {
        public string[] Usernames
        {
            get { return Results; }
            set { Results = value; }
        }

        public int GetTotalPages()
        {
            return GetTotalPages(Config.Search.UsersPerPage);
        }

        public new int GetTotalPages(int usersPerPage)
        {
            return base.GetTotalPages(usersPerPage);
        }

        protected override User LoadResult(string id)
        {
            try
            {
                return Classes.User.Load(id);
            }
            catch (NotFoundException)
            {
                return null;
            }
        }

        protected override User[] LoadResults(string[] ids)
        {
            DataTable dt = Classes.User.BulkLoad(ids);

            if (dt == null) return null;

            User[] users = new User[dt.Rows.Count];
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    users[i] = new User(dt.Rows[i]);
                }
            }
            catch (NotFoundException)
            {
                return null;
            }
            return users;
        }

        /// <summary>
        /// Use this method to get the search results
        /// Number of users per page is defined in Config.Search
        /// </summary>
        /// <param name="Page">Page number</param>
        /// <returns>Array of usernames</returns>
        public User[] GetPage(int Page)
        {
            return GetPage(Page, Config.Search.UsersPerPage);
        }

        /// <summary>
        /// Use this method to get the search results
        /// </summary>
        /// <param name="Page">Page number</param>
        /// <param name="usersPerPage">usersPerPage</param>
        /// <returns>Array of usernames</returns>
        public new User[] GetPage(int Page, int usersPerPage)
        {
            return base.GetPage(Page, usersPerPage);
        }

        public User[] Get()
        {
            return GetPage(1, Int32.MaxValue);
        }
    }


    [Serializable]
    public class BasicSearch
    {
        #region Properties

        private bool active;
        public bool active_isSet;
        private string city = "";
        private string country = "";
        private bool deleted;
        public bool deleted_isSet;
        private string email;
        private int? loginCountThreshold = null;

        private bool? faceControlApproved = Config.CommunityFaceControlSystem.EnableCommunityFaceControl
                                                ? (bool?)true
                                                : null;

        private Classes.User.eGender gender;
        public bool gender_isSet;
        private bool hasAnswer;
        public bool hasAnswer_isSet;
        private bool hasPhoto;
        public bool hasPhoto_isSet;
        private bool isOnline;

        private Classes.User.eGender interestedIn;
        public bool interestedIn_isSet;
        private string ip;
        private int? languageID;
        private DateTime? lastLogin;
        private DateTime? userSince;
        private int maxAge = Config.Users.MaxAge;

        private int minAge = Config.Users.MinAge;
        private string name;
        private bool paid;
        public bool paid_isSet;
        private bool sortAsc;
        private string sortColumn;
        private bool? spamSuspected;
        private string state = "";
        private string username;
        private bool visible;
        public bool visible_isSet;
        private string zip = "";
        private List<string> excludeUsername = new List<string>();

        public List<string> ExcludeUsername
        {
            get { return excludeUsername; }
            set { excludeUsername = value; }
        }

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Classes.User.eGender Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                gender_isSet = true;
            }
        }

        public Classes.User.eGender InterestedIn
        {
            get { return interestedIn; }
            set
            {
                interestedIn = value;
                interestedIn_isSet = true;
                // value == Classes.User.eGender.Male || value == Classes.User.eGender.Female; //|| value == Classes.User.eGender.Couple;
            }
        }

        public int MinAge
        {
            get { return minAge; }
            set
            {
                if (minAge < Config.Users.MinAge)
                    minAge = Config.Users.MinAge;
                else
                    minAge = value;
            }
        }

        public int MaxAge
        {
            get { return maxAge; }
            set
            {
                if (maxAge > Config.Users.MaxAge)
                    maxAge = Config.Users.MaxAge;
                else
                    maxAge = value;
            }
        }

        public string City
        {
            get { return city; }
            set { city = value; }
        }

        public string Country
        {
            get { return country; }
            set
            {
                //if (Config.Users.CountriesHash[value] != null)
                //    country = (string) Config.Users.CountriesHash[value];
                //else
                country = value;
            }
        }

        public string State
        {
            get { return state; }
            set
            {
                //if (Config.Users.StateHash[value] != null)
                //{
                //    if ((string) Config.Users.StateHash[value] == "NA")
                //        state = "";
                //    else
                //        state = (string) Config.Users.StateHash[value];
                //}
                //else
                state = value;
            }
        }

        public string Zip
        {
            get { return zip; }
            set { zip = value; }
        }

        public bool IsOnline
        {
            get { return isOnline; }
            set { isOnline = value; }
        }

        public bool HasPhoto
        {
            get { return hasPhoto; }
            set
            {
                hasPhoto = value;
                hasPhoto_isSet = true;
            }
        }

        public bool HasAnswer
        {
            get { return hasAnswer; }
            set
            {
                hasAnswer = value;
                hasAnswer_isSet = true;
            }
        }

        public bool Visible
        {
            get { return visible; }
            set
            {
                visible = value;
                visible_isSet = true;
            }
        }

        public bool Active
        {
            get { return active; }
            set
            {
                active = value;
                active_isSet = true;
            }
        }

        public bool Deleted
        {
            get { return deleted; }
            set
            {
                deleted = value;
                deleted_isSet = true;
            }
        }

        public bool Paid
        {
            get { return paid; }
            set
            {
                paid = value;
                paid_isSet = true;
            }
        }

        public string IP
        {
            get { return ip; }
            set { ip = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public bool? SpamSuspected
        {
            get { return spamSuspected; }
            set { spamSuspected = value; }
        }

        public bool? FaceControlApproved
        {
            get { return faceControlApproved; }
            set { faceControlApproved = value; }
        }

        public int? LoginCountThreshold
        {
            get { return loginCountThreshold; }
            set { loginCountThreshold = value; }
        }

        public int? LanguageID
        {
            get { return languageID; }
            set { languageID = value; }
        }

        public DateTime? LastLogin
        {
            get { return lastLogin; }
            set { lastLogin = value; }
        }

        public DateTime? UserSince
        {
            get { return userSince; }
            set { userSince = value; }
        }

        public string SortColumn
        {
            get { return sortColumn; }
            set { sortColumn = value; }
        }

        public bool SortAsc
        {
            get { return sortAsc; }
            set { sortAsc = value; }
        }

        private string viewerUsername;
        public string ViewerUsername
        {
            get { return viewerUsername; }
            set { viewerUsername = value; }
        }

        #endregion

        public class UserBasicSelect
        {
            public string Username;
            public string Name;
            public string Email;
            public string IpAddress;
            public int Gender;
            public DateTime LastOnline;
            public DateTime UserSince;
            public UserBasicSelect()
            {
            }

            public UserBasicSelect(string username, string name, string email, string ipAddress,
                            int gender, DateTime lastOnline, DateTime userSince)
            {
                this.Username = username;
                this.Name = name;
                this.Email = email;
                this.Gender = gender;
                this.IpAddress = ipAddress;
                LastOnline = lastOnline;
                UserSince = userSince;
            }
        }
        public BasicSearch()
        {
            // Set defaults
            HasAnswer = true;
            Visible = true;
            Active = true;
            Deleted = false;
            SortColumn = "LastOnline";
            SortAsc = false;
            LoginCountThreshold = 1;
        }

        public IQueryable<Model.User> GetQueryableResult()
        {
            DateTime fromBirthdate = DateTime.Now.Subtract(TimeSpan.FromDays((maxAge + 1) * 365.25));
            DateTime toBirthdate = DateTime.Now.Subtract(TimeSpan.FromDays(minAge * 365.25));

            var db = new ezFixUpDataContext();
            
            IQueryable<Model.User> res = db.Users.AsQueryable<Model.User>();
            
            if (excludeUsername.Count > 0 )
                res = res.Where(u=>!excludeUsername.Contains(u.u_username));
            
            if (LoginCountThreshold.HasValue)
                res = res.Where(u=> LoginCountThreshold.Value <= u.u_logincount || u.u_signup_ip.StartsWith("192.168."));
                
            if (username.IsNotNullOrEmpty())
                res = res.Where(u=> u.u_username.Contains(username));

            if (name.IsNotNullOrEmpty())
                res = res.Where(u=> u.u_name.Contains(name));

            if (gender_isSet)
                res=res.Where(u=> u.u_gender == (int)gender);

            if (Config.Users.InterestedInFieldEnabled && interestedIn_isSet)
                res = res.Where(u=> (u.u_interested_in != (int)User.eGenderSearch.Matchmaker &&
                                     u.u_interested_in == (int)interestedIn));
                             
            res = res.Where(u=> (u.u_birthdate >= fromBirthdate && (u.u_gender != 3 || u.u_birthdate2 >= fromBirthdate))
                             && (u.u_birthdate <= toBirthdate && (u.u_gender != 3 || u.u_birthdate2 <= toBirthdate)));
                           
            if (city.IsNotNullOrEmpty())
                res = res.Where(u=> city == u.u_city);
            if (country.IsNotNullOrEmpty())
                res = res.Where(u=>country == u.u_country);
            if (state.IsNotNullOrEmpty())
                res = res.Where(u=> state == u.u_state);
            if (zip.IsNotNullOrEmpty())
                res = res.Where(u=> u.u_zip_code.Contains(zip));

            if (hasAnswer_isSet)
                res = res.Where(u=> ( hasAnswer &&  (from a in db.ProfileAnswers select a.u_username).Contains(u.u_username)) ||
                                    (!hasAnswer && !(from a in db.ProfileAnswers select a.u_username).Contains(u.u_username)));

            if (ip.IsNotNullOrEmpty())
                res = res.Where(u=>u.u_signup_ip.Contains(ip));
            if (email.IsNotNullOrEmpty())
                res = res.Where(u=> u.u_email.Contains(email));
            if (spamSuspected.HasValue)
                res=res.Where(u=> spamSuspected == u.u_spamsuspected);
            if (faceControlApproved.HasValue)
                res=res.Where(u=> faceControlApproved == u.u_face_control_approved);
            if (visible_isSet)
                res=res.Where(u=> visible == u.u_profilevisible);
            if (active_isSet)
                res=res.Where(u=> active == u.u_active);
            if (deleted_isSet)
                res=res.Where(u=> deleted == u.u_deleted);
            if (paid_isSet)
                res=res.Where(u=> paid == u.u_paid_member);
            if (languageID.HasValue)
                res=res.Where(u=> languageID == u.l_id);
            if (lastLogin.HasValue)
                res=res.Where(u=> (u.u_lastlogin.Date == lastLogin)
                               && (!userSince.HasValue || u.u_usersince.Date == userSince));

            return res;
        }
        public IQueryable<View_MaleInterestedInMaleSingle> GetQueryableResultM2M()
        {
            DateTime fromBirthdate = DateTime.Now.Subtract(TimeSpan.FromDays((maxAge + 1) * 365.25));
            DateTime toBirthdate = DateTime.Now.Subtract(TimeSpan.FromDays(minAge * 365.25));

            var db = new ezFixUpDataContext();
            IQueryable<View_MaleInterestedInMaleSingle> res = db.View_MaleInterestedInMaleSingles.AsQueryable<View_MaleInterestedInMaleSingle>();
            if (excludeUsername.Count > 0 )
                res=res.Where(u=>!excludeUsername.Contains(u.u_username));
            if (username.IsNotNullOrEmpty())
                res=res.Where(u=> u.u_username.Contains(username));
            if (name.IsNotNullOrEmpty())
                res=res.Where(u=> u.u_name.Contains(name));
            res=res.Where(u=> (u.u_birthdate >= fromBirthdate) && (u.u_birthdate <= toBirthdate));
            if (city.IsNotNullOrEmpty())
                res=res.Where(u=> city == u.u_city);
            if (country.IsNotNullOrEmpty())
                res=res.Where(u=> country == u.u_country);
            if (state.IsNotNullOrEmpty())
                res=res.Where(u=> state == u.u_state);
            if (hasAnswer_isSet)
                res=res.Where(u=> ( hasAnswer &&  (from a in db.ProfileAnswers select a.u_username).Contains(u.u_username)) ||
                                  (!hasAnswer && !(from a in db.ProfileAnswers select a.u_username).Contains(u.u_username))
                                );
            if (email.IsNotNullOrEmpty())
                res=res.Where(u=> email == u.u_email);
            if (paid_isSet)
                res=res.Where(u=> paid == u.u_paid_member);
            if (userSince.HasValue)
                res=res.Where(u=> u.u_usersince.Date == userSince);

            return res;
        }

        public IQueryable<View_MaleInterestedInFemaleSingle> GetQueryableResultM2F()
        {
            DateTime fromBirthdate = DateTime.Now.Subtract(TimeSpan.FromDays((maxAge + 1) * 365.25));
            DateTime toBirthdate = DateTime.Now.Subtract(TimeSpan.FromDays(minAge * 365.25));

            var db = new ezFixUpDataContext();
            var res = (from u in db.View_MaleInterestedInFemaleSingles
                       where (excludeUsername.Count == 0 || !excludeUsername.Contains(u.u_username))
                             && (username.IsNullOrEmpty() || u.u_username.Contains(username))
                             && (name.IsNullOrEmpty() || u.u_name.Contains(name))
                             && (u.u_birthdate >= fromBirthdate) && (u.u_birthdate <= toBirthdate)
                             && (city.IsNullOrEmpty() || city == u.u_city)
                             && (country.IsNullOrEmpty() || country == u.u_country)
                             && (state.IsNullOrEmpty() || state == u.u_state)
                             && (!hasAnswer_isSet ||
                                 (hasAnswer &&
                                  (from a in db.ProfileAnswers select a.u_username).Contains(u.u_username)) ||
                                 (!hasAnswer &&
                                  !(from a in db.ProfileAnswers select a.u_username).Contains(u.u_username))
                                )
                             && (email.IsNullOrEmpty() || email == u.u_email)
                             && (!paid_isSet || paid == u.u_paid_member)
                             && (!userSince.HasValue || u.u_usersince.Date == userSince)
                       select u);
            return res;
        }

        public IQueryable<View_FemaleInterestedInFemaleSingle> GetQueryableResultF2F()
        {
            DateTime fromBirthdate = DateTime.Now.Subtract(TimeSpan.FromDays((maxAge + 1) * 365.25));
            DateTime toBirthdate = DateTime.Now.Subtract(TimeSpan.FromDays(minAge * 365.25));

            var db = new ezFixUpDataContext();
            var res = (from u in db.View_FemaleInterestedInFemaleSingles
                       where (excludeUsername.Count == 0 || !excludeUsername.Contains(u.u_username))
                             && (username.IsNullOrEmpty() || u.u_username.Contains(username))
                             && (name.IsNullOrEmpty() || u.u_name.Contains(name))
                             && (u.u_birthdate >= fromBirthdate) && (u.u_birthdate <= toBirthdate)
                             && (city.IsNullOrEmpty() || city == u.u_city)
                             && (country.IsNullOrEmpty() || country == u.u_country)
                             && (state.IsNullOrEmpty() || state == u.u_state)
                             && (!hasAnswer_isSet ||
                                 (hasAnswer &&
                                  (from a in db.ProfileAnswers select a.u_username).Contains(u.u_username)) ||
                                 (!hasAnswer &&
                                  !(from a in db.ProfileAnswers select a.u_username).Contains(u.u_username))
                                )
                             && (email.IsNullOrEmpty() || email == u.u_email)
                             && (!paid_isSet || paid == u.u_paid_member)
                             && (!userSince.HasValue || u.u_usersince.Date == userSince)
                       select u);
            return res;
        }

        public IQueryable<View_FemaleInterestedInMaleSingle> GetQueryableResultF2M()
        {
            DateTime fromBirthdate = DateTime.Now.Subtract(TimeSpan.FromDays((maxAge + 1) * 365.25));
            DateTime toBirthdate = DateTime.Now.Subtract(TimeSpan.FromDays(minAge * 365.25));

            var db = new ezFixUpDataContext();
            var res = (from u in db.View_FemaleInterestedInMaleSingles
                       where ((ExcludeUsername.Count == 0 || !ExcludeUsername.Contains(u.u_username))
                             && (username.IsNullOrEmpty() || u.u_username.Contains(username))
                             && (name.IsNullOrEmpty() || u.u_name.Contains(name))
                             && (u.u_birthdate >= fromBirthdate) && (u.u_birthdate <= toBirthdate)
                             && (city.IsNullOrEmpty() || city == u.u_city)
                             && (country.IsNullOrEmpty() || country == u.u_country)
                             && (state.IsNullOrEmpty() || state == u.u_state)
                             && (!hasAnswer_isSet ||
                                 (hasAnswer &&
                                  (from a in db.ProfileAnswers select a.u_username).Contains(u.u_username)) ||
                                 (!hasAnswer &&
                                  !(from a in db.ProfileAnswers select a.u_username).Contains(u.u_username))
                                )
                             && (email.IsNullOrEmpty() || email == u.u_email)
                             && (!paid_isSet || paid == u.u_paid_member)
                             && (!userSince.HasValue || u.u_usersince.Date == userSince))
                       select u);
            return res;
        }

        public UserSearchResults GetResults()
        {
            IList<UserBasicSelect> res = null;
            //if (Gender == Classes.User.eGender.Male)
            //{
            //    if (InterestedIn == Classes.User.eGender.Male)
            //    {
            //        res = GetQueryableResultM2M().Select(
            //                u => new UserBasicSelect(u.u_username, u.u_name, u.u_email, u.u_signup_ip,
            //                                         u.u_gender, u.u_lastonline, u.u_usersince)).ToList();
            //    }
            //    else
            //    {
            //        res = GetQueryableResultM2F().Select(
            //                u => new UserBasicSelect(u.u_username, u.u_name, u.u_email, u.u_signup_ip,
            //                                         u.u_gender, u.u_lastonline, u.u_usersince)).ToList();
            //    }
            //}
            //else
            //{
            //    if (InterestedIn == Classes.User.eGender.Male)
            //    {
            //        var tmp = GetQueryableResultF2M();
            //        if (tmp != null && tmp.Count() > 0)
            //        {
            //            res = tmp.Select(u => new UserBasicSelect(
            //                                      u.u_username, u.u_name, u.u_email, u.u_signup_ip,
            //                                      u.u_gender, u.u_lastonline, u.u_usersince)
            //                             ).ToList();
            //        }
            //    }
            //    else
            //    {
            //        res = GetQueryableResultF2F().Select(
            //                u => new UserBasicSelect(u.u_username, u.u_name, u.u_email, u.u_signup_ip,
            //                                         u.u_gender, u.u_lastonline, u.u_usersince)).ToList();
            //    }
            //}
            TimeMeasure timestamps = new TimeMeasure();
            var tmpRes = GetQueryableResult().ToList();
            res = tmpRes.Select(
                            u => new UserBasicSelect(u.u_username, u.u_name, u.u_email, u.u_signup_ip,
                                                     u.u_gender, u.u_lastonline, u.u_usersince)).ToList();
            timestamps.AddTimestamp("BasicSearch.GetResults");
            switch (sortColumn)
            {
                case "Username":
                    res = res.OrderBy(u => u.Username).ToList();
                    break;
                case "Name":
                    res = res.OrderBy(u => u.Name).ToList();
                    break;
                case "Email":
                    res = res.OrderBy(u => u.Email).ToList();
                    break;
                case "SignupIP":
                    res = res.OrderBy(u => u.IpAddress).ToList();
                    break;
                case "Gender":
                    res = res.OrderBy(u => u.Gender).ToList();
                    break;
                case "LastOnline":
                    res = res.OrderBy(u => u.LastOnline).ToList();
                    break;
                case "SignupDate":
                    res = res.OrderBy(u => u.UserSince).ToList();
                    break;

                case "MutualFriendsCount":
                    timestamps.AddTimestamp("BasicSearch.GetResults");
                    res = res.OrderByDescending(u => u.LastOnline).ToList();
                    var resUsernames = res.Select(x => x.Username).ToList();
                    if (ViewerUsername.IsNotNullOrEmpty())
                    {
                        var dicHasConnections = new Dictionary<string, int>();
                        var dicNoConnections = new Dictionary<string, int>();

                        timestamps.AddTimestamp("BasicSearch.GetResults");
                        //foreach (var viewed in resUsernames)
                        for (int i = 0; i < resUsernames.Count; i++)
                        {
                            var viewed = resUsernames[i];
                            List<string> usersChain;
                            bool success = Classes.User.FindFriendsConnection(ViewerUsername, viewed, 2, out usersChain);

                            if (success)
                            {
                                dicHasConnections.Add(viewed, usersChain.Count);
                            }
                            else
                            {
                                dicNoConnections.Add(viewed, 0);
                            }
                        }

                        timestamps.AddTimestamp("BasicSearch.GetResults");

                        var lstHasConnections = dicHasConnections.Select(x => x.Key).ToList();
                        //dicHasConnections.OrderByDescending(x => x.Value).Select(x=>x.Key).ToList();
                        resUsernames = lstHasConnections.Concat(dicNoConnections.Keys).ToList();
                        //res = res.OrderBy(x => resUsernames.IndexOf(x.Username)).ToList();

                        timestamps.AddTimestamp("BasicSearch.GetResults");
                    }
                    break;
            }

            IList<string> lUsernames = res.Select(n => n.Username).ToList();
            if (IsOnline) UserSession.SortUsersByOnlineStatus(lUsernames);
            if (!sortAsc) lUsernames.Reverse();

            lUsernames = GetUsernamesOrderedByPhotosRequest(lUsernames);
            var results = new UserSearchResults();
            results.Usernames = lUsernames.Count > 0 ? lUsernames.ToArray() : null;
            return results;
        }

        protected IList<string> GetUsernamesOrderedByPhotosRequest(IList<string> usernames)
        {
            IList<string> lUsernames;
            using (var db = new ezFixUpDataContext())
            {
                if (true || HasPhoto)
                {
                    var usernamesWithPhotos = CompiledQueries.FetchUsernamesWithPhotos(db).ToList();
                    //from p in db.Photos where p.p_approved select p.u_username;
                    var usernamesWithPhoto =
                        (from r in usernames
                         where usernamesWithPhotos.Contains(r)
                         select r).ToList();
                    var usernamesWithoutPhoto =
                        (from r in usernames
                         where !usernamesWithPhotos.Contains(r)
                         select r).ToList();
                    lUsernames = usernamesWithPhoto.Concat(usernamesWithoutPhoto).ToList();
                }
                else
                {
                    lUsernames = usernames;
                }
            }
            return lUsernames;
        }
    }
    public enum OrderByColumnEnum
    {
        LastOnline,
        SignupDate,
        Name,
        Gender,
        Email,
        Username,
        FacebookMutualFriends
    };

    [Serializable]
    public class ProfileViewedSearch
    {
        public string Username { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }

        public string ExcludeUsername { get; set; }

        public ProfileViewedSearch()
        {
            Active = true;
            Deleted = false;
        }
        //var x = from a in user.ProfileViews where a.
        public UserSearchResults GetResults()
        {
            var retSearchResults = new UserSearchResults();
            if (!string.IsNullOrEmpty(Username))
            {
                using (var db = new ezFixUpDataContext())
                {
                    var viewers = CompiledQueries.
                        FetchViewersUsernamesForProfileViewedSearch(db, Username, ExcludeUsername).ToList();
                    //db.ProfileViews
                    //.Where( v => (ExcludeUsername.IsNullOrEmpty() || v.pv_viewer != ExcludeUsername) && 
                    //             v.pv_viewed == Username && 
                    //             v.User.u_active == Active && 
                    //             v.User.u_deleted == Deleted)
                    //.OrderBy(v => v.pv_timestamp).Select(v => v.pv_viewer);
                    if (viewers.Count() > 0)
                    {
                        retSearchResults.Usernames = viewers.ToArray();
                    }
                }
            }
            return retSearchResults;
        }
    }
    [Serializable]
    public class BasicFriendsSearch : BasicSearch
    {
        public OrderByColumnEnum SortByColumn { get; set; }
        public string FriendUsername { get; set; }

        public BasicFriendsSearch()
        {
            Active = true;
            Deleted = false;
            SortByColumn = OrderByColumnEnum.FacebookMutualFriends;
        }
        //var x = from a in user.ProfileViews where a.
        public UserSearchResults GetResults()
        {
            var retSearchResults = new UserSearchResults();
            if (!string.IsNullOrEmpty(Username))
            {
                using (var db = new ezFixUpDataContext())
                {
                    var user = CompiledQueries.FetchUserByUsername(db, Username);
                    //db.Users.Where(u => u.u_username == Username).SingleOrDefault();

                    if (user != null)
                    {
                        //int minBdayYear = DateTime.Now.Year - MaxAge;
                        //int maxBdayYear = DateTime.Now.Year - MinAge;
                        bool isFriendUsernameSearch = FriendUsername != null;


                        List<Friend> friends = null;
                        if (isFriendUsernameSearch)
                        {
                            friends = CompiledQueries
                                .FetchFriendsSearchResultByUsername(db, Username, FriendUsername.ToLower()).ToList();
                        }
                        else
                        {
                            friends = CompiledQueries
                                .FetchBasicFriendsSearchResultFiltered(db, Username, this).ToList();
                        }

                        if (friends != null && friends.Count() > 0)
                        {
                            switch (SortByColumn)
                            {
                                case OrderByColumnEnum.Username:
                                    friends = friends.OrderBy(u => u.User.u_username).ToList();
                                    break;
                                case OrderByColumnEnum.Email:
                                    friends = friends.OrderBy(u => u.User.u_email).ToList();
                                    break;
                                case OrderByColumnEnum.Gender:
                                    friends = friends.OrderBy(u => u.User.u_gender).ToList();
                                    break;
                                case OrderByColumnEnum.LastOnline:
                                    friends = friends.OrderBy(u => u.User.u_lastonline).ToList();
                                    break;
                                case OrderByColumnEnum.SignupDate:
                                    friends = friends.OrderBy(u => u.User.u_usersince).ToList();
                                    break;
                                case OrderByColumnEnum.Name:
                                    friends = friends.OrderBy(u => u.User.u_name).ToList();
                                    break;
                                case OrderByColumnEnum.FacebookMutualFriends:
                                default:
                                    friends =
                                        friends.OrderByDescending(u =>
                                            u.fb_mutualy_friends.IsNullOrEmpty()
                                                ? 0 : u.fb_mutualy_friends.Split(',').Count()).ToList();
                                    break;
                            }

                            var lst1 = friends.Select(f => f.u_username);
                            var lst2 = friends.Select(f => f.f_username);
                            var lst = lst1.Union(lst2).ToList();

                            if (ExcludeUsername.Count > 0)
                                lst = lst.Where(f => !ExcludeUsername.Contains(f)).ToList();

                            retSearchResults.Usernames = lst.ToArray();
                        }
                        else
                        {
                            retSearchResults.Usernames = null;
                        }
                    }
                }
            }
            return retSearchResults;
        }
    }
    [Serializable]
    public class OnlineFriendsSearch : BasicFriendsSearch
    {
        public bool RecentlyOnline { get; set; }

        public OnlineFriendsSearch()
        {
            RecentlyOnline = true;
            SortByColumn = OrderByColumnEnum.LastOnline;
        }

        public UserSearchResults GetResults()
        {
            return base.GetResults();
            //var retSearchResults = new UserSearchResults();
            //if (!string.IsNullOrEmpty(Username))
            //{
            //    using (var db = new ezFixUpDataContext())
            //    {
            //        var user = db.Users.Where(u => u.u_username == Username).SingleOrDefault();

            //        if (user != null)
            //        {
            //            var friends = (user.Friends.Where(
            //                f => f.User.u_active == Active && 
            //                     f.User.u_deleted == Deleted && 
            //                     f.User.u_lastonline > DateTime.Now.AddMinutes(-10)));

            //            switch (SortByColumn)
            //            {
            //                case OrderByColumnEnum.Username:
            //                    friends = friends.OrderBy(u => u.User.u_username);
            //                    break;
            //                case OrderByColumnEnum.Name:
            //                    friends = friends.OrderBy(u => u.User.u_name);
            //                    break;
            //                case OrderByColumnEnum.Email:
            //                    friends = friends.OrderBy(u => u.User.u_email);
            //                    break;
            //                case OrderByColumnEnum.Gender:
            //                    friends = friends.OrderBy(u => u.User.u_gender);
            //                    break;
            //                case OrderByColumnEnum.LastOnline:
            //                    friends = friends.OrderBy(u => u.User.u_lastonline);
            //                    break;
            //                case OrderByColumnEnum.SignupDate:
            //                    friends = friends.OrderBy(u => u.User.u_usersince);
            //                    break;
            //            }
            //            retSearchResults.Usernames = friends.Select(f => f.f_username).ToArray();
            //        }
            //    }
            //}
            //return retSearchResults;
        }
    }

    [Serializable]
    public class QuickSearch
    {
        private BasicSearch _basicSearch;

        #region properties

        public Classes.User.eGender Gender
        {
            get { return _basicSearch.Gender; }
            set { _basicSearch.Gender = value; }
        }
        public Classes.User.eGender InterestedIn
        {
            get { return _basicSearch.InterestedIn; }
            set { _basicSearch.InterestedIn = value; }
        }
        public int MinAge
        {
            get { return _basicSearch.MinAge; }
            set { _basicSearch.MinAge = value; }
        }
        public int MaxAge
        {
            get { return _basicSearch.MaxAge; }
            set { _basicSearch.MaxAge = value; }
        }
        public string Country
        {
            get { return _basicSearch.Country; }
            set { _basicSearch.Country = value; }
        }
        public string State
        {
            get { return _basicSearch.State; }
            set { _basicSearch.State = value; }
        }
        public string City
        {
            get { return _basicSearch.City; }
            set { _basicSearch.City = value; }
        }
        public string Username
        {
            get { return _basicSearch.Username; }
            set { _basicSearch.Username = value; }
        }

        public string FriendUsername { get; set; } // uses to filter results from friend list

        #endregion properties

        public QuickSearch()
        {
            _basicSearch = new BasicSearch();
        }

        public UserSearchResults GetResults()
        {
            var lUsernames = _basicSearch.GetQueryableResult()
                          .Where(u => FriendUsername.IsNullOrEmpty() ||
                                      u.Friends.Any(f => f.u_username == FriendUsername &&
                                                        (_basicSearch.ExcludeUsername.Count == 0 ||
                                                         !_basicSearch.ExcludeUsername.Contains(f.User.u_username))))
                         .Select(r => r.u_username).ToList();

            if (lUsernames.Count == 0)
                return null;

            var results = new UserSearchResults();
            results.Usernames = lUsernames.ToArray();
            return results;
        }
    }

    [Serializable]
    public class UsernameSearch
    {
        #region Properties

        private bool? hasAnswer;
        private string username;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public bool? HasAnswer
        {
            get { return hasAnswer; }
            set { hasAnswer = value; }
        }

        #endregion

        public UserSearchResults GetResults()
        {
            //using (var conn = Config.DB.Open())
            {
                var reader = 
                    SqlHelper.GetDB().ExecuteReader( "UsernameSearchUsers",
                                            "%" + username + "%", hasAnswer);

                var lResults = new List<string>();

                while (reader.Read())
                {
                    var _username = (string)reader["Username"];

                    lResults.Add(_username);
                }

                if (lResults.Count > 0)
                {
                    var results = new UserSearchResults();
                    results.Usernames = lResults.ToArray();
                    return results;
                }
                else
                    return null;
            }
        }
    }

    public class DistanceSearch
    {
        #region Properties From BasicSearch

        private Classes.User.eGender gender;
        private bool? hasAnswer;
        private int maxAge = Config.Users.MaxAge;

        private int minAge = Config.Users.MinAge;
        private bool photoReq;

        public Classes.User.eGender Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        public int MinAge
        {
            get { return minAge; }
            set
            {
                if (minAge < Config.Users.MinAge)
                    minAge = Config.Users.MinAge;
                else
                    minAge = value;
            }
        }

        public int MaxAge
        {
            get { return maxAge; }
            set
            {
                if (maxAge > Config.Users.MaxAge)
                    maxAge = Config.Users.MaxAge;
                else
                    maxAge = value;
            }
        }

        public bool PhotoReq
        {
            get { return photoReq; }
            set { photoReq = value; }
        }

        public bool? HasAnswer
        {
            get { return hasAnswer; }
            set { hasAnswer = value; }
        }

        #endregion

        #region DistanceSearchRelated Properties

        //private string fromZipCode;

        //public string FromZipCode
        //{
        //    get { return fromZipCode; }
        //    set { fromZipCode = value; }
        //}

        private double distance;
        private Location fromLocation;
        private int maxResults;

        public Location FromLocation
        {
            get { return fromLocation; }
            set { fromLocation = value; }
        }

        public double Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        public int MaxResults
        {
            get { return maxResults; }
            set { maxResults = value; }
        }

        #endregion

        public UserSearchResults GetResults()
        {
            if ( /*fromZipCode == null || fromZipCode == "" || */distance <= 0.0 || maxResults < 0)
                throw new ArgumentException("Invalid values for Distance Search");

            //Location location = ZipCode.DoLookupByZipCode(fromZipCode);

            if (fromLocation == null)
                throw new ArgumentException("There's no data for your location in our database");

            string[] usernames = Config.Users.GetUsersWithinRadius(fromLocation, hasAnswer, gender, maxAge, minAge,
                                                                   photoReq,
                                                                   distance, maxResults);

            //Location[] locations = Radius.UserLocationsWithinRadius(location, gender, maxAge, minAge, photoReq,
            //                                                        distance, maxResults);

            //string zipcodes = ZipCode.CreateCommaDelimitedZipCodeString(locations);

            ////using (var conn = Config.DB.Open())
            //{
            //    var reader = 
            //        SqlHelper.ExecuteReader(
            //            conn,
            //            "SearchUsersByZipCodes",
            //            (int) gender,
            //            DateTime.Now.Subtract(TimeSpan.FromDays((maxAge + 1)*365.25)),
            //            DateTime.Now.Subtract(TimeSpan.FromDays(minAge*365.25)),
            //            photoReq,
            //            zipcodes,
            //            maxResults);

            //    List<string> lResults = new List<string>();

            //    while (reader.Read())
            //    {
            //        string username = (string) reader["Username"];

            //        lResults.Add(username);
            //    }

            //if (lResults.Count > 0)
            if (usernames.Length > 0)
            {
                var results = new UserSearchResults();
                results.Usernames = usernames;
                return results;
            }
            else
                return null;
            //}
        }
    }

    public class OnlineSearch
    {
        #region Properties

        private Classes.User.eGender gender;
        public bool gender_isSet;
        private int? maxAge;

        private int? minAge;

        public Classes.User.eGender Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                gender_isSet = true;
            }
        }

        public int? MinAge
        {
            get { return minAge; }
            set { minAge = value; }
        }

        public int? MaxAge
        {
            get { return maxAge; }
            set { maxAge = value; }
        }

        #endregion

        public UserSearchResults GetResults()
        {
            ////using (var conn = Config.DB.Open())
            //{
            //    var reader = 
            //        SqlHelper.GetDB().ExecuteReader( "OnlineSearchUsers",
            //                                DateTime.Now.Subtract(TimeSpan.FromMinutes(Config.Users.OnlineCheckTime)),
            //                                gender_isSet ? (object)(int)gender : null,
            //                                maxAge == null ? null : (object) DateTime.Now.Subtract(TimeSpan.FromDays((maxAge.Value + 1) * 365.25)),
            //                                minAge == null ? null : (object) DateTime.Now.Subtract(TimeSpan.FromDays(minAge.Value * 365.25))
            //                                );

            //    List<string> lResults = new List<string>();

            //    while (reader.Read())
            //    {
            //        string username = (string) reader["Username"];

            //        lResults.Add(username);
            //    }

            //    if (lResults.Count > 0)
            //    {
            //        UserSearchResults results = new UserSearchResults();
            //        results.Usernames = lResults.ToArray();
            //        return results;
            //    }
            //    else
            //        return null;
            //}
            string[] onlineUsers = UserSession.getOnline(gender_isSet ? gender : (User.eGender?)null,
                                                         minAge, maxAge);

            if (onlineUsers.Length > 0)
            {
                var results = new UserSearchResults { Usernames = onlineUsers };
                return results;
            }
            return null;
        }
    }

    public class VideoBroadcastingSearch
    {
        #region Properties

        #endregion

        public UserSearchResults GetResults()
        {
            var lResults = new List<string>();

            foreach (var broadcast in VideoBroadcast.GetBroadcasts())
            {
                lResults.Add(broadcast.Key);
            }

            if (lResults.Count > 0)
            {
                var results = new UserSearchResults();
                results.Usernames = lResults.ToArray();
                return results;
            }
            else
                return null;
        }
    }

    public class BirthdaySearch
    {
        #region Fields

        private DateTime birthdate = DateTime.Now.Add(Config.Misc.TimeOffset);
        private Classes.User.eGender? gender;
        private int? maxAge;
        private int? minAge;

        #endregion

        #region Properties

        public DateTime Birthdate
        {
            get { return birthdate; }
            set { birthdate = value; }
        }

        public Classes.User.eGender? Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        public int? MinAge
        {
            get { return minAge; }
            set { minAge = value; }
        }

        public int? MaxAge
        {
            get { return maxAge; }
            set { maxAge = value; }
        }

        #endregion

        public UserSearchResults GetResults()
        {
            //using (var conn = Config.DB.Open())
            {
                var reader = 
                    SqlHelper.GetDB().ExecuteReader( "BirthdaySearchUsers",
                                            birthdate, gender,
                                            maxAge == null
                                                ? null
                                                : (object)
                                                  DateTime.Now.Subtract(TimeSpan.FromDays((maxAge.Value + 1) * 365.25)),
                                            minAge == null
                                                ? null
                                                : (object)DateTime.Now.Subtract(TimeSpan.FromDays(minAge.Value * 365.25)));

                var lResults = new List<string>();

                while (reader.Read())
                {
                    var username = (string)reader["Username"];

                    lResults.Add(username);
                }

                if (lResults.Count > 0)
                {
                    var results = new UserSearchResults();
                    results.Usernames = lResults.ToArray();
                    return results;
                }
                else
                    return null;
            }
        }
    }

    public class GroupUsersSearch
    {
        #region Fields

        public enum eSortColumn
        {
            JoinDate,
            LastOnline
        }

        private bool? active;
        private Classes.User.eGender? gender;
        private int? groupID;
        private DateTime? joinDate;
        private int maxAge = Config.Users.MaxAge;
        private int minAge = Config.Users.MinAge;
        private eSortColumn? sortColumn;
        private GroupMember.eType? type;

        #endregion

        #region Properties

        public int GroupID
        {
            get { return groupID.Value; }
            set { groupID = value; }
        }

        public Classes.User.eGender Gender
        {
            get { return gender.Value; }
            set { gender = value; }
        }

        public int MinAge
        {
            get { return minAge; }
            set
            {
                if (minAge < Config.Users.MinAge)
                    minAge = Config.Users.MinAge;
                else
                    minAge = value;
            }
        }

        public int MaxAge
        {
            get { return maxAge; }
            set
            {
                if (maxAge > Config.Users.MaxAge)
                    maxAge = Config.Users.MaxAge;
                else
                    maxAge = value;
            }
        }

        public GroupMember.eType Type
        {
            get { return type.Value; }
            set { type = value; }
        }

        public bool Active
        {
            get { return active.Value; }
            set { active = value; }
        }

        public DateTime JoinDate
        {
            get { return joinDate.Value; }
            set { joinDate = value; }
        }

        public eSortColumn? SortColumn
        {
            get { return sortColumn; }
            set { sortColumn = value; }
        }

        #endregion

        public UserSearchResults GetResults()
        {
            //using (var conn = Config.DB.Open())
            {
                var reader = 
                    SqlHelper.GetDB().ExecuteReader( "GroupUsersSearch",
                                            groupID, (int?)gender,
                                            DateTime.Now.Subtract(TimeSpan.FromDays((maxAge + 1) * 365.25)),
                                            DateTime.Now.Subtract(TimeSpan.FromDays(minAge * 365.25)),
                                            (int?)type, active, joinDate, (int?)sortColumn);

                var lResults = new List<string>();

                while (reader.Read())
                {
                    var username = (string)reader["Username"];

                    lResults.Add(username);
                }

                if (lResults.Count > 0)
                {
                    var results = new UserSearchResults();
                    results.Usernames = lResults.ToArray();
                    return results;
                }
                else
                    return null;
            }
        }
    }

    public class KeywordSearch
    {
        #region Properties

        private Classes.User.eGender interestedIn;
        public bool interestedIn_isSet;
        private string keyword;

        public string Keyword
        {
            get { return keyword; }
            set
            {
                keyword = value;
                keyword = keyword.Replace("%", "\\%");
                keyword = keyword.Replace("_", "\\_");
            }
        }

        public Classes.User.eGender InterestedIn
        {
            get { return interestedIn; }
            set
            {
                interestedIn = value;
                interestedIn_isSet = true;
            }
        }

        #endregion

        public UserSearchResults GetResults()
        {
            //using (var conn = Config.DB.Open())
            {
                var reader = 
                    SqlHelper.GetDB().ExecuteReader( "KeywordSearchUsers",
                                            Keyword,
                                            Config.Users.InterestedInFieldEnabled && interestedIn_isSet
                                                ? (object)(int)interestedIn
                                                : null);

                var lResults = new List<string>();

                while (reader.Read())
                {
                    var username = (string)reader["Username"];

                    lResults.Add(username);
                }

                if (lResults.Count > 0)
                {
                    var results = new UserSearchResults();
                    results.Usernames = lResults.ToArray();
                    return results;
                }
                else
                    return null;
            }
        }
    }

    public class MatchByLastSavedCustomSearch : CustomSearch
    {
        #region Properties

        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;

                SavedSearch savedSearch = SavedSearch.Load(_username, "_lastsearch_");

                var userSearch = new UsernameSearch { Username = _username };

                UserSearchResults results = userSearch.GetResults();
                User[] users = results.Get();
                User u = users[0];
                IList<ProfileAnswer[]> lSearchTerms = new List<ProfileAnswer[]>();

                if (savedSearch == null)
                { // if not last search saved, useing the profile answers
                    ProfileAnswer[] answers = u.FetchAnswers();
                    IList<int> lSearchIDs = answers.Select(answer => (answer.Question.FetchChoices().
                        Where(c => c.Value == answer.Value).Select(c => c.Id)).SingleOrDefault()).ToList();
                    IList<ProfileQuestion> questions = answers.Select(a => a.Question).Distinct().ToList();
                    lSearchTerms = questions.Select(question => answers.Where(a => a.Question.Id == question.Id).ToArray()).ToList();
                    savedSearch = SavedSearch.Create(_username, "_lastsearch_", u.InterestedIn, u.Country, null,
                        null, null, u.Age - 7, u.Age + 7, false, lSearchIDs.ToArray(), false, 0, null);
                    savedSearch.Save();
                }
                this.MinAge = savedSearch.AgeFrom;
                this.MaxAge = savedSearch.AgeTo;
                this.Country = savedSearch.Country;
                this.State = savedSearch.State;
                this.City = savedSearch.City;
                this.InterestedIn = savedSearch.Gender;
                this.HasPhoto = savedSearch.PhotoRequired;
                this.Answers = lSearchTerms.ToArray();
            }
        }
        public ProfileAnswer[][] Answers { get; set; }

        #endregion
    }

    public class CustomSearch : BasicSearch
    {
        #region Properties

        public ProfileAnswer[][] Answers { get; set; }

        #endregion

        public new UserSearchResults GetResults()
        {
            UserSearchResults results = base.GetResults();

            var lMatches = new List<string>();

            if (results != null)
            {
                foreach (string username in results.Usernames)
                {
                    bool isMatch = false;

                    for (int qId = 0; qId < Answers.Length; qId++)
                    {
                        isMatch = false;

                        for (int aId = 0; aId < Answers[qId].Length; aId++)
                        {
                            ProfileAnswer uAnswer;

                            try
                            {
                                uAnswer =
                                    ProfileAnswer.Fetch(username, Answers[qId][aId].Question.Id);
                            }
                            catch
                            {
                                continue;
                            }

                            foreach (string sourceVal in Answers[qId][aId].Value.Split(':'))
                                foreach (string destVal in uAnswer.Value.Split(':'))
                                    if (destVal == sourceVal)
                                    {
                                        isMatch = true;
                                        break;
                                    }
                        }

                        if (!isMatch)
                            break;
                    }

                    if (isMatch || Answers.Length == 0)
                        lMatches.Add(username);
                }

                lMatches = (List<string>)GetUsernamesOrderedByPhotosRequest(lMatches);
                results.Usernames = lMatches.ToArray();
            }

            return results;
        }
    }

    public class TopUsersSearch
    {
        #region Properties

        private Classes.User.eGender gender;

        private int minVotes = Config.Ratings.TopUsersMinVotes;

        private int usersCount = Config.Ratings.TopUsersCount;

        public Classes.User.eGender Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        public int MinVotes
        {
            get { return minVotes; }
            set { minVotes = value; }
        }

        public int UsersCount
        {
            get { return usersCount; }
            set { usersCount = value; }
        }

        #endregion

        public UserSearchResults GetResults()
        {
            string cacheKey = String.Format("TopUsersSearch_GetResults_{0}_{1}_{2}_{3}",
                                            gender, minVotes, Config.Users.TopUserMaxTimeAway, usersCount);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as UserSearchResults;
            }

            //using (var conn = Config.DB.Open())
            {
                var reader = 
                    SqlHelper.GetDB().ExecuteReader(
                                            "TopUsersSearch", (int)gender, minVotes, Config.Users.TopUserMaxTimeAway,
                                            usersCount);

                var lResults = new List<string>();

                while (reader.Read())
                {
                    var username = (string)reader["Username"];

                    lResults.Add(username);
                }

                if (lResults.Count > 0)
                {
                    var results = new UserSearchResults();
                    results.Usernames = lResults.ToArray();

                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.Cache.Insert(cacheKey, results, null, DateTime.Now.AddMinutes(1),
                                                         Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
                    }

                    return results;
                }
                else
                    return null;
            }
        }
    }

    public class TopPhotosSearch
    {
        #region Properties

        private Classes.User.eGender gender;

        private int minVotes = Config.Ratings.TopPhotosMinVotes;

        private int usersCount = Config.Ratings.TopPhotosCount;

        public Classes.User.eGender Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        public int MinVotes
        {
            get { return minVotes; }
            set { minVotes = value; }
        }

        public int UsersCount
        {
            get { return usersCount; }
            set { usersCount = value; }
        }

        #endregion

        public UserSearchResults GetResults()
        {
            using (var db = new ezFixUpDataContext())
            {
                IQueryable<string> usernames = (from p in db.Photos
                                                join u in db.Users on p.u_username equals u.u_username
                                                where u.u_gender == (int)gender
                                                      &&
                                                      u.u_lastlogin.AddDays(Config.Users.TopUserMaxTimeAway) >=
                                                      DateTime.Now
                                                      && u.u_profilevisible && !u.u_deleted
                                                      && p.p_approved && !p.p_private
                                                      && (from r1 in db.PhotoRatings
                                                          where r1.p_id == p.p_id
                                                          select 1).Count() >= Config.Ratings.TopPhotosMinVotes
                                                orderby (from r in db.PhotoRatings
                                                         where r.p_id == p.p_id
                                                         select r).Average(r => (decimal)r.pr_rating) descending
                                                select p.u_username).Take(usersCount * usersCount);

                var lUsernames = new List<string>();
                foreach (string username in usernames)
                {
                    if (!lUsernames.Contains(username))
                        lUsernames.Add(username);
                    if (lUsernames.Count >= usersCount)
                        break;
                }
                if (lUsernames.Count > 0)
                {
                    var results = new UserSearchResults { Usernames = lUsernames.ToArray() };
                    return results;
                }
                return null;
            }
        }
    }

    public class TopModeratorsSearch
    {
        #region Properties

        private Classes.User.eGender gender;

        private int usersCount = Config.CommunityModeratedSystem.TopModeratorsCount;

        public Classes.User.eGender Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        public int UsersCount
        {
            get { return usersCount; }
            set { usersCount = value; }
        }

        #endregion

        public UserSearchResults GetResults()
        {
            string cacheKey = String.Format("TopModeratorsSearch_GetResults_{0}_{1}_{2}",
                                            gender, Config.CommunityModeratedSystem.TopModeratorsMaxTimeAway, usersCount);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as UserSearchResults;
            }

            //using (var conn = Config.DB.Open())
            {
                var reader = 
                    SqlHelper.GetDB().ExecuteReader(
                                            "TopModeratorsSearch", (int)gender,
                                            Config.CommunityModeratedSystem.TopModeratorsMaxTimeAway, usersCount);

                var lResults = new List<string>();

                while (reader.Read())
                {
                    var username = (string)reader["Username"];

                    lResults.Add(username);
                }

                if (lResults.Count > 0)
                {
                    var results = new UserSearchResults();
                    results.Usernames = lResults.ToArray();

                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.Cache.Insert(cacheKey, results, null, DateTime.Now.AddMinutes(1),
                                                         Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
                    }

                    return results;
                }
                else
                    return null;
            }
        }
    }

    public class NewUsersSearch
    {
        #region Properties

        private Classes.User.eGender gender;
        private bool genderSearch;
        private bool? isFeaturedMember;
        private int? maxAge;
        private int? minAge;
        private bool photoReq;
        private bool profileReq = true;
        private int? usersCount;
        private int? pageNumber;
        private int? loginCountThreshold = null;

        private DateTime usersSince = DateTime.Parse("01/01/2012");
        private bool usersSinceSearch;

        public bool GenderSearch
        {
            get { return genderSearch; }
        }
        public bool UsersSinceSearch
        {
            get { return usersSinceSearch; }
        }

        public Classes.User.eGender Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                genderSearch = true;
            }
        }

        public DateTime UsersSince
        {
            get { return usersSince; }
            set
            {
                usersSince = value;
                usersSinceSearch = true;
            }
        }

        public int? LoginCountThreshold
        {
            get { return loginCountThreshold; }
            set { loginCountThreshold = value; }
        }

        public int? UsersCount
        {
            get { return usersCount; }
            set { usersCount = value; }
        }

        public int? PageNumber
        {
            get { return pageNumber; }
            set { pageNumber = value; }
        }

        public bool PhotoReq
        {
            get { return photoReq; }
            set { photoReq = value; }
        }

        public bool ProfileReq
        {
            get { return profileReq; }
            set { profileReq = value; }
        }

        public int? MinAge
        {
            get { return minAge; }
            set { minAge = value; }
        }

        public int? MaxAge
        {
            get { return maxAge; }
            set { maxAge = value; }
        }

        public bool? IsFeaturedMember
        {
            get { return isFeaturedMember; }
            set { isFeaturedMember = value; }
        }

        #endregion

        public UserSearchResults GetResultsFromCache(bool useCache)
        {
            if (!useCache || HttpContext.Current == null)
                return GetResults();

            string cacheKey = String.Format("NewUsersSearch_{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}_{8}",
                                            gender, usersSince, usersSinceSearch, pageNumber, usersCount, photoReq,
                                            profileReq, minAge, maxAge);

            Cache cache = HttpContext.Current.Cache;
            if (cache[cacheKey] != null && cache[cacheKey] is UserSearchResults)
            {
                return (UserSearchResults)cache[cacheKey];
            }
            else
            {
                UserSearchResults results = GetResults();
                if (results != null)
                    cache.Add(cacheKey, results, null, DateTime.Now.Add(TimeSpan.FromMinutes(10)),
                              Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
                return results;
            }
        }

        public UserSearchResults GetResults()
        {
            DateTime? fromBirthdate = !maxAge.HasValue ? (DateTime?)null : DateTime.Now.Subtract(TimeSpan.FromDays((maxAge.Value + 1) * 365.25));
            DateTime? toBirthdate = !minAge.HasValue ? (DateTime?)null : DateTime.Now.Subtract(TimeSpan.FromDays(minAge.Value * 365.25));

            using (var db = new ezFixUpDataContext())
            {
                IQueryable<string> usernames = (from u in CompiledQueries.FetchUsersForNewUsersSearchResult(db, this)
                                                where (!fromBirthdate.HasValue || (u.u_birthdate >= fromBirthdate && (u.u_gender != 3 || u.u_birthdate2 >= fromBirthdate))) &&
                                                      (!toBirthdate.HasValue || (u.u_birthdate <= toBirthdate && (u.u_gender != 3 || u.u_birthdate2 <= toBirthdate))) &&
                                                      (!loginCountThreshold.HasValue || loginCountThreshold.Value <= u.u_logincount)
                                                select u.u_username)
                                                .Skip((pageNumber ?? 0) * (usersCount ?? 0))
                                                .Take(usersCount ?? 1000);

                var usernamesArray = usernames.ToArray();

                if (usernamesArray.Length > 0)
                {
                    var results = new UserSearchResults();
                    results.Usernames = usernamesArray;
                    return results;
                }
                else
                    return null;
            }
        }
    }

    public class MutualVoteSearch
    {
        #region Properties

        private string username;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        #endregion

        public UserSearchResults GetResults()
        {
            //using (var conn = Config.DB.Open())
            {
                var reader = 
                    SqlHelper.GetDB().ExecuteReader( "MutualVoteSearch", username);

                var lResults = new List<string>();

                while (reader.Read())
                {
                    lResults.Add((string)reader["Username"]);
                }

                if (lResults.Count > 0)
                {
                    var results = new UserSearchResults();
                    results.Usernames = lResults.ToArray();
                    return results;
                }
                else
                    return null;
            }
        }
    }

    public class IrregularSearchUsers
    {
        #region Properties

        #endregion

        public UserSearchResults GetResults()
        {
            //using (var conn = Config.DB.Open())
            {
                var reader = 
                    SqlHelper.GetDB().ExecuteReader(
                                            "IrregularSearchUsers",
                                            DateTime.Now.Subtract(TimeSpan.FromDays(Config.Misc.NotVisitedSiteDays)).
                                                Date);

                var lResults = new List<string>();

                while (reader.Read())
                {
                    var username = (string)reader["Username"];

                    lResults.Add(username);
                }

                if (lResults.Count > 0)
                {
                    var results = new UserSearchResults { Usernames = lResults.ToArray() };
                    return results;
                }
                return null;
            }
        }
    }
    /*
    [Serializable]
    public class FacebookMutualFriendsSearch
    {
        #region Properties

        private string viewed;
        private string viewer;

        public string Viewer
        {
            get { return viewer; }
            set { viewer = value; }
        }

        public string Viewed
        {
            get { return viewed; }
            set { viewed = value; }
        }

        #endregion

        public UserSearchResults GetResults()
        {
            if (Viewed.IsNullOrEmpty() || Viewer.IsNullOrEmpty())
                return null;

            User viewerUser = Classes.User.Load(Viewer);
            User viewedUser = Classes.User.Load(Viewed);

            if (viewerUser == null || viewedUser == null)
                return null;

            long? fbViewerId = viewerUser.FacebookID;
            long? fbViewedId = viewedUser.FacebookID;

            if (fbViewerId == null || fbViewedId == null ||
                fbViewerId <= 0 || fbViewedId <= 0)
                return null;

            FacebookApp facebook = FacebookHelper.GetSessionFacebookApp();

            Facebook.JsonObject user;
            long id;

            user = facebook.Api("/me") as Facebook.JsonObject;

            if (user != null && user["id"] != null &&
                long.TryParse((String)user["id"], out id) &&
                id > 0 && fbViewerId == id)
            {
                JsonObject mutualFriends = facebook.Api("/me/mutualfriends",
                    new Dictionary<string, object> { { "user", viewedUser } }) as JsonObject;
                if (mutualFriends != null)
                {
                    JsonArray mutualFriendsArr = mutualFriends["data"] as JsonArray;
                    if (mutualFriendsArr != null)
                    {
                        long[] fbMutualFriendsIDs = new long[mutualFriendsArr.Count];
                        for (int i = 0; i < mutualFriendsArr.Count; i++)
                        {
                            long.TryParse((string)((JsonObject)mutualFriendsArr[i])["id"], out fbMutualFriendsIDs[i]);
                        }
                    }
                }
            }

            var lResults = new List<string>();

            //while (reader.Read())
            {
                //  lResults.Add((string) reader["Friend"]);
            }

            if (lResults.Count > 0)
            {
                var results = new UserSearchResults { Usernames = lResults.ToArray() };
                return results;
            }

            return null;
        }

    }
    */
    [Serializable]
    public class MutualFriendsSearch
    {
        #region Properties

        private string viewed;
        private string viewer;

        public string Viewer
        {
            get { return viewer; }
            set { viewer = value; }
        }

        public string Viewed
        {
            get { return viewed; }
            set { viewed = value; }
        }

        #endregion

        public UserSearchResults GetResults()
        {
            var mmyFriends = Classes.User.FetchMutualMutuallyFriends(viewer, viewed);
            if (mmyFriends != null && mmyFriends.Length > 0)
            {
                var results = new UserSearchResults { Usernames = mmyFriends };
                return results;
            }
            return null;
            ////using (var conn = Config.DB.Open())
            //{
            //    var reader = 
            //        SqlHelper.GetDB().ExecuteReader( "MutualFriendsSearch", viewer, viewed);

            //    var lResults = new List<string>();

            //    while (reader.Read())
            //    {
            //        lResults.Add((string) reader["Friend"]);
            //    }

            //    if (lResults.Count > 0)
            //    {
            //        var results = new UserSearchResults {Usernames = lResults.ToArray()};
            //        return results;
            //    }

            //    return null;
            //}
        }
    }

    [Serializable]
    public class FriendsConnectionSearch
    {
        #region Properties

        public string Viewer { get; set; }

        public string Viewed { get; set; }

        #endregion

        public UserSearchResults GetResults()
        {
            List<string> usersChain;
            bool success = Classes.User.FindFriendsConnection(Viewer, Viewed, Config.Users.MaxFriendsHops,
                                                      out usersChain);
            if (!success) return null;
            var results = new UserSearchResults { Usernames = usersChain.ToArray() };
            return results;
        }
    }

    [Serializable]
    public class QuickSingleFriendsSearch
    {
        #region Properties

        public UserSession MatchMaker { get; set; }
        public string MatchmakerUsername { get; set; }
        public Classes.User.eGenderSearch FriendGender { get; set; }
        public Classes.User.eGender FriendInterestedIn { get; set; }
        public int? MaxAge { get; set; }
        public int? MinAge { get; set; }
        public int PageNumber { get; set; }
        public int UsersCount { get; set; }
        private string _country;
        public string Country { get { return _country ?? String.Empty; } set { _country = value; } }
        private string _state;
        public string State { get { return _state ?? String.Empty; } set { _state = value; } }
        private string _city;
        public string City { get { return _city ?? String.Empty; } set { _city = value; } }
        private string _usernameSearchString;
        public string UsernameSearchString { get { return _usernameSearchString ?? String.Empty; } set { _usernameSearchString = value; } }
        #endregion

        public QuickSingleFriendsSearch
            ()
        {
            FriendGender = Classes.User.eGenderSearch.All;
            FriendInterestedIn = Classes.User.eGender.Couple;
            MinAge = Config.Users.MinAge;
            MaxAge = Config.Users.MaxAge;
            MatchMaker = PageBase.GetCurrentUserSession();
            if (MatchMaker != null)
                MatchmakerUsername = MatchMaker.Username;
        }

        //public UserSearchResults GetResults(User.eGenderSearch gender, Classes.User.eGender interestedIn)
        //{
        //    FriendGender = gender;
        //    FriendInterestedIn = interestedIn;
        //    return GetResults();
        //}

        //public UserSearchResults GetResults(string matchmakerUsername)
        //{
        //    MatchmakerUsername = matchmakerUsername;
        //    return GetResults();
        //}

        public UserSearchResults GetResults()
        {
            List<string> friends = Classes.User.FetchFriends(MatchmakerUsername).ToList();
            if (friends.Count() == 0) return null;
            if (MatchmakerHelper.IsMatchToFriendAlredySelected)
            {
                List<string> exclude = new List<string> { MatchmakerHelper.MatchToUsername };
                friends = friends.Except(exclude).ToList();
            }
            var results = new UserSearchResults { Usernames = friends.ToArray() };

            if (UsernameSearchString.IsNotNullOrEmpty())
                results.Usernames = (results.Get()
                            .Where(r => r.Username.Contains(UsernameSearchString))
                            .Select(r => r.Username)).ToArray();
            else results.Usernames = (results.Get()
                        .Where(r => r.Age >= MinAge && r.Age <= MaxAge &&
                               r.Country.Contains(Country) && r.State.Contains(State) && r.City.Contains(City))
                        .Select(r => r.Username)).ToArray();

            if (FriendGender != Classes.User.eGenderSearch.All)
            {
                results.Usernames = (results.Get()
                        .Where(r => r.Gender == (User.eGender)FriendGender &&
                            //r.InterestedIn != Classes.User.eGender.Matchmaker
                                   (r.StatusText != "In a relationship" && r.StatusText != "Married")
                                    )
                        .Select(r => r.Username)).ToArray();
            }
            if (FriendInterestedIn != Classes.User.eGender.Couple)
            {
                results.Usernames = (results.Get()
                        .Where(r => r.InterestedIn == FriendInterestedIn)
                        .Select(r => r.Username)).ToArray();
            }
            return results;
        }
    }

    [Serializable]
    public class MatchmakingSearch
    {
        #region Properties

        public string MatchmakerUsername { get; set; }

        #endregion

        //public UserSearchResults GetResults(string matchmakerUsername)
        //{
        //    MatchmakerUsername = matchmakerUsername;
        //    return GetResults();
        //}

        public UserSearchResults GetResults()
        {
            MatchMaking[] matchMakings = MatchMaking.FetchMatchMakings(MatchmakerUsername);
            if (matchMakings.Length == 0) return null;

            string usersString = matchMakings.Aggregate("",
                (current, matchMaking) => current +
                    String.Format("{0};{1};", matchMaking.Friend1Username, matchMaking.Friend2Username));
            string[] usernames = usersString.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            return new UserSearchResults { Usernames = usernames };
        }
    }
}

using System;
using System.Data.Linq;
using System.Linq;
using ezFixUp.Model;

namespace ezFixUp.Data
{
    public static class CompiledQueries
    {
        public static Func<ezFixUpDataContext, string, string, string>
            FetchDisplayedName =
                CompiledQuery.Compile(
                    (ezFixUpDataContext dbContext, string viewerUsername, string viewedUsername) =>
                    (dbContext.Friends.Where(f => f.u_username == viewerUsername && f.f_username == viewedUsername).
                          Select(f =>
                                (((f.f_is_facebook_friend.HasValue && f.f_is_facebook_friend.Value) /*|| f.User.u_paid_member*/ || f.User1.u_name.Length < 4)
                                     ? f.User1.u_name
                                     : f.User1.u_name.Substring(0, 4) + "..."))).FirstOrDefault());
        //dbContext.Friends.Where(f => f.u_username == viewerUsername && f.f_username == viewedUsername &&
        //                                f.f_is_facebook_friend.HasValue && f.f_is_facebook_friend.Value)
        //                    .Select(f => f.User1.u_name).FirstOrDefault());

        public static Func<ezFixUpDataContext, long?, IQueryable<string>>
         FetchUsernameByFacebookId =
             CompiledQuery.Compile((ezFixUpDataContext dbContext, long? fbId) =>
                    dbContext.Users.Where(u => u.u_facebookid == fbId)
                                   .Select(u => u.u_username).Distinct());

        public static Func<ezFixUpDataContext, string, IQueryable<string>>
         FetchFriendshipAcceptedByUsernames =
             CompiledQuery.Compile((ezFixUpDataContext dbContext, string username) =>
                    (from f in dbContext.Friends
                     where f.u_username == username && f.f_accepted && f.User1.u_profilevisible && !f.User1.u_deleted
                     select f.f_username).Distinct());

        public static Func<ezFixUpDataContext, string, IQueryable<string>>
         FetchUsernamesThatFriendshipAcceptedBySpecificUsername =
             CompiledQuery.Compile((ezFixUpDataContext dbContext, string username) =>
                    (from f in dbContext.Friends
                     where f.f_username == username && f.f_accepted && f.User.u_profilevisible && !f.User.u_deleted
                     select f.u_username).Distinct());

        public static Func<ezFixUpDataContext, string, string, DateTime>
         FetchFriendshipRequestTimeStamp =
             CompiledQuery.Compile((ezFixUpDataContext db, string fUsername, string uUsername) =>
                    (from f in db.Friends
                     where f.u_username == uUsername && f.f_username == fUsername
                     select f.f_timestamp).FirstOrDefault());

        public static Func<ezFixUpDataContext, string, IQueryable<string>>
         FetchFriendshipRequestsBySpecificUsername =
             CompiledQuery.Compile((ezFixUpDataContext db, string username) =>
                    (from u in db.Users
                     join f in db.Friends
                     on
                     new { u.u_username } equals new { f.u_username }
                     where f.f_username == username
                           && (from f2 in db.Friends where f2.u_username == username && f2.f_username == u.u_username select f2.u_username).Count() == 0
                     select u.u_username).Distinct());

        public static Func<ezFixUpDataContext, string, IQueryable<string>>
         FetchOpenFriendshipRequestsToUsername =
             CompiledQuery.Compile((ezFixUpDataContext db, string username) =>
                    (from f in db.Friends
                     where f.f_username == username && f.f_accepted && !f.User.u_deleted && f.User.u_active && f.User.u_profilevisible
                           && (from f2 in db.Friends where f2.u_username == username && f2.f_username == f.u_username select f2.u_username).Count() == 0
                     select f.u_username));

        public static Func<ezFixUpDataContext, string, IQueryable<string>>
         FetchOpenFriendshipRequestedByUsername =
             CompiledQuery.Compile((ezFixUpDataContext db, string username) =>
                    (from f in db.Friends
                     where f.u_username == username && f.f_accepted && !f.User1.u_deleted && f.User1.u_active && f.User1.u_profilevisible
                           && (from f2 in db.Friends where f2.f_username == username && f2.u_username == f.f_username select f2.f_username).Count() == 0
                     select f.f_username));

        public static Func<ezFixUpDataContext, string, IQueryable<Friend>>
            FetchActiveFriendsForUsername =
                CompiledQuery.Compile((ezFixUpDataContext db, string username) =>
                              (from f in db.Friends
                               where (f.u_username == username && f.User1.u_active && !f.User1.u_deleted)
                               select f));

        public static Func<ezFixUpDataContext, int, IQueryable<Friend>>
            FetchActiveFriendsByBirthday =
                CompiledQuery.Compile((ezFixUpDataContext db, int daysForward) =>
                              (from f in db.Friends
                               where (f.User.u_active && !f.User.u_deleted &&
                                      f.User1.u_active && !f.User1.u_deleted &&
                                      f.User1.u_birthdate.AddYears(DateTime.Now.Year - f.User1.u_birthdate.Year) >= DateTime.Now &&
                                      f.User1.u_birthdate.AddYears(DateTime.Now.Year - f.User1.u_birthdate.Year) <= DateTime.Now.AddDays(daysForward))
                               select f));

        public static Func<ezFixUpDataContext, string, string, IQueryable<Friend>>
            FetchFriendsSearchResultByUsername =
                CompiledQuery.Compile((ezFixUpDataContext db, string username, string friendUsername) =>
                                       from f in db.Friends
                                       where f.u_username == username && (f.f_username + f.User1.u_name).ToLower().Contains(friendUsername)
                                          && f.f_accepted && f.User1.u_active && !f.User1.u_deleted
                                       select f);

        public static Func<ezFixUpDataContext, string, BasicFriendsSearch, IQueryable<Friend>>
            FetchBasicFriendsSearchResultFiltered =
                CompiledQuery.Compile((ezFixUpDataContext db, string username, BasicFriendsSearch basicFriendsSearch) =>
                                       from f in db.Friends
                                       where f.u_username == username
                                          && f.f_accepted && f.User1.u_active && !f.User1.u_deleted
                                          && (DateTime.Now.Year - basicFriendsSearch.MinAge) >= f.User1.u_birthdate.Year
                                          && (DateTime.Now.Year - basicFriendsSearch.MaxAge) <= f.User1.u_birthdate.Year
                                          && (!basicFriendsSearch.gender_isSet || f.User1.u_gender == (int)basicFriendsSearch.Gender)
                                          && (basicFriendsSearch.Country == "" || (f.User1.u_country != null && f.User1.u_country.Contains(basicFriendsSearch.Country)))
                                          && (basicFriendsSearch.State == "" || (f.User1.u_state != null && f.User1.u_state.Contains(basicFriendsSearch.State)))
                                          && (basicFriendsSearch.City == "" || (f.User1.u_city != null && f.User1.u_city.Contains(basicFriendsSearch.City)))
                                       select f);

        public static Func<ezFixUpDataContext, NewUsersSearch, IOrderedQueryable<Model.User>>
            FetchUsersForNewUsersSearchResult =
                CompiledQuery.Compile((ezFixUpDataContext db, NewUsersSearch newUsersSearch) =>
                                      from u in db.Users
                                      where
                                          (!newUsersSearch.UsersSinceSearch || u.u_usersince >= newUsersSearch.UsersSince) &&
                                          (u.u_interested_in != (int)User.eGenderSearch.Matchmaker) &&
                                          (!newUsersSearch.GenderSearch || u.u_gender == (int)newUsersSearch.Gender) &&
                                          (!newUsersSearch.PhotoReq || (from p in db.Photos where p.p_approved && !p.p_private select p.u_username).Contains(u.u_username)) &&
                                          (!newUsersSearch.ProfileReq || (u.u_profilevisible) &&
                                          //(from pa in db.ProfileAnswers select pa.u_username).Contains(u.u_username))) &&
                                          u.u_active && !u.u_deleted && //&& u.u_face_control_approved && u.u_profilevisible 
                                          (!newUsersSearch.IsFeaturedMember.HasValue || newUsersSearch.IsFeaturedMember == u.u_featuredmember))
                                      orderby u.u_usersince descending
                                      select u);

        public static Func<ezFixUpDataContext, string, Model.User>
            FetchUserByUsername =
                CompiledQuery.Compile((ezFixUpDataContext db, string username) =>
                                      db.Users.Where(u => u.u_username == username).SingleOrDefault());

        public static Func<ezFixUpDataContext, IQueryable<string>>
         FetchFacebookUsernamesWithoutPhoto =
             CompiledQuery.Compile((ezFixUpDataContext db) =>
                    from u in
                        (from u in db.Users
                         where (u.u_logincount == 0 && u.u_facebookid.HasValue &&
                               !u.u_face_control_approved && u.u_logincount == 0 &&
                               u.u_username == u.u_facebookid.Value.ToString())
                         select u.u_username)
                    join p in db.Photos on u equals p.u_username into ps
                    from p in ps.DefaultIfEmpty()
                    where p == null || !p.p_approved
                    select u);

        public static Func<ezFixUpDataContext, IQueryable<string>>
         FetchUsernamesWithPhotos =
             CompiledQuery.Compile((ezFixUpDataContext db) =>
                    from p in db.Photos where p.p_approved select p.u_username);

        public static Func<ezFixUpDataContext, IQueryable<string>>
         FetchNew24HourUsernames =
             CompiledQuery.Compile((ezFixUpDataContext db) =>
                    from u in db.Users where u.u_usersince >= DateTime.Now.AddHours(-24) select u.u_username);

        public static Func<ezFixUpDataContext, IQueryable<string>>
         FetchNew24HourFacebookFriendsUsernames =
             CompiledQuery.Compile((ezFixUpDataContext db) =>
                    from u in db.Users
                    where u.u_usersince >= DateTime.Now.AddDays(-14) &&
                          u.u_logincount == 0 && u.u_facebookid.HasValue &&
                          !u.u_face_control_approved && u.u_logincount == 0 &&
                          u.u_username == u.u_facebookid.Value.ToString()
                    select u.u_username);

        public static Func<ezFixUpDataContext, string, string, IQueryable<string>>
         FetchViewersUsernamesForProfileViewedSearch =
             CompiledQuery.Compile((ezFixUpDataContext db, string username, string excludeUsername) =>
                    db.ProfileViews
                        .Where(v => (excludeUsername == null || v.pv_viewer != excludeUsername) &&
                                     v.pv_viewed == username && v.User.u_profilevisible && v.User1.u_profilevisible &&
                                     v.User.u_active && !v.User.u_deleted)
                        .OrderBy(v => v.pv_timestamp).Select(v => v.pv_viewer));
    }
}

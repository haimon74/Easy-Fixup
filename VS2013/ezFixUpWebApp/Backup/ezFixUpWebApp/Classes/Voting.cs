using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using ezFixUp.Model;

namespace ezFixUp.Classes
{
    public static class UserVotes
    {
        public static int FetchVotesScore(string username)
        {
            object result;
            //using (var conn = Config.DB.Open())
            {
                result = SqlHelper.GetDB().ExecuteScalar( "FetchVotesScore", username);
            }

            if (result is int)
                return (int)result;
            return 0;
        }

        public static int FetchVote(string fromUsername, string toUsername)
        {
            object result;
            //using (var conn = Config.DB.Open())
            {
                result = SqlHelper.GetDB().ExecuteScalar( "FetchUserVote", fromUsername, toUsername);
            }

            if (result != null)
                return (int)result;
            throw new NotFoundException("There is no vote!");
        }

        public static void VoteUser(string fromUsername, string toUsername, bool positive)
        {
            int points = positive ? 1 : 0; // TODO: give more points for regular users

            //using (var conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "VoteUser", fromUsername, toUsername, points);
            }
        }

        public static Dictionary<string, double> FetchTopUsers(User.eGender gender, int minAge, int maxAge,
            TimeSpan period, int count)
        {
            DateTime fromBirthdate = DateTime.Now.Subtract(TimeSpan.FromDays((maxAge + 1) * 365.25));
            DateTime toBirthdate = DateTime.Now.Subtract(TimeSpan.FromDays(minAge * 365.25));

            string cacheKey = String.Format("Votes_FetchTopUsers_{0}_{1}_{2}_{3}_{4}", gender, minAge, 
                maxAge, period, count);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return (Dictionary<string, double>) HttpContext.Current.Cache[cacheKey];
            }

            var userRatings = new Dictionary<string, double>();
            var fromDate = DateTime.Now.Subtract(period);
            var averageRating = FetchAverageRating(period, gender, minAge, maxAge);
            var averageNumberOfVotes = FetchAverageNumberOfVotes(period, gender, minAge, maxAge);
            using (var db = new ezFixUpDataContext())
            {
                foreach (var rating in (from v in db.Votes
                         join u in db.Users on v.v_tousername equals u.u_username
                         where v.v_timestamp >= fromDate && u.u_gender == (int) gender
                            && u.u_birthdate >= fromBirthdate && u.u_birthdate <= toBirthdate
                         group v by v.v_tousername
                         into uv
                             select new
                                        {
                                            Username = uv.Key,
                                            Rating = ((averageNumberOfVotes*averageRating) +
                                                      (uv.Count()*uv.Average(v => v.v_score)))
                                                     /(averageNumberOfVotes + uv.Count())
                                        }).OrderByDescending(
                    uv => uv.Rating).Take(count))
                {
                    userRatings.Add(rating.Username, rating.Rating);
                }
            }

            if (HttpContext.Current != null)
            {
                HttpContext.Current.Cache.Insert(cacheKey, userRatings, null, DateTime.Now.AddMinutes(30),
                                                 Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable,
                                                 null);
            }

            return userRatings;
        }

        //public static double FetchBayesianAverage(string username, TimeSpan period)
        //{
        //    double average;
        //    var fromDate = DateTime.Now.Subtract(period);
        //    var averageRating = FetchAverageRating(period, gender, minAge, maxAge);
        //    var averageNumberOfVotes = FetchAverageNumberOfVotes(period, gender, minAge, maxAge);
        //    using (var db = new ezFixUpDataContext())
        //    {
        //        var userNumberOfVotes = db.Votes.Count(v => v.v_tousername == username
        //                                                    && v.v_timestamp >= fromDate);
        //        var userAverageRating = db.Votes.Where(v => v.v_tousername == username
        //                                                    && v.v_timestamp >= fromDate)
        //            .Average(v => (double?) v.v_score) ?? 0;
        //        average = ((averageNumberOfVotes*averageRating) + (userNumberOfVotes*userAverageRating))
        //                  /(averageNumberOfVotes + userNumberOfVotes);
        //    }

        //    return average;
        //}

        public static double FetchAverageRating(TimeSpan period, Classes.User.eGender gender, 
            int minAge, int maxAge)
        {
            string cacheKey = String.Format("Votes_FetchAverageRating_{0}_{1}_{2}_{3}", period, gender,
                minAge, maxAge);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return (double) HttpContext.Current.Cache[cacheKey];
            }

            double average;
            var fromDate = DateTime.Now.Subtract(period);
            DateTime fromBirthdate = DateTime.Now.Subtract(TimeSpan.FromDays((maxAge + 1) * 365.25));
            DateTime toBirthdate = DateTime.Now.Subtract(TimeSpan.FromDays(minAge * 365.25));

            using (var db = new ezFixUpDataContext())
            {
                average = (from v in db.Votes
                           join u in db.Users on v.v_tousername equals u.u_username
                           where v.v_timestamp >= fromDate && u.u_gender == (int)gender
                              && u.u_birthdate >= fromBirthdate && u.u_birthdate <= toBirthdate
                           select (double?)v.v_score).Average() ?? 0;
            }

            if (HttpContext.Current != null)
            {
                HttpContext.Current.Cache.Insert(cacheKey, average, null, DateTime.Now.AddHours(1),
                                                 Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, 
                                                 null);
            }

            return average;
        }

        public static double FetchAverageNumberOfVotes(TimeSpan period, Classes.User.eGender gender, 
            int minAge, int maxAge)
        {
            string cacheKey = String.Format("Votes_FetchAverageNumberOfVotes_{0}_{1}_{2}_{3}", period, 
                gender, minAge, maxAge);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return (double)HttpContext.Current.Cache[cacheKey];
            }

            double average;
            var fromDate = DateTime.Now.Subtract(period);
            DateTime fromBirthdate = DateTime.Now.Subtract(TimeSpan.FromDays((maxAge + 1) * 365.25));
            DateTime toBirthdate = DateTime.Now.Subtract(TimeSpan.FromDays(minAge * 365.25));
            using (var db = new ezFixUpDataContext())
            {
                average = (from v in db.Votes
                           join u in db.Users on v.v_tousername equals u.u_username
                           where v.v_timestamp >= fromDate && u.u_gender == (int)gender
                              && u.u_birthdate >= fromBirthdate && u.u_birthdate <= toBirthdate
                           group v by v.v_tousername
                           into uv
                               select (double?) uv.Count()).Average() ?? 0;
                           
            }

            if (HttpContext.Current != null)
            {
                HttpContext.Current.Cache.Insert(cacheKey, average, null, DateTime.Now.AddHours(1),
                                                 Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable,
                                                 null);
            }

            return average;
        }
    }
}
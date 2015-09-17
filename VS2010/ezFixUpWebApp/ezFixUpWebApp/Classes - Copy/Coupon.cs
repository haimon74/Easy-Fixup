/* ezFixUp 
 * Copyright (C) 2003-2010 eStream 
 * lovehitch.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ezFixUp license agreement.  
 * It can be found at lovehitch.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Caching;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ezFixUp.Data
{

    public class CouponType
    {
        public enum eCouponType
        {
            PremiumMonth1 = 1,
            PremiumMonth2 = 2,
            PremiumMonth3 = 3,
            Discount10pct = 4,
            Discount15pct = 5,
            Discount20pct = 6,
            Discount25pct = 7,
            Discount30pct = 8,
            Gift1 = 9,
            Gift2 = 10,
            Gift3 = 11
        } ;

        private int _id;
        public eCouponType TypeId;
        public string Name;
        public string Description;

        public static CouponType[] FetchAll()
        {
            var types = new List<CouponType>();

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = (SqlDataReader) SqlHelper.GetDB().ExecuteReader( "FetchCouponTypes");

                while (reader.Read())
                {
                    var cType = new CouponType();
                    cType._id = (int)reader["Id"];
                    cType.TypeId = (eCouponType)cType._id;
                    cType.Name = (string)reader["Name"];
                    cType.Description = (string)reader["Descriptor"];
                    types.Add(cType);
                }
            }
            return types.ToArray();
        }
    }

    public class Coupon
    {

        #region Properties

        private int id;

        public bool OnlyForNewUser;

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string code;

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        private DateTime startDate;

        /// <summary>
        /// Gets or sets a start date for the coupon.
        /// </summary>
        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        private DateTime dueDate;

        /// <summary>
        /// Gets or sets the due date for the coupon.
        /// </summary>
        public DateTime DueDate
        {
            get { return dueDate; }
            set { dueDate = value; }
        }

        private int maxUse;

        /// <summary>
        /// Gets or set the maximum use for that coupon.
        /// </summary>
        public int MaxUse
        {
            get { return maxUse; }
            set { maxUse = value; }
        }

        private int used;

        /// <summary>
        /// Gets the used counter for that coupon.
        /// </summary>
        public int Used
        {
            get { return used; }
        }

        private int couponTypeId;

        /// <summary>
        /// Gets or set the coupon type ID.
        /// </summary>
        public int CouponTypeId
        {
            get { return couponTypeId; }
            set { couponTypeId = value; }
        }

        private string username = null;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the coupon class.
        /// </summary>
        private Coupon()
        {
        }

        public static string ABC = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        public static string GenerateCode()
        {
            int seed = (int)(DateTime.Now.Ticks % int.MaxValue);
            Random r = new Random(seed);
            string code = String.Empty;
            for (int i = 0; i < 6; i++)
            {
                code += ABC.Substring(r.Next(0, ABC.Length - 1), 1);
            }
            return code;
        }
        /// <summary>
        /// Creates the specified code.
        /// </summary>
        /// <returns>Coupon with generated code</returns>
        public static Coupon Create()
        {
            Coupon[] coupons = FetchAll();
            string code = GenerateCode();
            var codes = coupons.Select(c => c.Code).ToList();

            while (codes.Contains(code))
            {
                code = GenerateCode();
            }
            Coupon coupon = new Coupon();
            coupon.Code = code;
            coupon.id = -1;
            coupon.StartDate = DateTime.Today;
            coupon.DueDate = DateTime.Today.AddMonths(1);
            coupon.OnlyForNewUser = true;
            return coupon;
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public void Save()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                var sp = (id > 0) ? "UpdateCoupon" : "CreateCoupon";
                object result = SqlHelper.GetDB().ExecuteScalar( sp,
                                                        username,
                                                        startDate,
                                                        dueDate,
                                                        code,
                                                        maxUse,
                                                        couponTypeId,
                                                        OnlyForNewUser);

                if (id == -1)
                {
                    id = Convert.ToInt32(result);
                }
            }

            string cacheKey = String.Format("Coupon_FetchAll");
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                HttpContext.Current.Cache.Remove(cacheKey);
            }
        }

        /// <summary>
        /// Deletes the specified code.
        /// </summary>
        /// <param name="code">The code.</param>
        public static void Delete(string code)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "DeleteCoupon", code);
            }

            string cacheKey = String.Format("Coupon_FetchAll");
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                HttpContext.Current.Cache.Remove(cacheKey);
            }
        }

        /// <summary>
        /// Fetches all.
        /// </summary>
        /// <returns></returns>
        public static Coupon[] FetchAll()
        {
            string cacheKey = String.Format("Coupon_FetchAll");
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as Coupon[];
            }

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = (SqlDataReader) SqlHelper.GetDB().ExecuteReader( "FetchCoupons");

                List<Coupon> lCoupons = new List<Coupon>();

                while (reader.Read())
                {
                    Coupon coupon = new Coupon();
                    coupon.id = (int)reader["Id"];
                    coupon.code = (string)reader["Code"];
                    coupon.startDate = (DateTime)reader["StartDate"];
                    coupon.dueDate = (DateTime)reader["DueDate"];
                    coupon.maxUse = (int)reader["MaxUse"];
                    coupon.couponTypeId = (int)reader["TypeId"];
                    coupon.username = (string)reader["Username"];
                    coupon.used = (int)reader["Used"];
                    coupon.OnlyForNewUser = (bool)reader["OnlyNewUser"];
                    lCoupons.Add(coupon);
                }

                Coupon[] coupons = lCoupons.ToArray();

                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Cache.Insert(cacheKey, coupons, null, DateTime.Now.AddMinutes(30),
                            Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
                }
                return coupons;
            }
        }

        public static Coupon Fetch(string code)
        {
            Coupon coupon = new Coupon();

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = (SqlDataReader) SqlHelper.GetDB().ExecuteReader( "FetchCoupon", code);

                if (reader.Read())
                {
                    coupon.id = (int)reader["Id"];
                    coupon.code = (string)reader["Code"];
                    coupon.startDate = (DateTime)reader["StartDate"];
                    coupon.dueDate = (DateTime)reader["DueDate"];
                    coupon.maxUse = (int)reader["MaxUse"];
                    coupon.couponTypeId = (int)reader["TypeId"];
                    coupon.used = (int)reader["Used"];
                    coupon.username = (string)reader["Username"];
                    coupon.OnlyForNewUser = (bool)reader["OnlyNewUser"];
                }
            }
            return coupon;
        }

        public static void UpdateUsedCounterByAddCount(string code, int addCount)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.GetDB().ExecuteScalar( "AddCouponUsedCounter", code, addCount);
            }
        }

        public static void SaveUsernameUsesCoupon(string code, string username)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.GetDB().ExecuteScalar( "SaveCouponUsesByUsername", username, code);
            }
        }

        public static bool IsCouponUsedByUsername(string code, string username)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.GetDB().ExecuteScalar( "IsCouponUsedByUsername", username, code);
                return (bool)result;
            }
        }
    }
}
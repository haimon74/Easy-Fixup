
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Data.SqlClient;
using System.Web;
using System.Web.Caching;
using System.Web.SessionState;

namespace AspNetDating.Classes
{
    public static class Extensions
    {
        public static bool IsNullOrEmpty(this string source)
        {
            return string.IsNullOrEmpty(source);
        }
        public static bool IsNotNullOrEmpty(this string source)
        {
            return !string.IsNullOrEmpty(source);
        }
        public static string PopulateIfHasValue(this string currentValue, string newValue)
        {
            if (newValue.IsNotNullOrEmpty())
                currentValue = newValue;
            return currentValue;
        }


        public static List<T> LinqCache<T>(this Table<T> query) where T : class
        {
            string tableName = query.Context.Mapping.GetTable(typeof(T)).TableName;
            List<T> result = HttpContext.Current.Cache[tableName] as List<T>;

            if (result == null)
            {
                using (SqlConnection cn = new SqlConnection(query.Context.Connection.ConnectionString))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand(query.Context.GetCommand(query).CommandText, cn);
                    cmd.Notification = null;
                    cmd.NotificationAutoEnlist = true;
                    SqlCacheDependencyAdmin.EnableNotifications(query.Context.Connection.ConnectionString);
                    if (!SqlCacheDependencyAdmin.GetTablesEnabledForNotifications(query.Context.Connection.ConnectionString).Contains(tableName))
                    {
                        SqlCacheDependencyAdmin.EnableTableForNotifications(query.Context.Connection.ConnectionString, tableName);
                    }

                    SqlCacheDependency dependency = new SqlCacheDependency(cmd);
                    cmd.ExecuteNonQuery();

                    result = query.ToList();
                    HttpContext.Current.Cache.Insert(tableName, result, dependency);
                }
            }
            return result;
        }

        #region Session Extensions
        //public static void LanguageId(this HttpRequest  )
        #endregion
    }
}

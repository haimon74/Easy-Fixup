using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using System.Web.Caching;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Linq;

namespace ezFixUp.Classes
{
    public class UserLevel
    {
        #region Fields

        private int? id;
        private int? levelNumber;
        private int minScore;
        private string name;
        private UserLevelRestrictions restrictions;

        #endregion

        #region Properties

        public int Id
        {
            get { return id.Value; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int MinScore
        {
            get { return minScore; }
            set { minScore = value; }
        }

        public UserLevelRestrictions Restrictions
        {
            get { return restrictions; }
            set { restrictions = value; }
        }

        public int LevelNumber
        {
            get
            {
                if (!levelNumber.HasValue)
                {
                    UserLevel[] levels = LoadAll();
                    if (levels == null || levels.Length == 0) return 0;
                    for (int i = 0; i < levels.Length; i++)
                    {
                        if (levels[i].id == id.Value)
                        {
                            levelNumber = i + 1;
                            break;
                        }
                    }
                }
                return levelNumber ?? 0;
            }
        }

        #endregion

        private UserLevel()
        {
        }

        public UserLevel(string name, int minScore)
        {
            this.name = name;
            this.minScore = minScore;
        }

        public static UserLevel[] LoadAll()
        {
            string cacheKey = String.Format("UserLevel_LoadAll");
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as UserLevel[];
            }

            List<UserLevel> lLevels = new List<UserLevel>();

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = (SqlDataReader) SqlHelper.GetDB().ExecuteReader( "FetchUserLevels", null);

                while (reader.Read())
                {
                    UserLevel level = new UserLevel();

                    level.id = (int) reader["Id"];
                    level.name = (string) reader["Name"];
                    level.minScore = (int) reader["MinScore"];
                    level.restrictions = reader["Restrictions"] as string != null
                                             ? Misc.FromXml<UserLevelRestrictions>((string) reader["Restrictions"])
                                             : new UserLevelRestrictions();

                    lLevels.Add(level);
                }
            }

            if (HttpContext.Current != null)
            {
                HttpContext.Current.Cache.Insert(cacheKey, lLevels.ToArray(), null, DateTime.Now.AddHours(1),
                                                 Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
            }

            return lLevels.ToArray();
        }

        public static UserLevel Load(int id)
        {
            UserLevel[] allLevels = LoadAll();
            return Array.Find(allLevels, delegate(UserLevel level) { return level.Id == id; });
        }

        public void Save()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.GetDB().ExecuteScalar( "SaveUserLevel", id, name,
                                                        minScore, Misc.ToXml(restrictions));

                if (id == null)
                {
                    id = Convert.ToInt32(result);
                }
            }

            if (HttpContext.Current != null)
            {
                string cacheKey = String.Format("UserLevel_LoadAll");
                if (HttpContext.Current.Cache[cacheKey] != null)
                {
                    HttpContext.Current.Cache.Remove(cacheKey);
                }
            }
        }

        public static void Delete(int id)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery(
                                          "DeleteUserLevel", id);
            }

            if (HttpContext.Current != null)
            {
                string cacheKey = String.Format("UserLevel_LoadAll");
                if (HttpContext.Current.Cache[cacheKey] != null)
                {
                    HttpContext.Current.Cache.Remove(cacheKey);
                }
            }
        }

        public static UserLevel GetLevelByScore(int score)
        {
            UserLevel[] levels = LoadAll();
            if (levels == null || levels.Length == 0) return null;
            UserLevel scoredLevel = levels.First(l => l.minScore == levels.Min(lvl => lvl.minScore));
            foreach (UserLevel level in levels)
            {
                if (level.minScore <= score)
                    scoredLevel = level;
                else
                    break;
            }
            return scoredLevel;
        }

        public string GetIconUrl()
        {
            return String.Format("{0}/images/levels/{1}.gif", Config.Urls.Home, LevelNumber);
        }
    }

    [Serializable]
    [Reflection.DescriptionAttribute("Level-based Restrictions")]
    public class UserLevelRestrictions : UserRestrictions
    {
        private bool allowToModeratePhotos = false;
        private bool allowToParticipateInFaceControl = false;

        [Reflection.Description("Allow to moderate photos")]
        public bool AllowToModeratePhotos
        {
            get { return allowToModeratePhotos; }
            set { allowToModeratePhotos = value; }
        }

        [Reflection.Description("Allow to participate in face control")]
        public bool AllowToParticipateInFaceControl
        {
            get { return allowToParticipateInFaceControl; }
            set { allowToParticipateInFaceControl = value; }
        }
    }
}
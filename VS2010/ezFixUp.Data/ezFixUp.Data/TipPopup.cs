using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using System.Web.Caching;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Runtime.Serialization;

namespace ezFixUp.Classes
{
    [Serializable]
    public class TipPopup //: ISerializable
    {
        #region Properties
        private int? _id;
        private string _title = null;
        private string _key = null;
        private string _content = "";
        private int _languageId;
        private int _showTimes;
        private int _loginCountTrigger;
        private int _daysCountTrigger;
        private string _showPageName;
        private int _minCreditsTrigger;
        private int _noSpendingDaysTrigger;
        private bool _isMatchmakerTip;

        public int? ID
        {
            get { return _id; }
        }

        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        public int LanguageID
        {
            get { return _languageId; }
            set { _languageId = value; }
        }

        public int ShowTimes
        {
            get { return _showTimes; }
            set { _showTimes = value; }
        }

        public int LoginCountTrigger
        {
            get { return _loginCountTrigger; }
            set { _loginCountTrigger = value; }
        }

        public int DaysCountTrigger
        {
            get { return _daysCountTrigger; }
            set { _daysCountTrigger = value; }
        }

        public int MinCreditsTrigger
        {
            get { return _minCreditsTrigger; }
            set { _minCreditsTrigger = value; }
        }

        public string ShowPageName
        {
            get { return _showPageName; }
            set { _showPageName = value; }
        }
        public int NoSpendingDaysTrigger
        {
            get { return _noSpendingDaysTrigger; }
            set { _noSpendingDaysTrigger = value; }
        }
        public bool IsMatchmakerTip
        {
            get { return _isMatchmakerTip; }
            set { _isMatchmakerTip = value; }
        }

        #endregion

        private TipPopup()
        {
        }

        public TipPopup(string key, int languageID)
        {
            this._key = key;
            this._languageId = languageID;
        }

        /// <summary>
        /// Fetches tip by its id from database.
        /// </summary>
        /// <returns>Popup Tip object.</returns>
        public static TipPopup Fetch(int id)
        {
            TipPopup tip = null;

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = (SqlDataReader) 
                    SqlHelper.GetDB().ExecuteReader( "FetchTip", id);

                if (reader.Read())
                {
                    tip = new TipPopup
                    {
                        _id = (int)reader["ID"],
                        LanguageID = (int)reader["LanguageId"],
                        Content = (string)reader["ContentText"],
                        Key = (string)reader["NameKey"],
                        Title = (string)reader["TitleText"],
                        ShowPageName = (string)reader["ShowPageName"],
                        DaysCountTrigger = (int)reader["DaysCountTrigger"],
                        LoginCountTrigger = (int)reader["LoginCountTrigger"],
                        MinCreditsTrigger = (int)reader["MinCreditsTrigger"],
                        NoSpendingDaysTrigger = (int)reader["NoSpendingDaysTrigger"],
                        ShowTimes = (int)reader["ShowTimes"],
                        IsMatchmakerTip = (bool)reader["IsMatchmakerTip"]
                    };
                }
            }
            return tip;
        }

        /// <summary>
        /// Fetches all tips by the specified language from database.
        /// </summary>
        /// <returns>Array of tips.</returns>
        public static TipPopup[] FetchTipsByLanguage(int languageID)
        {
            string cacheKey = String.Format("TipPopup_FetchTipsByLanguage_{0}", languageID);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as TipPopup[];
            }

            List<TipPopup> lstTips = new List<TipPopup>();

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = (SqlDataReader)
                    SqlHelper.GetDB().ExecuteReader(
                                            "FetchTipsForLanguage", languageID);

                while (reader.Read())
                {
                    TipPopup tip = new TipPopup
                    {
                        _id = (int)reader["ID"],
                        LanguageID = (int)reader["LanguageId"],
                        Content = (string)reader["ContentText"],
                        Key = (string)reader["NameKey"],
                        Title = (string)reader["TitleText"],
                        ShowPageName = (string)reader["ShowPageName"],
                        DaysCountTrigger = (int)reader["DaysCountTrigger"],
                        LoginCountTrigger = (int)reader["LoginCountTrigger"],
                        MinCreditsTrigger = (int)reader["MinCreditsTrigger"],
                        NoSpendingDaysTrigger = (int)reader["NoSpendingDaysTrigger"],
                        ShowTimes = (int)reader["ShowTimes"],
                        IsMatchmakerTip = (bool)reader["IsMatchmakerTip"]
                    };
                    lstTips.Add(tip);
                }
            }
            var arr = lstTips.ToArray();
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Cache.Insert(cacheKey, arr, null, DateTime.Now.AddMinutes(30),
                    Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
            }
            return arr;
        }

        public int Save()
        {
            if (_key.IsNullOrEmpty())
                throw new Exception("tip key name not set");

            using (SqlConnection conn = Config.DB.Open())
            {
                if (_id.HasValue && _id > 0)
                {
                    SqlHelper.GetDB().ExecuteNonQuery( "SaveTip", _id, _key, _languageId, _showTimes, _loginCountTrigger, _daysCountTrigger,
                        _showPageName, _minCreditsTrigger, _noSpendingDaysTrigger, _content, _title, _isMatchmakerTip);
                }
                else
                {
                    var languages = Language.FetchAll();
                    foreach (var lang in languages)
                    {
                        object scalar = SqlHelper.GetDB().ExecuteScalar( "CreateTip", lang.Id, _showTimes, _loginCountTrigger, _daysCountTrigger,
                                                    _showPageName, _minCreditsTrigger, _noSpendingDaysTrigger, _content, _title, _key, _isMatchmakerTip);
                        if (lang.Id == this.LanguageID)
                            _id = Convert.ToInt32(scalar);
                    }
                }
            }

            string cacheKey = String.Format("TipPopup_FetchTipsByLanguage_{0}", _languageId);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                HttpContext.Current.Cache.Remove(cacheKey);
            }
            return _id ?? 0;
        }

        public static void Delete(string key)
        {
            try
            {
                //TipPopup[] tips = FetchTips(key);
                Language[] languages = Language.FetchAll();
                foreach (var language in languages)
                {
                    string cacheKey = String.Format("TipPopup_FetchTipsByLanguage_{0}", language.Id);
                    if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
                    {
                        HttpContext.Current.Cache.Remove(cacheKey);
                    }
                }

            }
            catch (Exception err)
            {
                Global.Logger.LogError(err);
            }

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "DeleteTip", key);
            }
        }

        //#region ISerializable Members

        //public TipPopup(SerializationInfo info, StreamingContext context)
        //{
        //    this.Content = info.GetString("Content");
        //    this.DaysCountTrigger = info.GetInt32("DaysCountTrigger");
        //    this._id = (int?)info.GetInt32("ID");
        //    this.IsMatchmakerTip = info.GetBoolean("IsMatchmakerTip");
        //    this.Key = info.GetString("Key");
        //    this.LanguageID = info.GetInt32("LanguageID");
        //    this.LoginCountTrigger = info.GetInt32("LoginCountTrigger");
        //    this.MinCreditsTrigger = info.GetInt32("MinCreditsTrigger");
        //    this.NoSpendingDaysTrigger = info.GetInt32("NoSpendingDaysTrigger");
        //    this.ShowPageName = info.GetString("ShowPageName");
        //    this.ShowTimes = info.GetInt32("ShowTimes");
        //    this.Title = info.GetString("Title");
        //}

        //public void GetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    info.AddValue("Content", this.Content);
        //    info.AddValue("DaysCountTrigger", this.DaysCountTrigger);
        //    info.AddValue("ID", this.ID);
        //    info.AddValue("IsMatchmakerTip", this.IsMatchmakerTip);
        //    info.AddValue("Key", this.Key);
        //    info.AddValue("LanguageID", this.LanguageID);
        //    info.AddValue("LoginCountTrigger", this.LoginCountTrigger);
        //    info.AddValue("MinCreditsTrigger", this.MinCreditsTrigger);
        //    info.AddValue("NoSpendingDaysTrigger", this.NoSpendingDaysTrigger);
        //    info.AddValue("ShowPageName", this.ShowPageName);
        //    info.AddValue("ShowTimes", this.ShowTimes);
        //    info.AddValue("Title", this.Title);
        //}

        //#endregion
    }


    [Serializable]
    public class UserTipStatus
    {
        public string Username;
        public int ViewedTimes;
        public int TipId;
        public bool IsBlocked;
        public DateTime LastViewedDate;

        private int? _id;
        public int? ID
        {
            get { return _id; }
        }

        public UserTipStatus()
        {
        }

        public int Save()
        {
            if (TipId <= 0)
                throw new Exception("tip id not set");

            if (Username.IsNullOrEmpty())
                throw new Exception("username is not set");

            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.GetDB().ExecuteScalar( "SaveUserTipStatus", TipId, Username, ViewedTimes, IsBlocked, LastViewedDate);

                if (result != null)
                    _id = Convert.ToInt32(result);
            }
            return _id ?? 0;
        }

        public static UserTipStatus[] FetchTipStatusForUsername(string username)
        {
            List<UserTipStatus> lstTips = new List<UserTipStatus>();

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = (SqlDataReader)
                    SqlHelper.GetDB().ExecuteReader(
                                            "FetchTipsStatusByUsername", username);

                while (reader.Read())
                {
                    UserTipStatus status = new UserTipStatus
                    {
                        TipId = (int)reader["TipId"],
                        ViewedTimes = (int)reader["ViewedTimes"],
                        IsBlocked = (bool)reader["IsBlocked"],
                        LastViewedDate = (DateTime)reader["LastViewedDate"]
                    };
                    lstTips.Add(status);
                }
            }
            return lstTips.ToArray();
        }
    }
}

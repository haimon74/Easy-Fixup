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
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Caching;
using ezFixUp;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ezFixUp.Classes {
	public enum TranslationType {
		Unspecified,
		Admin,
		User
	}

	/// <summary>
	/// The class that handles the language specific functionality
	/// </summary>
	public static class Lang {
		/// <summary>
		/// Translates the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		public static string Translate(this string key) {
			return Trans(key, false);
		}

		public static string TranslateA(this string key) {
			return Trans(key, true);
		}

		public static string TransA(string key) {
			return Trans(key, true);
		}

		public static string Trans(string key) {
			return Trans(key, false);
		}
        public static string TranslateToEnglish(this string key)
        {
            int languageId = 1; //english
            return Trans(languageId, key, false);
        }
		/// <summary>
		/// Translates the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		private static string Trans(string key, bool adminPanel) {
			HttpContext context = HttpContext.Current;
			if(context == null || context.Session == null || context.Session["LanguageId"] == null) {
				return Trans(Config.Misc.DefaultLanguageId, key, adminPanel);
			}

			try {
				int languageId = Convert.ToInt32(context.Session["LanguageId"]);
				return Trans(languageId, key, adminPanel);
			} catch(Exception err) {
				Global.Logger.LogError(err);
				return Trans(Config.Misc.DefaultLanguageId, key, adminPanel);
			}
		}

		/// <summary>
		/// Translates the specified language id.
		/// </summary>
		/// <param name="languageId">The language id.</param>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		public static string Trans(int languageId, string key, bool adminPanel) {
			string value = Translation.FetchTranslation(languageId, key, adminPanel);
			if (value == null || value.Trim().Length == 0)
			{
                IList<string> keys = Translation.FetchTranslationKeys(adminPanel).ToList();
                if (keys.Contains(key)) 
                    FillEmptyTranslationsWithKeyValue();
                else if (languageId == 1) AddNewKeyToAllLanguages(key, adminPanel);
			    return key;
			}
            else
			{
                return (value.StartsWith("*")) ? String.Empty : value;
			}
                
		}

        private static void FillEmptyTranslationsWithKeyValue()//string key)
        {
            string[] keys = Translation.FetchTranslationKeys(false);
            Language[] languages = Language.FetchAll();
            foreach (string key in keys)
            {
                foreach (Language language in languages)
                {
                    string value = Translation.FetchTranslation(language.Id, key, false);
                    if (value == null || value.Trim().Length == 0)
                        try{Translation.SaveTranslation(language.Id, key, key, false);}
                        catch{}
                }
            }
        }

        private static void AddNewKeyToAllLanguages(string key, bool adminPanel)
	    {
	        Language[] languages = Language.FetchAll();
	        foreach (Language language in languages)
	        {
                try { Translation.SaveTranslation(language.Id, key, key, adminPanel); }
                catch{}
	        }
	    }
	}

	/// <summary>
	/// A language
	/// </summary>
	public class Language {
		#region Properties

		private int id;

		/// <summary>
		/// Gets or sets the id.
		/// </summary>
		/// <value>The id.</value>
		public int Id {
			get { return id; }
			set { id = value; }
		}

		private string name;

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name {
			get { return name; }
			set { name = value; }
		}

		private bool active;

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Language"/> is active.
		/// </summary>
		/// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
		public bool Active {
			get { return active; }
			set { active = value; }
		}

		private bool predefined;

		/// <summary>
		/// Gets a value indicating whether this <see cref="Language"/> is predefined.
		/// </summary>
		/// <value><c>true</c> if predefined; otherwise, <c>false</c>.</value>
		public bool Predefined {
			get { return predefined; }
		}

		private string browserLanguages = null;

		public string BrowserLanguages {
			get { return browserLanguages; }
			set { browserLanguages = value; }
		}

		private string ipCountries = null;

		public string IpCountries {
			get { return ipCountries; }
			set { ipCountries = value; }
		}

		private string theme = null;

		public string Theme {
			get { return theme; }
			set { theme = value; }
		}

		private string direction = "ltr";

		public string Direction {
			get { return direction; }
			set { direction = value; }
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="Language"/> class.
		/// </summary>
		private Language() {
		}

		/// <summary>
		/// Creates the specified name.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="active">if set to <c>true</c> [active].</param>
		/// <returns></returns>
		public static Language Create(string name, bool active) {
			Language language = new Language();
			language.id = -1;
			language.name = name;
			language.active = active;

			return language;
		}

		/// <summary>
		/// Saves this instance.
		/// </summary>
		public void Save() {
			using(SqlConnection conn = Config.DB.Open()) {
				object result = SqlHelper.GetDB().ExecuteScalar( "SaveLanguage", (id > 0) ? (object)id : null,
																								name, active, browserLanguages, ipCountries, theme);

				if(id == -1) {
					id = Convert.ToInt32(result);
				}
			}

			string cacheKey = String.Format("Language_FetchAll");
			if(HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null) {
				HttpContext.Current.Cache.Remove(cacheKey);
			}
		}

		/// <summary>
		/// Deletes the specified id.
		/// </summary>
		/// <param name="id">The id.</param>
		public static void Delete(int id) {
			using(SqlConnection conn = Config.DB.Open()) {
				SqlHelper.GetDB().ExecuteNonQuery( "DeleteLanguage", id);
			}

			string cacheKey = String.Format("Language_FetchAll");
			if(HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null) {
				HttpContext.Current.Cache.Remove(cacheKey);
			}
		}

		/// <summary>
		/// Fetches the specified id. Returns NULL if the specified language doesn't exists.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		public static Language Fetch(int id) {
			Language[] languages = Fetch(id, null);

			if(languages.Length > 0) {
				return languages[0];
			} else {
				return null;
			}
		}

		/// <summary>
		/// Fetches all.
		/// </summary>
		/// <returns></returns>
		public static Language[] FetchAll() {
			string cacheKey = String.Format("Language_FetchAll");
			if(HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null) {
				return HttpContext.Current.Cache[cacheKey] as Language[];
			}

			using(SqlConnection conn = Config.DB.Open()) {
				SqlDataReader reader = (SqlDataReader) SqlHelper.GetDB().ExecuteReader( "FetchLanguages");

				List<Language> lLanguages = new List<Language>();

				while(reader.Read()) {
					Language language = new Language();
					language.id = (int)reader["Id"];
					language.name = (string)reader["Language"];
					language.active = (bool)reader["Active"];
					language.predefined = (bool)reader["Predefined"];
					language.browserLanguages = reader["BrowserLanguages"] != DBNull.Value
																					? (string)reader["BrowserLanguages"]
																					: null;
					language.ipCountries = reader["IpCountries"] != DBNull.Value ? (string)reader["IpCountries"] : null;
					language.theme = reader["Theme"] != DBNull.Value ? (string)reader["Theme"] : null;

					lLanguages.Add(language);
				}

				Language[] languages = lLanguages.ToArray();

				if(HttpContext.Current != null) {
					HttpContext.Current.Cache.Insert(cacheKey, languages, null, DateTime.Now.AddMinutes(30),
							Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
				}

				return languages;
			}
		}

		private static Language[] Fetch(int id, string name) {
			List<Language> lLanguage = new List<Language>();

			using(SqlConnection conn = Config.DB.Open()) {
				SqlDataReader reader = (SqlDataReader) SqlHelper.GetDB().ExecuteReader( "FetchLanguage", id, name);

				while(reader.Read()) {
					Language language = new Language();
					language.id = (int)reader["Id"];
					language.name = (string)reader["Name"];
					language.active = (bool)reader["Active"];
					language.predefined = (bool)reader["Predefined"];
					language.browserLanguages = reader["BrowserLanguages"] != DBNull.Value
																					? (string)reader["BrowserLanguages"]
																					: null;
					language.ipCountries = reader["IpCountries"] != DBNull.Value ? (string)reader["IpCountries"] : null;
					language.theme = reader["Theme"] != DBNull.Value ? (string)reader["Theme"] : null;
					language.direction = reader["Direction"] != DBNull.Value ? (string)reader["Direction"] : "ltr";

					lLanguage.Add(language);
				}
			}

			return lLanguage.ToArray();
		}

		/// <summary>
		/// Changes the order.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="direction">The direction.</param>
		public static void ChangeOrder(int id, eDirections direction) {
			string direction_ = "";
			switch(direction) {
				case eDirections.Up:
					direction_ = "up";
					break;
				case eDirections.Down:
					direction_ = "down";
					break;
			}

			using(SqlConnection conn = Config.DB.Open()) {
				SqlHelper.GetDB().ExecuteNonQuery( "ChangeLanguageOrder", id, direction_);
			}

			string cacheKey = String.Format("Language_FetchAll");
			if(HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null) {
				HttpContext.Current.Cache.Remove(cacheKey);
			}
		}
	}

	/// <summary>
	/// Contains the translations for all languages
	/// </summary>
	public class Translation {
		private static Hashtable htCache = new Hashtable();
		private static Hashtable htCacheA = new Hashtable();

		/// <summary>
		/// Initializes the <see cref="Translation"/> class.
		/// </summary>
		static Translation() {
		}

		/// <summary>
		/// Fetches the translation.
		/// </summary>
		/// <param name="languageId">The language id.</param>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		public static string FetchTranslation(int languageId, string key, bool adminPanel) {
			Hashtable cache;
			if(adminPanel)
				cache = htCacheA;
			else
				cache = htCache;

			Hashtable htTranslations;
			if(!cache.ContainsKey(languageId)) {
				htTranslations = new Hashtable();
				lock(cache.SyncRoot) {
					if(!cache.ContainsKey(languageId))
						cache.Add(languageId, htTranslations);
					else
						htTranslations = (Hashtable)cache[languageId];
				}
			} else {
				htTranslations = (Hashtable)cache[languageId];
			}
			if(htTranslations.ContainsKey(key))
				return htTranslations[key] as string;

			using(SqlConnection conn = Config.DB.Open()) {
				object result = SqlHelper.GetDB().ExecuteScalar( "FetchTranslation", languageId, key, (int)(adminPanel ? TranslationType.Admin : TranslationType.User));

				if(result == DBNull.Value || String.IsNullOrEmpty((string)result)) {
					result = SqlHelper.GetDB().ExecuteScalar( "FetchTranslation", languageId, key, (int)(adminPanel ? TranslationType.User : TranslationType.Admin));
					if(result == DBNull.Value || String.IsNullOrEmpty((string)result)) {
						result = "";
					}

					SaveTranslation(languageId, key, (string)result, adminPanel);
				}

				lock(htTranslations.SyncRoot) {
					if(!htTranslations.ContainsKey(key))
						htTranslations.Add(key, result);
				}

				return (string)result;
			}
		}

		/// <summary>
		/// Saves the translation.
		/// </summary>
		/// <param name="languageId">The language id.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		public static void SaveTranslation(int languageId, string key, string value, bool adminPanel) {
			Hashtable cache;
			if(adminPanel)
				cache = htCacheA;
			else
				cache = htCache;
			using(SqlConnection conn = Config.DB.Open()) {
				SqlHelper.GetDB().ExecuteNonQuery( "SaveTranslation", languageId, key, value, (int)(adminPanel ? TranslationType.Admin : TranslationType.User));

				if(cache.ContainsKey(languageId)) {
					Hashtable htTranslations = (Hashtable)cache[languageId];
					lock(htTranslations.SyncRoot) {
						if(htTranslations.ContainsKey(key))
							htTranslations[key] = value;
					}
				}
			}
		}

		/// <summary>
		/// Fetches the translation keys.
		/// </summary>
		/// <returns></returns>
		public static string[] FetchTranslationKeys(bool adminPanel) {
			List<string> lTranslationKeys = new List<string>();

			using(SqlConnection conn = Config.DB.Open()) {
				SqlDataReader reader = (SqlDataReader) SqlHelper.GetDB().ExecuteReader( "FetchTranslationKeys", (int)(adminPanel ? TranslationType.Admin : TranslationType.User));
				while(reader.Read()) {
					lTranslationKeys.Add((string)reader["Key"]);
				}
			}

			return lTranslationKeys.OrderBy(t=>t).ToArray();
		}
	}
}
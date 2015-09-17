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
using System.Web;
using System.Web.Caching;
using ezFixUp;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ezFixUp.Classes {
	
    /// <summary>
    /// A Ask About Question
	/// </summary>
	public class AskAboutQuestion {
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

		private string question;

        /// <summary>
        /// Gets or sets the display question text.
        /// </summary>
        /// <value>The display question text.</value>
        public string QuestionDisplay
        {
			set { question=value; }
			get { return question; }
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="AskAboutQuestion"/> class.
		/// </summary>
        private AskAboutQuestion()
        {
		}

		/// <summary>
		/// Creates the specified name.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="active">if set to <c>true</c> [active].</param>
		/// <returns></returns>
        public static AskAboutQuestion Create(string name,string question, bool active)
        {
            AskAboutQuestion newQuestion = new AskAboutQuestion();
            newQuestion.id = -1;
            newQuestion.name = name;
            newQuestion.QuestionDisplay = question;
            newQuestion.active = active;

            return newQuestion;
		}

		/// <summary>
		/// Saves this instance.
		/// </summary>
		public void Save() {
			using(SqlConnection conn = Config.DB.Open()) {
                object result = SqlHelper.GetDB().ExecuteScalar( "SaveAskAboutQuestion", 
                    (id > 0) ? (object)id : null, name,question, active);

				if(id == -1) {
					id = Convert.ToInt32(result);
				}
			}

            string cacheKey = String.Format("AskAboutQuestion_FetchAll");
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
                SqlHelper.GetDB().ExecuteNonQuery( "DeleteAskAboutQuestion", id);
			}

            string cacheKey = String.Format("AskAboutQuestion_FetchAll");
			if(HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null) {
				HttpContext.Current.Cache.Remove(cacheKey);
			}
		}

		/// <summary>
		/// Fetches the specified id. Returns NULL if the specified language doesn't exists.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns>
        public static AskAboutQuestion Fetch(int id)
        {
            AskAboutQuestion[] questions = Fetch(id, null,null);

			if(questions.Length > 0) {
				return questions[0];
			} else {
				return null;
			}
		}

		/// <summary>
		/// Fetches all.
		/// </summary>
		/// <returns></returns>
        public static AskAboutQuestion[] FetchAll()
        {
            string cacheKey = String.Format("AskAboutQuestion_FetchAll");
			if(HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null) {
                return HttpContext.Current.Cache[cacheKey] as AskAboutQuestion[];
			}

			using(SqlConnection conn = Config.DB.Open()) {
                SqlDataReader reader = (SqlDataReader) SqlHelper.GetDB().ExecuteReader( "FetchAskAboutQuestions");

                List<AskAboutQuestion> lQuestions = new List<AskAboutQuestion>();

				while(reader.Read()) {
                    AskAboutQuestion aQuestion = new AskAboutQuestion();
					aQuestion.id = (int)reader["Id"];
					aQuestion.name = (string)reader["Name"];
					aQuestion.active = (bool)reader["Active"];
					aQuestion.question = (string)reader["QuestionDisplay"];
					lQuestions.Add(aQuestion);
				}

                AskAboutQuestion[] questions = lQuestions.ToArray();

				if(HttpContext.Current != null) {
                    //Global.AddCacheItem("AskAboutQuestions", cacheKey, questions);
					HttpContext.Current.Cache.Insert(cacheKey, questions, null, DateTime.Now.AddMinutes(30),
							Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
				}
				return questions;
			}
		}

        private static AskAboutQuestion[] Fetch(int id, string name, string question)
        {
            List<AskAboutQuestion> lAskAboutQuestion = new List<AskAboutQuestion>();

			using(SqlConnection conn = Config.DB.Open()) {
                SqlDataReader reader = (SqlDataReader) SqlHelper.GetDB().ExecuteReader( "FetchAskAboutQuestion", id, name);

				while(reader.Read()) {
                    AskAboutQuestion aQuestion = new AskAboutQuestion();
					aQuestion.id = (int)reader["Id"];
					aQuestion.name = (string)reader["Name"];
					aQuestion.question = (string)reader["QuestionDisplay"];
					aQuestion.active = (bool)reader["Active"];
					
                    lAskAboutQuestion.Add(aQuestion);
				}
			}
            return lAskAboutQuestion.ToArray();
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
                SqlHelper.GetDB().ExecuteNonQuery( "ChangeAskAboutQuestionOrder", id, direction_);
			}
            string cacheKey = String.Format("AskAboutQuestion_FetchAll");
			if(HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null) {
				HttpContext.Current.Cache.Remove(cacheKey);
			}
		}
	}
}
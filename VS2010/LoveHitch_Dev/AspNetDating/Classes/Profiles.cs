/* ASPnetDating 
 * Copyright (C) 2003-2010 eStream 
 * lovehitch.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at lovehitch.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using System.Web.Caching;
using Microsoft.ApplicationBlocks.Data;
using System.Web.UI.HtmlControls;

namespace AspNetDating.Classes
{
    public enum eDirections
    {
        Up,
        Down
    }
    [Serializable]
    public class ProfileAnswer
    {
        #region Properties

        private string _value;
        private bool approved = true;
        private ProfileQuestion question;
        private int questionId;
        private User user;
        private string username;

        public User User
        {
            get
            {
                if (user == null)
                {
                    user = User.Load(username);
                }
                return user;
            }
            set
            {
                user = value;
                username = value.Username;
            }
        }

        public ProfileQuestion Question
        {
            get
            {
                if (question == null)
                {
                    question = ProfileQuestion.Fetch(questionId);
                }
                return question;
            }
            set { question = value; }
        }

        public string Value
        {
            get { return _value.Trim(); }
            set { _value = value.Trim(); }
        }

        public bool Approved
        {
            get { return approved; }
            set { approved = value; }
        }

        #endregion

        public ProfileAnswer()
        {
        }

        public ProfileAnswer(string Username, int QuestionID)
        {
            username = Username;
            questionId = QuestionID;
        }

        /// <summary>
        /// Fetches profile answer by user and question
        /// </summary>
        /// <param name="Username">Username</param>
        /// <param name="QuestionID">ID of the Question</param>
        /// <returns>ProfileAnswer object</returns>
        public static ProfileAnswer Fetch(string Username, int QuestionID)
        {
            string cacheKey = String.Format("ProfileAnswer_Fetch_{0}_{1}", Username, QuestionID);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as ProfileAnswer;
            }

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn, "FetchProfileAnswer",
                                            Username, QuestionID);

                var answer = new ProfileAnswer {username = Username, questionId = QuestionID};

                if (reader.Read())
                {
                    answer._value = (string) reader["Value"];
                    answer.approved = (bool) reader["Approved"];
                }
                else
                {
                    throw new NotFoundException
                        (Lang.Trans("The requested answer does not exist!"));
                }

                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Cache.Insert(cacheKey, answer, null, Cache.NoAbsoluteExpiration,
                                                     TimeSpan.FromMinutes(15), CacheItemPriority.NotRemovable, null);
                }

                return answer;
            }
        }

        /// <summary>
        /// Fetches Profile Answers from the DB for the specified topic
        /// </summary>
        /// <param name="QuestionID">ID of the question</param>
        /// <returns>Array of ProfileAnswer objects</returns>
        public static ProfileAnswer[] FetchByQuestionID(int QuestionID)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn,
                                            "FetchProfileAnswerByQuestion", QuestionID);

                var lAnswers = new List<ProfileAnswer>();

                while (reader.Read())
                {
                    var answer = new ProfileAnswer
                                     {
                                         questionId = QuestionID,
                                         username = ((string) reader["Username"]),
                                         _value = ((string) reader["Value"]),
                                         approved = ((bool) reader["Approved"])
                                     };

                    lAnswers.Add(answer);
                }

                if (lAnswers.Count > 0)
                {
                    return lAnswers.ToArray();
                }
                return null;
            }
        }

        /// <summary>
        /// Fetches Profile Answers from the DB for the specified topic
        /// </summary>
        /// <param name="Username">ID of the question</param>
        /// <returns>Array of ProfileAnswer objects</returns>
        public static ProfileAnswer[] FetchByUsername(string Username)
        {
            string cacheKey = String.Format("ProfileAnswer_Fetch_{0}", Username);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as ProfileAnswer[];
            }

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn,
                                            "FetchProfileAnswerByUser", Username);

                var lAnswers = new List<ProfileAnswer>();

                while (reader.Read())
                {
                    var answer = new ProfileAnswer
                                     {
                                         username = Username,
                                         questionId = ((int) reader["QuestionID"]),
                                         _value = ((string) reader["Value"]),
                                         approved = ((bool) reader["Approved"])
                                     };

                    lAnswers.Add(answer);
                }

                ProfileAnswer[] answers = null;
                if (lAnswers.Count > 0)
                {
                    answers = lAnswers.ToArray();
                }

                if (HttpContext.Current != null && answers != null)
                {
                        HttpContext.Current.Cache.Insert(cacheKey, answers, null, Cache.NoAbsoluteExpiration,
                                                         TimeSpan.FromMinutes(15), CacheItemPriority.NotRemovable, null);
                }

                return answers;
            }
        }

        public static ProfileAnswer[] FetchNonApproved()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn,
                                            "FetchNonApprovedAnswers");

                var lAnswers = new List<ProfileAnswer>();

                while (reader.Read())
                {
                    var answer = new ProfileAnswer
                                     {
                                         questionId = ((int) reader["QuestionID"]),
                                         username = ((string) reader["Username"]),
                                         _value = ((string) reader["Value"]),
                                         approved = ((bool) reader["Approved"])
                                     };

                    lAnswers.Add(answer);
                }

                return lAnswers.ToArray();
            }
        }

        /// <summary>
        /// Saves the answer in the DB
        /// </summary>
        public void Save()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn,
                                          "SaveProfileAnswer", username, questionId, _value, approved);
            }

            if (HttpContext.Current != null)
            {
                string cacheKey = String.Format("ProfileAnswer_Fetch_{0}_{1}", username, questionId);
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);
                cacheKey = String.Format("ProfileAnswer_Fetch_{0}", username);
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);
                cacheKey = String.Format("User_FetchSlogan_{0}", username);
                if (Question.ShowStyle == ProfileQuestion.eShowStyle.Slogan
                    && HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);

                cacheKey = String.Format("ShowUser_ProfileHtml_{0}", User.Username);
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);
            }
        }

        public static void Delete(string username, int questionID)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn,
                                          "DeleteAnswer", username, questionID);
            }

            if (HttpContext.Current != null)
            {
                string cacheKey = String.Format("ProfileAnswer_Fetch_{0}_{1}", username, questionID);
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);
                cacheKey = String.Format("ProfileAnswer_Fetch_{0}", username);
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);
                cacheKey = String.Format("ShowUser_ProfileHtml_{0}", username);
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);
            }
        }

        public void Delete()
        {
            Delete(username, questionId);
        }
    }

    public class ProfileChoice
    {
        #region Properties

        private string _value;
        private int id;
        private ProfileQuestion question;

        private int questionId;

        public int Id
        {
            get { return id; }
        }

        public int QuestionID
        {
            set
            {
                if (questionId != 0)
                {
                    throw new Exception("You can't alter existing question id!");
                }
                if (value <= 0)
                {
                    throw new Exception("Invalid question id");
                }

                questionId = value;
            }
        }

        public ProfileQuestion Question
        {
            get
            {
                if (question == null)
                {
                    question = ProfileQuestion.Fetch(questionId);
                }
                return question;
            }
            set { question = value; }
        }


        public string Value
        {
            get { return _value.Trim(); }
            set { _value = value.Trim(); }
        }

        #endregion

        public ProfileChoice()
        {
        }

        public ProfileChoice(int ID)
        {
            id = ID;
        }

        /// <summary>
        /// Fetches Profile Choice from the DB
        /// </summary>
        /// <param name="Id">Id of the choice</param>
        /// <returns>ProfileChoice object</returns>
        /// <exception cref="NotFoundException">No choice was found with the requested Id</exception>
        public static ProfileChoice Fetch(int Id)
        {
            string cacheKey = String.Format("ProfileChoice_Fetch_{0}", Id);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as ProfileChoice;
            }

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn, "FetchProfileChoice", Id);

                var choice = new ProfileChoice {id = Id};

                if (reader.Read())
                {
                    choice.questionId = (int) reader["QuestionID"];
                    choice._value = (string) reader["Value"];
                }
                else
                {
                    throw new NotFoundException
                        (Lang.Trans("The requested choice does not exist!"));
                }

                if (HttpContext.Current != null)
                {
                    //Global.AddCacheItem("ProfileChoices", cacheKey, choice);
                    HttpContext.Current.Cache.Insert(cacheKey, choice, null, Cache.NoAbsoluteExpiration,TimeSpan.FromHours(1), CacheItemPriority.NotRemovable, null);
                }

                return choice;
            }
        }

        /// <summary>
        /// Fetches Profile Choices from the DB for the specified question
        /// </summary>
        /// <param name="QuestionID">ID of the question</param>
        /// <returns>Array of ProfileChoice objects</returns>
        public static ProfileChoice[] FetchByQuestionID(int QuestionID)
        {
            string cacheKey = String.Format("ProfileChoice_FetchByQuestionID_{0}", QuestionID);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as ProfileChoice[];
            }

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn, "FetchProfileChoiceByQuestion", QuestionID);

                var lChoices = new List<ProfileChoice>();

                while (reader.Read())
                {
                    var choice = new ProfileChoice
                                     {
                                         id = ((int) reader["ID"]),
                                         questionId = QuestionID,
                                         _value = ((string) reader["Value"])
                                     };

                    lChoices.Add(choice);
                }

                ProfileChoice[] result = lChoices.ToArray();
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Cache.Insert(cacheKey, result, null, Cache.NoAbsoluteExpiration,
                                                     TimeSpan.FromHours(1), CacheItemPriority.NotRemovable, null);
                }

                if (result.Length > 0)
                {
                    return result;
                }
                return null;
            }
        }

        public void Save()
        {
            if (id == 0 && questionId == 0)
            {
                throw new Exception("Cant create choice! No question id is provided");
            }

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "SaveProfileChoice",
                                          questionId, (id > 0) ? (object) id : null, _value);
            }

            if (HttpContext.Current != null)
            {
                string cacheKey = String.Format("ProfileChoice_FetchByQuestionID_{0}", questionId);
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);
            }

            if (HttpContext.Current != null)
            {
                string cacheKey = String.Format("ProfileChoice_Fetch_{0}", id);
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);
            }
        }

        public static void Delete(int id)
        {
            int questionId = Fetch(id).questionId;

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn,
                                          "DeleteChoice", id);
            }

            if (HttpContext.Current != null)
            {
                string cacheKey = String.Format("ProfileChoice_FetchByQuestionID_{0}", questionId);
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);
            }

            if (HttpContext.Current != null)
            {
                string cacheKey = String.Format("ProfileChoice_Fetch_{0}", id);
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);
            }
        }
    }

    [Serializable]
    public class ProfileQuestion
    {
        #region Properties

        #region eEditStyle enum

        public enum eEditStyle
        {
            Hidden = 0,
            SingleLine = 1,
            MultiLine = 2,
            //Numeric					= 3,
            SingleChoiceRadio = 4,
            SingleChoiceSelect = 5,
            MultiChoiceCheck = 6,
            MultiChoiceSelect = 7

            //left for future enhancements
            //Custom					= int.MaxValue
        }

        #endregion

        #region eSearchStyle enum

        public enum eSearchStyle
        {
            Hidden = 0,
            SingleChoice = 1,
            MultiChoiceCheck = 2,
            MultiChoiceSelect = 3,
            RangeChoiceSelect = 4

            //left for future enhancements
            //Custom					= int.MaxValue
        }

        #endregion

        #region eShowStyle enum

        public enum eShowStyle
        {
            Hidden = 0,
            Slogan = 1,
            SingleChoice = 2,
            SingleLine = 3,
            MultiLine = 4,
            MultiChoiceSmall = 5,
            MultiChoiceBig = 6,
            SkypeLink = 7,
            Link = 8

            //left for future enhancements
            //Custom					= int.MaxValue
        }

        #endregion

        private string altName;
        private string description;
        private eEditStyle editStyle;
        private string hint;

        private int id;
        private int? matchField;
        private string name;
        private int priority;
        private bool required;
        private bool requiresApproval;
        private bool visibleForPaidOnly = false;
        private bool editableForPaidOnly = false;
        private int? parentQuestionID;
        private string parentQuestionChoices;
        private eSearchStyle searchStyle;
        private eShowStyle showStyle;
        private ProfileTopic topic;

        private int topicId;
        private bool visibleForCouple = true;
        private bool visibleForFemale = true;
        private bool visibleForMale = true;

        public int Id
        {
            get { return id; }
        }

        public int TopicID
        {
            set
            {
                if (topicId != 0)
                {
                    throw new Exception("You can't alter existing topicId");
                }
                if (value <= 0)
                {
                    throw new Exception("Invalid topic id");
                }
                topicId = value;
            }
            get { return topicId; }
        }

        public ProfileTopic Topic
        {
            get
            {
                if (topic == null)
                {
                    topic = ProfileTopic.Fetch(topicId);
                }
                return topic;
            }
            set { topic = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string AltName
        {
            get
            {
                return string.IsNullOrEmpty(altName) ? name : altName;
            }
            set { altName = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string Hint
        {
            get { return hint; }
            set { hint = value; }
        }

        public eEditStyle EditStyle
        {
            get { return editStyle; }
            set { editStyle = value; }
        }

        public eShowStyle ShowStyle
        {
            get { return showStyle; }
            set { showStyle = value; }
        }

        public eSearchStyle SearchStyle
        {
            get { return searchStyle; }
            set { searchStyle = value; }
        }

        public bool Required
        {
            get { return required; }
            set { required = value; }
        }

        public bool RequiresApproval
        {
            get { return requiresApproval; }
            set { requiresApproval = value; }
        }

        public bool VisibleForPaidOnly
        {
            get { return visibleForPaidOnly; }
            set { visibleForPaidOnly = value; }
        }

        public bool EditableForPaidOnly
        {
            get { return editableForPaidOnly; }
            set { editableForPaidOnly = value; }
        }

        public int? ParentQuestionID
        {
            get { return parentQuestionID; }
            set { parentQuestionID = value; }
        }

        public string ParentQuestionChoices
        {
            get { return parentQuestionChoices; }
            set { parentQuestionChoices = value; }
        }

        public int Priority
        {
            get { return priority; }
//			set { priority = value; }
        }


        public bool VisibleForMale
        {
            get { return visibleForMale; }
            set { visibleForMale = value; }
        }

        public bool VisibleForFemale
        {
            get { return visibleForFemale; }
            set { visibleForFemale = value; }
        }

        public bool VisibleForCouple
        {
            get { return visibleForCouple; }
            set { visibleForCouple = value; }
        }

        public int? MatchField
        {
            get { return matchField; }
            set { matchField = value; }
        }

        public static int? SloganQuestionID
        {
            get
            {
                const string cacheKey = "ProfileQuestion_SloganQuestionID";
                if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
                {
                    return (int) HttpContext.Current.Cache[cacheKey];
                }

                foreach (var question in Fetch())
                {
                    if (question.ShowStyle == eShowStyle.Slogan)
                    {
                        if (HttpContext.Current != null)
                        {
                            HttpContext.Current.Cache.Insert(cacheKey, question.Id, null, Cache.NoAbsoluteExpiration,
                                                             TimeSpan.FromHours(1), CacheItemPriority.NotRemovable, null);
                        }

                        return question.Id;
                    }
                }

                return null;
            }
        }

        public static int? SkypeQuestionID
        {
            get
            {
                const string cacheKey = "ProfileQuestion_SkypeQuestionID";
                if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
                {
                    return (int)HttpContext.Current.Cache[cacheKey];
                }

                foreach (var question in Fetch())
                {
                    if (question.ShowStyle == eShowStyle.SkypeLink)
                    {
                        if (HttpContext.Current != null)
                        {
                            HttpContext.Current.Cache.Insert(cacheKey, question.Id, null, Cache.NoAbsoluteExpiration,
                                                             TimeSpan.FromHours(1), CacheItemPriority.NotRemovable, null);
                        }

                        return question.Id;
                    }
                }

                return null;
            }
        }

        #endregion

        public ProfileQuestion()
        {
        }

        public ProfileQuestion(int id)
        {
            this.id = id;
        }

        /// <summary>
        /// Fetches Profile Question from the DB. Throws NotFoundException if the question doesn't exist.
        /// </summary>
        /// <param name="Id">Id of the question</param>
        /// <returns>ProfileQuestion object</returns>
        /// <exception cref="NotFoundException">No question was found with the requested Id</exception>
        public static ProfileQuestion Fetch(int Id)
        {
            string cacheKey = String.Format("ProfileQuestion_Fetch_{0}", Id);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as ProfileQuestion;
            }

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn, "FetchProfileQuestion", Id);

                var question = new ProfileQuestion {id = Id};

                if (reader.Read())
                {
                    question.topicId = (int) reader["TopicID"];
                    question.name = (string) reader["Name"];
                    question.altName = (string) reader["AltName"];
                    question.description = (string) reader["Description"];
                    question.hint = (string) reader["Hint"];
                    question.editStyle = (eEditStyle) reader["EditStyle"];
                    question.showStyle = (eShowStyle) reader["ShowStyle"];
                    question.searchStyle = (eSearchStyle) reader["SearchStyle"];
                    question.required = (bool) reader["Required"];
                    question.priority = (int) reader["Priority"];
                    question.requiresApproval = (bool) reader["RequiresApproval"];
                    question.visibleForMale = (bool) reader["VisibleForMale"];
                    question.visibleForFemale = (bool) reader["VisibleForFemale"];
                    question.visibleForCouple = (bool) reader["VisibleForCouple"];
                    question.matchField = reader["MatchField"] != DBNull.Value ? (int?) reader["MatchField"] : null;
                    question.visibleForPaidOnly = (bool) reader["ViewPaidOnly"];
                    question.editableForPaidOnly = (bool)reader["EditPaidOnly"];
                    question.parentQuestionID = reader["ParentQuestionID"] != DBNull.Value
                                                    ? (int?) reader["ParentQuestionID"]
                                                    : null;
                    question.parentQuestionChoices = reader["ParentQuestionChoices"] != DBNull.Value
                                                         ? (string) reader["ParentQuestionChoices"]
                                                         : null;
                }
                else
                {
                    throw new NotFoundException
                        (Lang.Trans("The requested question does not exist!"));
                }

                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Cache.Insert(cacheKey, question, null, Cache.NoAbsoluteExpiration,
                                                     TimeSpan.FromHours(1), CacheItemPriority.NotRemovable, null);
                }

                return question;
            }
        }

        /// <summary>
        /// Fetches Profile Questions from the DB for the specified topic
        /// </summary>
        /// <param name="TopicID">ID of the topic</param>
        /// <returns>Array of ProfileQuestion objects</returns>
        public static ProfileQuestion[] FetchByTopicID(int TopicID)
        {
            string cacheKey = String.Format("ProfileQuestion_FetchByTopicID_{0}", TopicID);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as ProfileQuestion[];
            }

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn,
                                            "FetchProfileQuestionByTopic", TopicID);

                var lQuestions = new List<ProfileQuestion>();

                while (reader.Read())
                {
                    var question = new ProfileQuestion
                                       {
                                           id = ((int) reader["ID"]),
                                           topicId = TopicID,
                                           name = ((string) reader["Name"]),
                                           altName = ((string) reader["AltName"]),
                                           description = ((string) reader["Description"]),
                                           hint = ((string) reader["Hint"]),
                                           editStyle = ((eEditStyle) reader["EditStyle"]),
                                           showStyle = ((eShowStyle) reader["ShowStyle"]),
                                           searchStyle = ((eSearchStyle) reader["SearchStyle"]),
                                           required = ((bool) reader["Required"]),
                                           priority = ((int) reader["Priority"]),
                                           requiresApproval = ((bool) reader["RequiresApproval"]),
                                           visibleForMale = ((bool) reader["VisibleForMale"]),
                                           visibleForFemale = ((bool) reader["VisibleForFemale"]),
                                           visibleForCouple = ((bool) reader["VisibleForCouple"]),
                                           matchField =
                                               (reader["MatchField"] != DBNull.Value
                                                    ? (int?) reader["MatchField"]
                                                    : null),
                                           visibleForPaidOnly = (bool) reader["ViewPaidOnly"],
                                           editableForPaidOnly = (bool)reader["EditPaidOnly"],
                                           parentQuestionID = reader["ParentQuestionID"] != DBNull.Value
                                                    ? (int?) reader["ParentQuestionID"]
                                                    : null,
                                           parentQuestionChoices = reader["ParentQuestionChoices"] != DBNull.Value
                                                         ? (string) reader["ParentQuestionChoices"]
                                                         : null
                                       };

                    lQuestions.Add(question);
                }

                if (lQuestions.Count > 0)
                {
                    ProfileQuestion[] questions = lQuestions.ToArray();

                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.Cache.Insert(cacheKey, questions, null, Cache.NoAbsoluteExpiration,
                                                         TimeSpan.FromHours(1), CacheItemPriority.NotRemovable, null);
                    }

                    return questions;
                }
                return null;
            }
        }

        public static ProfileQuestion[] Fetch()
        {
            const string cacheKey = "ProfileQuestion_Fetch";
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as ProfileQuestion[];
            }

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn,
                                            "FetchProfileQuestion", null);

                var lQuestions = new List<ProfileQuestion>();

                while (reader.Read())
                {
                    var question = new ProfileQuestion
                                       {
                                           id = ((int) reader["ID"]),
                                           topicId = ((int) reader["TopicID"]),
                                           name = ((string) reader["Name"]),
                                           altName = ((string) reader["AltName"]),
                                           description = ((string) reader["Description"]),
                                           hint = ((string) reader["Hint"]),
                                           editStyle = ((eEditStyle) reader["EditStyle"]),
                                           showStyle = ((eShowStyle) reader["ShowStyle"]),
                                           searchStyle = ((eSearchStyle) reader["SearchStyle"]),
                                           required = ((bool) reader["Required"]),
                                           priority = ((int) reader["Priority"]),
                                           requiresApproval = ((bool) reader["RequiresApproval"]),
                                           visibleForMale = ((bool) reader["VisibleForMale"]),
                                           visibleForFemale = ((bool) reader["VisibleForFemale"]),
                                           visibleForCouple = ((bool) reader["VisibleForCouple"]),
                                           matchField =
                                               (reader["MatchField"] != DBNull.Value
                                                    ? (int?) reader["MatchField"]
                                                    : null),
                                           visibleForPaidOnly = (bool)reader["ViewPaidOnly"],
                                           editableForPaidOnly = (bool)reader["EditPaidOnly"],
                                           parentQuestionID = reader["ParentQuestionID"] != DBNull.Value
                                                    ? (int?)reader["ParentQuestionID"]
                                                    : null,
                                           parentQuestionChoices = reader["ParentQuestionChoices"] != DBNull.Value
                                                         ? (string)reader["ParentQuestionChoices"]
                                                         : null
                                       };

                    lQuestions.Add(question);
                }

                ProfileQuestion[] profileQuestions = lQuestions.ToArray();
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Cache.Insert(cacheKey, profileQuestions, null, Cache.NoAbsoluteExpiration,
                                                     TimeSpan.FromHours(1), CacheItemPriority.NotRemovable, null);
                }

                return profileQuestions;
            }
        }

        /// <summary>
        /// Fetches all answers for the current question
        /// </summary>
        /// <returns>Array of ProfileAnswer</returns>
        public ProfileAnswer[] FetchAnswers()
        {
            return ProfileAnswer.FetchByQuestionID(id);
        }

        /// <summary>
        /// Fetches answer for the current question
        /// </summary>
        /// <param name="Username">Username</param>
        /// <returns>ProfileAnswer object</returns>
        public ProfileAnswer FetchAnswer(string Username)
        {
            return ProfileAnswer.Fetch(Username, id);
        }

        /// <summary>
        /// Fetches all choices for the current question
        /// </summary>
        /// <returns>Array of ProfileChoice</returns>
        public ProfileChoice[] FetchChoices()
        {
            return ProfileChoice.FetchByQuestionID(id);
        }

        /// <summary>
        /// Makes ProfileQuestion priority higher or lower depending on the chosen direction
        /// </summary>
        /// <param name="topicID"></param>
        /// <param name="questionID">id identifying ProfileQuestion's priority to be changed</param>
        /// <param name="direction">direction in which selected question is going to move in the list of topics
        /// up: alter current question priority to lower
        /// down: alter current question priority to higher
        /// </param>
        public static void ChangeOrder(int topicID, int questionID, eDirections direction)
        {
            string direction_ = "";
            switch (direction)
            {
                case eDirections.Up:
                    direction_ = "up";
                    break;
                case eDirections.Down:
                    direction_ = "down";
                    break;
            }

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "ChangeProfileQuestionOrder", topicID, questionID, direction_);
            }

            if (HttpContext.Current != null)
            {
                string cacheKey = String.Format("ProfileQuestion_FetchByTopicID_{0}", topicID);
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);
                cacheKey = String.Format("ProfileQuestion_SloganQuestionID");
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);
                cacheKey = String.Format("ProfileQuestion_SkypeQuestionID");
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);
                cacheKey = String.Format("ProfileQuestion_Fetch");
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);

                if (questionID > 0)
                {
                    cacheKey = String.Format("ProfileQuestion_Fetch_{0}", questionID);
                    if (HttpContext.Current.Cache[cacheKey] != null)
                        HttpContext.Current.Cache.Remove(cacheKey);
                }
            }
        }


        public void Save()
        {
            if (id == 0 && topicId == 0)
            {
                throw new Exception("Cant create question! No topic id is provided");
            }

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "SaveProfileQuestion",
                                          topicId, (id > 0) ? (object) id : null, name, altName, description,
                                          hint, editStyle, showStyle, searchStyle, required, requiresApproval,
                                          visibleForMale, visibleForFemale, visibleForCouple, matchField,
                                          visibleForPaidOnly, editableForPaidOnly, parentQuestionID, parentQuestionChoices);
            }

            if (HttpContext.Current != null)
            {
                string cacheKey = String.Format("ProfileQuestion_FetchByTopicID_{0}", topicId);
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);
                cacheKey = String.Format("ProfileQuestion_SloganQuestionID");
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);
                cacheKey = String.Format("ProfileQuestion_SkypeQuestionID");
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);
                cacheKey = String.Format("ProfileQuestion_Fetch");
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);
                
                if (id > 0)
                {
                    cacheKey = String.Format("ProfileQuestion_Fetch_{0}", id);
                    if (HttpContext.Current.Cache[cacheKey] != null)
                        HttpContext.Current.Cache.Remove(cacheKey);
                }
            }
        }

        public static void Delete(int id)
        {
            int topicId = Fetch(id).TopicID;

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn,
                                          "DeleteQuestion", id);
            }

            if (HttpContext.Current != null)
            {
                string cacheKey = String.Format("ProfileQuestion_Fetch_{0}", id);
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);
                cacheKey = String.Format("ProfileQuestion_SkypeQuestionID");
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);
                cacheKey = String.Format("ProfileQuestion_FetchByTopicID_{0}", topicId);
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);
                cacheKey = String.Format("ProfileQuestion_SloganQuestionID");
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);
                cacheKey = String.Format("ProfileQuestion_Fetch");
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);
            }
        }

        public bool IsVisible(User.eGender gender)
        {
            if ((gender == User.eGender.Male && visibleForMale)
                || (gender == User.eGender.Female && visibleForFemale)
                || (gender == User.eGender.Couple && visibleForCouple))
            {
                return true;
            }
            return false;
        }
    }

    [Serializable]
    public class ProfileTopic
    {
        #region Properties

        private int editColumns = 1;
        private int id;

        private string name;

        private int priority;
        private int viewColumns = 1;

        public int ID
        {
            get { return id; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        public int EditColumns
        {
            get { return editColumns; }
            set { editColumns = value; }
        }

        public int ViewColumns
        {
            get { return viewColumns; }
            set { viewColumns = value; }
        }

        #endregion

        public ProfileTopic()
        {
        }

        public ProfileTopic(int ID)
        {
            id = ID;
        }
        public static bool IsTopicCompletedByUser(string username, int topicId)
        {
            var questions = Fetch(topicId).FetchQuestions();
            foreach (ProfileQuestion profileQuestion in questions)
            {
                try
                {
                    profileQuestion.FetchAnswer(username);
                }
                catch(NotFoundException)
                {
                    return false;
                }
            }
            return true;
        }

        public static int PercentageAnsweredTopicQuestionsByUser(string username, int topicId)
        {
            var questions = Fetch(topicId).FetchQuestions();
            var questionCounter = 0;
            var answeredCounter = 0;
            foreach (ProfileQuestion profileQuestion in questions)
            {
                if (profileQuestion.VisibleForFemale && profileQuestion.VisibleForMale)
                {
                    questionCounter++;
                    try
                    {
                        profileQuestion.FetchAnswer(username);
                        answeredCounter++;
                    }
                    catch (NotFoundException){}
                }
            }
            if (questionCounter != 0)
                return answeredCounter * 100 / questionCounter;
            else return 0;
        }

        /// <summary>
        /// Fetches Profile Topic from the DB. Throws NotFoundException if the topic doesn't exist.
        /// </summary>
        /// <param name="Id">Id of the topic</param>
        /// <returns>ProfileTopic object</returns>
        /// <exception cref="NotFoundException">No topic was found with the requested Id</exception>
        public static ProfileTopic Fetch(int Id)
        {
            string cacheKey = String.Format("ProfileTopic_Fetch_{0}", Id);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as ProfileTopic;
            }

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn, "FetchProfileTopic", Id);

                var topic = new ProfileTopic {id = Id};

                if (reader.Read())
                {
                    topic.name = (string) reader["Name"];
                    topic.priority = (int) reader["Priority"];
                    topic.editColumns = (int) reader["EditColumns"];
                    topic.viewColumns = (int) reader["ViewColumns"];
                }
                else
                {
                    throw new NotFoundException
                        (Lang.Trans("The requested topic does not exist!"));
                }

                if (HttpContext.Current != null)
                {
                    //Global.AddCacheItem("ProfileTopics", cacheKey, topic);
                    HttpContext.Current.Cache.Insert(cacheKey, topic, null, Cache.NoAbsoluteExpiration,
                                                     TimeSpan.FromHours(1), CacheItemPriority.NotRemovable, null);
                }

                return topic;
            }
        }

        /// <summary>
        /// Fetches all Profile Topics from the DB
        /// </summary>
        /// <returns>Array of ProfileTopic objects</returns>
        public static ProfileTopic[] Fetch()
        {
            const string cacheKey = "ProfileTopic_Fetch";
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as ProfileTopic[];
            }

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn, "FetchProfileTopic");

                var lTopics = new List<ProfileTopic>();

                while (reader.Read())
                {
                    var topic = new ProfileTopic
                                    {
                                        id = ((int) reader["ID"]),
                                        name = ((string) reader["Name"]),
                                        priority = ((int) reader["Priority"]),
                                        editColumns = ((int) reader["EditColumns"]),
                                        viewColumns = ((int) reader["ViewColumns"])
                                    };

                    lTopics.Add(topic);
                }

                if (lTopics.Count > 0)
                {
                    ProfileTopic[] topics = lTopics.ToArray();

                    if (HttpContext.Current != null)
                    {
                        //Global.AddCacheItem("ProfileTopics", cacheKey, topics);
                        HttpContext.Current.Cache.Insert(cacheKey, topics, null, Cache.NoAbsoluteExpiration,
                                                         TimeSpan.FromHours(1), CacheItemPriority.NotRemovable, null);
                    }

                    return topics;
                }
                return null;
            }
        }

        /// <summary>
        /// Fetches all questions for the current topic
        /// </summary>
        /// <returns>Array of ProfileQuestion objects</returns>
        public ProfileQuestion[] FetchQuestions()
        {
            return ProfileQuestion.FetchByTopicID(id);
        }

        /// <summary>
        /// Creates new or updates current topic using given title and column number
        /// </summary>
        public void Save()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "SaveProfileTopic",
                                          (id > 0) ? (object) id : null, name, editColumns, viewColumns);
            }

            if (HttpContext.Current != null)
            {
                string cacheKey = "ProfileTopic_Fetch";
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);
                cacheKey = "ProfileTopic_Fetch";
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);
                if (id > 0)
                {
                    cacheKey = String.Format("ProfileTopic_Fetch_{0}", id);
                    if (HttpContext.Current.Cache[cacheKey] != null)
                        HttpContext.Current.Cache.Remove(cacheKey);
                }
            }
        }

        /// <summary>
        /// Makes ProfileTopic priority higher or lower depending on the chosen direction
        /// </summary>
        /// <param name="id">id identifying ProfileTopic's priority to be changed</param>
        /// <param name="direction">direction in which selected topic is going to move in the list of topics
        /// up: alter current topic priority to lower
        /// down: alter current topic priority to higher
        /// </param>
        public static void ChangeOrder(int id, eDirections direction)
        {
            string direction_ = "";
            switch (direction)
            {
                case eDirections.Up:
                    direction_ = "up";
                    break;
                case eDirections.Down:
                    direction_ = "down";
                    break;
            }

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "ChangeProfileTopicOrder", id, direction_);
            }

            if (HttpContext.Current != null)
            {
                string cacheKey = "ProfileTopic_Fetch";
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);
                if (id > 0)
                {
                    cacheKey = String.Format("ProfileTopic_Fetch_{0}", id);
                    if (HttpContext.Current.Cache[cacheKey] != null)
                        HttpContext.Current.Cache.Remove(cacheKey);
                }
            }
        }

        public static void Delete(int id)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn,
                                          "DeleteTopic", id);
            }

            if (HttpContext.Current != null)
            {
                string cacheKey = "ProfileTopic_Fetch";
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);
                cacheKey = String.Format("ProfileTopic_Fetch_{0}", id);
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);
            }
        }
    }

    public interface IProfileQuestionComponent
    {
        bool AdminMode { get; set; }

        User User { set; }

        ProfileQuestion Question { set; }

        ProfileAnswer Answer { get; }

        HtmlGenericControl UserControlPanel { get; }
    }

    public interface IProfileAnswerComponent
    {
        void LoadAnswer(string Username, int QuestionID);
    }

    public interface IProfileSearchComponent
    {
        ProfileQuestion Question { set; }

        ProfileAnswer[] Answers { get; }

        int[] ChoiceIds { get; set; }

        HtmlGenericControl UserControlPanel { get; }

        void ClearAnswers();
    }

    public interface ICascadeQuestionComponent
    {
        void GenerateJSForChildVisibility(Dictionary<string, object[]> dChildClientIDsWithParentQuestionChoices);
        void GenerateResetValuesJS();
    }
}
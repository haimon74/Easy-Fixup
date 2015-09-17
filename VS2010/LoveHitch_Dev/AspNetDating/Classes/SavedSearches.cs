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
using System.IO;
using System.Text;
using System.Threading;
using System.Timers;
using System.Xml.Serialization;
using Microsoft.ApplicationBlocks.Data;
using Timer = System.Timers.Timer;

namespace AspNetDating.Classes
{
    public class SavedSearch
    {
        #region Properties

        private int id = -1;

        private string username;

        private string name;

        private string country;

        private string state;

        private string zip;

        private string city;

        private int ageFrom;

        private int ageTo;

        private bool photoRequired;

        private int[] choiceIds;

        private bool emailMatches;

        private int emailFrequency = 7;

        private DateTime? nextEmailDate;

        public int Id
        {
            get { return id; }
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

        public enum eGender
        {
            Male = 1,
            Female = 2,
            Couple = 3
        }

        private User.eGender gender;

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>The gender.</value>
        public User.eGender Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        public string Country
        {
            get { return country; }
            set { country = value; }
        }

        public string State
        {
            get { return state; }
            set { state = value; }
        }

        public string Zip
        {
            get { return zip; }
            set { zip = value; }
        }

        public string City
        {
            get { return city; }
            set { city = value; }
        }

        public int AgeFrom
        {
            get { return ageFrom; }
            set { ageFrom = value; }
        }

        public int AgeTo
        {
            get { return ageTo; }
            set { ageTo = value; }
        }

        public bool PhotoRequired
        {
            get { return photoRequired; }
            set { photoRequired = value; }
        }

        public int[] ChoiceIds
        {
            get { return choiceIds; }
            set { choiceIds = value; }
        }

        public bool EmailMatches
        {
            get { return emailMatches; }
            set { emailMatches = value; }
        }

        public int EmailFrequency
        {
            get { return emailFrequency; }
            set { emailFrequency = value; }
        }

        public DateTime? NextEmailDate
        {
            get { return nextEmailDate; }
            set { nextEmailDate = value; }
        }

        #endregion

        private SavedSearch()
        {
        }

        /// <summary>
        /// Creates Saved Search object with id = -1.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="name">The name.</param>
        /// <param name="gender">The gender.</param>
        /// <param name="country">The country.</param>
        /// <param name="state">The state.</param>
        /// <param name="zip">The zip.</param>
        /// <param name="city">The city.</param>
        /// <param name="ageFrom">The age from.</param>
        /// <param name="ageTo">The age to.</param>
        /// <param name="photoRequired">if set to <c>true</c> [photo required].</param>
        /// <param name="choiceIds">The choice ids.</param>
        /// <returns>SavedSearch object</returns>
        public static SavedSearch Create(string username, string name, User.eGender gender,
                                         string country, string state, string zip, string city, int ageFrom,
                                         int ageTo, bool photoRequired, int[] choiceIds,
                                         bool emailMatches, int emailFrequency, DateTime? nextEmailDate)
        {
            SavedSearch ss = new SavedSearch();

            ss.username = username;
            ss.name = name;
            ss.gender = gender;
            ss.country = country;
            ss.state = state;
            ss.zip = zip;
            ss.city = city;
            ss.ageFrom = ageFrom;
            ss.ageTo = ageTo;
            ss.photoRequired = photoRequired;
            ss.choiceIds = choiceIds;
            ss.emailMatches = emailMatches;
            ss.emailFrequency = emailFrequency;
            ss.nextEmailDate = nextEmailDate;

            return ss;
        }

        /// <summary>
        /// Saves this instance into database.
        /// </summary>
        public void Save()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                MemoryStream ms = new MemoryStream();
                XmlSerializer xmls = new XmlSerializer(typeof(int[]));
                xmls.Serialize(ms, choiceIds);
                string xmlChoiceIds = Encoding.UTF8.GetString(ms.ToArray());

                object result = SqlHelper.ExecuteScalar(conn, "SaveSavedSearch", id, username, name, gender, country,
                                                        state, zip, city, ageFrom, ageTo, photoRequired, xmlChoiceIds,
                                                        emailMatches, emailFrequency, nextEmailDate);
                if (id == -1)
                {
                    id = Convert.ToInt32(result);
                }
            }
        }

        /// <summary>
        /// Loads SavedSearch array from database by specified id, username and name.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="username">The username.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        private static SavedSearch[] Load(object id, string username, string name)
        {
            return Load(id, username, name, null);
        }

        /// <summary>
        /// Loads SavedSearch array from database by specified id, username and name.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="username">The username.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        private static SavedSearch[] Load(object id, string username, string name, object emailMatches)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn, "LoadSavedSearch", id, username, name, emailMatches);

                SavedSearch savedSearch = null;

                List<SavedSearch> lSavedSearches = new List<SavedSearch>();
                while (reader.Read())
                {
                    savedSearch = new SavedSearch();

                    savedSearch.id = (int)reader["ID"];
                    savedSearch.username = (string)reader["Username"];
                    savedSearch.name = (string)reader["Name"];
                    savedSearch.gender = (User.eGender)reader["Gender"];
                    savedSearch.country = (string)reader["Country"];
                    savedSearch.state = (string)reader["State"];
                    savedSearch.zip = (string)reader["Zip"];
                    savedSearch.city = (string)reader["City"];
                    savedSearch.ageFrom = (int)reader["AgeFrom"];
                    savedSearch.ageTo = (int)reader["AgeTo"];
                    savedSearch.photoRequired = (bool)reader["PhotoRequired"];
                    string xmlIds = (string)reader["ChoiceIDs"];

                    MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(xmlIds));
                    XmlSerializer xmls = new XmlSerializer(typeof(int[]));
                    savedSearch.choiceIds = (int[])xmls.Deserialize(ms);

                    savedSearch.emailMatches = (bool)reader["EmailMatches"];
                    savedSearch.emailFrequency = (int)reader["EmailFrequency"];
                    savedSearch.nextEmailDate = reader["NextEmailDate"] == DBNull.Value ? null : (DateTime?)reader["NextEmailDate"];

                    lSavedSearches.Add(savedSearch);
                }
                return lSavedSearches.ToArray();
            }
        }

        /// <summary>
        /// Loads SavedSearch from database by specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static SavedSearch Load(int id)
        {
            SavedSearch[] ss = Load(id, null, null);
            if (ss.Length == 0)
            {
                return null;
            }
            else
            {
                return ss[0];
            }
        }

        /// <summary>
        /// Loads SavedSearch by specified username and name.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static SavedSearch Load(string username, string name)
        {
            SavedSearch[] ss = Load(null, username, name);
            if (ss.Length == 0)
            {
                return null;
            }
            else
            {
                return ss[0];
            }
        }

        /// <summary>
        /// Loads SavedSearch from database by specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static SavedSearch[] Load(string username)
        {
            return Load(null, username, null);
        }

        /// <summary>
        /// Deletes SavedSearch from database by specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        public static void Delete(int id)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "DeleteSavedSearch", id);
            }
        }

        public static SavedSearch[] GetSavedSearchesToMail()
        {
            return Load(null, null, null, true);
        }

        /// <summary>
        /// Sets the next email date.
        /// </summary>
        public void SetNextEmailDate()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "SetNextEmailDate", id, nextEmailDate.Value.AddDays(emailFrequency));
            }
        }
    }

    public class SavedSearchesEmailMatches
    {

        static SavedSearchesEmailMatches()
        {
        }

        private static Timer tMailer;
        private static bool mailerLock = false;

        public static void InitializeMailerTimer()
        {
            tMailer = new Timer();
            tMailer.AutoReset = true;
            tMailer.Interval = TimeSpan.FromHours(1).TotalMilliseconds;
            tMailer.Elapsed += new ElapsedEventHandler(tMailer_Elapsed);
            tMailer.Start();

            // Run processing the 1st time
            tMailer_Elapsed(null, null);
        }

        private static void tMailer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(AsyncProcessMailerQueue), null);
        }

        private static void AsyncProcessMailerQueue(object data)
        {
            if (mailerLock || DateTime.Now.DayOfWeek != DayOfWeek.Friday
                || !(DateTime.Now.Hour >= 8 && DateTime.Now.Hour <= 10))
            {
                return;
            }

            try
            {
                mailerLock = true;

                CustomSearch search = new CustomSearch();

                foreach (SavedSearch savedSearchToMail in SavedSearch.GetSavedSearchesToMail())
                {
                    User recipient = null;

                    try
                    {
                        recipient = User.Load(savedSearchToMail.Username);

                        if (!recipient.ReceiveEmails || recipient.Deleted)
                            continue;
                        
                        if (recipient.Email.ToLower().Contains("@lovehitch.com"))
                            continue;
                    }
                    catch (NotFoundException)
                    {
                        continue;
                    }

                    search.Gender = savedSearchToMail.Gender;
                    if (Config.Users.InterestedInFieldEnabled)
                    {
                        search.InterestedIn = recipient.Gender;
                    }
                    search.MinAge = savedSearchToMail.AgeFrom;
                    search.MaxAge = savedSearchToMail.AgeTo;
                    search.Country = savedSearchToMail.Country;
                    search.State = savedSearchToMail.State;
                    search.City = savedSearchToMail.City;
                    search.Zip = savedSearchToMail.Zip;
                    if (savedSearchToMail.PhotoRequired)
                        search.HasPhoto = true;
                    search.SortAsc = false;
                    search.SortColumn = "SignupDate";

                    #region Set Answers

                    int lastQuestionId = -1;
                    List<ProfileAnswer[]> groupedAnswers = new List<ProfileAnswer[]>();
                    List<ProfileAnswer> profileAnswers = new List<ProfileAnswer>();

                    foreach (int choiceId in savedSearchToMail.ChoiceIds)
                    {
                        ProfileChoice choice = null;
                        try
                        {
                            choice = ProfileChoice.Fetch(choiceId);
                        }
                        catch (NotFoundException)
                        {
                            continue;
                        }

                        if (lastQuestionId != -1 && choice.Question.Id != lastQuestionId)
                        {
                            if (profileAnswers.Count > 0)
                                groupedAnswers.Add(profileAnswers.ToArray());
                            profileAnswers.Clear();
                        }

                        ProfileAnswer answer = new ProfileAnswer(recipient.Username, choice.Question.Id);
                        answer.Value = choice.Value;
                        profileAnswers.Add(answer);

                        if (savedSearchToMail.ChoiceIds[savedSearchToMail.ChoiceIds.Length - 1] == choiceId)
                        {
                            if (profileAnswers.Count > 0)
                                groupedAnswers.Add(profileAnswers.ToArray());
                        }

                        lastQuestionId = choice.Question.Id;
                    }

                    search.Answers = groupedAnswers.ToArray();

                    #endregion

                    UserSearchResults results = search.GetResults();

                    EmailTemplates.SavedSearchMatches matchesTemplate =
                        new EmailTemplates.SavedSearchMatches(recipient.LanguageId);

                    if (results != null
                        && results.Usernames.Length >= matchesTemplate.NumberOfMatchesToMail)
                    {
                        string[] matches = new string[matchesTemplate.NumberOfMatchesToMail];

                        Array.Copy(results.Usernames, matches, matches.Length);

                        Email.Send(Config.Misc.SiteTitle, Config.Misc.SiteEmail, recipient.Name, recipient.Email,
                            matchesTemplate.GetFormattedSubject(recipient.Name),
                            matchesTemplate.GetFormattedBody(matchesTemplate, recipient.Name, matches), true);

                        savedSearchToMail.SetNextEmailDate();
                    }
                }
            }
            catch (Exception err)
            {
                Global.Logger.LogError("SavedSearchesEmailMatches", err);
            }
            finally
            {
                mailerLock = false;
            }
        }
    }
}
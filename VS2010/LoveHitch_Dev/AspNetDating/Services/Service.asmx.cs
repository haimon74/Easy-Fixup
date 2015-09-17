using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using AjaxControlToolkit;
using AspNetDating.Classes;

namespace AspNetDating.Services
{
    /// <summary>
    /// General usage web service
    /// </summary>
    [WebService(Namespace = "AspNetDating.Service")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class Service : WebService
    {
        [WebMethod(EnableSession = true, CacheDuration = 3600)]
        public CascadingDropDownNameValue[] GetLanguages(string knownCategoryValues, string category)
        {
            var values = new List<CascadingDropDownNameValue>();
            int defaultLanguageId = (int)(HttpContext.Current.Session["LanguageId"] ?? 1);
            Language[] languages = Language.FetchAll();
            foreach (Language language in languages)
            {
                if (!language.Active) continue;
                bool isDefault = (language.Id == defaultLanguageId);
                values.Add(new CascadingDropDownNameValue(language.Name, language.Id.ToString(), isDefault));
            } 
            return values.ToArray();
        }

        [WebMethod(CacheDuration = 3600)]
        public CascadingDropDownNameValue[] GetHebrewWinkCategories(string knownCategoryValues, string category)
        {
            WinkCategory[] categories = 
                WinkCategory.FetchCategoriesByLaguageId(6, EcardCategory.eSortColumn.Title);

            return categories.Select(
                winkCategory => new CascadingDropDownNameValue(
                    winkCategory.Title, winkCategory.ID.ToString(), false)).ToArray();
        }

        [WebMethod(CacheDuration = 3600)]
        public CascadingDropDownNameValue[] GetWinkCategoriesByLanguageId(string knownCategoryValues, string category)
        {
            StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(
                knownCategoryValues);
            if (!kv.ContainsKey("WinkLanguage")) return null;

            WinkCategory[] categories = 
                WinkCategory.FetchCategoriesByLaguageId(int.Parse(kv["WinkLanguage"]), EcardCategory.eSortColumn.Title);

            return categories.Select(
                winkCategory => new CascadingDropDownNameValue(
                    winkCategory.Title, winkCategory.ID.ToString(), false)).ToArray();
        }

        [WebMethod(CacheDuration = 3600)]
        public CascadingDropDownNameValue[] GetWinksByCategoryId(string knownCategoryValues, string category)
        {
            StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(
                knownCategoryValues);
            if (!kv.ContainsKey("WinkCategory")) return null;

            WinkType[] winks = 
                WinkType.FetchWinksForCategory(int.Parse(kv["WinkCategory"]));

            return winks.Select(
                wink => new CascadingDropDownNameValue(
                    wink.Phrase, wink.ID.ToString(), false)).ToArray();
        }

        [WebMethod(CacheDuration = 3600)]
        public DataTable GetAllWinks()
        {
            DataTable dtWinks = new DataTable("winks");
            dtWinks.Columns.Add("Name");
            dtWinks.Columns.Add("Value");
            WinkType[] winks = 
                WinkType.Fetch();
            foreach (WinkType wink in winks)
            {
                dtWinks.Rows.Add(wink.Phrase, wink.ID.ToString());
            }
            return dtWinks;
        }

        
        #region Used by AspNetDating Notifier

        /// <summary>
        /// Fetches the new messages.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="fromId">From id.</param>
        /// <returns></returns>
        [WebMethod(CacheDuration = 60)]
        public NewMessagesResponse FetchNewMessages(string username, string password, int fromId)
        {
            var resp = new NewMessagesResponse();

            try
            {
                Classes.User.Authorize(username, password, null);
            }
            catch (Exception err)
            {
                resp.Error = err.Message;
                return resp;
            }

            int[] messageIds = Message.Search(0, null, Message.eFolder.None, username, Message.eFolder.Inbox,
                                              7, true, true, true, false, null, null);

            var messages = new List<MessageType>();
            foreach (int messageId in messageIds)
            {
                if (messageId > fromId)
                {
                    Message msg = Message.Fetch(messageId);
                    var msgt = new MessageType
                                   {
                                       Id = msg.Id,
                                       FromUsername = msg.fromUsername,
                                       ToUsername = msg.toUsername,
                                       Timestamp = msg.Timestamp,
                                       Body = msg.Body
                                   };
                    messages.Add(msgt);
                }
            }

            resp.Messages = messages.ToArray();
            return resp;
        }

        [WebMethod(CacheDuration = 60)]
        public NewNotificationsResponse FetchNewNotification(string username, string password)
        {
            var resp = new NewNotificationsResponse();

            try
            {
                Classes.User.Authorize(username, password, null);
            }
            catch (Exception err)
            {
                resp.Error = err.Message;
                return resp;
            }

            UserSession.UpdateLastNotifierCheck(username);

            var notifications = RealtimeNotification.RetrieveNotifications(username)
                ?? new List<RealtimeNotification>();
            resp.Notifications = notifications
                    .FindAll(n => n is GenericEventNotification)
                    .ConvertAll(n => (GenericEventNotification)n)
                    .ToArray();

            return resp;
        }

        /// <summary>
        /// Generates the auth token.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        [WebMethod]
        public string GenerateAuthToken(string username, string password)
        {
            Classes.User.Authorize(username, password, null);

            return GuidAuth.Create(username);
        }

        #region Nested type: MessageType

        public struct MessageType
        {
            public string Body;
            public string FromUsername;
            public int Id;
            public DateTime Timestamp;
            public string ToUsername;
        }

        #endregion

        #region Nested type: NewMessagesResponse

        public struct NewMessagesResponse
        {
            public string Error;
            public MessageType[] Messages;
        }

        #endregion

        public struct NewNotificationsResponse
        {
            public string Error;
            public GenericEventNotification[] Notifications;
        }

        #endregion

        #region Used by AspNetDating Administration Tool

        /// <summary>
        /// Fetches the admin stats.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        [WebMethod]
        public AdminStatsResponse FetchAdminStats(string username, string password)
        {
            var resp = new AdminStatsResponse();

            try
            {
                Classes.Admin.Authorize(username, password);
            }
            catch (Exception err)
            {
                resp.Error = err.Message;
                return resp;
            }

            var stats = new AdminStats();
            Photo[] pendingPhotos = Config.CommunityFaceControlSystem.EnableCommunityFaceControl 
                ? Photo.FetchNonApproved(true) : Photo.FetchNonApproved();
            stats.PendingPhotos = pendingPhotos != null ? pendingPhotos.Length : 0;
            ProfileAnswer[] pendingAnswers = ProfileAnswer.FetchNonApproved();
            stats.PendingAnswers = pendingAnswers != null ? pendingAnswers.Length : 0;
            resp.Stats = stats;
            return resp;
        }

        /// <summary>
        /// Fetches the pending photos.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        [WebMethod]
        public PendingPhotosResponse FetchPendingPhotos(string username, string password)
        {
            var resp = new PendingPhotosResponse();

            try
            {
                Classes.Admin admin = Classes.Admin.Authorize(username, password);

                if (username != Config.Users.SystemUsername && Config.AdminSettings.AdminPermissionsEnabled
                    && (admin.Privileges.photoApproval & Classes.Admin.eAccess.Read) == 0)
                {
                    resp.Error = Lang.Trans("Access denied!");
                    return resp;
                }
            }
            catch (Exception err)
            {
                resp.Error = err.Message;
                return resp;
            }

            var pendingPhotos = new List<PendingPhoto>();
            Photo[] nonApprovedPhotos = Config.CommunityFaceControlSystem.EnableCommunityFaceControl ? Photo.FetchNonApproved(true) : Photo.FetchNonApproved();
            if (nonApprovedPhotos != null)
            {
                foreach (Photo photo in nonApprovedPhotos)
                {
                    var pendingPhoto = new PendingPhoto
                                           {
                                               Id = photo.Id,
                                               Username = photo.Username,
                                               Name = StripControlChars(photo.Name),
                                               Description = StripControlChars(photo.Description)
                                           };
                    pendingPhotos.Add(pendingPhoto);
                }
                resp.PendingPhotos = pendingPhotos.ToArray();
            }
            return resp;
        }

        /// <summary>
        /// Fetches the pending answers.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        [WebMethod]
        public PendingAnswersResponse FetchPendingAnswers(string username, string password)
        {
            var resp = new PendingAnswersResponse();

            try
            {
                Classes.Admin admin = Classes.Admin.Authorize(username, password);

                if (username != Config.Users.SystemUsername && Config.AdminSettings.AdminPermissionsEnabled
                    && (admin.Privileges.answerApproval & Classes.Admin.eAccess.Read) == 0)
                {
                    resp.Error = Lang.Trans("Access denied!");
                    return resp;
                }
            }
            catch (Exception err)
            {
                resp.Error = err.Message;
                return resp;
            }

            var pendingAnswers = new List<PendingAnswer>();
            ProfileAnswer[] nonApprovedAnswers = ProfileAnswer.FetchNonApproved();
            if (nonApprovedAnswers != null)
            {
                foreach (ProfileAnswer answer in nonApprovedAnswers)
                {
                    var pendingAnswer = new PendingAnswer
                                            {
                                                QuestionId = answer.Question.Id,
                                                Question = answer.Question.Name,
                                                Username = answer.User.Username,
                                                Value = StripControlChars(answer.Value)
                                            };
                    pendingAnswers.Add(pendingAnswer);
                }
                resp.PendingAnswers = pendingAnswers.ToArray();
            }
            return resp;
        }

        /// <summary>
        /// Approves the reject photos.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="req">The request.</param>
        /// <returns></returns>
        [WebMethod]
        public ApproveRejectPhotosResponse ApproveRejectPhotos(string username, string password,
                                                               ApproveRejectPhotosRequest req)
        {
            var resp = new ApproveRejectPhotosResponse();

            try
            {
                Classes.Admin admin = Classes.Admin.Authorize(username, password);

                if (Config.AdminSettings.ReadOnly
                    || (username != Config.Users.SystemUsername && Config.AdminSettings.AdminPermissionsEnabled
                        && (admin.Privileges.photoApproval & Classes.Admin.eAccess.Write) == 0))
                {
                    resp.Error = Lang.Trans("Access denied!");
                    return resp;
                }
            }
            catch (Exception err)
            {
                resp.Error = err.Message;
                return resp;
            }

            if (req.ApprovedPhotos != null)
            {
                foreach (ApprovedPhotoInfo photoInfo in req.ApprovedPhotos)
                {
                    Photo photo = Photo.Fetch(photoInfo.Id);
                    if (Config.Photos.EnableExplicitPhotos)
                    {
                        photo.ExplicitPhoto = photoInfo.Explicit;
                        if (photo.ExplicitPhoto && Config.Photos.MakeExplicitPhotosPrivate)
                        {
                            photo.PrivatePhoto = true;
                        }
                    }
                    photo.ApprovePhoto(username);
                    Classes.User.AddScore(photo.Username, Config.UserScores.ApprovedPhoto, "ApprovedPhoto");
                    try
                    {
                        var approvePhotoMessageTemplate =
                            new MiscTemplates.ApprovePhotoMessage(photo.User.LanguageId);
                        Message.Send(Config.Users.SystemUsername, photo.User.Username,
                                     approvePhotoMessageTemplate.Message, 0);
                    }
                    catch (NotFoundException ex)
                    {
                        Global.Logger.LogError(ex);
                    }
                }
            }

            if (req.RejectedPhotos != null)
            {
                foreach (RejectedPhotoInfo photoInfo in req.RejectedPhotos)
                {
                    Photo photo = Photo.Fetch(photoInfo.Id);
                    var rejectPhotoMessageTemplate =
                        new MiscTemplates.RejectPhotoMessage(photo.User.LanguageId);
                    string reasonMessage = rejectPhotoMessageTemplate.WithReasonMessage;

                    if (photoInfo.Reason != null && photoInfo.Reason.Trim().Length > 0)
                    {
                        reasonMessage = reasonMessage.Replace("%%REASON%%", photoInfo.Reason);
                        Message.Send(Config.Users.SystemUsername, photo.User.Username, reasonMessage, 0);
                    }
                    else
                        Message.Send(Config.Users.SystemUsername, photo.User.Username,
                                     rejectPhotoMessageTemplate.WithNoReasonMessage, 0);

                    Photo.Delete(photoInfo.Id);
                    Classes.User.AddScore(photo.Username, Config.UserScores.RejectedPhoto, "RejectedPhoto");

                    if (photoInfo.DeleteUser)
                    {
                        Classes.User.Delete(photo.Username, photoInfo.Reason);
                    }
                }
            }

            return resp;
        }

        /// <summary>
        /// Approves the reject answers.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="req">The request.</param>
        /// <returns></returns>
        [WebMethod]
        public ApproveRejectAnswersResponse ApproveRejectAnswers(string username, string password,
                                                                 ApproveRejectAnswersRequest req)
        {
            var resp = new ApproveRejectAnswersResponse();

            try
            {
                Classes.Admin admin = Classes.Admin.Authorize(username, password);

                if (Config.AdminSettings.ReadOnly
                    || (username != Config.Users.SystemUsername && Config.AdminSettings.AdminPermissionsEnabled
                        && (admin.Privileges.answerApproval & Classes.Admin.eAccess.Write) == 0))
                {
                    resp.Error = Lang.Trans("Access denied!");
                    return resp;
                }
            }
            catch (Exception err)
            {
                resp.Error = err.Message;
                return resp;
            }

            if (req.ApprovedAnswers != null)
            {
                foreach (ApprovedAnswerInfo answerInfo in req.ApprovedAnswers)
                {
                    ProfileAnswer answer = ProfileAnswer.Fetch(answerInfo.Username, answerInfo.QuestionId);
                    answer.Approved = true;
                    answer.Value = answerInfo.Value;
                    answer.Save();
                }
            }

            if (req.RejectedAnswers != null)
            {
                foreach (RejectedAnswerInfo answerInfo in req.RejectedAnswers)
                {
                    ProfileAnswer.Delete(answerInfo.Username, answerInfo.QuestionId);

                    if (answerInfo.DeleteUser)
                    {
                        Classes.User.Delete(answerInfo.Username, answerInfo.DeleteReason);
                    }
                }
            }

            return resp;
        }

        #region Nested type: AdminStats

        public struct AdminStats
        {
            public int PendingAnswers;
            public int PendingPhotos;
        }

        #endregion

        #region Nested type: AdminStatsResponse

        public struct AdminStatsResponse
        {
            public string Error;
            public AdminStats Stats;
        }

        #endregion

        #region Nested type: ApprovedAnswerInfo

        public struct ApprovedAnswerInfo
        {
            public int QuestionId;
            public string Username;
            public string Value;
        }

        #endregion

        #region Nested type: ApprovedPhotoInfo

        public struct ApprovedPhotoInfo
        {
            public bool Explicit;
            public int Id;
        }

        #endregion

        #region Nested type: ApproveRejectAnswersRequest

        public struct ApproveRejectAnswersRequest
        {
            public ApprovedAnswerInfo[] ApprovedAnswers;
            public RejectedAnswerInfo[] RejectedAnswers;
        }

        #endregion

        #region Nested type: ApproveRejectAnswersResponse

        public struct ApproveRejectAnswersResponse
        {
            public string Error;
        }

        #endregion

        #region Nested type: ApproveRejectPhotosRequest

        public struct ApproveRejectPhotosRequest
        {
            public ApprovedPhotoInfo[] ApprovedPhotos;
            public RejectedPhotoInfo[] RejectedPhotos;
        }

        #endregion

        #region Nested type: ApproveRejectPhotosResponse

        public struct ApproveRejectPhotosResponse
        {
            public string Error;
        }

        #endregion

        #region Nested type: PendingAnswer

        public struct PendingAnswer
        {
            public string Question;
            public int QuestionId;
            public string Username;
            public string Value;
        }

        #endregion

        #region Nested type: PendingAnswersResponse

        public struct PendingAnswersResponse
        {
            public string Error;
            public PendingAnswer[] PendingAnswers;
        }

        #endregion

        #region Nested type: PendingPhoto

        public struct PendingPhoto
        {
            public string Description;
            public int Id;
            public string Name;
            public string Username;
        }

        #endregion

        #region Nested type: PendingPhotosResponse

        public struct PendingPhotosResponse
        {
            public string Error;
            public PendingPhoto[] PendingPhotos;
        }

        #endregion

        #region Nested type: RejectedAnswerInfo

        public struct RejectedAnswerInfo
        {
            public string DeleteReason;
            public bool DeleteUser;
            public int QuestionId;
            public string Username;
        }

        #endregion

        #region Nested type: RejectedPhotoInfo

        public struct RejectedPhotoInfo
        {
            public bool DeleteUser;
            public int Id;
            public string Reason;
        }

        #endregion

        #endregion

        #region Used by AjaxToolkit

        /// <summary>
        /// Gets the countries.
        /// </summary>
        /// <param name="knownCategoryValues">The known category values.</param>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        //[WebMethod(CacheDuration = 3600)]
        [WebMethod(EnableSession = true)]
        public CascadingDropDownNameValue[] GetCountries(string knownCategoryValues, string category)
        {
            return GetCountries(true);
        }

        [WebMethod(CacheDuration = 3600)]
        public CascadingDropDownNameValue[] GetCountriesForSavedSearch(string knownCategoryValues, string category)
        {
            return GetCountries(false);
        }
        [WebMethod(EnableSession=true)]
        //[WebMethod(CacheDuration = 3600)]
        public static CascadingDropDownNameValue[] GetCountries(bool setDefault)
        {
            var t1 = DateTime.Now;
            var values = new List<CascadingDropDownNameValue>();
            
            //values.Add(new CascadingDropDownNameValue("", "", true));
            string defaultCountry = "ישראל".Translate();
                //Config.Users.DefaultCountry;
            string forceCountry = Config.Users.ForceCountry;

            foreach (string country in Config.Users.GetCountries())
            {
                bool isDefault = false;
                if (forceCountry.Trim().Length > 0)
                {
                    if (country.ToLower() != forceCountry.ToLower()) continue;
                    isDefault = true;
                }
                else
                {
                    if (defaultCountry.Trim().Length > 0)
                    {
                        isDefault = setDefault && country.ToLower() == defaultCountry.ToLower();
                    }
                }
                var displayName = GetHebrewLocationTranslated(country);
                values.Add(new CascadingDropDownNameValue(displayName, country, isDefault));
            }
            values = values.OrderBy(x => x.name).ToList();
            var t2 = DateTime.Now;
            var dt = t2 - t1;
            return values.ToArray();
        }
        private static int GetSessionLanguageId()
        {
            return HttpContext.Current.Session != null ? (int)HttpContext.Current.Session["LanguageId"] : 6;            
        }

        private static string GetHebrewLocationTranslated(string location)
        {
            int sessionLanguageId = GetSessionLanguageId();
            string displayName = location;
            if (sessionLanguageId != 6)
            {// not hebrew session case
                char ch1 = location.ToCharArray()[0];
                if (ch1 >= 'א' && ch1 <= 'ת')
                {
                    displayName = location.TranslateToEnglish();
                }
            }
            return displayName;
        }
        public class GetCountriesState
        {
            public object previousState;
            public GetCountriesAsyncStub asyncStub;
        }

        [WebMethod]
        public IAsyncResult BeginGetCountries(string knownCategoryValues,
                                                     string category,
                                                     AsyncCallback cb,
                                                     object s)
        {
            GetCountriesAsyncStub stub = new GetCountriesAsyncStub(GetCountries);
            var ms = new GetCountriesState();
            ms.previousState = s;
            ms.asyncStub = stub;
            return stub.BeginInvoke(knownCategoryValues, category, cb, ms);
        }

        [WebMethod]
        public CascadingDropDownNameValue[] EndGetCountries(IAsyncResult call)
        {
            var ms = (GetCountriesState)call.AsyncState;
            return ms.asyncStub.EndInvoke(call);
        }
        


        public delegate CascadingDropDownNameValue[] GetRegionsByCountryAsyncStub(string knownCategoryValues, string category);
        public delegate CascadingDropDownNameValue[] GetCountriesAsyncStub(string knownCategoryValues, string category);

        public class GetRegionsByCountryState
        {
            public object previousState;
            public GetRegionsByCountryAsyncStub asyncStub;
        }

        [WebMethod]
        public IAsyncResult BeginGetRegionsByCountry(string knownCategoryValues, 
                                                     string category,
                                                     AsyncCallback cb, 
                                                     object s)
        {
            GetRegionsByCountryAsyncStub stub
                = new GetRegionsByCountryAsyncStub(GetRegionsByCountry);
            var ms = new GetRegionsByCountryState();
            ms.previousState = s;
            ms.asyncStub = stub;
            return stub.BeginInvoke(knownCategoryValues,category, cb, ms);
        }

        [WebMethod]
        public CascadingDropDownNameValue[] EndGetRegionsByCountry(IAsyncResult call)
        {
            var ms = (GetRegionsByCountryState)call.AsyncState;
            return ms.asyncStub.EndInvoke(call);
        }
        /// <summary>
        /// Gets the regions by country.
        /// </summary>
        /// <param name="knownCategoryValues">The known category values.</param>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        [WebMethod(CacheDuration = 00, EnableSession = true)]
        public CascadingDropDownNameValue[] GetRegionsByCountry(string knownCategoryValues, string category)
        {
            //return null;
            StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(
                knownCategoryValues);
            if (!kv.ContainsKey("Country")) return null;

            var values = new List<CascadingDropDownNameValue>();
            
            string[] regions = Config.Users.GetRegions(kv["Country"]);
            if (regions != null && regions.Length > 0)
            {
                string forceRegion = Config.Users.ForceRegion.Trim();
                foreach (string region in regions)
                {
                    if (forceRegion.Length > 0)
                    {
                        if (region.ToLower() != forceRegion.ToLower()) continue;
                        //values[0].isDefaultValue = false;
                    }
                    else if (region.Length == 0)
                        values.Add(new CascadingDropDownNameValue(Lang.Trans("All"), " ", false));
                    else
                    {
                        var displayName = GetHebrewLocationTranslated(region);
                        values.Add(new CascadingDropDownNameValue(displayName, region));
                    }
                }
            }
            else
            {
                values.Add(new CascadingDropDownNameValue("-", "-", true));
            }
            values = values.OrderBy(x => x.name).ToList();
            return values.ToArray();
        }

        /// <summary>
        /// Gets the cities by country and region.
        /// </summary>
        /// <param name="knownCategoryValues">The known category values.</param>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        [WebMethod(CacheDuration = 00, EnableSession = true)]
        public CascadingDropDownNameValue[] GetCitiesByCountryAndRegion(string knownCategoryValues, string category)
        {
            StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(
                knownCategoryValues);
            if (!kv.ContainsKey("Country") || !kv.ContainsKey("Region")) return null;

            var values = new List<CascadingDropDownNameValue>();
            
            string[] cities = Config.Users.GetCities(kv["Country"], kv["Region"].Trim());
            if (cities != null && cities.Length > 0)
            {
                string forceCity = Config.Users.ForceCity.Trim();
                foreach (string city in cities)
                {
                    bool isDefault = false;
                    if (forceCity.Length > 0)
                    {
                        if (city.ToLower() != forceCity.ToLower()) continue;
                        isDefault = true;
                    }
                    var displayName = GetHebrewLocationTranslated(city);
                    values.Add(new CascadingDropDownNameValue(displayName, city, isDefault));
                }
            }
            else
            {
                values.Add(new CascadingDropDownNameValue("-", "-", false));
            }
            values = values.OrderBy(x => x.name).ToList();
            return values.ToArray();
        }

        #endregion

        /// <summary>
        /// Strips the control chars.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public string StripControlChars(string source)
        {
            string result = source;
            foreach (Char c in source)
            {
                if (Char.IsControl(c))
                    result = result.Replace(c, ' ');
            }
            return result;
        }
    }
}
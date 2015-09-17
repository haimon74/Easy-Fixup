/* ASPnetDating 
 * Copyright (C) 2003-2010 eStream 
 * ezFixUp.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at ezFixUp.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Timers;
using ezFixUp.Model;
using Timer = System.Timers.Timer;

namespace ezFixUp.Classes
{
    public class EmailQueue
    {
        static EmailQueue()
        {
        }

        private static Timer tMailer;
        private static bool mailerLock = false;

        public static void InitializeMailerTimer()
        {
            tMailer = new Timer();
            tMailer.AutoReset = true;
            tMailer.Interval = TimeSpan.FromMinutes(100).TotalMilliseconds;
            tMailer.Elapsed += new ElapsedEventHandler(tMailer_Elapsed);
            tMailer.Start();

            // Run mailer the 1st time
            tMailer_Elapsed(null, null);
        }

        private static void tMailer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(AsyncProcessEmailQueue), null);
        }

        private static void AsyncProcessEmailQueue(object data)
        {
            if (mailerLock)
            {
                return;
            }

            try
            {
                mailerLock = true;
                var queueItems = EmailQueueItem.LoadQueue().Take(600);
                foreach (EmailQueueItem eqItem in queueItems)
                {
                    ProcessEmailQueueItem(eqItem, false);
                }
            }
            catch (Exception err)
            {
                ExceptionLogger.Log("AsyncProcessEmailQueue", err);
                //Global.Logger.LogError("AsyncProcessEmailQueue", err.Message + err.StackTrace);
            }
            finally
            {
                mailerLock = false;
            }
        }

        public static void ProcessEmailQueueItem(object eqItem)
        {
            ProcessEmailQueueItem((EmailQueueItem)eqItem, true);
        }

        public static void ProcessEmailQueueItem(EmailQueueItem eqItem, bool checkMailerLock)
        {
            if (checkMailerLock && mailerLock)
            {
                return;
            }
            if (eqItem.ToEmail.Contains("@ezFixUp.com"))
                return;
            try
            {
                MailMessage message = new MailMessage();
                if (eqItem.FromName == null)
                    message.From = new MailAddress(eqItem.FromEmail);
                else
                {
                    message.From = new MailAddress(eqItem.FromEmail, eqItem.FromName, Encoding.UTF8);
                }

                if (eqItem.ToName == null)
                    message.To.Add(new MailAddress(eqItem.ToEmail));
                else
                {
                    message.To.Add(new MailAddress(eqItem.ToEmail, eqItem.ToName, Encoding.UTF8));
                }

                if (eqItem.Cc != null)
                {
                    message.CC.Add(eqItem.Cc);
                }

                if (eqItem.Bcc != null)
                {
                    message.Bcc.Add(eqItem.Bcc);
                }

                message.Subject = eqItem.Subject;

                #region Set Subject Encoding

                if (Properties.Settings.Default.EmailSubjectEncoding == ""
                    || Properties.Settings.Default.EmailSubjectEncoding == null)
                {
                    message.SubjectEncoding = Encoding.UTF8;
                }
                else
                {
                    try
                    {
                        message.SubjectEncoding = Encoding.GetEncoding(Properties.Settings.Default.EmailSubjectEncoding);
                    }
                    catch (ArgumentException)
                    {
                        message.SubjectEncoding = Encoding.UTF8;
                    }
                }

                #endregion

                message.IsBodyHtml = true;
                message.Body = eqItem.Body;

                #region Set Body Encoding

                if (Properties.Settings.Default.EmailBodyEncoding == ""
                    || Properties.Settings.Default.EmailBodyEncoding == null)
                {
                    message.BodyEncoding = Encoding.UTF8;
                }
                else
                {
                    try
                    {
                        message.BodyEncoding = Encoding.GetEncoding(Properties.Settings.Default.EmailBodyEncoding);
                    }
                    catch (ArgumentException)
                    {
                        message.BodyEncoding = Encoding.UTF8;
                    }
                }

                #endregion

                SmtpClient smtpClient = new SmtpClient();
								//smtpClient.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis;
								smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Send(message);

                EmailQueueItem.Delete(eqItem.Id);
            }
            catch (Exception err)
            {
                eqItem.LastError = err.Message;
                while (err.InnerException != null)
                {
                    eqItem.LastError = eqItem.LastError + err.InnerException.ToString();
                    err = err.InnerException;
                }
                eqItem.LastTry = DateTime.Now;
                eqItem.NextTry = DateTime.Now.AddHours(Config.Mailing.RetryInterval);
                eqItem.Tries++;
                eqItem.Save();
            }
        }
    }

    public class EmailQueueItem
    {
        static EmailQueueItem()
        {
        }

        #region Properties

        private int id;

        private string fromEmail;

        private string toEmail;

        private string fromName;

        private string toName;

        private string cc;

        private string bcc;

        private string subject;

        private string body;

        private int tries;

        private DateTime lastTry;

        private string lastError;

        private DateTime nextTry;

        public int Id
        {
            get { return id; }
        }

        public string FromEmail
        {
            get { return fromEmail; }
            set { fromEmail = value; }
        }

        public string ToEmail
        {
            get { return toEmail; }
            set { toEmail = value; }
        }

        public string FromName
        {
            get { return fromName; }
            set { fromName = value; }
        }

        public string ToName
        {
            get { return toName; }
            set { toName = value; }
        }

        public string Cc
        {
            get { return cc; }
            set { cc = value; }
        }

        public string Bcc
        {
            get { return bcc; }
            set { bcc = value; }
        }

        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        public string Body
        {
            get { return body; }
            set { body = value; }
        }

        public int Tries
        {
            get { return tries; }
            set { tries = value; }
        }

        public DateTime LastTry
        {
            get { return lastTry; }
            set { lastTry = value; }
        }

        public string LastError
        {
            get { return lastError; }
            set { lastError = value; }
        }

        public DateTime NextTry
        {
            get { return nextTry; }
            set { nextTry = value; }
        }

        #endregion

        public static EmailQueueItem Create(string to, string subject, string body)
        {
            return Create(Config.Misc.SiteEmail, null, to, null, subject, body);
        }

        public static EmailQueueItem Create(string fromEmail, string fromName, string toEmail, string toName, string subject, string body)
        {
            if (toEmail.ToLower().Contains("@ezFixUp.com"))
                return null;

            EmailQueueItem qItem = new EmailQueueItem();
            qItem.id = -1;
            qItem.fromEmail = fromEmail;
            qItem.toEmail = toEmail;
            qItem.fromName = fromName;
            qItem.toName = toName;
            qItem.subject = subject;
            qItem.body = body;
            qItem.tries = 0;
            qItem.lastTry = DateTime.MinValue;
            qItem.nextTry = DateTime.MinValue;

            return qItem;
        }

        public void Save(bool sendImmediately)
        {
            Save();
            if (sendImmediately)
                ThreadPool.QueueUserWorkItem(new WaitCallback(
                                                 EmailQueue.ProcessEmailQueueItem), this);
        }

        public void Save()
        {
            if (toEmail.ToLower().Contains("@ezFixUp.com"))
                return;
            
            //using (var conn = Config.DB.Open())
            {
                object result = SqlHelper.GetDB().ExecuteScalar( "SaveEmailQueue", id, fromEmail, toEmail, fromName, toName, cc, bcc,
                                                        subject, body, tries,
                                                        lastTry == DateTime.MinValue ? null : (object)lastTry,
                                                        lastError,
                                                        nextTry == DateTime.MinValue ? null : (object)nextTry);

                if (id == -1)
                {
                    id = Convert.ToInt32(result);
                }
            }
        }

        public static EmailQueueItem[] LoadQueue()
        {
            return Load(-1, null, -1, Config.Mailing.RetryCount, DateTime.Now);
        }

        private static EmailQueueItem[] Load(int id, string to, int minTries, int maxTries,
                                             DateTime minNextTryDate)
        {
            List<EmailQueueItem> lEmailQueueItems = new List<EmailQueueItem>();

            //using (var conn = Config.DB.Open())
            {
                using (var reader = SqlHelper.GetDB().ExecuteReader("LoadEmailQueue", id, to, minTries, maxTries,
                                                                    minNextTryDate == DateTime.MinValue
                                                                        ? null
                                                                        : (object) minNextTryDate))
                {

                    while (reader.Read())
                    {
                        EmailQueueItem eqItem = new EmailQueueItem();

                        eqItem.id = (int) reader["Id"];
                        eqItem.fromEmail = (string) reader["From"];
                        eqItem.toEmail = (string) reader["To"];
                        eqItem.fromName = reader["FromName"] != DBNull.Value ? (string) reader["FromName"] : null;
                        eqItem.toName = reader["ToName"] != DBNull.Value ? (string) reader["ToName"] : null;
                        if (reader["CC"] is string)
                        {
                            eqItem.cc = (string) reader["CC"];
                        }
                        if (reader["BCC"] is string)
                        {
                            eqItem.bcc = (string) reader["BCC"];
                        }
                        eqItem.subject = (string) reader["Subject"];
                        eqItem.body = (string) reader["Body"];
                        eqItem.tries = (int) reader["Tries"];
                        eqItem.lastTry = reader["LastTry"] is DateTime
                                             ? (DateTime) reader["LastTry"]
                                             : DateTime.MinValue;
                        if (reader["LastError"] is string)
                        {
                            eqItem.lastError = (string) reader["LastError"];
                        }
                        eqItem.nextTry = reader["NextTry"] is DateTime
                                             ? (DateTime) reader["NextTry"]
                                             : DateTime.MinValue;

                        lEmailQueueItems.Add(eqItem);
                    }
                    reader.Close();
                }
            }

            return lEmailQueueItems.ToArray();
        }

        public static void Delete(int id)
        {
            //using (var conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "DeleteEmailQueue", id);
            }
        }
    }

    public class Email
    {
        static Email()
        {
        }

        /// <summary>
        /// Sends the specified from.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        public static void Send(string from, string to, string subject, string body, bool immediately)
        {
            Send(null, from, null, to, subject, body, immediately);
        }

        /// <summary>
        /// Sends the specified from name.
        /// </summary>
        /// <param name="fromName">From name.</param>
        /// <param name="fromEmail">From email.</param>
        /// <param name="toName">To name.</param>
        /// <param name="toEmail">To email.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        public static void Send(string fromName, string fromEmail, string toName, string toEmail, string subject,
                                string body, bool immediately)
        {
            if (toEmail.ToLower().Contains("@ezFixUp.com"))
                return;

            EmailQueueItem qItem = EmailQueueItem.Create(fromEmail, fromName, toEmail, toName, subject, body);
            qItem.Save(immediately);
        }

        /// <summary>
        /// Applies the formatter.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="formatter">The formatter.</param>
        private static void applyFormatter(ref string subject, ref string body,
                                           NameValueCollection formatter)
        {
            if (formatter != null)
            {
                foreach (string key in formatter.AllKeys)
                {
                    subject = subject.Replace("%%" + key + "%%", formatter[key]);
                    body = body.Replace("%%" + key + "%%", formatter[key]);
                }
            }
        }

        /// <summary>
        /// Send template email to the user.
        /// </summary>
        /// <param name="templateType">Type of the template.</param>
        /// <param name="toEmail">To email.</param>
        public static void SendTemplateEmail(Type templateType, string toEmail, bool immediately, int languageId)
        {
            if (toEmail.ToLower().Contains("@ezFixUp.com"))
                return;
            SendTemplateEmail(templateType, toEmail, null, immediately, languageId);
        }

        /// <summary>
        /// Send template email to the user.
        /// </summary>
        /// <param name="templateType">Type of the template.</param>
        /// <param name="toEmail">To email.</param>
        /// <param name="formatter">additional formatter</param>
        public static void SendTemplateEmail(Type templateType, string toEmail,
                                             NameValueCollection formatter, bool immediately, int languageId)
        {
            if (toEmail.ToLower().Contains("@ezFixUp.com"))
                return;
            
            if (templateType.GetInterface("IEmailTemplate") == null)
                throw new Exception("Given type is not email template class type");

            EmailTemplates.IEmailTemplate template =
                (EmailTemplates.IEmailTemplate)
                Activator.CreateInstance(templateType, new object[] { languageId });

            string subject = template.Subject;
            string body = template.Body;

            if (formatter != null)
                applyFormatter(ref subject, ref body, formatter);
            Send(Config.Misc.SiteEmail, toEmail, subject, body, immediately);
        }
    }

    public class BirthdayEmails
    {
        static BirthdayEmails()
        {
        }

        private static System.Timers.Timer tMailer;
        private static bool mailerLock = false;

        public static void InitializeMailerTimer()
        {
            tMailer = new Timer();
            tMailer.AutoReset = true;
            tMailer.Interval = TimeSpan.FromHours(1).TotalMilliseconds;
            tMailer.Elapsed += new ElapsedEventHandler(tMailer_Elapsed);
            tMailer.Start();

            // Run payment processing the 1st time
            tMailer_Elapsed(null, null);
        }

        private static void tMailer_Elapsed(object sender, ElapsedEventArgs e)
        {
            DateTime dtLastTick = DBSettings.Get("BirthdayEmails_LastTimerTick", DateTime.Now);

            if (DateTime.Now.Subtract(dtLastTick) >= TimeSpan.FromHours(1) &&
                (DateTime.Now.Hour >= 0 && DateTime.Now.Hour < 1)) // to start between 00:00~01:00 oclock
            { // TODO: fetch in the async process the birthdays by GMT time 00:00~01:00
                ThreadPool.QueueUserWorkItem(new WaitCallback(AsyncProcessMailerQueue), null);
                DBSettings.Set("BirthdayEmails_LastTimerTick", DateTime.Now);
            }
        }

        private static void AsyncProcessMailerQueue(object data)
        {
            if (mailerLock)
            {
                return;
            }

            try
            {
                mailerLock = true;

                BirthdaySearch search = new BirthdaySearch();

                UserSearchResults results = search.GetResults();

                if (results != null)
                {
                    foreach (string username in results.Usernames)
                    {
                        User user = null;
                        try
                        {
                            user = Classes.User.Load(username);
                        }
                        catch (NotFoundException)
                        {
                            continue;
                        }

                        EmailTemplates.HappyBirthday happyBirthdayTemplate =
                            new EmailTemplates.HappyBirthday(user.LanguageId);

                        Email.Send(Config.Misc.SiteTitle, Config.Misc.SiteEmail, user.Name, user.Email,
                                   happyBirthdayTemplate.GetFormattedSubject(user.Name),
                                   happyBirthdayTemplate.GetFormattedBody(user.Name), false);
                    }
                }
            }
            catch (Exception err)
            {
                Global.Logger.LogError("BirthdayEmails", err);
            }
            finally
            {
                mailerLock = false;
            }
        }
    }

    public class FriendBirthdayEmails
    {
        static FriendBirthdayEmails()
        {
        }

        private static System.Timers.Timer tMailer;
        private static bool mailerLock = false;

        public static void InitializeMailerTimer()
        {
            tMailer = new Timer();
            tMailer.AutoReset = true;
            tMailer.Interval = TimeSpan.FromHours(1).TotalMilliseconds;
            tMailer.Elapsed += new ElapsedEventHandler(tMailer_Elapsed);
            tMailer.Start();

            // Run payment processing the 1st time
            tMailer_Elapsed(null, null);
        }

        private static void tMailer_Elapsed(object sender, ElapsedEventArgs e)
        {
            DateTime dtLastTick = DBSettings.Get("FriendBirthdayEmails_LastTimerTick", DateTime.Now);

            if (DateTime.Now.Subtract(dtLastTick) >= TimeSpan.FromHours(1) &&
                (DateTime.Now.Hour >= 0 && DateTime.Now.Hour < 1)) // to start between 00:00~01:00 oclock
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(AsyncProcessMailerQueue), null);
                DBSettings.Set("FriendBirthdayEmails_LastTimerTick", DateTime.Now);
            }
        }

        private static void AsyncProcessMailerQueue(object data)
        {
            if (mailerLock)
            {
                return;
            }

            try
            {
                mailerLock = true;

                if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
                {
                    using (var db = new ezFixUpDataContext())
                    {
                        var friendsWithCommingBirthdays = CompiledQueries.FetchActiveFriendsByBirthday(db, 7);
                        var usernames = friendsWithCommingBirthdays.Select(f => f.u_username).Distinct();

                        foreach (string recipient in usernames)
                        {
                            User u = null;
                            try
                            {
                                u = Classes.User.Load(recipient);
                            }
                            catch (NotFoundException)
                            {
                                continue;
                            }

                            var hisBirthdaysFriends = friendsWithCommingBirthdays
                                                                .Where(f => f.u_username == recipient)
                                                                .Select(f => f.f_username).ToArray();

                            MiscTemplates.FriendBirthday friendBirthdayTemplate =
                                                                new MiscTemplates.FriendBirthday(u.LanguageId);

                            var frUsernames = string.Join(",\r\n", hisBirthdaysFriends);
                            Message.Send(Config.Users.SystemUsername, recipient,
                                         friendBirthdayTemplate.Message.Replace("%%USERNAMES%%", frUsernames),
                                         0);
                        }
                    }
                }

                BirthdaySearch search = new BirthdaySearch();

                UserSearchResults results = search.GetResults();

                if (results != null && results.Usernames != null)
                {
                    foreach (string username in results.Usernames)
                    {
                        User user = null;
                        try
                        {
                            user = Classes.User.Load(username);
                        }
                        catch (NotFoundException) { continue; }

                        Event evt = new Event(username);
                        evt.Type = Event.eType.FriendBirthday;
                        evt.Save();

                        string[] usernames = Classes.User.FetchMutuallyFriends(username);

                        foreach (string recipient in usernames)
                        {
                            User u = null;
                            try
                            {
                                u = Classes.User.Load(recipient);
                            }
                            catch (NotFoundException)
                            {
                                continue;
                            }
                            //MiscTemplates.FriendBirthday friendBirthdayTemplate =
                            //    new MiscTemplates.FriendBirthday(u.LanguageId);
                            //Message.Send(Config.Users.SystemUsername, recipient,
                            //             friendBirthdayTemplate.Message.Replace("%%USERNAME%%", username), 0);

                            if (Config.Users.NewEventNotification)
                            {
                                int imageID = 0;
                                try
                                {
                                    imageID = Photo.GetPrimary(user.Username).Id;
                                }
                                catch (NotFoundException)
                                {
                                    imageID = ImageHandler.GetPhotoIdByGender(user.Gender);
                                }
                                string text = String.Format("{0} has a birthday today".Translate(),
                                                      "<b>" + user.Username + "</b>");

                                string thumbnailUrl = ImageHandler.CreateImageUrl(imageID, 50, 50, false, true, true);
                                Classes.User.SendOnlineEventNotification(user.Username, recipient, text, thumbnailUrl,
                                                                         UrlRewrite.CreateShowUserUrl(user.Username));
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                Global.Logger.LogError("FriendBirthdayEmails", err);
            }
            finally
            {
                mailerLock = false;
            }
        }
    }

    public class VisitSiteReminderEmails
    {
        static VisitSiteReminderEmails()
        {
        }

        private static System.Timers.Timer tMailer;
        private static bool mailerLock = false;

        public static void InitializeMailerTimer()
        {
            tMailer = new Timer();
            tMailer.AutoReset = true;
            tMailer.Interval = TimeSpan.FromDays(1).TotalMilliseconds;
            tMailer.Elapsed += new ElapsedEventHandler(tMailer_Elapsed);
            tMailer.Start();

            // Run payment processing the 1st time
            tMailer_Elapsed(null, null);
        }

        private static void tMailer_Elapsed(object sender, ElapsedEventArgs e)
        {
            DateTime dtLastTick = DBSettings.Get("VisitSiteReminderEmails_LastTimerTick", DateTime.Now);

            if (DateTime.Now.Subtract(dtLastTick) >= TimeSpan.FromHours(24))
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(AsyncProcessMailerQueue), null);
                DBSettings.Set("VisitSiteReminderEmails_LastTimerTick", DateTime.Now);
            }
        }

        private static void AsyncProcessMailerQueue(object data)
        {
            if (mailerLock)
            {
                return;
            }

            try
            {
                mailerLock = true;

                IrregularSearchUsers search = new IrregularSearchUsers();

                UserSearchResults results = search.GetResults();

                if (results != null && results.Usernames != null)
                {
                    foreach (string username in results.Usernames)
                    {
                        User user = null;
                        try
                        {
                            user = Classes.User.Load(username);
                        }
                        catch (NotFoundException)
                        {
                            continue;
                        }

                        EmailTemplates.VisitSiteReminder visitSiteReminderTemplate =
                            new EmailTemplates.VisitSiteReminder(user.LanguageId);

                        Email.Send(Config.Misc.SiteTitle, Config.Misc.SiteEmail, user.Name, user.Email,
                                   visitSiteReminderTemplate.GetFormattedSubject(user.Name),
                                   visitSiteReminderTemplate.GetFormattedBody(user.Name), false);
                    }
                }
            }
            catch (Exception err)
            {
                Global.Logger.LogError("VisitSiteReminderEmails", err);
            }
            finally
            {
                mailerLock = false;
            }
        }
    }

    public class ScheduledAnnouncementEmails
    {
        static ScheduledAnnouncementEmails()
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

            tMailer_Elapsed(null, null);
        }

        private static void tMailer_Elapsed(object sender, ElapsedEventArgs e)
        {
            DateTime dtLastTick = DBSettings.Get("ScheduledAnnouncementEmails_LastTimerTick", DateTime.Now);

            if (DateTime.Now.Subtract(dtLastTick) >= TimeSpan.FromHours(24))
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(AsyncProcessMailerQueue), null);
                DBSettings.Set("ScheduledAnnouncementEmails_LastTimerTick", DateTime.Now);
            }
        }

        private static void AsyncProcessMailerQueue(object data)
        {
            if (mailerLock)
            {
                return;
            }

            try
            {
                mailerLock = true;

                var announcements = ScheduledAnnouncement.Fetch().Take(100000);
                if (announcements.Count() > 0)
                {
                    foreach (ScheduledAnnouncement announcement in announcements)
                    {
                        BasicSearch search = new BasicSearch();
                        search.hasAnswer_isSet = false;

                        if (announcement.Gender.HasValue)
                            if (announcement.Gender.Value == Classes.User.eGender.Matchmaker)
                                search.InterestedIn = announcement.Gender.Value;
                            else search.Gender = announcement.Gender.Value;

                        if (announcement.PaidMember.HasValue)
                            search.Paid = announcement.PaidMember.Value;
                        if (announcement.HasPhotos.HasValue)
                            search.HasPhoto = announcement.HasPhotos.Value;
                        if (announcement.HasProfile.HasValue)
                            search.HasAnswer = announcement.HasProfile.Value;
                        if (!String.IsNullOrEmpty(announcement.Country))
                            search.Country = announcement.Country;
                        if (!String.IsNullOrEmpty(announcement.Region))
                            search.State = announcement.Region;
                        if (announcement.LanguageId.HasValue)
                            search.LanguageID = announcement.LanguageId;

                        switch (announcement.Type)
                        {
                            case ScheduledAnnouncement.eType.SpecificDate:
                                if (DateTime.Now.Date == announcement.Date.Value.Date)
                                {
                                    ScheduledAnnouncement.Delete(announcement.ID);
                                }
                                else continue;
                                break;

                            case ScheduledAnnouncement.eType.DaysAfterInactivity:
                                if (announcement.Days.HasValue)
                                {
                                    search.LastLogin = (DateTime.Now - TimeSpan.FromDays(announcement.Days.Value)).Date;
                                }
                                break;

                            case ScheduledAnnouncement.eType.Weekly:

                                if (!announcement.Days.HasValue || announcement.Days.Value < 0)
                                    announcement.Days = 0;
                                if (announcement.Days.Value > 6)
                                    announcement.Days = 6;

                                if (DateTime.Now.DayOfWeek != (DayOfWeek.Sunday + announcement.Days))
                                {
                                    search.LastLogin = DateTime.MaxValue;
                                }
                                break;

                            case ScheduledAnnouncement.eType.Monthly:

                                if (!announcement.Days.HasValue || announcement.Days.Value < 1)
                                    announcement.Days = 1;
                                if (announcement.Days.Value > 28)
                                    announcement.Days = 28;

                                if (DateTime.Now.Day != announcement.Days)
                                {
                                    search.LastLogin = DateTime.MaxValue;
                                }
                                break;

                            case ScheduledAnnouncement.eType.DaysAfterRegistration:
                                if (announcement.Days.HasValue)
                                {
                                    search.UserSince = (DateTime.Now - TimeSpan.FromDays(announcement.Days.Value)).Date;
                                }
                                break;

                            case ScheduledAnnouncement.eType.DaysBeforeMembershipEnd:
                                if (announcement.Days.HasValue)
                                {
                                    search.Paid = true;
                                }
                                break;
                        }

                        UserSearchResults results = search.GetResults();

                        if (results == null)
                        {
                            continue;
                        }

                        string[] users = null;

                        if (announcement.Type == ScheduledAnnouncement.eType.DaysBeforeMembershipEnd)
                        {
                            var elapsedUsernames =
                                Subscription.FetchActiveElapsedSubscriptions(
                                    DateTime.Today.AddDays(announcement.Days.Value - 1),
                                    DateTime.Today.AddDays(announcement.Days.Value + 1)
                                    ).Select(s => s.Username);
                            users = results.Usernames.Where(u => elapsedUsernames.Contains(u)).ToArray();
                        }
                        else
                        {
                            users = results.Usernames;
                        }

                        foreach (string username in users)
                        {
                            string subject = announcement.Subject;
                            string text = announcement.Body;

                            User user = Classes.User.Load(username);
                            subject = subject.Replace("%%USER%%", user.Username);
                            subject = subject.Replace("%%NAME%%", user.Name);
                            text = text.Replace("%%USER%%", user.Username);
                            text = text.Replace("%%NAME%%", user.Name);

                            Email.Send(Config.Misc.SiteTitle, Config.Misc.SiteEmail, user.Name, user.Email, subject, text, false);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                Global.Logger.LogError("ScheduledAnnouncementEmails", err);
            }
            finally
            {
                mailerLock = false;
            }
        }
    }
}
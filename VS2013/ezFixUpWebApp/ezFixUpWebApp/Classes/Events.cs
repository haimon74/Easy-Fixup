using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Web;
using System.Web.UI;
using ezFixUp.Components;
using ezFixUp.Model;
using Timer = System.Timers.Timer;

namespace ezFixUp.Classes
{
    public class Event
    {
        #region fields

        #region eSortColumn enum

        /// <summary>
        /// Specifies the colomn on which to sort.
        /// </summary>
        public enum eSortColumn
        {
            None,
            Date
        }

        #endregion

        #region eType enum

        public enum eType : ulong
        {
            FriendBirthday = 1L,
            FriendUpdatedProfile = 1L << 1,
            FriendAttendingEvent = 1L << 2,
            NewGroupPhoto = 1L << 3,
            FriendEntersContest = 1L << 4,
            FriendJoinedGroup = 1L << 5,
            FriendLeftGroup = 1L << 6,
            NewPhotoComment = 1L << 7,
            NewFriendPhoto = 1L << 8,
            NewFriendVideoUpload = 1L << 9,
            NewFriendBlogPost = 1L << 10,
            NewFriendGroup = 1L << 11,
            NewFriendFriend = 1L << 12,
            NewGroupTopic = 1L << 13,
            NewGroupEvent = 1L << 14,
            NewProfileComment = 1L << 15,
            FriendUpdatedStatus = 1L << 16,
            TaggedOnPhoto = 1L << 17,
            NewFriendAudioUpload = 1L << 18,
            NewFriendYouTubeUpload = 1L << 19,
            RemovedFriendFriend = 1L << 20,
            NewFriendRelationship = 1L << 21,
            RemovedFriendRelationship = 1L << 22
        }

        #endregion

        public const int passedDaysWithinUserNotLoggedIn = 30;

        private static bool mailerLock;
        private static Timer timer;
        private string fromUsername;
        private DateTime date = DateTime.Now;
        private string detailsXML;
        private int? fromGroup;
        private int? id;
        private eType type;

        #endregion

        #region Constructors

        private Event()
        {
        }

        public Event(string fromUsername)
        {
            this.fromUsername = fromUsername;
        }

        public Event(int fromGroup)
        {
            this.fromGroup = fromGroup;
        }

        #endregion

        #region Properties

        public int ID
        {
            get
            {
                if (id.HasValue)
                {
                    return id.Value;
                }
                throw new Exception("The field ID is not set!");
            }
        }

        public string FromUsername
        {
            get { return fromUsername; }
        }

        public int? FromGroup
        {
            get { return fromGroup; }
            set { fromGroup = value; }
        }

        public eType Type
        {
            get { return type; }
            set { type = value; }
        }

        public string DetailsXML
        {
            get { return detailsXML; }
            set { detailsXML = value; }
        }

        public DateTime Date
        {
            get { return date; }
        }

        public eSortColumn SortColumn { get; set; }

        #endregion

        #region Methods

        public static Event[] Fetch()
        {
            return Fetch(null, null, null, null, null, null, null, eSortColumn.None);
        }

        public static Event[] Fetch(string fromUsername, eType type, DateTime date)
        {
            return Fetch(null, type, fromUsername, null, date, null, null, eSortColumn.None);
        }

        public static Event[] Fetch(int fromGroup, eType type, DateTime date)
        {
            return Fetch(null, type, null, fromGroup, date, null, null, eSortColumn.None);
        }

        public static Event Fetch(int id)
        {
            Event[] events = Fetch(id, null, null, null, null, null, null, eSortColumn.None);

            if (events.Length > 0)
            {
                return events[0];
            }
            return null;
        }

        public static Event[] Fetch(string username, ulong typeMask, int numberOfEvents)
        {
            using (var db = new ezFixUpDataContext())
            {
                var events = from e in db.Events
                             where (e.e_type & (long?)typeMask) > 0
                                    && (e.e_fromusername == username || e.e_fromusername != null
                                                                         &&
                                                                         (from f in db.Friends
                                                                          where f.u_username == username && f.f_accepted
                                                                          select f.f_username).Contains(e.e_fromusername)
                                                                     || e.e_fromgroup.HasValue
                                                                         &&
                                                                         (from g in db.Groups
                                                                          join gm in db.GroupMembers on g.g_id equals gm.g_id
                                                                          where gm.u_username == username
                                                                          select g.g_id).Contains(e.e_fromgroup.Value)
                                       )
                             select new Event
                             {
                                 id = e.e_id,
                                 fromUsername = e.e_fromusername,
                                 type = (eType)e.e_type,
                                 fromGroup = e.e_fromgroup,
                                 detailsXML = e.e_details,
                                 date = e.e_date
                             };

                return events.OrderByDescending(e => e.date).Take(numberOfEvents).ToArray();
            }
        }

        public static Event[] Fetch(string fromUsername)
        {
            using (var db = new ezFixUpDataContext())
            {
                var events = from e in db.Events
                             where e.e_fromusername == fromUsername
                             select new Event
                                        {
                                            id = e.e_id,
                                            fromUsername = e.e_fromusername,
                                            type = (eType)e.e_type,
                                            fromGroup = e.e_fromgroup,
                                            detailsXML = e.e_details,
                                            date = e.e_date
                                        };
                return events.OrderByDescending(e => e.date).ToArray();
            }
        }

        private static Event[] Fetch(int? id, eType? type, string fromUsername,
                                     int? fromGroup, DateTime? date, ulong? bitMask,
                                     int? numberOfEvents, eSortColumn sortColumn)
        {
            date = date.HasValue ? date.Value.Date : (DateTime?)null;

            using (var db = new ezFixUpDataContext())
            {
                var events = from e in db.Events
                             where (!id.HasValue || e.e_id == id)
                                   && (!type.HasValue || e.e_type == (long?)type)
                                   && (fromUsername == null || e.e_fromusername == fromUsername)
                                   && (!fromGroup.HasValue || e.e_fromgroup == fromGroup)
                                   && (!date.HasValue || e.e_date.Date == date)
                                   && (!bitMask.HasValue || (e.e_type & (long?)bitMask) > 0)
                             select new Event
                             {
                                 id = e.e_id,
                                 fromUsername = e.e_fromusername,
                                 type = (eType)e.e_type,
                                 fromGroup = e.e_fromgroup,
                                 detailsXML = e.e_details,
                                 date = e.e_date
                             };

                if (sortColumn == eSortColumn.Date)
                    events = events.OrderByDescending(e => e.date);

                if (numberOfEvents.HasValue)
                    events = events.Take(numberOfEvents.Value);

                return events.ToArray();
            }
        }

        public void Save()
        {
            if (detailsXML != null && detailsXML.Length > 4000)
            {
                Global.Logger.LogWarning(String.Format("Event {0} was not saved because it is more than 4000 chars", id));
                return;
            }

            if (fromUsername != null)
            {
                User user = null;
                try
                {
                    user = Classes.User.Load(fromUsername);
                }
                catch (NotFoundException) { return; }

                switch (type)
                {
                    case eType.FriendBirthday:
                    case eType.FriendUpdatedProfile:
                    case eType.FriendAttendingEvent:
                    case eType.NewGroupPhoto:
                    case eType.FriendEntersContest:
                    case eType.FriendJoinedGroup:
                    case eType.FriendLeftGroup:
                    case eType.NewPhotoComment:
                    case eType.NewFriendPhoto:
                    case eType.NewFriendVideoUpload:
                    case eType.NewFriendBlogPost:
                    case eType.NewFriendGroup:
                    case eType.NewFriendFriend:
                    case eType.NewGroupTopic:
                    //                case eType.NewGroupEvent:
                    //                    if (!IsEventsSettingEnabled(type, user)) return;
                    //                    break;
                    case eType.NewProfileComment:
                    case eType.FriendUpdatedStatus:
                    case eType.TaggedOnPhoto:
                    case eType.NewFriendAudioUpload:
                    case eType.NewFriendYouTubeUpload:
                    case eType.RemovedFriendFriend:
                    case eType.NewFriendRelationship:
                    case eType.RemovedFriendRelationship:
                        if (!IsEventsSettingEnabled(type, user)) return;
                        break;
                }

                using (var db = new ezFixUpDataContext())
                {
                    var result = db.SaveEvent(id, (int?)type, fromUsername, fromGroup,
                        detailsXML, date);

                    if (id == null)
                        id = result.First().Id;
                }
            }
        }

        public static void Delete(int id)
        {
            using (var db = new ezFixUpDataContext())
            {
                var comments = db.EventComments.Where(c => c.e_id == id);
                db.EventComments.DeleteAllOnSubmit(comments);
                Model.Event ev = db.Events.Single(e => e.e_id == id);
                db.Events.DeleteOnSubmit(ev);
                db.SubmitChanges();
            }
        }

        public static void Delete(int? id, string fromUsername, int? fromGroup, eType? type,
                                  DateTime? date)
        {
            using (var db = new ezFixUpDataContext())
            {
                var events = from e in db.Events
                             where (!id.HasValue || e.e_id == id)
                                   && (!type.HasValue || e.e_type == (long?)type)
                                   && (fromUsername == null || e.e_fromusername == fromUsername)
                                   && (!fromGroup.HasValue || e.e_fromgroup == fromGroup)
                                   && (!date.HasValue || e.e_date.Date == date)
                             select e;
                foreach (var ev in events)
                {
                    db.EventComments.DeleteAllOnSubmit(ev.EventComments);
                }

                db.Events.DeleteAllOnSubmit(events);
                db.SubmitChanges();
            }
        }

        public static bool IsEventsSettingEnabled(eType type, User user)
        {
            return (user.EventsSettings & (ulong)type) == (ulong)type;
        }

        public static void InitializeEventsCleanupTimer()
        {
            timer = new Timer { AutoReset = true, Interval = TimeSpan.FromDays(1).TotalMilliseconds };
            timer.Elapsed += timer_Elapsed;
            timer.Start();

            // Run processing the 1st time
            timer_Elapsed(null, null);
        }

        private static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            DateTime dtLastTick = DBSettings.Get("EventsCleanup_LastTimerTick", DateTime.Now);

            if (DateTime.Now.Subtract(dtLastTick) >= TimeSpan.FromHours(24))
            {
                ThreadPool.QueueUserWorkItem(EventsCleanup, null);
                DBSettings.Set("EventsCleanup_LastTimerTick", DateTime.Now);
            }
        }

        private static void EventsCleanup(object data)
        {
            if (mailerLock)
            {
                return;
            }

            try
            {
                mailerLock = true;

                Global.Logger.LogStatus("EventsCleanup", "Events Cleanup starting " + DateTime.Now.ToShortTimeString());
                DateTime date = DateTime.Now.Subtract(TimeSpan.FromDays(90));
                var roundedDate = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
                using (var db = new ezFixUpDataContext())
                {
                    db.EventsCleanup(roundedDate);
                }
                Global.Logger.LogStatus("EventsCleanup", "Events Cleanup ended " + DateTime.Now.ToShortTimeString());
            }
            catch (Exception err)
            {
                Global.Logger.LogError("EventsCleanup", err);
            }
            finally
            {
                mailerLock = false;
            }
        }

        public static List<Control> PrepareEventsControls(string username, ulong? typeMask, int numberOfEvents)
        {
            if (HttpContext.Current == null) return null;
            var page = (Page)HttpContext.Current.Handler;
            var Server = HttpContext.Current.Server;

            List<Event> events = null;
            events = !typeMask.HasValue
                         ? new List<Event>(Fetch(username))
                         : new List<Event>(Fetch(username, typeMask.Value, numberOfEvents));

            //if (events.Count == 0) return null;
            string name = Classes.User.Load(username).Name;
            DateTime lastDate = DateTime.MaxValue;
            int limitCounter = 0;
            User user = null;
            Group group = null;
            GroupTopic groupTopic = null;
            GroupPhoto groupPhoto = null;
            GroupEvent groupEvent = null;
            Event[] filteredEvents = null;
            UserEvent eventCtrl = null;

            var controls = new List<Control>();

            while (events.Count > 0 && limitCounter++ < 100)
            {
                Event ev = events[0];
                string fromDisplayName = UserSession.GetViewedUserDisplayedNameByUsername(username, ev.FromUsername);
                
                switch (ev.Type)
                {
                    #region FriendBirthday

                    case eType.FriendBirthday:
                        filteredEvents = filterEvents(events, ev);
                        eventCtrl = (UserEvent)page.LoadControl("~/Components/UserEvent.ascx");
                        eventCtrl.ID = ev.ID.ToString();
                        eventCtrl.FromUsername = ev.FromUsername;
                        eventCtrl.Time = ev.Date;
                        eventCtrl.UserImageIDs = new List<int>();
                        eventCtrl.Type = eType.FriendBirthday;
                        eventCtrl.UserImageIDs.Add(Photo.GetPrimaryOrDefaultId(ev.FromUsername));
                        eventCtrl.Text = String.Format("{0} has a birthday".Translate(),
                                                       String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                                     UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                                                                     fromDisplayName));
                        controls.Add(eventCtrl);
                        break;

                    #endregion

                    #region FriendUpdatedProfile

                    case eType.FriendUpdatedProfile:
                        filteredEvents = filterEvents(events, ev);
                        eventCtrl = (UserEvent)page.LoadControl("~/Components/UserEvent.ascx");
                        eventCtrl.ID = ev.ID.ToString();
                        eventCtrl.FromUsername = ev.FromUsername;
                        eventCtrl.Time = ev.Date;
                        eventCtrl.UserImageIDs = new List<int>();
                        eventCtrl.Type = eType.FriendUpdatedProfile;
                        eventCtrl.UserImageIDs.Add(Photo.GetPrimaryOrDefaultId(ev.FromUsername));
                        
                        UpdatedProfile updatedProfile = null;

                        if (filteredEvents.Length == 1)
                        {
                            updatedProfile = Misc.FromXml<UpdatedProfile>(filteredEvents[0].DetailsXML);

                            ProfileQuestion question = null;

                            if (updatedProfile.QuestionIDs.Count == 1)
                            {
                                try
                                {
                                    question = ProfileQuestion.Fetch(updatedProfile.QuestionIDs[0]);
                                }
                                catch (NotFoundException)
                                {
                                    break;
                                }
                                eventCtrl.Text =
                                    String.Format(
                                        "{0} has updated the \"{1}\" section in their profile".Translate(),
                                        String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                      UrlRewrite.CreateShowUserUrl(filteredEvents[0].FromUsername),
                                                      fromDisplayName/*filteredEvents[0].FromUsername*/), question.Name);
                            }
                            else
                            {
                                eventCtrl.Text = String.Format("{0} has updated {1} answers on their profile".Translate(),
                                                     String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                                   UrlRewrite.CreateShowUserUrl(filteredEvents[0].FromUsername),
                                                                   fromDisplayName/*filteredEvents[0].FromUsername*/),
                                                     updatedProfile.QuestionIDs.Count);
                            }
                        }
                        else
                        {
                            List<int> updatedAnswers = new List<int>();
                            foreach (Event e in filteredEvents)
                            {
                                updatedProfile = Misc.FromXml<UpdatedProfile>(e.DetailsXML);
                                foreach (int questionID in updatedProfile.QuestionIDs)
                                {
                                    if (!updatedAnswers.Contains(questionID)) updatedAnswers.Add(questionID);
                                }
                            }
                            eventCtrl.Text = String.Format("{0} has updated {1} answers on their profile".Translate(),
                                                 String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                               UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                                                               fromDisplayName),
                                                 updatedAnswers.Count);
                        }

                        controls.Add(eventCtrl);
                        break;

                    #endregion

                    #region FriendAttendingEvent

                    case eType.FriendAttendingEvent:
                        filteredEvents = filterEvents(events, ev);
                        eventCtrl = (UserEvent)page.LoadControl("~/Components/UserEvent.ascx");
                        eventCtrl.ID = ev.ID.ToString();
                        eventCtrl.FromUsername = ev.FromUsername;
                        eventCtrl.Time = ev.Date;
                        eventCtrl.UserImageIDs = new List<int>();
                        eventCtrl.GroupImageIDs = new List<int>();
                        eventCtrl.Type = eType.FriendAttendingEvent;
                        eventCtrl.UserImageIDs.Add(Photo.GetPrimaryOrDefaultId(ev.FromUsername));
                        
                        FriendAttendingEvent friendAttendingEvent = null;

                        if (filteredEvents.Length == 1)
                        {
                            friendAttendingEvent = Misc.FromXml<FriendAttendingEvent>(filteredEvents[0].DetailsXML);

                            groupEvent = GroupEvent.Fetch(friendAttendingEvent.EventID);

                            if (groupEvent != null)
                            {
                                group = Group.Fetch(groupEvent.GroupID);

                                if (group != null)
                                {
                                    eventCtrl.GroupImageIDs.Add(group.ID);
                                    eventCtrl.Text =
                                        String.Format("{0} is attending the {1} event from the {2} group".Translate(),
                                                      String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                                    UrlRewrite.CreateShowUserUrl(filteredEvents[0].FromUsername),
                                                                    fromDisplayName/*filteredEvents[0].FromUsername*/),
                                                      String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                                    UrlRewrite.CreateShowGroupEventsUrl(groupEvent.GroupID.ToString(),
                                                                        groupEvent.ID.ToString()), Server.HtmlEncode(groupEvent.Title)),
                                                                    String.Format("<a class=\"skinlinkcolor\" href={0}>{1}</a>",
                                                                                  UrlRewrite.CreateShowGroupUrl(
                                                                                      group.ID.ToString()),
                                                                                  Server.HtmlEncode(group.Name)));
                                }
                            }
                        }
                        else
                        {
                            List<string> lEvents = new List<string>();

                            foreach (Event e in filteredEvents)
                            {
                                friendAttendingEvent = Misc.FromXml<FriendAttendingEvent>(e.DetailsXML);
                                groupEvent = GroupEvent.Fetch(friendAttendingEvent.EventID);
                                if (groupEvent != null)
                                {
                                    lEvents.Add(String.Format("<a href=\"{0}\">{1}</a>",
                                                              UrlRewrite.CreateShowGroupEventsUrl(
                                                                  groupEvent.GroupID.ToString(),
                                                                  groupEvent.ID.ToString()),
                                                              Server.HtmlEncode(groupEvent.Title)));
                                }
                            }

                            eventCtrl.Text = String.Format("{0} is attending the following events: {1}".Translate(), String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>", UrlRewrite.CreateShowUserUrl(ev.FromUsername), fromDisplayName), String.Join(", ", lEvents.ToArray()));
                        }

                        controls.Add(eventCtrl);
                        break;

                    #endregion

                    #region FriendJoinedGroup

                    case eType.FriendJoinedGroup:
                        filteredEvents = filterEvents(events, ev);
                        eventCtrl = (UserEvent)page.LoadControl("~/Components/UserEvent.ascx");
                        eventCtrl.ID = ev.ID.ToString();
                        eventCtrl.FromUsername = ev.FromUsername;
                        eventCtrl.Time = ev.Date;
                        eventCtrl.UserImageIDs = new List<int>();
                        eventCtrl.GroupImageIDs = new List<int>();
                        eventCtrl.Type = eType.FriendJoinedGroup;

                        try
                        {
                            eventCtrl.UserImageIDs.Add(Photo.GetPrimary(ev.FromUsername).Id);
                        }
                        catch (NotFoundException)
                        {
                            eventCtrl.UserImageIDs.Add(getPhotoIDByGender(ev.FromUsername));
                        }
                        if (filteredEvents.Length == 1)
                        {
                            var friendJoinedGroup = Misc.FromXml<FriendJoinedGroup>(filteredEvents[0].DetailsXML);
                            group = Group.Fetch(friendJoinedGroup.GroupID);
                            if (group == null) break;
                            eventCtrl.GroupImageIDs.Add(group.ID);
                            eventCtrl.Text = String.Format(String.Format("{0} has joined the {1} group".Translate(),
                                "<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                "<a class=\"skinlinkcolor\" href=\"{2}\">{3}</a>"),
                                UrlRewrite.CreateShowUserUrl(ev.FromUsername), fromDisplayName,
                                UrlRewrite.CreateShowGroupUrl(group.ID.ToString()), group.Name);
                        }
                        else
                        {
                            var lGroupStrings = new List<string>();
                            foreach (Event e in filteredEvents)
                            {
                                var friendJoinedGroup = Misc.FromXml<FriendJoinedGroup>(e.DetailsXML);
                                group = Group.Fetch(friendJoinedGroup.GroupID);
                                if (group == null) break;
                                eventCtrl.GroupImageIDs.Add(group.ID);
                                lGroupStrings.Add(String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                          UrlRewrite.CreateShowGroupUrl(group.ID.ToString()),
                                                          Server.HtmlEncode(group.Name)));
                            }

                            eventCtrl.Text = String.Format(String.Format("{0} has joined the following groups: {1}".Translate(),
                                "<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>", String.Join(", ", lGroupStrings.ToArray())),
                                UrlRewrite.CreateShowUserUrl(ev.FromUsername), fromDisplayName);
                        }

                        controls.Add(eventCtrl);
                        break;

                    #endregion

                    #region FriendEntersContest

                    case eType.FriendEntersContest:
                        filteredEvents = filterEvents(events, ev);
                        eventCtrl = (UserEvent)page.LoadControl("~/Components/UserEvent.ascx");
                        eventCtrl.ID = ev.ID.ToString();
                        eventCtrl.FromUsername = ev.FromUsername;
                        eventCtrl.Time = ev.Date;
                        eventCtrl.UserImageIDs = new List<int>();
                        eventCtrl.Type = eType.FriendEntersContest;

                        FriendEntersContest friendEntersContest = null;

                        if (filteredEvents.Length == 1)
                        {
                            friendEntersContest = Misc.FromXml<FriendEntersContest>(filteredEvents[0].DetailsXML);

                            PhotoContestEntry pce = PhotoContestEntry.Load(friendEntersContest.PhotoContestEntriesID);

                            if (pce != null)
                            {
                                Photo photo = null;
                                try
                                {
                                    photo = Photo.Fetch(pce.PhotoId);
                                    eventCtrl.UserImageIDs.Add(photo.Id);
                                }
                                catch (NotFoundException)
                                {
                                }

                                PhotoContest photoContest = PhotoContest.Load(pce.ContestId);

                                if (photoContest != null)
                                {
                                    eventCtrl.Text = String.Format("{0} has entered the {1} contest".Translate(), String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                                          UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                                                                          fromDisplayName), String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                         String.Format("PhotoContest.aspx?cid={0}",
                                                                       photoContest.Id), photoContest.Name));
                                }
                            }
                        }
                        else
                        {
                            List<string> lPhotoContest = new List<string>();

                            foreach (Event e in filteredEvents)
                            {
                                friendEntersContest = Misc.FromXml<FriendEntersContest>(e.DetailsXML);
                                PhotoContestEntry pce =
                                    PhotoContestEntry.Load(friendEntersContest.PhotoContestEntriesID);

                                if (pce != null)
                                {
                                    Photo photo = null;
                                    try
                                    {
                                        photo = Photo.Fetch(pce.PhotoId);
                                        eventCtrl.UserImageIDs.Add(photo.Id);
                                    }
                                    catch (NotFoundException)
                                    {
                                    }

                                    PhotoContest photoContest = PhotoContest.Load(pce.ContestId);

                                    if (photoContest != null)
                                    {
                                        lPhotoContest.Add(String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                                        String.Format("PhotoContest.aspx?cid={0}", photoContest.Id),
                                                                        photoContest.Name));
                                    }
                                }
                            }

                            eventCtrl.Text = String.Format("{0} has entered the following contests: {1}".Translate(), String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                                  UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                                                                  fromDisplayName), String.Join(", ", lPhotoContest.ToArray()));
                        }

                        controls.Add(eventCtrl);
                        break;

                    #endregion

                    #region FriendLeftGroup

                    case eType.FriendLeftGroup:
                        filteredEvents = filterEvents(events, ev);
                        eventCtrl = (UserEvent)page.LoadControl("~/Components/UserEvent.ascx");
                        eventCtrl.ID = ev.ID.ToString();
                        eventCtrl.FromUsername = ev.FromUsername;
                        eventCtrl.Time = ev.Date;
                        eventCtrl.UserImageIDs = new List<int>();
                        eventCtrl.GroupImageIDs = new List<int>();
                        eventCtrl.Type = eType.FriendLeftGroup;

                        try
                        {
                            eventCtrl.UserImageIDs.Add(Photo.GetPrimary(ev.FromUsername).Id);
                        }
                        catch (NotFoundException)
                        {
                            eventCtrl.UserImageIDs.Add(getPhotoIDByGender(ev.FromUsername));
                        }

                        FriendLeftGroup friendLeftGroup = null;

                        if (filteredEvents.Length == 1)
                        {
                            friendLeftGroup = Misc.FromXml<FriendLeftGroup>(filteredEvents[0].DetailsXML);
                            group = Group.Fetch(friendLeftGroup.GroupID);

                            if (group != null)
                            {
                                eventCtrl.GroupImageIDs.Add(group.ID);
                                eventCtrl.Text = String.Format("{0} has left the {1} group".Translate(), String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                                      UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                                                                      ev.FromUsername), String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                     UrlRewrite.CreateShowGroupUrl(group.ID.ToString()),
                                                     Server.HtmlEncode(group.Name)));
                            }
                        }
                        else
                        {
                            List<string> lGroups = new List<string>();
                            foreach (Event e in filteredEvents)
                            {
                                friendLeftGroup = Misc.FromXml<FriendLeftGroup>(e.DetailsXML);
                                group = Group.Fetch(friendLeftGroup.GroupID);

                                if (group != null)
                                {
                                    lGroups.Add(String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                              UrlRewrite.CreateShowGroupUrl(group.ID.ToString()),
                                                              Server.HtmlEncode(group.Name)));
                                    eventCtrl.GroupImageIDs.Add(group.ID);
                                }
                            }

                            eventCtrl.Text = String.Format("{0} has left the following groups: {1}".Translate(), String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                                  UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                                                                  fromDisplayName), String.Join(", ", lGroups.ToArray()));
                        }

                        controls.Add(eventCtrl);
                        break;

                    #endregion

                    #region NewProfileComment

                    case eType.NewProfileComment:
                        filteredEvents = filterEvents(events, ev);
                        eventCtrl = (UserEvent)page.LoadControl("~/Components/UserEvent.ascx");
                        eventCtrl.ID = ev.ID.ToString();
                        eventCtrl.FromUsername = ev.FromUsername;
                        eventCtrl.Time = ev.Date;
                        eventCtrl.UserImageIDs = new List<int>();
                        eventCtrl.Type = eType.NewProfileComment;

                        if (filteredEvents.Length == 1)
                        {
                            NewProfileComment newProfileComment =
                                Misc.FromXml<NewProfileComment>(filteredEvents[0].DetailsXML);

                            Comment comment = null;

                            try
                            {
                                comment = Comment.Load(newProfileComment.CommentID);
                            }
                            catch (NotFoundException)
                            {
                                break;
                            }

                            eventCtrl.UserImageIDs.Add(Photo.GetPrimaryOrDefaultId(ev.FromUsername));
                        
                            if (username == ev.FromUsername)
                            {
                                if (comment.FromUsername == ev.FromUsername)
                                {
                                    eventCtrl.Text = String.Format("{0} commented on their own profile - {1}".Translate(),
                                                      String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                                    UrlRewrite.CreateShowUserUrl(username),
                                                                    name), comment.CommentText);
                                }
                                else
                                {
                                    try
                                    {
                                        eventCtrl.UserImageIDs.Add(Photo.GetPrimary(comment.FromUsername).Id);
                                    }
                                    catch (NotFoundException)
                                    {
                                        eventCtrl.UserImageIDs.Add(getPhotoIDByGender(comment.FromUsername));
                                    }
                                    
                                    var commentedName = UserSession.GetViewedUserDisplayedNameByUsername(username, comment.FromUsername);
                                    
                                    eventCtrl.Text =
                                        String.Format("{0} commented on {1}'s profile - {2}".Translate(),
                                                      String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                                    UrlRewrite.CreateShowUserUrl(comment.FromUsername),
                                                                    commentedName),
                                                      String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                                    UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                                                                    fromDisplayName), 
                                                      comment.CommentText);
                                }
                            }
                            else
                            {
                                if (comment.FromUsername == ev.FromUsername)
                                {
                                    eventCtrl.Text = String.Format("{0} commented on their own profile - {1}".Translate(),
                                                                   String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                                                 UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                                                                                 fromDisplayName), 
                                                                   comment.CommentText);
                                }
                                else
                                {
                                    eventCtrl.UserImageIDs.Add(Photo.GetPrimaryOrDefaultId(comment.FromUsername));

                                    var commentedName = UserSession.GetViewedUserDisplayedNameByUsername(username, comment.FromUsername);

                                    eventCtrl.Text =
                                        String.Format("{0} commented on {1}'s profile - {2}".Translate(),
                                                      String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                                    UrlRewrite.CreateShowUserUrl(comment.FromUsername),
                                                                    commentedName),
                                                      String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                                    UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                                                                    fromDisplayName), 
                                                      comment.CommentText);
                                }
                            }

                            #region old

                            //                            if (username == comment.FromUsername)
                            //                            {
                            //                                if (comment.FromUsername == ev.FromUsername)
                            //                                {
                            //                                    eventCtrl.Text = String.Format("{0} commented on their own profile - {1}".Translate(),
                            //                                                      String.Format("<a href=\"{0}\">{1}</a>",
                            //                                                                    UrlRewrite.CreateShowUserUrl(
                            //                                                                        comment.FromUsername),
                            //                                                                    username), comment.CommentText);
                            //                                }
                            //                                else
                            //                                {
                            //                                    try
                            //                                    {
                            //                                        eventCtrl.UserImageIDs.Add(Photo.GetPrimary(ev.FromUsername).Id);
                            //                                    }
                            //                                    catch (NotFoundException)
                            //                                    {
                            //                                    }
                            //
                            //                                    eventCtrl.Text =
                            //                                        String.Format("{0} commented on {1}'s profile - {2}".Translate(),
                            //                                                      String.Format("<a href=\"{0}\">{1}</a>",
                            //                                                                    UrlRewrite.CreateShowUserUrl(
                            //                                                                        username),
                            //                                                                    username),
                            //                                                      String.Format("<a href=\"{0}\">{1}</a>",
                            //                                                                    UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                            //                                                                    ev.FromUsername), comment.CommentText);
                            //                                }
                            //                            }
                            //                            else if (username != ev.FromUsername)
                            //                            {
                            //                                if (comment.FromUsername == ev.FromUsername)
                            //                                {
                            //                                    eventCtrl.Text =
                            //                                        String.Format("{0} commented on their own profile - {1}".Translate(),
                            //                                                      String.Format("<a href=\"{0}\">{1}</a>",
                            //                                                                    UrlRewrite.CreateShowUserUrl(
                            //                                                                        comment.FromUsername),
                            //                                                                    ev.FromUsername), comment.CommentText);
                            //                                }
                            //                                else
                            //                                {
                            //                                    try
                            //                                    {
                            //                                        eventCtrl.UserImageIDs.Add(Photo.GetPrimary(ev.FromUsername).Id);
                            //                                    }
                            //                                    catch (NotFoundException)
                            //                                    {
                            //                                    }
                            //
                            //                                    eventCtrl.Text =
                            //                                        String.Format("{0} commented on {1}'s profile - {2}".Translate(),
                            //                                                      String.Format("<a href=\"{0}\">{1}</a>",
                            //                                                                    UrlRewrite.CreateShowUserUrl(
                            //                                                                        comment.FromUsername),
                            //                                                                    comment.FromUsername),
                            //                                                      String.Format("<a href=\"{0}\">{1}</a>",
                            //                                                                    UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                            //                                                                    ev.FromUsername), comment.CommentText);
                            //                                }
                            //                            }
                            //                            else if (username == ev.FromUsername)
                            //                            {
                            //                                try
                            //                                {
                            //                                    eventCtrl.UserImageIDs.Add(Photo.GetPrimary(ev.FromUsername).Id);
                            //                                }
                            //                                catch (NotFoundException)
                            //                                {
                            //                                }
                            //
                            //                                eventCtrl.Text =
                            //                                    String.Format("{0} commented on {1}'s profile - {2}".Translate(),
                            //                                                  String.Format("<a href=\"{0}\">{1}</a>",
                            //                                                                UrlRewrite.CreateShowUserUrl(
                            //                                                                    comment.FromUsername),
                            //                                                                comment.FromUsername),
                            //                                                  String.Format("<a href=\"{0}\">{1}</a>",
                            //                                                                UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                            //                                                                ev.FromUsername), comment.CommentText);
                            //                            }
                            //                            else
                            //                            {
                            //                                eventCtrl.Text =
                            //                                    String.Format(
                            //                                        "{0} has left a {1}new comment{2} on your profile - {3}".Translate(),
                            //                                        String.Format("<a href=\"{0}\">{1}</a>",
                            //                                                      UrlRewrite.CreateShowUserUrl(comment.FromUsername),
                            //                                                      comment.FromUsername),
                            //                                        "<a href=\"Comments.aspx\">", "</a>", comment.CommentText);
                            //                            }

                            #endregion
                        }
                        else
                        {
                            eventCtrl.UserImageIDs.Add(Photo.GetPrimaryOrDefaultId(ev.FromUsername));
                        
                            eventCtrl.Text =
                                String.Format(
                                    "{0} has {1} new comments on their profile".Translate(),
                                    String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>", 
                                                  UrlRewrite.CreateShowUserUrl(ev.FromUsername), 
                                                  fromDisplayName),
                                    String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>", 
                                                  UrlRewrite.CreateShowUserUrl(ev.fromUsername), 
                                                  filteredEvents.Length));
                        }

                        controls.Add(eventCtrl);
                        break;

                    #endregion

                    #region NewPhotoComment

                    case eType.NewPhotoComment:
                        filteredEvents = filterEvents(events, ev);
                        eventCtrl = (UserEvent)page.LoadControl("~/Components/UserEvent.ascx");
                        eventCtrl.ID = ev.ID.ToString();
                        eventCtrl.FromUsername = ev.FromUsername;
                        eventCtrl.Time = ev.Date;
                        eventCtrl.UserImageIDs = new List<int>();
                        eventCtrl.Type = eType.NewPhotoComment;

                        NewPhotoComment newPhotoComment = null;

                        if (filteredEvents.Length == 1)
                        {
                            newPhotoComment = Misc.FromXml<NewPhotoComment>(filteredEvents[0].DetailsXML);
                            PhotoComment photoComment = PhotoComment.Fetch(newPhotoComment.PhotoCommentID);

                            if (photoComment != null)
                            {
                                eventCtrl.UserImageIDs.Add(Photo.GetPrimaryOrDefaultId(ev.FromUsername));
                                eventCtrl.UserImageIDs.Add(photoComment.PhotoID);

                                if (username == ev.FromUsername)
                                {
                                    if (photoComment.Username == ev.FromUsername)
                                    {
                                        eventCtrl.Text = String.Format(
                                            "{0} commented on their own photo - {1}".Translate(),
                                            String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                          UrlRewrite.CreateShowUserUrl(username),
                                                          name), 
                                            photoComment.Comment);
                                    }
                                    else
                                    {
                                        var commentedName = UserSession.GetViewedUserDisplayedNameByUsername(username, photoComment.Username);
                                        
                                        eventCtrl.Text = String.Format(
                                            "{0} commented on {1}'s photo - {2}".Translate(),
                                            String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                          UrlRewrite.CreateShowUserUrl(photoComment.Username),
                                                          commentedName),
                                            String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                          UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                                                          fromDisplayName), 
                                            photoComment.Comment);
                                    }
                                }
                                else
                                {
                                    if (photoComment.Username == ev.FromUsername)
                                    {
                                        eventCtrl.Text = String.Format(
                                            "{0} commented on their own photo - {1}".Translate(),
                                            String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                          UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                                                          fromDisplayName), 
                                            photoComment.Comment);
                                    }
                                    else
                                    {
                                        var commentedName = UserSession.GetViewedUserDisplayedNameByUsername(username, photoComment.Username);
                                        
                                        eventCtrl.Text = String.Format(
                                            "{0} commented on {1}'s photo - {2}".Translate(),
                                            String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                          UrlRewrite.CreateShowUserUrl(photoComment.Username),
                                                          commentedName),
                                            String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                          UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                                                          fromDisplayName), 
                                            photoComment.Comment);
                                    }
                                }

                                #region old

                                //                                if (username == photoComment.Username)
                                //                                {
                                //                                    if (photoComment.Username == ev.FromUsername)
                                //                                    {
                                //                                        eventCtrl.Text =
                                //                                            String.Format("{0} commented on their own photo - {1}".Translate(),
                                //                                                          String.Format("<a href=\"{0}\">{1}</a>",
                                //                                                                        UrlRewrite.CreateShowUserUrl(
                                //                                                                            photoComment.Username),
                                //                                                                        username), photoComment.Comment);
                                //                                    }
                                //                                    else
                                //                                    {
                                //                                        eventCtrl.Text =
                                //                                            String.Format("{0} commented on {1}'s photo - {2}".Translate(),
                                //                                                          String.Format("<a href=\"{0}\">{1}</a>",
                                //                                                                        UrlRewrite.CreateShowUserUrl(
                                //                                                                            username),
                                //                                                                        username),
                                //                                                          String.Format("<a href=\"{0}\">{1}</a>",
                                //                                                                        UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                                //                                                                        ev.FromUsername), photoComment.Comment);
                                //                                    }
                                //                                }
                                //                                else if (username != ev.FromUsername)
                                //                                {
                                //                                    if (photoComment.Username == ev.FromUsername)
                                //                                    {
                                //                                        eventCtrl.Text =
                                //                                            String.Format("{0} commented on their own photo - {1}".Translate(),
                                //                                                          String.Format("<a href=\"{0}\">{1}</a>",
                                //                                                                        UrlRewrite.CreateShowUserUrl(
                                //                                                                            photoComment.Username),
                                //                                                                        ev.FromUsername), photoComment.Comment);
                                //                                    }
                                //                                    else
                                //                                    {
                                //                                        eventCtrl.Text =
                                //                                            String.Format("{0} commented on {1}'s profile - {2}".Translate(),
                                //                                                          String.Format("<a href=\"{0}\">{1}</a>",
                                //                                                                        UrlRewrite.CreateShowUserUrl(
                                //                                                                            photoComment.Username),
                                //                                                                        photoComment.Username),
                                //                                                          String.Format("<a href=\"{0}\">{1}</a>",
                                //                                                                        UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                                //                                                                        ev.FromUsername), photoComment.Comment);
                                //                                    }
                                //                                }
                                //                                else
                                //                                {
                                //                                    eventCtrl.Text =
                                //                                        String.Format(
                                //                                            "{0} has left a {1}new comment{2} on your photo - {3}".Translate(),
                                //                                            String.Format("<a href=\"{0}\">{1}</a>",
                                //                                                          UrlRewrite.CreateShowUserUrl(photoComment.Username),
                                //                                                          photoComment.Username),
                                //                                            "<a href=\"Comments.aspx\">", "</a>", photoComment.Comment);
                                //                                }

                                #endregion
                            }
                        }
                        else
                        {
                            eventCtrl.UserImageIDs.Add(Photo.GetPrimaryOrDefaultId(ev.FromUsername));
                        
                            foreach (Event e in filteredEvents)
                            {
                                newPhotoComment = Misc.FromXml<NewPhotoComment>(e.DetailsXML);
                                PhotoComment photoComment = PhotoComment.Fetch(newPhotoComment.PhotoCommentID);

                                if (photoComment != null)
                                {
                                    eventCtrl.UserImageIDs.Add(photoComment.PhotoID);
                                }
                            }

                            eventCtrl.Text =
                                String.Format(
                                    "{0} has {1} new comments on their photos".Translate(),
                                    String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>", 
                                                  UrlRewrite.CreateShowUserUrl(ev.FromUsername), 
                                                  fromDisplayName),
                                    String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>", 
                                                  UrlRewrite.CreateShowUserPhotosUrl(ev.fromUsername), 
                                                  filteredEvents.Length));
                        }

                        controls.Add(eventCtrl);
                        break;

                    #endregion

                    #region NewFriendPhoto

                    case eType.NewFriendPhoto:
                        filteredEvents = filterEvents(events, ev);
                        eventCtrl = (UserEvent)page.LoadControl("~/Components/UserEvent.ascx");
                        eventCtrl.ID = ev.ID.ToString();
                        eventCtrl.FromUsername = ev.FromUsername;
                        eventCtrl.Time = ev.Date;
                        eventCtrl.UserImageIDs = new List<int>();
                        eventCtrl.Type = eType.NewFriendPhoto;

                        NewFriendPhoto newFriendPhoto = null;

                        if (filteredEvents.Length == 1)
                        {
                            newFriendPhoto = Misc.FromXml<NewFriendPhoto>(filteredEvents[0].DetailsXML);
                            eventCtrl.UserImageIDs.Add(newFriendPhoto.PhotoID);
                            eventCtrl.Text = String.Format("{0} has uploaded a new photo".Translate(),
                                                 String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                               UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                                                               fromDisplayName));
                        }
                        else
                        {
                            foreach (Event e in filteredEvents)
                            {
                                newFriendPhoto = Misc.FromXml<NewFriendPhoto>(e.DetailsXML);
                                eventCtrl.UserImageIDs.Add(newFriendPhoto.PhotoID);
                            }

                            eventCtrl.Text = String.Format("{0} has uploaded {1} new photos".Translate(),
                                                 String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                               UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                                                               fromDisplayName), filteredEvents.Length);
                        }

                        controls.Add(eventCtrl);
                        break;

                    #endregion

                    #region NewFriendVideoUpload

                    case eType.NewFriendVideoUpload:
                        filteredEvents = filterEvents(events, ev);
                        eventCtrl = (UserEvent)page.LoadControl("~/Components/UserEvent.ascx");
                        eventCtrl.ID = ev.ID.ToString();
                        eventCtrl.FromUsername = ev.FromUsername;
                        eventCtrl.Time = ev.Date;
                        eventCtrl.UserImageIDs = new List<int>();
                        eventCtrl.VideoThumbnailsUrls = new List<string>();
                        eventCtrl.Type = eType.NewFriendVideoUpload;

                        try
                        {
                            eventCtrl.UserImageIDs.Add(Photo.GetPrimary(ev.FromUsername).Id);
                        }
                        catch (NotFoundException)
                        {
                            eventCtrl.UserImageIDs.Add(getPhotoIDByGender(ev.FromUsername));
                        }

                        NewFriendVideoUpload newFriendVideoUpload = null;

                        if (filteredEvents.Length == 1)
                        {
                            newFriendVideoUpload = Misc.FromXml<NewFriendVideoUpload>(filteredEvents[0].DetailsXML);

                            VideoUpload videoUpload = VideoUpload.Load(newFriendVideoUpload.VideoUploadID);

                            if (videoUpload != null)
                            {
                                string thumbnail = String.Format("{0}/UserFiles/{1}/video_{2}.png", Config.Urls.Home,
                                                                 videoUpload.Username, videoUpload.Id);
                                if (!File.Exists(Server.MapPath(String.Format("~/UserFiles/{0}/video_{1}.png",
                                                                              videoUpload.Username, videoUpload.Id))))
                                {
                                    thumbnail = Config.Urls.Home + "/Images/uploadedvideo.gif";
                                }

                                eventCtrl.VideoThumbnailsUrls.Add(thumbnail);

                                eventCtrl.Text = String.Format("{0} has uploaded a new video".Translate(),
                                                     String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                                   UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                                                                   ev.FromUsername));
                            }
                        }
                        else
                        {
                            foreach (Event e in filteredEvents)
                            {
                                newFriendVideoUpload = Misc.FromXml<NewFriendVideoUpload>(e.DetailsXML);
                                VideoUpload videoUpload = VideoUpload.Load(newFriendVideoUpload.VideoUploadID);

                                if (videoUpload != null)
                                {
                                    string thumbnail = String.Format("{0}/UserFiles/{1}/video_{2}.png",
                                                                     Config.Urls.Home,
                                                                     videoUpload.Username, videoUpload.Id);
                                    if (!File.Exists(Server.MapPath(String.Format("~/UserFiles/{0}/video_{1}.png",
                                                                                  videoUpload.Username,
                                                                                  videoUpload.Id))))
                                    {
                                        thumbnail = Config.Urls.Home + "/Images/uploadedvideo.gif";
                                    }

                                    eventCtrl.VideoThumbnailsUrls.Add(thumbnail);
                                }

                            }

                            eventCtrl.Text = String.Format("{0} has uploaded {1} new videos".Translate(),
                                                 String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                               UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                                                               fromDisplayName), filteredEvents.Length);
                        }

                        controls.Add(eventCtrl);
                        break;

                    #endregion

                    #region NewFriendBlogPost

                    case eType.NewFriendBlogPost:
                        filteredEvents = filterEvents(events, ev);
                        eventCtrl = (UserEvent)page.LoadControl("~/Components/UserEvent.ascx");
                        eventCtrl.ID = ev.ID.ToString();
                        eventCtrl.FromUsername = ev.FromUsername;
                        eventCtrl.Time = ev.Date;
                        eventCtrl.UserImageIDs = new List<int>();
                        eventCtrl.Type = eType.NewFriendBlogPost;

                        try
                        {
                            eventCtrl.UserImageIDs.Add(Photo.GetPrimary(ev.FromUsername).Id);
                        }
                        catch (NotFoundException)
                        {
                            eventCtrl.UserImageIDs.Add(getPhotoIDByGender(ev.FromUsername));
                        }

                        NewFriendBlogPost newFriendBlogPost = null;

                        if (filteredEvents.Length == 1)
                        {
                            newFriendBlogPost = Misc.FromXml<NewFriendBlogPost>(filteredEvents[0].DetailsXML);

                            BlogPost blogPost = null;
                            try
                            {
                                blogPost = BlogPost.Load(newFriendBlogPost.BlogPostID);
                            }
                            catch (NotFoundException)
                            {
                                break;
                            }
                            eventCtrl.Text = String.Format("{0} has a new blog post: {1}".Translate(), String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                                  UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                                                                  ev.FromUsername), String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                 UrlRewrite.CreateShowUserBlogUrl(ev.FromUsername, blogPost.Id),
                                                 Server.HtmlEncode(blogPost.Title)));
                        }
                        else
                        {
                            List<string> lBlogPosts = new List<string>();
                            foreach (Event e in filteredEvents)
                            {
                                newFriendBlogPost = Misc.FromXml<NewFriendBlogPost>(e.DetailsXML);
                                BlogPost blogPost = null;
                                try
                                {
                                    blogPost = BlogPost.Load(newFriendBlogPost.BlogPostID);
                                }
                                catch (NotFoundException)
                                {
                                    continue;
                                }
                                lBlogPosts.Add(String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                             UrlRewrite.CreateShowUserBlogUrl(
                                                                 ev.FromUsername, blogPost.Id),
                                                             Server.HtmlEncode(blogPost.Title)));
                            }

                            eventCtrl.Text = String.Format("{0} has {1} new blog posts: {2}".Translate(), String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                                  UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                                                                  fromDisplayName), filteredEvents.Length, String.Join(", ", lBlogPosts.ToArray()));
                        }

                        controls.Add(eventCtrl);
                        break;

                    #endregion

                    #region NewFriendGroup

                    case eType.NewFriendGroup:
                        filteredEvents = filterEvents(events, ev);
                        eventCtrl = (UserEvent)page.LoadControl("~/Components/UserEvent.ascx");
                        eventCtrl.ID = ev.ID.ToString();
                        eventCtrl.FromUsername = ev.FromUsername;
                        eventCtrl.Time = ev.Date;
                        eventCtrl.UserImageIDs = new List<int>();
                        eventCtrl.GroupImageIDs = new List<int>();
                        eventCtrl.Type = eType.NewFriendGroup;

                        try
                        {
                            eventCtrl.UserImageIDs.Add(Photo.GetPrimary(ev.FromUsername).Id);
                        }
                        catch (NotFoundException)
                        {
                            eventCtrl.UserImageIDs.Add(getPhotoIDByGender(ev.FromUsername));
                        }
                        NewFriendGroup newFriendGroup = null;

                        if (filteredEvents.Length == 1)
                        {
                            newFriendGroup = Misc.FromXml<NewFriendGroup>(filteredEvents[0].DetailsXML);
                            group = Group.Fetch(newFriendGroup.GroupID);

                            if (group != null)
                            {
                                eventCtrl.GroupImageIDs.Add(group.ID);

                                eventCtrl.Text = String.Format("{0} has created the {1} group".Translate(), String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                                      UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                                                                      fromDisplayName), String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                     UrlRewrite.CreateShowGroupUrl(group.ID.ToString()),
                                                     Server.HtmlEncode(group.Name)));
                            }
                        }
                        else
                        {
                            List<string> lGroups = new List<string>();
                            foreach (Event e in filteredEvents)
                            {
                                newFriendGroup = Misc.FromXml<NewFriendGroup>(e.DetailsXML);
                                group = Group.Fetch(newFriendGroup.GroupID);

                                if (group != null)
                                {
                                    lGroups.Add(String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                              UrlRewrite.CreateShowGroupUrl(group.ID.ToString()),
                                                              Server.HtmlEncode(group.Name)));

                                    eventCtrl.GroupImageIDs.Add(group.ID);
                                }
                            }

                            eventCtrl.Text = String.Format("{0} has created the following groups: {1}".Translate(), String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                                  UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                                                                  fromDisplayName), String.Join(", ", lGroups.ToArray()));
                        }

                        controls.Add(eventCtrl);
                        break;

                    #endregion

                    #region NewFriendFriend

                    case eType.NewFriendFriend:
                        removeDuplicateEvents(events, ev);
                        eventCtrl = (UserEvent)page.LoadControl("~/Components/UserEvent.ascx");
                        eventCtrl.ID = ev.ID.ToString();
                        eventCtrl.FromUsername = ev.FromUsername;
                        eventCtrl.Time = ev.Date;
                        eventCtrl.UserImageIDs = new List<int>();
                        eventCtrl.Type = eType.NewFriendFriend;

                        eventCtrl.UserImageIDs.Add(Photo.GetPrimaryOrDefaultId(ev.FromUsername));
                        
                        NewFriendFriend newFriendFriend = Misc.FromXml<NewFriendFriend>(ev.DetailsXML);
                        try
                        {
                            user = Classes.User.Load(newFriendFriend.Username);
                        }
                        catch (NotFoundException)
                        {
                            break;
                        }

                        eventCtrl.UserImageIDs.Add(Photo.GetPrimaryOrDefaultId(user.Username));
                        
                        var newFriendsDisplayName = UserSession.GetViewedUserDisplayedNameByUsername(username, user.Username);
                    
                        eventCtrl.Text = String.Format("{0} added {1} as a friend".Translate(),
                            String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                          UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                                          fromDisplayName),
                            String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                          UrlRewrite.CreateShowUserUrl(user.Username),
                                          newFriendsDisplayName));

                        controls.Add(eventCtrl);
                        break;

                    #endregion

                    #region RemovedFriendFriend

                    case eType.RemovedFriendFriend:
                        removeDuplicateEvents(events, ev);
                        eventCtrl = (UserEvent)page.LoadControl("~/Components/UserEvent.ascx");
                        eventCtrl.ID = ev.ID.ToString();
                        eventCtrl.FromUsername = ev.FromUsername;
                        eventCtrl.Time = ev.Date;
                        eventCtrl.UserImageIDs = new List<int>();
                        eventCtrl.Type = eType.RemovedFriendFriend;

                        eventCtrl.UserImageIDs.Add(Photo.GetPrimaryOrDefaultId(ev.FromUsername));
                        
                        RemovedFriendFriend removedFriendFriend = removedFriendFriend = Misc.FromXml<RemovedFriendFriend>(ev.DetailsXML);
                        try
                        {
                            user = Classes.User.Load(removedFriendFriend.Username);
                        }
                        catch (NotFoundException)
                        {
                            break;
                        }

                        eventCtrl.UserImageIDs.Add(Photo.GetPrimaryOrDefaultId(user.Username));

                        var unFriendsDisplayName = UserSession.GetViewedUserDisplayedNameByUsername(username, user.Username);

                        eventCtrl.Text = String.Format("{0} and {1} are no longer friends".Translate(),
                                                       String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                                     UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                                                                     fromDisplayName),
                                                       String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                                     UrlRewrite.CreateShowUserUrl(user.Username),
                                                                     unFriendsDisplayName));

                        controls.Add(eventCtrl);
                        break;

                    #endregion

                    #region NewGroupTopic

                    case eType.NewGroupTopic:
                        filteredEvents = filterEvents(events, ev);
                        eventCtrl = (UserEvent)page.LoadControl("~/Components/UserEvent.ascx");
                        eventCtrl.ID = ev.ID.ToString();
                        eventCtrl.FromUsername = ev.FromUsername;
                        eventCtrl.Time = ev.Date;
                        eventCtrl.UserImageIDs = new List<int>();
                        eventCtrl.GroupImageIDs = new List<int>();
                        eventCtrl.Type = eType.NewGroupTopic;

                        NewGroupTopic newGroupTopic = null;

                        if (filteredEvents.Length == 1)
                        {
                            newGroupTopic = Misc.FromXml<NewGroupTopic>(filteredEvents[0].DetailsXML);

                            groupTopic = GroupTopic.Fetch(newGroupTopic.GroupTopicID);

                            if (groupTopic != null)
                            {
                                group = Group.Fetch(groupTopic.GroupID);

                                if (group != null)
                                {
                                    eventCtrl.GroupImageIDs.Add(group.ID);
                                    eventCtrl.Text = String.Format("{0} has posted a new topic {1} in the {2} group".Translate(),
                                                                    String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                                          UrlRewrite.CreateShowUserUrl(ev.FromUsername), fromDisplayName),
                                                                              String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                         UrlRewrite.CreateShowGroupTopicsUrl(groupTopic.GroupID.ToString(), groupTopic.ID.ToString()),
                                                         Server.HtmlEncode(groupTopic.Name)), String.Format("<a class=\"skinlinkcolor\" href={0}>{1}</a>",
                                                                                UrlRewrite.CreateShowGroupUrl(
                                                                                    group.ID.ToString()),
                                                                                Server.HtmlEncode(group.Name)));
                                }
                            }
                        }
                        else
                        {
                            group = Group.Fetch(ev.FromGroup.Value);

                            if (group != null)
                            {
                                eventCtrl.GroupImageIDs.Add(group.ID);
                                eventCtrl.Text = String.Format("There are {0} new topics in the {1} group".Translate(), String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                                      UrlRewrite.CreateShowGroupTopicsUrl(group.ID.ToString()), filteredEvents.Length),
                                                                      String.Format("<a class=\"skinlinkcolor\" href={0}>{1}</a>", UrlRewrite.CreateShowGroupUrl(group.ID.ToString()),
                                                                        Server.HtmlEncode(group.Name)));
                            }
                        }

                        controls.Add(eventCtrl);
                        break;

                    #endregion

                    #region NewGroupPhoto

                    case eType.NewGroupPhoto:
                        filteredEvents = filterEvents(events, ev);
                        eventCtrl = (UserEvent)page.LoadControl("~/Components/UserEvent.ascx");
                        eventCtrl.ID = ev.ID.ToString();
                        eventCtrl.FromUsername = ev.FromUsername;
                        eventCtrl.Time = ev.Date;
                        eventCtrl.UserImageIDs = new List<int>();
                        eventCtrl.GroupImageIDs = new List<int>();
                        eventCtrl.GroupPhotoIDs = new List<int>();
                        eventCtrl.Type = eType.NewGroupPhoto;

                        NewGroupPhoto newGroupPhoto = null;

                        if (filteredEvents.Length == 1)
                        {
                            newGroupPhoto = Misc.FromXml<NewGroupPhoto>(filteredEvents[0].DetailsXML);
                            groupPhoto = GroupPhoto.Fetch(newGroupPhoto.GroupPhotoID);

                            if (groupPhoto != null)
                            {
                                group = Group.Fetch(ev.FromGroup.Value);

                                if (group != null)
                                {
                                    eventCtrl.GroupImageIDs.Add(group.ID);
                                    eventCtrl.GroupPhotoIDs.Add(newGroupPhoto.GroupPhotoID);

                                    eventCtrl.Text = String.Format("{0} has uploaded a {1}new photo{2} in the {3} group".Translate(),
                                                        String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                            UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                                                            fromDisplayName),
                                                        String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">",
                                                            UrlRewrite.CreateShowGroupPhotosUrl(group.ID.ToString())),
                                                        "</a>",
                                                        String.Format("<a class=\"skinlinkcolor\" href={0}>{1}</a>",
                                                            UrlRewrite.CreateShowGroupUrl(group.ID.ToString()),
                                                            Server.HtmlEncode(group.Name)));
                                }
                            }
                        }
                        else
                        {
                            group = Group.Fetch(ev.FromGroup.Value);

                            if (group != null)
                            {
                                eventCtrl.GroupImageIDs.Add(group.ID);

                                foreach (Event e in filteredEvents)
                                {
                                    newGroupPhoto = Misc.FromXml<NewGroupPhoto>(e.DetailsXML);
                                    try
                                    {
                                        eventCtrl.GroupPhotoIDs.Add(newGroupPhoto.GroupPhotoID);
                                    }
                                    catch (NotFoundException)
                                    {
                                    }
                                }

                                eventCtrl.Text = String.Format("There are {0} new photos in the {1} group".Translate(), String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                     UrlRewrite.CreateShowGroupPhotosUrl(group.ID.ToString()),
                                                     filteredEvents.Length), String.Format("<a class=\"skinlinkcolor\" href={0}>{1}</a>",
                                                                            UrlRewrite.CreateShowGroupUrl(
                                                                                group.ID.ToString()),
                                                                            Server.HtmlEncode(group.Name)));
                            }
                        }

                        controls.Add(eventCtrl);
                        break;

                    #endregion

                    #region NewGroupEvent

                    case eType.NewGroupEvent:
                        filteredEvents = filterEvents(events, ev);
                        eventCtrl = (UserEvent)page.LoadControl("~/Components/UserEvent.ascx");
                        eventCtrl.ID = ev.ID.ToString();
                        eventCtrl.FromUsername = ev.FromUsername;
                        eventCtrl.Time = ev.Date;
                        eventCtrl.UserImageIDs = new List<int>();
                        eventCtrl.GroupImageIDs = new List<int>();
                        eventCtrl.GroupEventImageIDs = new List<int>();
                        eventCtrl.Type = eType.NewGroupEvent;

                        NewGroupEvent newGroupEvent = null;

                        if (filteredEvents.Length == 1)
                        {
                            newGroupEvent = Misc.FromXml<NewGroupEvent>(filteredEvents[0].DetailsXML);
                            groupEvent = GroupEvent.Fetch(newGroupEvent.GroupEventID);

                            if (groupEvent != null)
                            {
                                eventCtrl.GroupEventImageIDs.Add(newGroupEvent.GroupEventID);

                                group = Group.Fetch(ev.FromGroup.Value);

                                if (group != null)
                                {
                                    eventCtrl.GroupImageIDs.Add(group.ID);

                                    eventCtrl.Text = String.Format("There is a new event {0} in the {1} group".Translate(),
                                                        String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>", UrlRewrite.CreateShowGroupEventsUrl(group.ID.ToString(), groupEvent.ID.ToString()),
                                                         Server.HtmlEncode(groupEvent.Title)),
                                                         String.Format("<a class=\"skinlinkcolor\" href={0}>{1}</a>", UrlRewrite.CreateShowGroupUrl(group.ID.ToString()), Server.HtmlEncode(group.Name)));
                                }
                            }
                        }
                        else
                        {
                            group = Group.Fetch(ev.FromGroup.Value);

                            if (group != null)
                            {
                                foreach (Event e in filteredEvents)
                                {
                                    newGroupEvent = Misc.FromXml<NewGroupEvent>(e.DetailsXML);
                                    eventCtrl.GroupEventImageIDs.Add(newGroupEvent.GroupEventID);
                                }

                                eventCtrl.GroupImageIDs.Add(group.ID);
                                eventCtrl.Text = String.Format("There are {0} new events in the {1} group".Translate(), String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                     UrlRewrite.CreateShowGroupEventsUrl(group.ID.ToString()),
                                                     filteredEvents.Length), String.Format("<a class=\"skinlinkcolor\" href={0}>{1}</a>",
                                                                            UrlRewrite.CreateShowGroupUrl(
                                                                                group.ID.ToString()),
                                                                            Server.HtmlEncode(group.Name)));
                            }
                        }

                        controls.Add(eventCtrl);
                        break;

                    #endregion

                    #region FriendUpdatedStatus

                    case eType.FriendUpdatedStatus:
                        eventCtrl = (UserEvent)page.LoadControl("~/Components/UserEvent.ascx");
                        eventCtrl.ID = ev.ID.ToString();
                        eventCtrl.FromUsername = ev.FromUsername;
                        eventCtrl.Time = ev.Date;
                        eventCtrl.Type = eType.FriendUpdatedStatus;

                        events.Remove(ev);
                        eventCtrl.UserImageIDs = new List<int>();
                        eventCtrl.UserImageIDs.Add(Photo.GetPrimaryOrDefaultId(ev.FromUsername));
                        
                        Classes.FriendUpdatedStatus friendUpdatedStatus = Misc.FromXml<FriendUpdatedStatus>(ev.DetailsXML);

                        if (friendUpdatedStatus != null && !String.IsNullOrEmpty(friendUpdatedStatus.Status))
                        {
                            eventCtrl.Text = String.Format("{0} has changed their status to {1}".Translate(), String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                                  UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                                                                  fromDisplayName), Server.HtmlEncode(friendUpdatedStatus.Status));
                        }

                        controls.Add(eventCtrl);
                        break;

                    #endregion

                    #region TaggedOnPhoto

                    case eType.TaggedOnPhoto:
                        filteredEvents = filterEvents(events, ev);
                        eventCtrl = (UserEvent)page.LoadControl("~/Components/UserEvent.ascx");
                        eventCtrl.ID = ev.ID.ToString();
                        eventCtrl.FromUsername = ev.FromUsername;
                        eventCtrl.Time = ev.Date;
                        eventCtrl.UserImageIDs = new List<int>();
                        eventCtrl.Type = eType.TaggedOnPhoto;

                        TaggedOnPhoto taggedPhoto = null;

                        eventCtrl.UserImageIDs.Add(Photo.GetPrimaryOrDefaultId(ev.FromUsername));
                        
                        if (filteredEvents.Length == 1)
                        {
                            taggedPhoto = Misc.FromXml<TaggedOnPhoto>(filteredEvents[0].DetailsXML);

                            PhotoNote[] notes = PhotoNote.Load(taggedPhoto.NoteID, null, null);

                            if (notes.Length > 0)
                            {
                                eventCtrl.UserImageIDs.Add(notes[0].PhotoId);
                                
                                var notedName = UserSession.GetViewedUserDisplayedNameByUsername(username, notes[0].Username);

                                eventCtrl.Text = String.Format("{0} has tagged {1} on their photo".Translate(),
                                                     String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                                   UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                                                                   fromDisplayName),
                                                     String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                                   UrlRewrite.CreateShowUserUrl(notes[0].Username),
                                                                   notedName ));
                            }
                        }
                        else
                        {
                            List<string> lUsernames = new List<string>();
                            List<string> lTaggedUsernames = new List<string>();
                            foreach (Event e in filteredEvents)
                            {
                                taggedPhoto = Misc.FromXml<TaggedOnPhoto>(e.DetailsXML);

                                PhotoNote[] notes = PhotoNote.Load(taggedPhoto.NoteID, null, null);

                                if (notes.Length > 0)
                                {
                                    if (!lUsernames.Contains(notes[0].Username))
                                    {
                                        lUsernames.Add(notes[0].Username);
                                        lTaggedUsernames.Add(String.Format("<a href=\"{0}\">{1}</a>",
                                                                           UrlRewrite.CreateShowUserUrl(
                                                                               notes[0].Username),
                                                                           notes[0].Username));
                                    }

                                    eventCtrl.UserImageIDs.Add(notes[0].PhotoId);
                                }
                            }

                            eventCtrl.Text = String.Format("{0} has tagged {1} on their photos".Translate(),
                                                 String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                               UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                                                               fromDisplayName),
                                                 String.Join(", ", lTaggedUsernames.ToArray()));
                        }

                        controls.Add(eventCtrl);
                        break;

                    #endregion

                    #region NewFriendAudioUpload

                    case eType.NewFriendAudioUpload:
                        filteredEvents = filterEvents(events, ev);
                        eventCtrl = (UserEvent)page.LoadControl("~/Components/UserEvent.ascx");
                        eventCtrl.ID = ev.ID.ToString();
                        eventCtrl.FromUsername = ev.FromUsername;
                        eventCtrl.Time = ev.Date;
                        eventCtrl.UserImageIDs = new List<int>();
                        eventCtrl.Type = eType.NewFriendAudioUpload;

                        try
                        {
                            eventCtrl.UserImageIDs.Add(Photo.GetPrimary(ev.FromUsername).Id);
                        }
                        catch (NotFoundException)
                        {
                            eventCtrl.UserImageIDs.Add(getPhotoIDByGender(ev.FromUsername));
                        }

                        if (filteredEvents.Length == 1)
                        {
                            eventCtrl.Text =
                                String.Format(
                                    "{0} has uploaded a new audio".Translate(),
                                    String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                  UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                                                  fromDisplayName));
                        }
                        else
                        {
                            eventCtrl.Text = String.Format("{0} has uploaded {1} new audios".Translate(),
                                                 String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                               UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                                                               fromDisplayName), filteredEvents.Length);
                        }

                        controls.Add(eventCtrl);
                        break;

                    #endregion

                    #region NewFriendYouTubeUpload

                    case eType.NewFriendYouTubeUpload:
                        filteredEvents = filterEvents(events, ev);
                        eventCtrl = (UserEvent)page.LoadControl("~/Components/UserEvent.ascx");
                        eventCtrl.ID = ev.ID.ToString();
                        eventCtrl.FromUsername = ev.FromUsername;
                        eventCtrl.Time = ev.Date;
                        eventCtrl.UserImageIDs = new List<int>();
                        eventCtrl.VideoThumbnailsUrls = new List<string>();
                        eventCtrl.Type = eType.NewFriendYouTubeUpload;

                        try
                        {
                            eventCtrl.UserImageIDs.Add(Photo.GetPrimary(ev.FromUsername).Id);
                        }
                        catch (NotFoundException)
                        {
                            eventCtrl.UserImageIDs.Add(getPhotoIDByGender(ev.FromUsername));
                        }

                        NewFriendYouTubeUpload newFriendYouTubeUpload = null;

                        if (filteredEvents.Length == 1)
                        {
                            newFriendYouTubeUpload =
                                Misc.FromXml<NewFriendYouTubeUpload>(filteredEvents[0].DetailsXML);
                            VideoEmbed embed = VideoEmbed.Load(newFriendYouTubeUpload.YouTubeUploadID);

                            if (embed != null)
                            {
                                string thumbnail = embed.ThumbUrl;
                                eventCtrl.VideoThumbnailsUrls.Add(thumbnail);
                                eventCtrl.Text = String.Format("{0} has uploaded a new video".Translate(),
                                                     String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                                   UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                                                                   fromDisplayName));
                            }
                        }
                        else
                        {
                            foreach (Event e in filteredEvents)
                            {
                                newFriendYouTubeUpload = Misc.FromXml<NewFriendYouTubeUpload>(e.DetailsXML);
                                VideoEmbed embed = VideoEmbed.Load(newFriendYouTubeUpload.YouTubeUploadID);
                                if (embed != null)
                                {
                                    string thumbnail = embed.ThumbUrl;

                                    eventCtrl.VideoThumbnailsUrls.Add(thumbnail);
                                }
                            }

                            eventCtrl.Text = String.Format("{0} has uploaded {1} new videos".Translate(),
                                                 String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                               UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                                                               fromDisplayName),
                                                 filteredEvents.Length);
                        }

                        controls.Add(eventCtrl);
                        break;

                    #endregion

                    #region NewFriendRelationship

                    case eType.NewFriendRelationship:
                        removeDuplicateEvents(events, ev);
                        eventCtrl = (UserEvent)page.LoadControl("~/Components/UserEvent.ascx");
                        eventCtrl.ID = ev.ID.ToString();
                        eventCtrl.FromUsername = ev.FromUsername;
                        eventCtrl.Time = ev.Date;
                        eventCtrl.UserImageIDs = new List<int>();
                        eventCtrl.Type = eType.NewFriendRelationship;

                        eventCtrl.UserImageIDs.Add(Photo.GetPrimaryOrDefaultId(ev.FromUsername));
                        
                        NewFriendRelationship newRelationship = null;

                        newRelationship = Misc.FromXml<NewFriendRelationship>(ev.DetailsXML);
                        User u = null;
                        try
                        {
                            u = Classes.User.Load(newRelationship.Username);
                        }
                        catch (NotFoundException)
                        {
                            break;
                        }

                        eventCtrl.UserImageIDs.Add(Photo.GetPrimaryOrDefaultId(u.Username));
                        
                        var relatedName = UserSession.GetViewedUserDisplayedNameByUsername(username, u.Username);

                        eventCtrl.Text = String.Format("{0} and {1} are now in relationship ({2})".Translate(),
                                                       String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                                     UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                                                                     fromDisplayName),
                                                       String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                                     UrlRewrite.CreateShowUserUrl(u.Username),
                                                                     relatedName),
                                                       Relationship.GetRelationshipStatusString(newRelationship.Type));
                        controls.Add(eventCtrl);
                        break;

                    #endregion

                    #region RemovedFriendRelationship

                    case eType.RemovedFriendRelationship:
                        removeDuplicateEvents(events, ev);
                        eventCtrl = (UserEvent)page.LoadControl("~/Components/UserEvent.ascx");
                        eventCtrl.ID = ev.ID.ToString();
                        eventCtrl.FromUsername = ev.FromUsername;
                        eventCtrl.Time = ev.Date;
                        eventCtrl.UserImageIDs = new List<int>();
                        eventCtrl.Type = eType.RemovedFriendRelationship;

                        eventCtrl.UserImageIDs.Add(Photo.GetPrimaryOrDefaultId(ev.FromUsername));
                        
                        RemovedFriendRelationship removedRelationship =
                            removedRelationship = Misc.FromXml<RemovedFriendRelationship>(ev.DetailsXML);

                        try
                        {
                            user = Classes.User.Load(removedRelationship.Username);
                        }
                        catch (NotFoundException)
                        {
                            break;
                        }
                        eventCtrl.UserImageIDs.Add(Photo.GetPrimaryOrDefaultId(user.Username));
                        
                        var unrelatedName = UserSession.GetViewedUserDisplayedNameByUsername(username, user.Username);
        
                        eventCtrl.Text = String.Format("{0} and {1} are no longer in relationship ({2})".Translate(),
                                                                          String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                                                        UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                                                                                        fromDisplayName), String.Format("<a class=\"skinlinkcolor\" href=\"{0}\">{1}</a>",
                                                                                                                        UrlRewrite.CreateShowUserUrl(user.Username),
                                                                                                                        unrelatedName),
                                                                          Relationship.GetRelationshipStatusString(removedRelationship.Type));
                        controls.Add(eventCtrl);
                        break;
                    //                        if (filteredEvents.Length == 1)
                    //                        {
                    //                            removedRelationship = Misc.FromXml<RemovedFriendRelationship>(filteredEvents[0].DetailsXML);
                    //                            User user = null;
                    //                            try
                    //                            {
                    //                                user = Classes.User.Load(removedRelationship.Username);
                    //                            }
                    //                            catch (NotFoundException)
                    //                            {
                    //                                break;
                    //                            }
                    //
                    //                            try
                    //                            {
                    //                                eventCtrl.UserImageIDs.Add(Photo.GetPrimary(user.Username).Id);
                    //                            }
                    //                            catch (NotFoundException)
                    //                            {
                    //                            }
                    //
                    //                            eventCtrl.Text =
                    //                                String.Format("{0} and {1} are no longer in relationship ({2})".Translate(),
                    //                                              String.Format("<a href=\"{0}\">{1}</a>",
                    //                                                            UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                    //                                                            ev.FromUsername), String.Format("<a href=\"{0}\">{1}</a>",
                    //                                                                                            UrlRewrite.CreateShowUserUrl
                    //                                                                                                (user.Username),
                    //                                                                                            user.Username),
                    //                                              Relationship.GetRelationshipStatusString(removedRelationship.Type));
                    //                        }
                    //                        else
                    //                        {
                    //                            List<string> lUser = new List<string>();
                    //                            foreach (Event e in filteredEvents)
                    //                            {
                    //                                removedRelationship = Misc.FromXml<RemovedFriendRelationship>(e.DetailsXML);
                    //                                User user = null;
                    //                                try
                    //                                {
                    //                                    user = Classes.User.Load(removedRelationship.Username);
                    //                                }
                    //                                catch (NotFoundException)
                    //                                {
                    //                                    continue;
                    //                                }
                    //
                    //                                try
                    //                                {
                    //                                    eventCtrl.UserImageIDs.Add(Photo.GetPrimary(user.Username).Id);
                    //                                }
                    //                                catch (NotFoundException)
                    //                                {
                    //                                }
                    //
                    //                                lUser.Add(String.Format("<a href=\"{0}\">{1}</a>",
                    //                                                           UrlRewrite.CreateShowUserUrl(user.Username),
                    //                                                           user.Username));
                    //                            }
                    //
                    //                            eventCtrl.Text = String.Format("{0} is no longer in relationship with {1}".Translate(), String.Format("<a href=\"{0}\">{1}</a>",
                    //                                                                  UrlRewrite.CreateShowUserUrl(ev.FromUsername),
                    //                                                                  ev.FromUsername), String.Join(", ", lUser.ToArray()));
                    //                        }

                    //                        controls.Add(eventCtrl);
                    //                        break;

                    #endregion
                }
            }

            return controls;
        }

        private static Event[] filterEvents(List<Event> events, Event e)
        {
            List<Event> result = null;
            result = events.Where(u => u.FromUsername == e.FromUsername && u.FromGroup == e.FromGroup && u.Type == e.Type && u.Date >= e.Date.AddDays(-1)).ToList();
            events.RemoveAll(result.Contains);
            //            if (e.Type == eType.NewFriendFriend)
            //            {
            //                events.RemoveAll(ev => ev.Type == eType.NewFriendFriend &&
            //                                 result.Exists(
            //                                     r => r.FromUsername == Misc.FromXml<NewFriendFriend>(ev.DetailsXML).Username
            //                                     && ev.FromUsername == Misc.FromXml<NewFriendFriend>(r.DetailsXML).Username));
            //            }
            //            else if (e.Type == eType.RemovedFriendFriend)
            //            {
            //                events.RemoveAll(ev => ev.Type == eType.RemovedFriendFriend &&
            //                                 result.Exists(
            //                                     r => r.FromUsername == Misc.FromXml<RemovedFriendFriend>(ev.DetailsXML).Username
            //                                     && ev.FromUsername == Misc.FromXml<RemovedFriendFriend>(r.DetailsXML).Username));
            //            }
            //            else if (e.Type == eType.NewFriendRelationship)
            //            {
            //                events.RemoveAll(ev => ev.Type == eType.NewFriendRelationship &&
            //                                 result.Exists(
            //                                     r => r.FromUsername == Misc.FromXml<NewFriendRelationship>(ev.DetailsXML).Username
            //                                     && ev.FromUsername == Misc.FromXml<NewFriendRelationship>(r.DetailsXML).Username));
            //            }
            //            else if (e.Type == eType.RemovedFriendRelationship)
            //            {
            //                events.RemoveAll(ev => ev.Type == eType.RemovedFriendRelationship &&
            //                                 result.Exists(
            //                                     r => r.FromUsername == Misc.FromXml<RemovedFriendRelationship>(ev.DetailsXML).Username
            //                                     && ev.FromUsername == Misc.FromXml<RemovedFriendRelationship>(r.DetailsXML).Username));
            //            }
            return result.ToArray();
        }

        private static void removeDuplicateEvents(List<Event> events, Event e)
        {
            events.Remove(e);
            if (e.Type == eType.NewFriendRelationship)
            {
                events.RemoveAll(ev => ev.Type == eType.NewFriendRelationship &&
                                     e.FromUsername == Misc.FromXml<NewFriendRelationship>(ev.DetailsXML).Username
                                     && ev.FromUsername == Misc.FromXml<NewFriendRelationship>(e.DetailsXML).Username);
            }
            else if (e.Type == eType.RemovedFriendRelationship)
            {
                events.RemoveAll(ev => ev.Type == eType.RemovedFriendRelationship &&
                                 e.FromUsername == Misc.FromXml<RemovedFriendRelationship>(ev.DetailsXML).Username
                                     && ev.FromUsername == Misc.FromXml<RemovedFriendRelationship>(e.DetailsXML).Username);
            }
            else if (e.Type == eType.NewFriendFriend)
            {
                events.RemoveAll(ev => ev.Type == eType.NewFriendFriend &&
                                 e.FromUsername == Misc.FromXml<NewFriendFriend>(ev.DetailsXML).Username
                                     && ev.FromUsername == Misc.FromXml<NewFriendFriend>(e.DetailsXML).Username);
            }
            else if (e.Type == eType.RemovedFriendFriend)
            {
                events.RemoveAll(ev => ev.Type == eType.RemovedFriendFriend &&
                                 e.FromUsername == Misc.FromXml<RemovedFriendFriend>(ev.DetailsXML).Username
                                     && ev.FromUsername == Misc.FromXml<RemovedFriendFriend>(e.DetailsXML).Username);
            }
        }

        private static int getPhotoIDByGender(string username)
        {
            int imageId = 0;

            try
            {
                User user = Classes.User.Load(username);
                imageId = ImageHandler.GetPhotoIdByGender(user.Gender);
            }
            catch (NotFoundException)
            {
            }

            return imageId;
        }

        #endregion
    }

    [Serializable]
    public class UpdatedProfile
    {
        private List<int> questionIDs;

        public List<int> QuestionIDs
        {
            get { return questionIDs; }
            set { questionIDs = value; }
        }
    }

    [Serializable]
    public class FriendAttendingEvent
    {
        private int eventID;

        public int EventID
        {
            get { return eventID; }
            set { eventID = value; }
        }
    }

    //    [Serializable]
    //    public class FriendPostedTopic
    //    {
    //        private int topicID;
    //
    //        public int TopicID
    //        {
    //            get { return topicID; }
    //            set { topicID = value; }
    //        }
    //    }

    [Serializable]
    public class FriendEntersContest
    {
        private int photoContestEntriesID;

        public int PhotoContestEntriesID
        {
            get { return photoContestEntriesID; }
            set { photoContestEntriesID = value; }
        }
    }

    [Serializable]
    public class FriendJoinedGroup
    {
        private int groupID;

        public int GroupID
        {
            get { return groupID; }
            set { groupID = value; }
        }
    }

    [Serializable]
    public class FriendLeftGroup
    {
        private int groupID;

        public int GroupID
        {
            get { return groupID; }
            set { groupID = value; }
        }
    }

    [Serializable]
    public class NewProfileComment
    {
        private int commentID;

        public int CommentID
        {
            get { return commentID; }
            set { commentID = value; }
        }
    }

    [Serializable]
    public class NewPhotoComment
    {
        private int photoCommentID;

        public int PhotoCommentID
        {
            get { return photoCommentID; }
            set { photoCommentID = value; }
        }
    }

    [Serializable]
    public class NewFriendPhoto
    {
        private int photoID;

        public int PhotoID
        {
            get { return photoID; }
            set { photoID = value; }
        }
    }

    [Serializable]
    public class NewFriendVideoUpload
    {
        private int videoUploadID;

        public int VideoUploadID
        {
            get { return videoUploadID; }
            set { videoUploadID = value; }
        }
    }

    [Serializable]
    public class NewFriendYouTubeUpload
    {
        private int youTubeUploadID;

        public int YouTubeUploadID
        {
            get { return youTubeUploadID; }
            set { youTubeUploadID = value; }
        }
    }

    [Serializable]
    public class NewFriendBlogPost
    {
        private int blogPostID;

        public int BlogPostID
        {
            get { return blogPostID; }
            set { blogPostID = value; }
        }
    }

    [Serializable]
    public class NewFriendGroup
    {
        private int groupID;

        public int GroupID
        {
            get { return groupID; }
            set { groupID = value; }
        }
    }

    [Serializable]
    public class NewFriendFriend
    {
        private string username;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }
    }

    [Serializable]
    public class RemovedFriendFriend
    {
        private string username;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }
    }

    [Serializable]
    public class NewFriendRelationship
    {
        private string username;
        private Relationship.eRelationshipStatus type;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public Relationship.eRelationshipStatus Type
        {
            get { return type; }
            set { type = value; }
        }
    }

    [Serializable]
    public class RemovedFriendRelationship
    {
        private string username;
        private Relationship.eRelationshipStatus type;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public Relationship.eRelationshipStatus Type
        {
            get { return type; }
            set { type = value; }
        }
    }



    [Serializable]
    public class NewGroupTopic
    {
        private int groupTopicID;

        public int GroupTopicID
        {
            get { return groupTopicID; }
            set { groupTopicID = value; }
        }
    }

    [Serializable]
    public class NewSubscribedGroupPost
    {
        private int groupPostID;

        public int GroupPostID
        {
            get { return groupPostID; }
            set { groupPostID = value; }
        }
    }

    [Serializable]
    public class NewGroupPhoto
    {
        private int groupPhotoID;

        public int GroupPhotoID
        {
            get { return groupPhotoID; }
            set { groupPhotoID = value; }
        }
    }

    [Serializable]
    public class NewGroupEvent
    {
        private int groupEventID;

        public int GroupEventID
        {
            get { return groupEventID; }
            set { groupEventID = value; }
        }
    }

    [Serializable]
    public class FriendUpdatedStatus
    {
        public string Status { get; set; }
    }

    [Serializable]
    public class TaggedOnPhoto
    {
        private int noteID;

        public int NoteID
        {
            get { return noteID; }
            set { noteID = value; }
        }
    }

    [Serializable]
    public class NewFriendAudioUpload
    {
        private int audioUploadID;

        public int AudioUploadID
        {
            get { return audioUploadID; }
            set { audioUploadID = value; }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using ezFixUp.Classes;

namespace ezFixUp.Components.WebParts
{
    [Themeable(true), Editable]
    public partial class UserEventsWebPart : WebPartUserControl
    {
        protected UserSession CurrentUserSession
        {
            get { return ((PageBase) Page).CurrentUserSession; }
        }

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Number of events")]
        public int NumberOfEvents
        {
            get { return (int) (ViewState["UserEvents_NumberOfEvents"] ?? 10); }
            set { ViewState["UserEvents_NumberOfEvents"] = value; }
        }

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Your friend has a birthday")]
        public bool FriendBirthday
        {
            get { return (bool)(ViewState["UserEvents_FriendBirthday"] ?? true); }
            set { ViewState["UserEvents_FriendBirthday"] = value; }
        }

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Your friend has updated his profile")]
        public bool FriendUpdatedProfile
        {
            get { return (bool)(ViewState["UserEvents_FriendUpdatedProfile"] ?? true); }
            set { ViewState["UserEvents_FriendUpdatedProfile"] = value; }
        }

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Your friend is attending an event")]
        public bool FriendAttendingEvent
        {
            get { return (bool)(ViewState["UserEvents_FriendAttendingEvent"] ?? true); }
            set { ViewState["UserEvents_FriendAttendingEvent"] = value; }
        }

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Your friend has posted new topic")]
        public bool FriendPostedTopic
        {
            get { return (bool)(ViewState["UserEvents_FriendPostedTopic"] ?? true); }
            set { ViewState["UserEvents_FriendPostedTopic"] = value; }
        }

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Your friend has uploaded new group photo")]
        public bool FriendUploadedGroupPhoto
        {
            get { return (bool)(ViewState["UserEvents_FriendUploadedGroupPhoto"] ?? true); }
            set { ViewState["UserEvents_FriendUploadedGroupPhoto"] = value; }
        }

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Your friend has enter contest")]
        public bool FriendEntersContest
        {
            get { return (bool)(ViewState["UserEvents_FriendEntersContest"] ?? true); }
            set { ViewState["UserEvents_FriendEntersContest"] = value; }
        }

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Your friend has joined to group")]
        public bool FriendJoinedGroup
        {
            get { return (bool)(ViewState["UserEvents_FriendJoinedGroup"] ?? true); }
            set { ViewState["UserEvents_FriendJoinedGroup"] = value; }
        }

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Your friend has left a group")]
        public bool FriendLeftGroup
        {
            get { return (bool)(ViewState["UserEvents_FriendLeftGroup"] ?? true); }
            set { ViewState["UserEvents_FriendLeftGroup"] = value; }
        }

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("There is a new profile comment")]
        public bool NewProfileComment
        {
            get { return (bool)(ViewState["UserEvents_NewProfileComment"] ?? true); }
            set { ViewState["UserEvents_NewProfileComment"] = value; }
        }

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("There is a new photo comment")]
        public bool NewPhotoComment
        {
            get { return (bool)(ViewState["UserEvents_NewPhotoComment"] ?? true); }
            set { ViewState["UserEvents_NewPhotoComment"] = value; }
        }

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Your friend has new photo")]
        public bool NewFriendPhoto
        {
            get { return (bool)(ViewState["UserEvents_NewFriendPhoto"] ?? true); }
            set { ViewState["UserEvents_NewFriendPhoto"] = value; }
        }

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Your friend has new video upload")]
        public bool NewFriendVideoUpload
        {
            get { return (bool)(ViewState["UserEvents_NewFriendVideoUpload"] ?? true); }
            set { ViewState["UserEvents_NewFriendVideoUpload"] = value; }
        }

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Your friend has new blog post")]
        public bool NewFriendBlogPost
        {
            get { return (bool)(ViewState["UserEvents_NewFriendBlogPost"] ?? true); }
            set { ViewState["UserEvents_NewFriendBlogPost"] = value; }
        }

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Your friend has created a new group")]
        public bool NewFriendGroup
        {
            get { return (bool)(ViewState["UserEvents_NewFriendGroup"] ?? true); }
            set { ViewState["UserEvents_NewFriendGroup"] = value; }
        }

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Your friend has new friend")]
        public bool NewFriendFriend
        {
            get { return (bool)(ViewState["UserEvents_NewFriendFriend"] ?? true); }
            set { ViewState["UserEvents_NewFriendFriend"] = value; }
        }

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("There is a new group topic")]
        public bool NewGroupTopic
        {
            get { return (bool)(ViewState["UserEvents_NewGroupTopic"] ?? true); }
            set { ViewState["UserEvents_NewGroupTopic"] = value; }
        }

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("There is a new post in topic for which you are subscribed to")]
        public bool NewSubscribedGroupPost
        {
            get { return (bool)(ViewState["UserEvents_NewSubscribedGroupPost"] ?? true); }
            set { ViewState["UserEvents_NewSubscribedGroupPost"] = value; }
        }

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("There is a new group photo")]
        public bool NewGroupPhoto
        {
            get { return (bool)(ViewState["UserEvents_NewGroupPhoto"] ?? true); }
            set { ViewState["UserEvents_NewGroupPhoto"] = value; }
        }

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("There is a new group event")]
        public bool NewGroupEvent
        {
            get { return (bool)(ViewState["UserEvents_NewGroupEvent"] ?? true); }
            set { ViewState["UserEvents_NewGroupEvent"] = value; }
        }

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("There is a new interest")]
        public bool NewInterest
        {
            get { return (bool)(ViewState["UserEvents_NewInterest"] ?? true); }
            set { ViewState["UserEvents_NewInterest"] = value; }
        }

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Your friend has updated their status")]
        public bool FriendUpdatedStatus
        {
            get { return (bool)(ViewState["UserEvents_FriendUpdatedStatus"] ?? true); }
            set { ViewState["UserEvents_FriendUpdatedStatus"] = value; }
        }

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Your friend has tagged you on photo")]
        public bool TaggedOnPhoto
        {
            get { return (bool)(ViewState["UserEvents_TaggedOnPhoto"] ?? true); }
            set { ViewState["UserEvents_TaggedOnPhoto"] = value; }
        }

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Your friend has new audio upload")]
        public bool NewFriendAudioUpload
        {
            get { return (bool)(ViewState["UserEvents_NewFriendAudioUpload"] ?? true); }
            set { ViewState["UserEvents_NewFriendAudioUpload"] = value; }
        }

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Your friend has new youtube video upload")]
        public bool NewFriendYouTubeUpload
        {
            get { return (bool)(ViewState["UserEvents_NewFriendYouTubeUpload"] ?? true); }
            set { ViewState["UserEvents_NewFriendYouTubeUpload"] = value; }
        }

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Your friend removed somebody from their friends list")]
        public bool RemovedFriendFriend
        {
            get { return (bool)(ViewState["UserEvents_RemovedFriendFriend"] ?? true); }
            set { ViewState["UserEvents_RemovedFriendFriend"] = value; }
        }

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Your friend has new relationship")]
        public bool NewFriendRelationship
        {
            get { return (bool)(ViewState["UserEvents_NewFriendRelationship"] ?? true); }
            set { ViewState["UserEvents_NewFriendRelationship"] = value; }
        }

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Your friend is no longer in relationship")]
        public bool RemovedFriendRelationship
        {
            get { return (bool)(ViewState["UserEvents_RemovedFriendRelationship"] ?? true); }
            set { ViewState["UserEvents_RemovedFriendRelationship"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = Lang.Translate("User Events");
            loadEvents();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        private void loadEvents()
        {
            ulong typeMask = 0;

//            if (AddedToFriends) typeMask |= (ulong) Event.eType.AddedToFriends;
            if (FriendBirthday) typeMask |= (ulong)Event.eType.FriendBirthday;
            if (FriendUpdatedProfile) typeMask |= (ulong)Event.eType.FriendUpdatedProfile;
            if (FriendAttendingEvent) typeMask |= (ulong)Event.eType.FriendAttendingEvent;
//            if (FriendPostedTopic) typeMask |= (ulong)Event.eType.FriendPostedTopic;
//            if (FriendUploadedGroupPhoto) typeMask |= (ulong)Event.eType.FriendUploadedGroupPhoto;
//            if (FriendEntersContest) typeMask |= (ulong)Event.eType.FriendEntersContest;
//            if (FriendJoinedGroup) typeMask |= (ulong)Event.eType.FriendJoinedGroup;
//            if (FriendLeftGroup) typeMask |= (ulong)Event.eType.FriendLeftGroup;
            if (NewProfileComment) typeMask |= (ulong)Event.eType.NewProfileComment;
            if (NewPhotoComment) typeMask |= (ulong)Event.eType.NewPhotoComment;
            if (NewFriendPhoto) typeMask |= (ulong)Event.eType.NewFriendPhoto;
//            if (NewFriendVideoUpload) typeMask |= (ulong)Event.eType.NewFriendVideoUpload;
//            if (NewFriendBlogPost) typeMask |= (ulong)Event.eType.NewFriendBlogPost;
//            if (NewFriendGroup) typeMask |= (ulong)Event.eType.NewFriendGroup;
            if (NewFriendFriend) typeMask |= (ulong)Event.eType.NewFriendFriend;
//            if (NewGroupTopic) typeMask |= (ulong)Event.eType.NewGroupTopic;
//            if (NewSubscribedGroupPost) typeMask |= (ulong)Event.eType.NewSubscribedGroupPost;
//            if (NewGroupPhoto) typeMask |= (ulong)Event.eType.NewGroupPhoto;
//            if (NewGroupEvent) typeMask |= (ulong)Event.eType.NewGroupEvent;
//            if (FriendUpdatedStatus) typeMask |= (ulong)Event.eType.FriendUpdatedStatus;
            if (TaggedOnPhoto) typeMask |= (ulong)Event.eType.TaggedOnPhoto;
//            if (NewFriendAudioUpload) typeMask |= (ulong)Event.eType.NewFriendAudioUpload;
//            if (NewFriendYouTubeUpload) typeMask |= (ulong)Event.eType.NewFriendYouTubeUpload;
            if (RemovedFriendFriend) typeMask |= (ulong)Event.eType.RemovedFriendFriend;
            if (NewFriendRelationship) typeMask |= (ulong)Event.eType.NewFriendRelationship;
            if (RemovedFriendRelationship) typeMask |= (ulong)Event.eType.RemovedFriendRelationship;

            List<Control> eventControls = Event.PrepareEventsControls(CurrentUserSession.Username, typeMask,
                                                                      NumberOfEvents);

            foreach (var eventCtrl in eventControls)
            {
                plhEvents.Controls.Add(eventCtrl);
            }

            //Page.RegisterJQuery();
            //Page.RegisterJQueryLightbox();
            //Page.Header.Controls.Add(new LiteralControl(
            //        "<link href=\"images/jquery.lightbox.css\" rel=\"stylesheet\" type=\"text/css\" />"));

            if (eventControls.Count > 0)
            {
                mvUserEvents.SetActiveView(viewEvents);

                //ScriptManager.RegisterStartupScript(this, typeof(UserEventsWebPart), "lightbox",
                //    "$(function() {$('div.eventimg a').lightBox();});", true);
            }
            else
            {
                mvUserEvents.SetActiveView(viewNoEvents);
            }
        }
    }
}
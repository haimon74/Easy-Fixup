using System;

namespace ezFixUp.Classes
{
    /// <summary>
    /// This is the base class for the user restrictions
    /// </summary>
    [Serializable]
    [Reflection.Description("User Restrictions")]
    public class UserRestrictions
    {
        private int maxMessagesPerDay = -1;

        [Reflection.Description("Maximum messages user can send a day (use -1 for unlimited)")]
        public int MaxMessagesPerDay
        {
            get { return maxMessagesPerDay; }
            set { maxMessagesPerDay = value; }
        }

        private int maxWinksPerDay = -1;

        [Reflection.Description("Maximum winks user can send a day (use -1 for unlimited)")]
        public int MaxWinksPerDay
        {
            get { return maxWinksPerDay; }
            set { maxWinksPerDay = value; }
        }

        private int maxEcardsPerDay = -1;

        [Reflection.Description("Maximum e-cards user can send a day (use -1 for unlimited)")]
        public int MaxEcardsPerDay
        {
            get { return maxEcardsPerDay; }
            set { maxEcardsPerDay = value; }
        }

        private int maxPhotos = Config.Photos.MaxPhotos;

        /// <summary>
        /// Gets or sets the max photos.
        /// </summary>
        /// <value>The max photos.</value>
        [Reflection.Description("Maximum number of photos the member can upload")]
        public int MaxPhotos
        {
            get { return maxPhotos; }
            set { maxPhotos = value; }
        }

        private int maxVideos = 6;// = Config.Misc.MaxYouTubeVideos;

        /// <summary>
        /// Gets or sets the max videos.
        /// </summary>
        /// <value>The max videos.</value>
        [Reflection.Description("Maximum number of videos the member can embed")]
        public int MaxVideos
        {
            get { return maxVideos; }
            set { maxVideos = value; }
        }

        private bool canCreateGroups = true;

        /// <summary>
        /// Gets or sets a value indicating whether this instance can create groups.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can create groups; otherwise, <c>false</c>.
        /// </value>
        [Reflection.Description("Member can create groups")]
        public bool CanCreateGroups
        {
            get { return canCreateGroups; }
            set { canCreateGroups = value; }
        }

        private bool canSeeMessageStatus = Config.Users.UsersCanSeeMessageStatus;

        /// <summary>
        /// Gets or sets a value indicating whether this instance can create groups.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can create groups; otherwise, <c>false</c>.
        /// </value>
        [Reflection.Description("Can see message status")]
        public bool CanSeeMessageStatus
        {
            get { return canSeeMessageStatus; }
            set { canSeeMessageStatus = value; }
        }

        private bool canRateProfiles = Config.Ratings.EnableProfileRatings;

        [Reflection.Description("Member can rate profiles")]
        public bool CanRateProfiles
        {
            get { return canRateProfiles; }
            set { canRateProfiles = value; }
        }

        private bool canRatePhotos = Config.Ratings.EnablePhotoRatings;

        [Reflection.Description("Member can rate photos")]
        public bool CanRatePhotos
        {
            get { return canRatePhotos; }
            set { canRatePhotos = value; }
        }

        private bool canCreateBlogs = Config.Misc.EnableBlogs;

        /// <summary>
        /// Gets or sets a value indicating whether this instance can create blogs.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can create blogs; otherwise, <c>false</c>.
        /// </value>
        [Reflection.Description("Member can create blogs")]
        public bool CanCreateBlogs
        {
            get { return canCreateBlogs; }
            set { canCreateBlogs = value; }
        }

        private int maxGroupsPerMember = 30;//Config.Groups.MaxGroupsPerMember;

        /// <summary>
        /// Gets or sets the max groups per member.
        /// </summary>
        /// <value>The max groups per member.</value>
        [Reflection.Description("Maximum number of groups the member can join")]
        public int MaxGroupsPerMember
        {
            get { return maxGroupsPerMember; }
            set { maxGroupsPerMember = value; }
        }

        private int maxVideoUploads = 3;//Config.Misc.MaxVideoUploads;

        /// <summary>
        /// Gets or sets the max video uploads.
        /// </summary>
        /// <value>The max video uploads.</value>
        [Reflection.Description("Maximum number of videos the member can upload")]
        public int MaxVideoUploads
        {
            get { return maxVideoUploads; }
            set { maxVideoUploads = value; }
        }

        private int maxAudioUploads = 3;

        [Reflection.Description("Maximum number of audio uploads the member can upload")]
        public int MaxAudioUploads
        {
            get { return maxAudioUploads; }
            set { maxAudioUploads = value; }
        }

        private bool autoApproveAnswers = false;

        /// <summary>
        /// Gets or sets a value indicating whether [auto approve answers].
        /// </summary>
        /// <value><c>true</c> if [auto approve answers]; otherwise, <c>false</c>.</value>
        [Reflection.Description("Auto approve answers")]
        public bool AutoApproveAnswers
        {
            get { return autoApproveAnswers; }
            set { autoApproveAnswers = value; }
        }

        private bool autoApprovePhotos = Config.Photos.AutoApprovePhotos;

        /// <summary>
        /// Gets or sets a value indicating whether [auto approve photos].
        /// </summary>
        /// <value><c>true</c> if [auto approve photos]; otherwise, <c>false</c>.</value>
        [Reflection.Description("Auto approve photos")]
        public bool AutoApprovePhotos
        {
            get { return autoApprovePhotos; }
            set { autoApprovePhotos = value; }
        }

        private bool autoApproveVideos = false;

        /// <summary>
        /// Gets or sets a value indicating whether [auto approve videos].
        /// </summary>
        /// <value><c>true</c> if [auto approve videos]; otherwise, <c>false</c>.</value>
        [Reflection.Description("Auto approve videos")]
        public bool AutoApproveVideos
        {
            get { return autoApproveVideos; }
            set { autoApproveVideos = value; }
        }

        private bool autoApproveAudioUploads = false;

        /// <summary>
        /// Gets or sets a value indicating whether [auto approve audio uploads].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [auto approve audio uploads]; otherwise, <c>false</c>.
        /// </value>
        [Reflection.Description("Auto approve audio uploads")]
        public bool AutoApproveAudioUploads
        {
            get { return autoApproveAudioUploads; }
            set { autoApproveAudioUploads = value; }
        }

        private bool autoApproveAds = false;

        [Reflection.Description("Auto approve classifieds")]
        public bool AutoApproveAds
        {
            get { return autoApproveAds; }
            set { autoApproveAds = value; }
        }

        private int maxActiveAds = 5;

        [Reflection.Description("Maximum active classifieds")]
        public int MaxActiveAds
        {
            get { return maxActiveAds; }
            set { maxActiveAds = value; }
        }

        private bool userCanReportAbuse = true;

        /// <summary>
        /// Gets or sets a value indicating whether [user can report abuse].
        /// </summary>
        /// <value><c>true</c> if [user can report abuse]; otherwise, <c>false</c>.</value>
        [Reflection.Description("Can report abuse")]
        public bool UserCanReportAbuse
        {
            get { return userCanReportAbuse; }
            set { userCanReportAbuse = value; }
        }

        private bool userCanUseChat = true;

        /// <summary>
        /// Gets or sets a value indicating whether [user can use chat].
        /// </summary>
        /// <value><c>true</c> if [user can use chat]; otherwise, <c>false</c>.</value>
        [Reflection.Description("Can use chat")]
        public bool UserCanUseChat
        {
            get { return userCanUseChat; }
            set { userCanUseChat = value; }
        }

        private bool userCanUseSkin = Config.Users.EnableProfileSkins;

        [Reflection.Description("Can add comments")]
        public bool UserCanAddComments
        {
            get { return userCanAddComments; }
            set { userCanAddComments = value; }
        }

        private bool userCanAddComments = true;

        [Reflection.Description("Can use skin")]
        public bool UserCanUseSkin
        {
            get { return userCanUseSkin; }
            set { userCanUseSkin = value; }
        }

        private bool userCanEditSkin = Config.Users.EnableProfileSkins;

        /// <summary>
        /// Gets or sets a value indicating whether [user can use chat].
        /// </summary>
        /// <value><c>true</c> if [user can use chat]; otherwise, <c>false</c>.</value>
        [Reflection.Description("Can edit skin")]
        public bool UserCanEditSkin
        {
            get { return userCanEditSkin; }
            set { userCanEditSkin = value; }
        }

        private bool userCanBrowseClassifieds = Config.Ads.Enable;

        [Reflection.Description("Can browse classifieds")]
        public bool UserCanBrowseClassifieds
        {
            get { return userCanBrowseClassifieds; }
            set { userCanBrowseClassifieds = value; }
        }

        private bool userCanPostAd = Config.Ads.Enable;

        [Reflection.Description("Can post classifieds")]
        public bool UserCanPostAd
        {
            get { return userCanPostAd; }
            set { userCanPostAd = value; }
        }
    }
}
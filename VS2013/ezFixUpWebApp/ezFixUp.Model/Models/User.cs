using System;
using System.Collections.Generic;

namespace ezFixUp.Model.Models
{
    public class User
    {
        public User()
        {
            this.AdComments = new List<AdComment>();
            this.Ads = new List<Ad>();
            this.AffiliateCommissions = new List<AffiliateCommission>();
            this.AudioAccesses = new List<AudioAccess>();
            this.AudioAccesses1 = new List<AudioAccess>();
            this.AudioUploads = new List<AudioUpload>();
            this.BlockedUsers = new List<BlockedUser>();
            this.BlockedUsers1 = new List<BlockedUser>();
            this.BlogPostComments = new List<BlogPostComment>();
            this.Blogs = new List<Blog>();
            this.Comments = new List<Comment>();
            this.Comments1 = new List<Comment>();
            this.CommunityPhotoApprovals = new List<CommunityPhotoApproval>();
            this.CommunityProfileApprovals = new List<CommunityProfileApproval>();
            this.CommunityProfileApprovals1 = new List<CommunityProfileApproval>();
            this.CreditsHistories = new List<CreditsHistory>();
            this.Ecards = new List<Ecard>();
            this.Ecards1 = new List<Ecard>();
            this.EstablishedCommunications = new List<EstablishedCommunication>();
            this.EstablishedCommunications1 = new List<EstablishedCommunication>();
            this.EventComments = new List<EventComment>();
            this.Events = new List<Event>();
            this.Favourites = new List<Favourite>();
            this.Favourites1 = new List<Favourite>();
            this.Friends = new List<Friend>();
            this.Friends1 = new List<Friend>();
            this.GroupBans = new List<GroupBan>();
            this.GroupEvents = new List<GroupEvent>();
            this.GroupEventsComments = new List<GroupEventsComment>();
            this.GroupMembers = new List<GroupMember>();
            this.GroupPhotos = new List<GroupPhoto>();
            this.GroupPollsAnswers = new List<GroupPollsAnswer>();
            this.GroupPosts = new List<GroupPost>();
            this.GroupTopics = new List<GroupTopic>();
            this.GroupTopicSubscriptions = new List<GroupTopicSubscription>();
            this.Interests = new List<Interest>();
            this.Interests1 = new List<Interest>();
            this.Messages = new List<Message>();
            this.Messages1 = new List<Message>();
            this.MessagesSandboxes = new List<MessagesSandbox>();
            this.PaymentHistories = new List<PaymentHistory>();
            this.PhotoAccesses = new List<PhotoAccess>();
            this.PhotoAccesses1 = new List<PhotoAccess>();
            this.PhotoAlbums = new List<PhotoAlbum>();
            this.PhotoComments = new List<PhotoComment>();
            this.PhotoContestRanks = new List<PhotoContestRank>();
            this.PhotoContestVotes = new List<PhotoContestVote>();
            this.PhotoNotes = new List<PhotoNote>();
            this.PhotoRatings = new List<PhotoRating>();
            this.Photos = new List<Photo>();
            this.PollAnswers = new List<PollAnswer>();
            this.ProfileAnswers = new List<ProfileAnswer>();
            this.ProfileViews = new List<ProfileView>();
            this.ProfileViews1 = new List<ProfileView>();
            this.Ratings = new List<Rating>();
            this.Ratings1 = new List<Rating>();
            this.Relationships = new List<Relationship>();
            this.Relationships1 = new List<Relationship>();
            this.SavedSearches = new List<SavedSearch>();
            this.Subscriptions = new List<Subscription>();
            this.UnlockedSections = new List<UnlockedSection>();
            this.UnlockedSections1 = new List<UnlockedSection>();
            this.UserScoresLogs = new List<UserScoresLog>();
            this.VerifiedUsers = new List<VerifiedUser>();
            this.VerifiedUsers1 = new List<VerifiedUser>();
            this.VideoAccesses = new List<VideoAccess>();
            this.VideoAccesses1 = new List<VideoAccess>();
            this.VideoEmbeds = new List<VideoEmbed>();
            this.VideoProfiles = new List<VideoProfile>();
            this.GroupEvents1 = new List<GroupEvent>();
        }

        public string u_username { get; set; }
        public string u_password { get; set; }
        public string u_email { get; set; }
        public string u_name { get; set; }
        public int u_gender { get; set; }
        public System.DateTime u_birthdate { get; set; }
        public Nullable<System.DateTime> u_birthdate2 { get; set; }
        public System.DateTime u_usersince { get; set; }
        public bool u_active { get; set; }
        public bool u_smsconfirmed { get; set; }
        public System.DateTime u_prevlogin { get; set; }
        public System.DateTime u_lastlogin { get; set; }
        public int u_logincount { get; set; }
        public System.DateTime u_lastonline { get; set; }
        public int u_profileviews { get; set; }
        public bool u_profilevisible { get; set; }
        public bool u_receiveemails { get; set; }
        public string u_state { get; set; }
        public string u_zip_code { get; set; }
        public string u_city { get; set; }
        public bool u_deleted { get; set; }
        public bool u_paid_member { get; set; }
        public string u_signup_ip { get; set; }
        public Nullable<int> u_interested_in { get; set; }
        public string u_lastsessionid { get; set; }
        public int u_message_verifications_left { get; set; }
        public bool u_verifiedbyadmin { get; set; }
        public int l_id { get; set; }
        public string u_incoming_messages_restrictions { get; set; }
        public string u_billingdetails { get; set; }
        public string u_invited_by { get; set; }
        public Nullable<int> a_id { get; set; }
        public long u_options { get; set; }
        public string u_deletereason { get; set; }
        public Nullable<double> u_longitude { get; set; }
        public Nullable<double> u_latitude { get; set; }
        public int u_score { get; set; }
        public string u_tokenuniqueid { get; set; }
        public string u_personalizationinfo { get; set; }
        public int u_credits { get; set; }
        public int u_moderationscore { get; set; }
        public bool u_spamsuspected { get; set; }
        public bool u_face_control_approved { get; set; }
        public string u_profile_skin { get; set; }
        public string u_status_text { get; set; }
        public bool u_featuredmember { get; set; }
        public string u_myspaceid { get; set; }
        public Nullable<long> u_facebookid { get; set; }
        public Nullable<long> u_eventssettings { get; set; }
        public string u_country { get; set; }
        public Nullable<int> u_invitedby_count { get; set; }
        public virtual ICollection<AdComment> AdComments { get; set; }
        public virtual ICollection<Ad> Ads { get; set; }
        public virtual ICollection<AffiliateCommission> AffiliateCommissions { get; set; }
        public virtual ICollection<AudioAccess> AudioAccesses { get; set; }
        public virtual ICollection<AudioAccess> AudioAccesses1 { get; set; }
        public virtual ICollection<AudioUpload> AudioUploads { get; set; }
        public virtual ICollection<BlockedUser> BlockedUsers { get; set; }
        public virtual ICollection<BlockedUser> BlockedUsers1 { get; set; }
        public virtual ICollection<BlogPostComment> BlogPostComments { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Comment> Comments1 { get; set; }
        public virtual ICollection<CommunityPhotoApproval> CommunityPhotoApprovals { get; set; }
        public virtual ICollection<CommunityProfileApproval> CommunityProfileApprovals { get; set; }
        public virtual ICollection<CommunityProfileApproval> CommunityProfileApprovals1 { get; set; }
        public virtual ICollection<CreditsHistory> CreditsHistories { get; set; }
        public virtual ICollection<Ecard> Ecards { get; set; }
        public virtual ICollection<Ecard> Ecards1 { get; set; }
        public virtual ICollection<EstablishedCommunication> EstablishedCommunications { get; set; }
        public virtual ICollection<EstablishedCommunication> EstablishedCommunications1 { get; set; }
        public virtual ICollection<EventComment> EventComments { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Favourite> Favourites { get; set; }
        public virtual ICollection<Favourite> Favourites1 { get; set; }
        public virtual ICollection<Friend> Friends { get; set; }
        public virtual ICollection<Friend> Friends1 { get; set; }
        public virtual ICollection<GroupBan> GroupBans { get; set; }
        public virtual ICollection<GroupEvent> GroupEvents { get; set; }
        public virtual ICollection<GroupEventsComment> GroupEventsComments { get; set; }
        public virtual ICollection<GroupMember> GroupMembers { get; set; }
        public virtual ICollection<GroupPhoto> GroupPhotos { get; set; }
        public virtual ICollection<GroupPollsAnswer> GroupPollsAnswers { get; set; }
        public virtual ICollection<GroupPost> GroupPosts { get; set; }
        public virtual ICollection<GroupTopic> GroupTopics { get; set; }
        public virtual ICollection<GroupTopicSubscription> GroupTopicSubscriptions { get; set; }
        public virtual ICollection<Interest> Interests { get; set; }
        public virtual ICollection<Interest> Interests1 { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<Message> Messages1 { get; set; }
        public virtual ICollection<MessagesSandbox> MessagesSandboxes { get; set; }
        public virtual ICollection<PaymentHistory> PaymentHistories { get; set; }
        public virtual ICollection<PhotoAccess> PhotoAccesses { get; set; }
        public virtual ICollection<PhotoAccess> PhotoAccesses1 { get; set; }
        public virtual ICollection<PhotoAlbum> PhotoAlbums { get; set; }
        public virtual ICollection<PhotoComment> PhotoComments { get; set; }
        public virtual ICollection<PhotoContestRank> PhotoContestRanks { get; set; }
        public virtual ICollection<PhotoContestVote> PhotoContestVotes { get; set; }
        public virtual ICollection<PhotoNote> PhotoNotes { get; set; }
        public virtual ICollection<PhotoRating> PhotoRatings { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<PollAnswer> PollAnswers { get; set; }
        public virtual ICollection<ProfileAnswer> ProfileAnswers { get; set; }
        public virtual ICollection<ProfileView> ProfileViews { get; set; }
        public virtual ICollection<ProfileView> ProfileViews1 { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<Rating> Ratings1 { get; set; }
        public virtual ICollection<Relationship> Relationships { get; set; }
        public virtual ICollection<Relationship> Relationships1 { get; set; }
        public virtual ICollection<SavedSearch> SavedSearches { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
        public virtual TwitterCredential TwitterCredential { get; set; }
        public virtual ICollection<UnlockedSection> UnlockedSections { get; set; }
        public virtual ICollection<UnlockedSection> UnlockedSections1 { get; set; }
        public virtual ICollection<UserScoresLog> UserScoresLogs { get; set; }
        public virtual ICollection<VerifiedUser> VerifiedUsers { get; set; }
        public virtual ICollection<VerifiedUser> VerifiedUsers1 { get; set; }
        public virtual ICollection<VideoAccess> VideoAccesses { get; set; }
        public virtual ICollection<VideoAccess> VideoAccesses1 { get; set; }
        public virtual ICollection<VideoEmbed> VideoEmbeds { get; set; }
        public virtual ICollection<VideoProfile> VideoProfiles { get; set; }
        public virtual ICollection<GroupEvent> GroupEvents1 { get; set; }
    }
}

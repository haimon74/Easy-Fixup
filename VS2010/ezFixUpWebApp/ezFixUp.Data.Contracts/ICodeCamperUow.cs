using ezFixUp.Model;
using ezFixUp.Model.Models;

namespace ezFixUp.Data.Contracts
{
    /// <summary>
    /// Interface for the Code Camper "Unit of Work"
    /// </summary>
    public interface IezFixUpUow
    {
        // Save pending changes to the data store.
        void Commit();

        // Repositories
        IPersonsRepository Persons { get; }
        IRepository<Room> Rooms { get; }
        ISessionsRepository Sessions { get; }
        IRepository<TimeSlot> TimeSlots { get; }
        IRepository<Track> Tracks { get; }
        IAttendanceRepository Attendance { get; }




        IRepository<AbuseReport> AbuseReports { get; }
        IRepository<AdComment> AdComments { get; }
        IRepository<Admin> Admins { get; }
        IRepository<AdPhoto> AdPhotos { get; }
        IRepository<Ad> Ads { get; }
        IRepository<AdsCategory> AdsCategories { get; }
        IRepository<AffiliateBanner> AffiliateBanners { get; }
        IRepository<AffiliateCommission> AffiliateCommissions { get; }
        IRepository<Affiliate> Affiliates { get; }
        IRepository<AffiliatesHistory> AffiliatesHistories { get; }
        IRepository<archive_Messages> archive_Messages { get; }
        IRepository<archive_Photos> archive_Photos { get; }
        IRepository<archive_ProfileAnswers> archive_ProfileAnswers { get; }
        IRepository<AskAboutQuestion> AskAboutQuestions { get; }
        IRepository<AudioAccess> AudioAccesses { get; }
        IRepository<AudioUpload> AudioUploads { get; }
        IRepository<BannerCode> BannerCodes { get; }
        IRepository<BillingPlan> BillingPlans { get; }
        IRepository<BlockedUser> BlockedUsers { get; }
        IRepository<BlogPostComment> BlogPostComments { get; }
        IRepository<BlogPost> BlogPosts { get; }
        IRepository<Blog> Blogs { get; }
        IRepository<Category> Categories { get; }
        IRepository<ChatAdmin> ChatAdmins { get; }
        IRepository<ChatBan> ChatBans { get; }
        IRepository<ChatIgnoredUser> ChatIgnoredUsers { get; }
        IRepository<ChatMessage> ChatMessages { get; }
        IRepository<ChatPresence> ChatPresences { get; }
        IRepository<ChatRoomActivity> ChatRoomActivities { get; }
        IRepository<ChatRoomAdmin> ChatRoomAdmins { get; }
        IRepository<ChatRoom> ChatRooms { get; }
        IRepository<ChatSetting> ChatSettings { get; }
        IRepository<ChatTranslation> ChatTranslations { get; }
        IRepository<ChatUser> ChatUsers { get; }
        IRepository<Comment> Comments { get; }
        IRepository<CommunityPhotoApproval> CommunityPhotoApprovals { get; }
        IRepository<CommunityProfileApproval> CommunityProfileApprovals { get; }
        IRepository<ContactU> ContactUs { get; }
        IRepository<ContentPage> ContentPages { get; }
        IRepository<ContentView> ContentViews { get; }
        IRepository<Coupon> Coupons { get; }
        IRepository<CouponType> CouponTypes { get; }
        IRepository<CouponUs> CouponUses { get; }
        IRepository<CreditsHistory> CreditsHistories { get; }
        IRepository<CreditsPackage> CreditsPackages { get; }
        IRepository<Discount> Discounts { get; }
        IRepository<ECardCategory> ECardCategories { get; }
        IRepository<Ecard> Ecards { get; }
        IRepository<EcardType> EcardTypes { get; }
        IRepository<EmailQueue> EmailQueues { get; }
        IRepository<EmailTemplate> EmailTemplates { get; }
        IRepository<EstablishedCommunication> EstablishedCommunications { get; }
        IRepository<EventComment> EventComments { get; }
        IRepository<Event> Events { get; }
        IRepository<FacebookCity> FacebookCities { get; }
        IRepository<FacebookCountry> FacebookCountries { get; }
        IRepository<FacebookInvitation> FacebookInvitations { get; }
        IRepository<FaqCategory> FaqCategories { get; }
        IRepository<FAQ> FAQs { get; }
        IRepository<Favourite> Favourites { get; }
        IRepository<Friend> Friends { get; }
        IRepository<GiftCategory> GiftCategories { get; }
        IRepository<GiftsMessage> GiftsMessages { get; }
        IRepository<GiftType> GiftTypes { get; }
        IRepository<GroupBan> GroupBans { get; }
        IRepository<GroupEvent> GroupEvents { get; }
        IRepository<GroupEventsComment> GroupEventsComments { get; }
        IRepository<GroupMember> GroupMembers { get; }
        IRepository<GroupPhoto> GroupPhotos { get; }
        IRepository<GroupPollsAnswer> GroupPollsAnswers { get; }
        IRepository<GroupPollsChoice> GroupPollsChoices { get; }
        IRepository<GroupPost> GroupPosts { get; }
        IRepository<Group> Groups { get; }
        IRepository<GroupTopic> GroupTopics { get; }
        IRepository<GroupTopicSubscription> GroupTopicSubscriptions { get; }
        IRepository<HourlyStat> HourlyStats { get; }
        IRepository<Interest> Interests { get; }
        IRepository<IPLog> IPLogs { get; }
        IRepository<Language> Languages { get; }
        IRepository<Matchmaking> Matchmakings { get; }
        IRepository<Message> Messages { get; }
        IRepository<MessagesHistoryCache> MessagesHistoryCaches { get; }
        IRepository<MessagesSandbox> MessagesSandboxes { get; }
        IRepository<MessagesType> MessagesTypes { get; }
        IRepository<News> News { get; }
        IRepository<PaymentHistory> PaymentHistories { get; }
        IRepository<PendingGuid> PendingGuids { get; }
        IRepository<PhotoAccess> PhotoAccesses { get; }
        IRepository<PhotoAlbum> PhotoAlbums { get; }
        IRepository<PhotoComment> PhotoComments { get; }
        IRepository<PhotoContestEntry> PhotoContestEntries { get; }
        IRepository<PhotoContestRank> PhotoContestRanks { get; }
        IRepository<PhotoContest> PhotoContests { get; }
        IRepository<PhotoContestVote> PhotoContestVotes { get; }
        IRepository<PhotoNote> PhotoNotes { get; }
        IRepository<PhotoRating> PhotoRatings { get; }
        IRepository<Photo> Photos { get; }
        IRepository<PollAnswer> PollAnswers { get; }
        IRepository<PollChoice> PollChoices { get; }
        IRepository<Poll> Polls { get; }
        IRepository<ProfileAnswer> ProfileAnswers { get; }
        IRepository<ProfileChoice> ProfileChoices { get; }
        IRepository<ProfileQuestion> ProfileQuestions { get; }
        IRepository<ProfileTopic> ProfileTopics { get; }
        IRepository<ProfileView> ProfileViews { get; }
        IRepository<Rating> Ratings { get; }
        IRepository<Relationship> Relationships { get; }
        IRepository<SavedSearch> SavedSearches { get; }
        IRepository<ScheduledAnnouncement> ScheduledAnnouncements { get; }
        IRepository<Setting> Settings { get; }
        IRepository<Subscription> Subscriptions { get; }
        IRepository<Template> Templates { get; }
        IRepository<TipsPopup> TipsPopups { get; }
        IRepository<Translation> Translations { get; }
        IRepository<TwitterCredential> TwitterCredentials { get; }
        IRepository<UnlockedSection> UnlockedSections { get; }
        IRepository<UserLevel> UserLevels { get; }
        IRepository<User> Users { get; }
        IRepository<UserScoresLog> UserScoresLogs { get; }
        IRepository<UserTipsStatu> UserTipsStatus { get; }
        IRepository<VerifiedUser> VerifiedUsers { get; }
        IRepository<VideoAccess> VideoAccesses { get; }
        IRepository<VideoEmbed> VideoEmbeds { get; }
        IRepository<VideoProfile> VideoProfiles { get; }
        IRepository<VideoUpload> VideoUploads { get; }
        IRepository<Vote> Votes { get; }
        IRepository<WinkCategory> WinkCategories { get; }
        IRepository<WinkMessaage> WinkMessaages { get; }
        IRepository<WinkType> WinkTypes { get; }
        IRepository<View_FemaleInterestedInFemaleSingles> View_FemaleInterestedInFemaleSingles { get; }
        IRepository<View_FemaleInterestedInMaleSingles> View_FemaleInterestedInMaleSingles { get; }
        IRepository<View_MaleInterestedInFemaleSingles> View_MaleInterestedInFemaleSingles { get; }
        IRepository<View_MaleInterestedInMaleSingles> View_MaleInterestedInMaleSingles { get; }
    }
}
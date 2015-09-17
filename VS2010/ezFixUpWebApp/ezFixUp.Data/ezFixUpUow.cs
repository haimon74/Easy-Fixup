using System;
using ezFixUp.Model;
using ezFixUp.Model.Models;
using IAttendanceRepository = ezFixUp.Data.Contracts.IAttendanceRepository;
using IezFixUpUow = ezFixUp.Data.Contracts.IezFixUpUow;
using IPersonsRepository = ezFixUp.Data.Contracts.IPersonsRepository;
using ISessionsRepository = ezFixUp.Data.Contracts.ISessionsRepository;

//using ezFixUp.Model;

namespace ezFixUp.Data
{
    /// <summary>
    /// The Code Camper "Unit of Work"
    ///     1) decouples the repos from the controllers
    ///     2) decouples the DbContext and EF from the controllers
    ///     3) manages the UoW
    /// </summary>
    /// <remarks>
    /// This class implements the "Unit of Work" pattern in which
    /// the "UoW" serves as a facade for querying and saving to the database.
    /// Querying is delegated to "repositories".
    /// Each repository serves as a container dedicated to a particular
    /// root entity type such as a <see cref="Person"/>.
    /// A repository typically exposes "Get" methods for querying and
    /// will offer add, update, and delete methods if those features are supported.
    /// The repositories rely on their parent UoW to provide the interface to the
    /// data layer (which is the EF DbContext in Code Camper).
    /// </remarks>
    public class ezFixUpUow : IezFixUpUow, IDisposable
    {
        public ezFixUpUow(IRepositoryProvider repositoryProvider)
        {
            CreateDbContext();

            repositoryProvider.DbContext = DbContext;
            RepositoryProvider = repositoryProvider;       
        }

        // Code Camper repositories

        public Contracts.IRepository<Room> Rooms { get { return GetStandardRepo<Room>(); } }
        public Contracts.IRepository<TimeSlot> TimeSlots { get { return GetStandardRepo<TimeSlot>(); } }
        public Contracts.IRepository<Track> Tracks { get { return GetStandardRepo<Track>(); } }
        public ISessionsRepository Sessions { get { return GetRepo<ISessionsRepository>(); } }
        public IPersonsRepository Persons { get { return GetRepo<IPersonsRepository>(); } }
        public IAttendanceRepository Attendance { get { return GetRepo<IAttendanceRepository>(); } }



        // ezFixUp Repositories

        public Contracts.IRepository<AbuseReport> AbuseReports { get { return GetStandardRepo<AbuseReport>(); } }
        public Contracts.IRepository<AdComment> AdComments { get { return GetStandardRepo<AdComment>(); } }
        public Contracts.IRepository<Admin> Admins { get { return GetStandardRepo<Admin>(); } }
        public Contracts.IRepository<AdPhoto> AdPhotos { get { return GetStandardRepo<AdPhoto>(); } }
        public Contracts.IRepository<Ad> Ads { get { return GetStandardRepo<Ad>(); } }
        public Contracts.IRepository<AdsCategory> AdsCategories { get { return GetStandardRepo<AdsCategory>(); } }
        public Contracts.IRepository<AffiliateBanner> AffiliateBanners { get { return GetStandardRepo<AffiliateBanner>(); } }
        public Contracts.IRepository<AffiliateCommission> AffiliateCommissions { get { return GetStandardRepo<AffiliateCommission>(); } }
        public Contracts.IRepository<Affiliate> Affiliates { get { return GetStandardRepo<Affiliate>(); } }
        public Contracts.IRepository<AffiliatesHistory> AffiliatesHistories { get { return GetStandardRepo<AffiliatesHistory>(); } }
        public Contracts.IRepository<archive_Messages> archive_Messages { get { return GetStandardRepo<archive_Messages>(); } }
        public Contracts.IRepository<archive_Photos> archive_Photos { get { return GetStandardRepo<archive_Photos>(); } }
        public Contracts.IRepository<archive_ProfileAnswers> archive_ProfileAnswers { get { return GetStandardRepo<archive_ProfileAnswers>(); } }
        public Contracts.IRepository<AskAboutQuestion> AskAboutQuestions { get { return GetStandardRepo<AskAboutQuestion>(); } }
        public Contracts.IRepository<AudioAccess> AudioAccesses { get { return GetStandardRepo<AudioAccess>(); } }
        public Contracts.IRepository<AudioUpload> AudioUploads { get { return GetStandardRepo<AudioUpload>(); } }
        public Contracts.IRepository<BannerCode> BannerCodes { get { return GetStandardRepo<BannerCode>(); } }
        public Contracts.IRepository<BillingPlan> BillingPlans { get { return GetStandardRepo<BillingPlan>(); } }
        public Contracts.IRepository<BlockedUser> BlockedUsers { get { return GetStandardRepo<BlockedUser>(); } }
        public Contracts.IRepository<BlogPostComment> BlogPostComments { get { return GetStandardRepo<BlogPostComment>(); } }
        public Contracts.IRepository<BlogPost> BlogPosts { get { return GetStandardRepo<BlogPost>(); } }
        public Contracts.IRepository<Blog> Blogs { get { return GetStandardRepo<Blog>(); } }
        public Contracts.IRepository<Category> Categories { get { return GetStandardRepo<Category>(); } }
        public Contracts.IRepository<ChatAdmin> ChatAdmins { get { return GetStandardRepo<ChatAdmin>(); } }
        public Contracts.IRepository<ChatBan> ChatBans { get { return GetStandardRepo<ChatBan>(); } }
        public Contracts.IRepository<ChatIgnoredUser> ChatIgnoredUsers { get { return GetStandardRepo<ChatIgnoredUser>(); } }
        public Contracts.IRepository<ChatMessage> ChatMessages { get { return GetStandardRepo<ChatMessage>(); } }
        public Contracts.IRepository<ChatPresence> ChatPresences { get { return GetStandardRepo<ChatPresence>(); } }
        public Contracts.IRepository<ChatRoomActivity> ChatRoomActivities { get { return GetStandardRepo<ChatRoomActivity>(); } }
        public Contracts.IRepository<ChatRoomAdmin> ChatRoomAdmins { get { return GetStandardRepo<ChatRoomAdmin>(); } }
        public Contracts.IRepository<ChatRoom> ChatRooms { get { return GetStandardRepo<ChatRoom>(); } }
        public Contracts.IRepository<ChatSetting> ChatSettings { get { return GetStandardRepo<ChatSetting>(); } }
        public Contracts.IRepository<ChatTranslation> ChatTranslations { get { return GetStandardRepo<ChatTranslation>(); } }
        public Contracts.IRepository<ChatUser> ChatUsers { get { return GetStandardRepo<ChatUser>(); } }
        public Contracts.IRepository<Comment> Comments { get { return GetStandardRepo<Comment>(); } }
        public Contracts.IRepository<CommunityPhotoApproval> CommunityPhotoApprovals { get { return GetStandardRepo<CommunityPhotoApproval>(); } }
        public Contracts.IRepository<CommunityProfileApproval> CommunityProfileApprovals { get { return GetStandardRepo<CommunityProfileApproval>(); } }
        public Contracts.IRepository<ContactU> ContactUs { get { return GetStandardRepo<ContactU>(); } }
        public Contracts.IRepository<ContentPage> ContentPages { get { return GetStandardRepo<ContentPage>(); } }
        public Contracts.IRepository<ContentView> ContentViews { get { return GetStandardRepo<ContentView>(); } }
        public Contracts.IRepository<Coupon> Coupons { get { return GetStandardRepo<Coupon>(); } }
        public Contracts.IRepository<CouponType> CouponTypes { get { return GetStandardRepo<CouponType>(); } }
        public Contracts.IRepository<CouponUs> CouponUses { get { return GetStandardRepo<CouponUs>(); } }
        public Contracts.IRepository<CreditsHistory> CreditsHistories { get { return GetStandardRepo<CreditsHistory>(); } }
        public Contracts.IRepository<CreditsPackage> CreditsPackages { get { return GetStandardRepo<CreditsPackage>(); } }
        public Contracts.IRepository<Discount> Discounts { get { return GetStandardRepo<Discount>(); } }
        public Contracts.IRepository<ECardCategory> ECardCategories { get { return GetStandardRepo<ECardCategory>(); } }
        public Contracts.IRepository<Ecard> Ecards { get { return GetStandardRepo<Ecard>(); } }
        public Contracts.IRepository<EcardType> EcardTypes { get { return GetStandardRepo<EcardType>(); } }
        public Contracts.IRepository<EmailQueue> EmailQueues { get { return GetStandardRepo<EmailQueue>(); } }
        public Contracts.IRepository<EmailTemplate> EmailTemplates { get { return GetStandardRepo<EmailTemplate>(); } }
        public Contracts.IRepository<EstablishedCommunication> EstablishedCommunications { get { return GetStandardRepo<EstablishedCommunication>(); } }
        public Contracts.IRepository<EventComment> EventComments { get { return GetStandardRepo<EventComment>(); } }
        public Contracts.IRepository<Event> Events { get { return GetStandardRepo<Event>(); } }
        public Contracts.IRepository<FacebookCity> FacebookCities { get { return GetStandardRepo<FacebookCity>(); } }
        public Contracts.IRepository<FacebookCountry> FacebookCountries { get { return GetStandardRepo<FacebookCountry>(); } }
        public Contracts.IRepository<FacebookInvitation> FacebookInvitations { get { return GetStandardRepo<FacebookInvitation>(); } }
        public Contracts.IRepository<FaqCategory> FaqCategories { get { return GetStandardRepo<FaqCategory>(); } }
        public Contracts.IRepository<FAQ> FAQs { get { return GetStandardRepo<FAQ>(); } }
        public Contracts.IRepository<Favourite> Favourites { get { return GetStandardRepo<Favourite>(); } }
        public Contracts.IRepository<Friend> Friends { get { return GetStandardRepo<Friend>(); } }
        public Contracts.IRepository<GiftCategory> GiftCategories { get { return GetStandardRepo<GiftCategory>(); } }
        public Contracts.IRepository<GiftsMessage> GiftsMessages { get { return GetStandardRepo<GiftsMessage>(); } }
        public Contracts.IRepository<GiftType> GiftTypes { get { return GetStandardRepo<GiftType>(); } }
        public Contracts.IRepository<GroupBan> GroupBans { get { return GetStandardRepo<GroupBan>(); } }
        public Contracts.IRepository<GroupEvent> GroupEvents { get { return GetStandardRepo<GroupEvent>(); } }
        public Contracts.IRepository<GroupEventsComment> GroupEventsComments { get { return GetStandardRepo<GroupEventsComment>(); } }
        public Contracts.IRepository<GroupMember> GroupMembers { get { return GetStandardRepo<GroupMember>(); } }
        public Contracts.IRepository<GroupPhoto> GroupPhotos { get { return GetStandardRepo<GroupPhoto>(); } }
        public Contracts.IRepository<GroupPollsAnswer> GroupPollsAnswers { get { return GetStandardRepo<GroupPollsAnswer>(); } }
        public Contracts.IRepository<GroupPollsChoice> GroupPollsChoices { get { return GetStandardRepo<GroupPollsChoice>(); } }
        public Contracts.IRepository<GroupPost> GroupPosts { get { return GetStandardRepo<GroupPost>(); } }
        public Contracts.IRepository<Group> Groups { get { return GetStandardRepo<Group>(); } }
        public Contracts.IRepository<GroupTopic> GroupTopics { get { return GetStandardRepo<GroupTopic>(); } }
        public Contracts.IRepository<GroupTopicSubscription> GroupTopicSubscriptions { get { return GetStandardRepo<GroupTopicSubscription>(); } }
        public Contracts.IRepository<HourlyStat> HourlyStats { get { return GetStandardRepo<HourlyStat>(); } }
        public Contracts.IRepository<Interest> Interests { get { return GetStandardRepo<Interest>(); } }
        public Contracts.IRepository<IPLog> IPLogs { get { return GetStandardRepo<IPLog>(); } }
        public Contracts.IRepository<Language> Languages { get { return GetStandardRepo<Language>(); } }
        public Contracts.IRepository<Matchmaking> Matchmakings { get { return GetStandardRepo<Matchmaking>(); } }
        public Contracts.IRepository<Message> Messages { get { return GetStandardRepo<Message>(); } }
        public Contracts.IRepository<MessagesHistoryCache> MessagesHistoryCaches { get { return GetStandardRepo<MessagesHistoryCache>(); } }
        public Contracts.IRepository<MessagesSandbox> MessagesSandboxes { get { return GetStandardRepo<MessagesSandbox>(); } }
        public Contracts.IRepository<MessagesType> MessagesTypes { get { return GetStandardRepo<MessagesType>(); } }
        public Contracts.IRepository<News> News { get { return GetStandardRepo<News>(); } }
        public Contracts.IRepository<PaymentHistory> PaymentHistories { get { return GetStandardRepo<PaymentHistory>(); } }
        public Contracts.IRepository<PendingGuid> PendingGuids { get { return GetStandardRepo<PendingGuid>(); } }
        public Contracts.IRepository<PhotoAccess> PhotoAccesses { get { return GetStandardRepo<PhotoAccess>(); } }
        public Contracts.IRepository<PhotoAlbum> PhotoAlbums { get { return GetStandardRepo<PhotoAlbum>(); } }
        public Contracts.IRepository<PhotoComment> PhotoComments { get { return GetStandardRepo<PhotoComment>(); } }
        public Contracts.IRepository<PhotoContestEntry> PhotoContestEntries { get { return GetStandardRepo<PhotoContestEntry>(); } }
        public Contracts.IRepository<PhotoContestRank> PhotoContestRanks { get { return GetStandardRepo<PhotoContestRank>(); } }
        public Contracts.IRepository<PhotoContest> PhotoContests { get { return GetStandardRepo<PhotoContest>(); } }
        public Contracts.IRepository<PhotoContestVote> PhotoContestVotes { get { return GetStandardRepo<PhotoContestVote>(); } }
        public Contracts.IRepository<PhotoNote> PhotoNotes { get { return GetStandardRepo<PhotoNote>(); } }
        public Contracts.IRepository<PhotoRating> PhotoRatings { get { return GetStandardRepo<PhotoRating>(); } }
        public Contracts.IRepository<Photo> Photos { get { return GetStandardRepo<Photo>(); } }
        public Contracts.IRepository<PollAnswer> PollAnswers { get { return GetStandardRepo<PollAnswer>(); } }
        public Contracts.IRepository<PollChoice> PollChoices { get { return GetStandardRepo<PollChoice>(); } }
        public Contracts.IRepository<Poll> Polls { get { return GetStandardRepo<Poll>(); } }
        public Contracts.IRepository<ProfileAnswer> ProfileAnswers { get { return GetStandardRepo<ProfileAnswer>(); } }
        public Contracts.IRepository<ProfileChoice> ProfileChoices { get { return GetStandardRepo<ProfileChoice>(); } }
        public Contracts.IRepository<ProfileQuestion> ProfileQuestions { get { return GetStandardRepo<ProfileQuestion>(); } }
        public Contracts.IRepository<ProfileTopic> ProfileTopics { get { return GetStandardRepo<ProfileTopic>(); } }
        public Contracts.IRepository<ProfileView> ProfileViews { get { return GetStandardRepo<ProfileView>(); } }
        public Contracts.IRepository<Rating> Ratings { get { return GetStandardRepo<Rating>(); } }
        public Contracts.IRepository<Relationship> Relationships { get { return GetStandardRepo<Relationship>(); } }
        public Contracts.IRepository<SavedSearch> SavedSearches { get { return GetStandardRepo<SavedSearch>(); } }
        public Contracts.IRepository<ScheduledAnnouncement> ScheduledAnnouncements { get { return GetStandardRepo<ScheduledAnnouncement>(); } }
        public Contracts.IRepository<Setting> Settings { get { return GetStandardRepo<Setting>(); } }
        public Contracts.IRepository<Subscription> Subscriptions { get { return GetStandardRepo<Subscription>(); } }
        public Contracts.IRepository<Template> Templates { get { return GetStandardRepo<Template>(); } }
        public Contracts.IRepository<TipsPopup> TipsPopups { get { return GetStandardRepo<TipsPopup>(); } }
        public Contracts.IRepository<Translation> Translations { get { return GetStandardRepo<Translation>(); } }
        public Contracts.IRepository<TwitterCredential> TwitterCredentials { get { return GetStandardRepo<TwitterCredential>(); } }
        public Contracts.IRepository<UnlockedSection> UnlockedSections { get { return GetStandardRepo<UnlockedSection>(); } }
        public Contracts.IRepository<UserLevel> UserLevels { get { return GetStandardRepo<UserLevel>(); } }
        public Contracts.IRepository<User> Users { get { return GetStandardRepo<User>(); } }
        public Contracts.IRepository<UserScoresLog> UserScoresLogs { get { return GetStandardRepo<UserScoresLog>(); } }
        public Contracts.IRepository<UserTipsStatu> UserTipsStatus { get { return GetStandardRepo<UserTipsStatu>(); } }
        public Contracts.IRepository<VerifiedUser> VerifiedUsers { get { return GetStandardRepo<VerifiedUser>(); } }
        public Contracts.IRepository<VideoAccess> VideoAccesses { get { return GetStandardRepo<VideoAccess>(); } }
        public Contracts.IRepository<VideoEmbed> VideoEmbeds { get { return GetStandardRepo<VideoEmbed>(); } }
        public Contracts.IRepository<VideoProfile> VideoProfiles { get { return GetStandardRepo<VideoProfile>(); } }
        public Contracts.IRepository<VideoUpload> VideoUploads { get { return GetStandardRepo<VideoUpload>(); } }
        public Contracts.IRepository<Vote> Votes { get { return GetStandardRepo<Vote>(); } }
        public Contracts.IRepository<WinkCategory> WinkCategories { get { return GetStandardRepo<WinkCategory>(); } }
        public Contracts.IRepository<WinkMessaage> WinkMessaages { get { return GetStandardRepo<WinkMessaage>(); } }
        public Contracts.IRepository<WinkType> WinkTypes { get { return GetStandardRepo<WinkType>(); } }
        public Contracts.IRepository<View_FemaleInterestedInFemaleSingles> View_FemaleInterestedInFemaleSingles { get { return GetStandardRepo<View_FemaleInterestedInFemaleSingles>(); } }
        public Contracts.IRepository<View_FemaleInterestedInMaleSingles> View_FemaleInterestedInMaleSingles { get { return GetStandardRepo<View_FemaleInterestedInMaleSingles>(); } }
        public Contracts.IRepository<View_MaleInterestedInFemaleSingles> View_MaleInterestedInFemaleSingles { get { return GetStandardRepo<View_MaleInterestedInFemaleSingles>(); } }
        public Contracts.IRepository<View_MaleInterestedInMaleSingles> View_MaleInterestedInMaleSingles { get { return GetStandardRepo<View_MaleInterestedInMaleSingles>(); } }




        /// <summary>
        /// Save pending changes to the database
        /// </summary>
        public void Commit()
        {
            //System.Diagnostics.Debug.WriteLine("Committed");
            DbContext.SaveChanges();
        }

        protected void CreateDbContext()
        {
            DbContext = new ezFixUpDbContext();

            // Do NOT enable proxied entities, else serialization fails
            DbContext.Configuration.ProxyCreationEnabled = false;

            // Load navigation properties explicitly (avoid serialization trouble)
            DbContext.Configuration.LazyLoadingEnabled = false;

            // Because Web API will perform validation, we don't need/want EF to do so
            DbContext.Configuration.ValidateOnSaveEnabled = false;

            //DbContext.Configuration.AutoDetectChangesEnabled = false;
            // We won't use this performance tweak because we don't need 
            // the extra performance and, when autodetect is false,
            // we'd have to be careful. We're not being that careful.
        }

        protected IRepositoryProvider RepositoryProvider { get; set; }

        private Contracts.IRepository<T> GetStandardRepo<T>() where T : class
        {
            return RepositoryProvider.GetRepositoryForEntityType<T>();
        }
        private T GetRepo<T>() where T : class
        {
            return RepositoryProvider.GetRepository<T>();
        }

        private ezFixUpDbContext DbContext { get; set; }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (DbContext != null)
                {
                    DbContext.Dispose();
                }
            }
        }

        #endregion
    }
}
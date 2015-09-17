using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using ezFixUp.Model.Models.Mapping;

namespace ezFixUp.Model.Models
{
    public class ezFixUpContext : DbContext
    {
        static ezFixUpContext()
        {
            Database.SetInitializer<ezFixUpContext>(null);
        }

		public ezFixUpContext()
			: base("Name=ezFixUpContext")
		{
		}

        public DbSet<AbuseReport> AbuseReports { get; set; }
        public DbSet<AdComment> AdComments { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<AdPhoto> AdPhotos { get; set; }
        public DbSet<Ad> Ads { get; set; }
        public DbSet<AdsCategory> AdsCategories { get; set; }
        public DbSet<AffiliateBanner> AffiliateBanners { get; set; }
        public DbSet<AffiliateCommission> AffiliateCommissions { get; set; }
        public DbSet<Affiliate> Affiliates { get; set; }
        public DbSet<AffiliatesHistory> AffiliatesHistories { get; set; }
        public DbSet<archive_Messages> archive_Messages { get; set; }
        public DbSet<archive_Photos> archive_Photos { get; set; }
        public DbSet<archive_ProfileAnswers> archive_ProfileAnswers { get; set; }
        public DbSet<AskAboutQuestion> AskAboutQuestions { get; set; }
        public DbSet<AudioAccess> AudioAccesses { get; set; }
        public DbSet<AudioUpload> AudioUploads { get; set; }
        public DbSet<BannerCode> BannerCodes { get; set; }
        public DbSet<BillingPlan> BillingPlans { get; set; }
        public DbSet<BlockedUser> BlockedUsers { get; set; }
        public DbSet<BlogPostComment> BlogPostComments { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ChatAdmin> ChatAdmins { get; set; }
        public DbSet<ChatBan> ChatBans { get; set; }
        public DbSet<ChatIgnoredUser> ChatIgnoredUsers { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<ChatPresence> ChatPresences { get; set; }
        public DbSet<ChatRoomActivity> ChatRoomActivities { get; set; }
        public DbSet<ChatRoomAdmin> ChatRoomAdmins { get; set; }
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<ChatSetting> ChatSettings { get; set; }
        public DbSet<ChatTranslation> ChatTranslations { get; set; }
        public DbSet<ChatUser> ChatUsers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommunityPhotoApproval> CommunityPhotoApprovals { get; set; }
        public DbSet<CommunityProfileApproval> CommunityProfileApprovals { get; set; }
        public DbSet<ContactU> ContactUs { get; set; }
        public DbSet<ContentPage> ContentPages { get; set; }
        public DbSet<ContentView> ContentViews { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<CouponType> CouponTypes { get; set; }
        public DbSet<CouponUs> CouponUses { get; set; }
        public DbSet<CreditsHistory> CreditsHistories { get; set; }
        public DbSet<CreditsPackage> CreditsPackages { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<ECardCategory> ECardCategories { get; set; }
        public DbSet<Ecard> Ecards { get; set; }
        public DbSet<EcardType> EcardTypes { get; set; }
        public DbSet<EmailQueue> EmailQueues { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<EstablishedCommunication> EstablishedCommunications { get; set; }
        public DbSet<EventComment> EventComments { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<FacebookCity> FacebookCities { get; set; }
        public DbSet<FacebookCountry> FacebookCountries { get; set; }
        public DbSet<FacebookInvitation> FacebookInvitations { get; set; }
        public DbSet<FaqCategory> FaqCategories { get; set; }
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<Favourite> Favourites { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<GiftCategory> GiftCategories { get; set; }
        public DbSet<GiftsMessage> GiftsMessages { get; set; }
        public DbSet<GiftType> GiftTypes { get; set; }
        public DbSet<GroupBan> GroupBans { get; set; }
        public DbSet<GroupEvent> GroupEvents { get; set; }
        public DbSet<GroupEventsComment> GroupEventsComments { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<GroupPhoto> GroupPhotos { get; set; }
        public DbSet<GroupPollsAnswer> GroupPollsAnswers { get; set; }
        public DbSet<GroupPollsChoice> GroupPollsChoices { get; set; }
        public DbSet<GroupPost> GroupPosts { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupTopic> GroupTopics { get; set; }
        public DbSet<GroupTopicSubscription> GroupTopicSubscriptions { get; set; }
        public DbSet<HourlyStat> HourlyStats { get; set; }
        public DbSet<Interest> Interests { get; set; }
        public DbSet<IPLog> IPLogs { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Matchmaking> Matchmakings { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MessagesHistoryCache> MessagesHistoryCaches { get; set; }
        public DbSet<MessagesSandbox> MessagesSandboxes { get; set; }
        public DbSet<MessagesType> MessagesTypes { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<PaymentHistory> PaymentHistories { get; set; }
        public DbSet<PendingGuid> PendingGuids { get; set; }
        public DbSet<PhotoAccess> PhotoAccesses { get; set; }
        public DbSet<PhotoAlbum> PhotoAlbums { get; set; }
        public DbSet<PhotoComment> PhotoComments { get; set; }
        public DbSet<PhotoContestEntry> PhotoContestEntries { get; set; }
        public DbSet<PhotoContestRank> PhotoContestRanks { get; set; }
        public DbSet<PhotoContest> PhotoContests { get; set; }
        public DbSet<PhotoContestVote> PhotoContestVotes { get; set; }
        public DbSet<PhotoNote> PhotoNotes { get; set; }
        public DbSet<PhotoRating> PhotoRatings { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<PollAnswer> PollAnswers { get; set; }
        public DbSet<PollChoice> PollChoices { get; set; }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<ProfileAnswer> ProfileAnswers { get; set; }
        public DbSet<ProfileChoice> ProfileChoices { get; set; }
        public DbSet<ProfileQuestion> ProfileQuestions { get; set; }
        public DbSet<ProfileTopic> ProfileTopics { get; set; }
        public DbSet<ProfileView> ProfileViews { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Relationship> Relationships { get; set; }
        public DbSet<SavedSearch> SavedSearches { get; set; }
        public DbSet<ScheduledAnnouncement> ScheduledAnnouncements { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<TipsPopup> TipsPopups { get; set; }
        public DbSet<Translation> Translations { get; set; }
        public DbSet<TwitterCredential> TwitterCredentials { get; set; }
        public DbSet<UnlockedSection> UnlockedSections { get; set; }
        public DbSet<UserLevel> UserLevels { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserScoresLog> UserScoresLogs { get; set; }
        public DbSet<UserTipsStatu> UserTipsStatus { get; set; }
        public DbSet<VerifiedUser> VerifiedUsers { get; set; }
        public DbSet<VideoAccess> VideoAccesses { get; set; }
        public DbSet<VideoEmbed> VideoEmbeds { get; set; }
        public DbSet<VideoProfile> VideoProfiles { get; set; }
        public DbSet<VideoUpload> VideoUploads { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<WinkCategory> WinkCategories { get; set; }
        public DbSet<WinkMessaage> WinkMessaages { get; set; }
        public DbSet<WinkType> WinkTypes { get; set; }
        public DbSet<View_FemaleInterestedInFemaleSingles> View_FemaleInterestedInFemaleSingles { get; set; }
        public DbSet<View_FemaleInterestedInMaleSingles> View_FemaleInterestedInMaleSingles { get; set; }
        public DbSet<View_MaleInterestedInFemaleSingles> View_MaleInterestedInFemaleSingles { get; set; }
        public DbSet<View_MaleInterestedInMaleSingles> View_MaleInterestedInMaleSingles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AbuseReportMap());
            modelBuilder.Configurations.Add(new AdCommentMap());
            modelBuilder.Configurations.Add(new AdminMap());
            modelBuilder.Configurations.Add(new AdPhotoMap());
            modelBuilder.Configurations.Add(new AdMap());
            modelBuilder.Configurations.Add(new AdsCategoryMap());
            modelBuilder.Configurations.Add(new AffiliateBannerMap());
            modelBuilder.Configurations.Add(new AffiliateCommissionMap());
            modelBuilder.Configurations.Add(new AffiliateMap());
            modelBuilder.Configurations.Add(new AffiliatesHistoryMap());
            modelBuilder.Configurations.Add(new archive_MessagesMap());
            modelBuilder.Configurations.Add(new archive_PhotosMap());
            modelBuilder.Configurations.Add(new archive_ProfileAnswersMap());
            modelBuilder.Configurations.Add(new AskAboutQuestionMap());
            modelBuilder.Configurations.Add(new AudioAccessMap());
            modelBuilder.Configurations.Add(new AudioUploadMap());
            modelBuilder.Configurations.Add(new BannerCodeMap());
            modelBuilder.Configurations.Add(new BillingPlanMap());
            modelBuilder.Configurations.Add(new BlockedUserMap());
            modelBuilder.Configurations.Add(new BlogPostCommentMap());
            modelBuilder.Configurations.Add(new BlogPostMap());
            modelBuilder.Configurations.Add(new BlogMap());
            modelBuilder.Configurations.Add(new CategoryMap());
            modelBuilder.Configurations.Add(new ChatAdminMap());
            modelBuilder.Configurations.Add(new ChatBanMap());
            modelBuilder.Configurations.Add(new ChatIgnoredUserMap());
            modelBuilder.Configurations.Add(new ChatMessageMap());
            modelBuilder.Configurations.Add(new ChatPresenceMap());
            modelBuilder.Configurations.Add(new ChatRoomActivityMap());
            modelBuilder.Configurations.Add(new ChatRoomAdminMap());
            modelBuilder.Configurations.Add(new ChatRoomMap());
            modelBuilder.Configurations.Add(new ChatSettingMap());
            modelBuilder.Configurations.Add(new ChatTranslationMap());
            modelBuilder.Configurations.Add(new ChatUserMap());
            modelBuilder.Configurations.Add(new CommentMap());
            modelBuilder.Configurations.Add(new CommunityPhotoApprovalMap());
            modelBuilder.Configurations.Add(new CommunityProfileApprovalMap());
            modelBuilder.Configurations.Add(new ContactUMap());
            modelBuilder.Configurations.Add(new ContentPageMap());
            modelBuilder.Configurations.Add(new ContentViewMap());
            modelBuilder.Configurations.Add(new CouponMap());
            modelBuilder.Configurations.Add(new CouponTypeMap());
            modelBuilder.Configurations.Add(new CouponUsMap());
            modelBuilder.Configurations.Add(new CreditsHistoryMap());
            modelBuilder.Configurations.Add(new CreditsPackageMap());
            modelBuilder.Configurations.Add(new DiscountMap());
            modelBuilder.Configurations.Add(new ECardCategoryMap());
            modelBuilder.Configurations.Add(new EcardMap());
            modelBuilder.Configurations.Add(new EcardTypeMap());
            modelBuilder.Configurations.Add(new EmailQueueMap());
            modelBuilder.Configurations.Add(new EmailTemplateMap());
            modelBuilder.Configurations.Add(new EstablishedCommunicationMap());
            modelBuilder.Configurations.Add(new EventCommentMap());
            modelBuilder.Configurations.Add(new EventMap());
            modelBuilder.Configurations.Add(new FacebookCityMap());
            modelBuilder.Configurations.Add(new FacebookCountryMap());
            modelBuilder.Configurations.Add(new FacebookInvitationMap());
            modelBuilder.Configurations.Add(new FaqCategoryMap());
            modelBuilder.Configurations.Add(new FAQMap());
            modelBuilder.Configurations.Add(new FavouriteMap());
            modelBuilder.Configurations.Add(new FriendMap());
            modelBuilder.Configurations.Add(new GiftCategoryMap());
            modelBuilder.Configurations.Add(new GiftsMessageMap());
            modelBuilder.Configurations.Add(new GiftTypeMap());
            modelBuilder.Configurations.Add(new GroupBanMap());
            modelBuilder.Configurations.Add(new GroupEventMap());
            modelBuilder.Configurations.Add(new GroupEventsCommentMap());
            modelBuilder.Configurations.Add(new GroupMemberMap());
            modelBuilder.Configurations.Add(new GroupPhotoMap());
            modelBuilder.Configurations.Add(new GroupPollsAnswerMap());
            modelBuilder.Configurations.Add(new GroupPollsChoiceMap());
            modelBuilder.Configurations.Add(new GroupPostMap());
            modelBuilder.Configurations.Add(new GroupMap());
            modelBuilder.Configurations.Add(new GroupTopicMap());
            modelBuilder.Configurations.Add(new GroupTopicSubscriptionMap());
            modelBuilder.Configurations.Add(new HourlyStatMap());
            modelBuilder.Configurations.Add(new InterestMap());
            modelBuilder.Configurations.Add(new IPLogMap());
            modelBuilder.Configurations.Add(new LanguageMap());
            modelBuilder.Configurations.Add(new MatchmakingMap());
            modelBuilder.Configurations.Add(new MessageMap());
            modelBuilder.Configurations.Add(new MessagesHistoryCacheMap());
            modelBuilder.Configurations.Add(new MessagesSandboxMap());
            modelBuilder.Configurations.Add(new MessagesTypeMap());
            modelBuilder.Configurations.Add(new NewsMap());
            modelBuilder.Configurations.Add(new PaymentHistoryMap());
            modelBuilder.Configurations.Add(new PendingGuidMap());
            modelBuilder.Configurations.Add(new PhotoAccessMap());
            modelBuilder.Configurations.Add(new PhotoAlbumMap());
            modelBuilder.Configurations.Add(new PhotoCommentMap());
            modelBuilder.Configurations.Add(new PhotoContestEntryMap());
            modelBuilder.Configurations.Add(new PhotoContestRankMap());
            modelBuilder.Configurations.Add(new PhotoContestMap());
            modelBuilder.Configurations.Add(new PhotoContestVoteMap());
            modelBuilder.Configurations.Add(new PhotoNoteMap());
            modelBuilder.Configurations.Add(new PhotoRatingMap());
            modelBuilder.Configurations.Add(new PhotoMap());
            modelBuilder.Configurations.Add(new PollAnswerMap());
            modelBuilder.Configurations.Add(new PollChoiceMap());
            modelBuilder.Configurations.Add(new PollMap());
            modelBuilder.Configurations.Add(new ProfileAnswerMap());
            modelBuilder.Configurations.Add(new ProfileChoiceMap());
            modelBuilder.Configurations.Add(new ProfileQuestionMap());
            modelBuilder.Configurations.Add(new ProfileTopicMap());
            modelBuilder.Configurations.Add(new ProfileViewMap());
            modelBuilder.Configurations.Add(new RatingMap());
            modelBuilder.Configurations.Add(new RelationshipMap());
            modelBuilder.Configurations.Add(new SavedSearchMap());
            modelBuilder.Configurations.Add(new ScheduledAnnouncementMap());
            modelBuilder.Configurations.Add(new SettingMap());
            modelBuilder.Configurations.Add(new SubscriptionMap());
            modelBuilder.Configurations.Add(new TemplateMap());
            modelBuilder.Configurations.Add(new TipsPopupMap());
            modelBuilder.Configurations.Add(new TranslationMap());
            modelBuilder.Configurations.Add(new TwitterCredentialMap());
            modelBuilder.Configurations.Add(new UnlockedSectionMap());
            modelBuilder.Configurations.Add(new UserLevelMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new UserScoresLogMap());
            modelBuilder.Configurations.Add(new UserTipsStatuMap());
            modelBuilder.Configurations.Add(new VerifiedUserMap());
            modelBuilder.Configurations.Add(new VideoAccessMap());
            modelBuilder.Configurations.Add(new VideoEmbedMap());
            modelBuilder.Configurations.Add(new VideoProfileMap());
            modelBuilder.Configurations.Add(new VideoUploadMap());
            modelBuilder.Configurations.Add(new VoteMap());
            modelBuilder.Configurations.Add(new WinkCategoryMap());
            modelBuilder.Configurations.Add(new WinkMessaageMap());
            modelBuilder.Configurations.Add(new WinkTypeMap());
            modelBuilder.Configurations.Add(new View_FemaleInterestedInFemaleSinglesMap());
            modelBuilder.Configurations.Add(new View_FemaleInterestedInMaleSinglesMap());
            modelBuilder.Configurations.Add(new View_MaleInterestedInFemaleSinglesMap());
            modelBuilder.Configurations.Add(new View_MaleInterestedInMaleSinglesMap());
        }
    }
}

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
using System.Diagnostics;
using System.Xml.Serialization;
using Microsoft.ApplicationBlocks.Data;

namespace AspNetDating.Classes
{
    /// <summary>
    /// Specifies cycle units.
    /// </summary>
    public enum CycleUnits
    {
        /// <summary>
        /// No cycle unit is selected.
        /// </summary>
        NoneSelected = 0,

        /// <summary>
        /// Represents days.
        /// </summary>
        Days = 1,

        /// <summary>
        /// Represents weeks.
        /// </summary>
        Weeks = 2,

        /// <summary>
        /// Represents months.
        /// </summary>
        Months = 3,

        /// <summary>
        /// Represents years.
        /// </summary>
        Years = 4
    }

    /// <summary>
    /// Represents billing plan. Provides methods to create, retrieve, update and delete.
    /// </summary>
    public class BillingPlan
    {
        #region Properties

        private int id;

        /// <summary>
        /// Gets the ID.
        /// The property is read-only.
        /// </summary>
        /// <value>The ID.</value>
        public int ID
        {
            get
            {
                Debug.Assert(id > 0, "plan ID should be set first");
                return id;
            }
        }

        private string title;

        /// <summary>
        /// Gets or sets the title of the billing plan.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get
            {
                Debug.Assert(title != "", "title should be set first");
                return title;
            }
            set { title = value; }
        }

        private float amount;

        /// <summary>
        /// Gets or sets the amount of the billing plan..
        /// </summary>
        /// <value>The amount.</value>
        public float Amount
        {
            get
            {
                Debug.Assert(amount > 0.0f, "amount should be set first");
                return amount;
            }
            set { amount = value; }
        }

        private int cycle;

        /// <summary>
        /// Gets or sets the cycle of the billing plan.
        /// </summary>
        /// <value>The cycle.</value>
        public int Cycle
        {
            get
            {
                Debug.Assert(cycle > 0, "cycle should be set first");
                return cycle;
            }
            set { cycle = value; }
        }

        private CycleUnits cycleUnit;

        /// <summary>
        /// Gets or sets the cycle unit of the billing plan.
        /// </summary>
        /// <value>The cycle unit.</value>
        public CycleUnits CycleUnit
        {
            get
            {
                Debug.Assert(cycleUnit != CycleUnits.NoneSelected, "cycle unit should be set first");
                return cycleUnit;
            }
            set { cycleUnit = value; }
        }

        private BillingPlanOptions options;

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>The options.</value>
        public BillingPlanOptions Options
        {
            get { return options; }
            set { options = value; }
        }
        #endregion

        /// <summary>
        /// Initializes the <see cref="BillingPlan"/> class.
        /// </summary>
        static BillingPlan()
        {
        }

        /// <summary>
        /// Fetches all not deleted billing plans from DB
        /// </summary>
        /// <returns>array of all billing plans</returns>
        public static BillingPlan[] Fetch()
        {
            List<BillingPlan> lBillingPlans = new List<BillingPlan>();

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn,
                                            "FetchBillingPlan", null);

                while (reader.Read())
                {
                    BillingPlan plan = new BillingPlan();

                    plan.id = (int)reader["PlanID"];
                    plan.title = (string)reader["Title"];
                    plan.amount = Convert.ToSingle(reader["Amount"]);
                    plan.cycle = (int)reader["Cycle"];
                    plan.cycleUnit = (CycleUnits)Convert.ToInt32(reader["CycleUnit"]);
                    plan.options = reader["Options"] as string != null ?
                        Misc.FromXml<BillingPlanOptions>((string)reader["Options"]) : new BillingPlanOptions();


                    lBillingPlans.Add(plan);
                }
            }

            return lBillingPlans.ToArray();
        }

        /// <summary>
        /// Fetches billing plan by giving plan id even if it's marked deleted
        /// </summary>
        /// <param name="id">plan's id</param>
        /// <returns>requested billing plan</returns>
        public static BillingPlan Fetch(int id)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn,
                                            "FetchBillingPlan", id);

                if (reader.Read())
                {
                    BillingPlan plan = new BillingPlan();

                    plan.id = (int)reader["PlanID"];
                    plan.title = (string)reader["Title"];
                    plan.amount = Convert.ToSingle(reader["Amount"]);
                    plan.cycle = (int)reader["Cycle"];
                    plan.cycleUnit = (CycleUnits)Convert.ToInt32(reader["CycleUnit"]);
                    plan.options = reader["Options"] as string != null ?
                        Misc.FromXml<BillingPlanOptions>((string)reader["Options"]) : new BillingPlanOptions();

                    return plan;
                }
                else return null;
            }
        }

        /// <summary>
        /// Fetches the billing plan by specified subscription ID.
        /// </summary>
        /// <param name="subscriptionID">The subscription ID.</param>
        /// <returns></returns>
        public static BillingPlan FetchBySubscriptionID(int subscriptionID)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn,
                                            "FetchBillingPlanBySubscriptionID", subscriptionID);

                if (reader.Read())
                {
                    BillingPlan plan = new BillingPlan();

                    plan.id = (int)reader["PlanID"];
                    plan.title = (string)reader["Title"];
                    plan.amount = Convert.ToSingle(reader["Amount"]);
                    plan.cycle = (int)reader["Cycle"];
                    plan.cycleUnit = (CycleUnits)Convert.ToInt32(reader["CycleUnit"]);
                    plan.options = reader["Options"] as string != null ?
                        Misc.FromXml<BillingPlanOptions>((string)reader["Options"]) : new BillingPlanOptions();

                    return plan;
                }
                else return null;
            }
        }

        /// <summary>
        /// Fetches the billing plan by his data.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="cycle">The cycle.</param>
        /// <param name="cycleUnits">The cycle units.</param>
        /// <returns></returns>
        public static BillingPlan FetchByPlanData(float amount, int cycle, int cycleUnits)
        {
            int planID;
            using (SqlConnection conn = Config.DB.Open())
            {
                planID = (int)SqlHelper.ExecuteScalar(conn,
                                                       "FetchBillingPlanIDByPlanData", amount, cycle, cycleUnits);
            }

            return Fetch(planID);
        }

        /// <summary>
        /// Deletes billing plan by its id.
        /// </summary>
        /// <param name="id">plan's id</param>
        public static void Delete(int id)
        {
            if (id < 1)
                throw new Exception("Invalid billing plan ID!");

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn,
                                          "DeleteBillingPlan", id);
            }
        }

        /// <summary>
        /// Updates current billing plan.
        /// </summary>
        public void Update()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn,
                                          "UpdateBillingPlan", ID, Title, Amount, Cycle, CycleUnit, Misc.ToXml(options));
            }
        }

        /// <summary>
        /// Inserts new billing plan in the database using given BillingPlan object.
        /// </summary>
        /// <param name="plan">billing plan to be inserted in DB.All properties must be set except plan ID</param>
        public static void Create(BillingPlan plan)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn,
                                          "CreateBillingPlan", plan.Title, plan.Amount, plan.Cycle, plan.CycleUnit, Misc.ToXml(plan.options));
            }
        }

        /// <summary>
        /// Applies the discounts.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="billingPlans">The billing plans.</param>
        public static void ApplyDiscounts(User user, BillingPlan[] billingPlans)
        {
            if (user != null && billingPlans != null)
            {
                Discount[] discounts = Discount.Fetch();

                foreach (Discount discount in discounts)
                {
                    if (discount.Field == Discount.ConditionField.State && user.State == discount.Match ||
                        discount.Field == Discount.ConditionField.Country && user.Country == discount.Match)
                    {
                        foreach (BillingPlan plan in billingPlans)
                        {
                            if (discount.Type == Discount.DiscountType.Percent)
                            {
                                plan.amount -= (plan.amount * (float)discount.Amount);
                            }
                            else if (discount.Type == Discount.DiscountType.Amount)
                            {
                                plan.amount -= (float)discount.Amount;
                            }
                        }

                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Applies the discounts.
        /// </summary>
        /// <param name="user">The user.</param>
        public void ApplyDiscounts(User user)
        {
            ApplyDiscounts(user, new BillingPlan[] { this });
        }
    }

    /// <summary>
    /// This class handles the discounts
    /// </summary>
    public class Discount
    {
        #region enums

        /// <summary>
        /// The type of the discount
        /// </summary>
        public enum DiscountType
        {
            /// <summary>
            /// A fixed amount
            /// </summary>
            Amount = 1,
            /// <summary>
            /// A percentage
            /// </summary>
            Percent
        }

        /// <summary>
        /// The condition to apply a discount
        /// </summary>
        public enum ConditionField
        {
            /// <summary>
            /// A specific state
            /// </summary>
            State = 1,
            /// <summary>
            /// A specific country
            /// </summary>
            Country
        }

        #endregion

        #region Properties

        private int id;
        /// <summary>
        /// Gets the ID.
        /// </summary>
        /// <value>The ID.</value>
        public int ID
        {
            get
            {
                return id;
            }
        }

        private decimal amount;
        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        public decimal Amount
        {
            get
            {
                return amount;
            }

            set
            {
                amount = value;
            }
        }

        private DiscountType type;
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public DiscountType Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }

        private ConditionField field;
        /// <summary>
        /// Gets or sets the field.
        /// </summary>
        /// <value>The field.</value>
        public ConditionField Field
        {
            get
            {
                return field;
            }

            set
            {
                field = value;
            }
        }

        private string match;
        /// <summary>
        /// Gets or sets the match.
        /// </summary>
        /// <value>The match.</value>
        public string Match
        {
            get
            {
                return match;
            }

            set
            {
                match = value;
            }
        }

        #endregion

        private Discount() { }

        /// <summary>
        /// Creates the specified amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        public static Discount Create(decimal amount)
        {
            Discount discount = new Discount();
            discount.id = -1;
            discount.amount = amount;
            discount.type = DiscountType.Amount;
            discount.field = ConditionField.State;
            return discount;
        }

        /// <summary>
        /// Fetches this instance.
        /// </summary>
        /// <returns></returns>
        public static Discount[] Fetch()
        {
            return FetchDiscounts(-1);
        }

        /// <summary>
        /// Fetches the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static Discount Fetch(int id)
        {
            Discount[] discounts = FetchDiscounts(id);

            if (discounts.Length > 0)
                return discounts[0];
            else return null;
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public void Save()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.ExecuteScalar(conn, "SaveDiscount", id, amount, type, field, match);

                if (id == -1)
                {
                    id = Convert.ToInt32(result);
                }
            }
        }

        private static Discount[] FetchDiscounts(int id)
        {
            List<Discount> lDiscounts = new List<Discount>();

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = SqlHelper.ExecuteReader(conn, "FetchDiscounts", id);

                while (reader.Read())
                {
                    Discount discount = new Discount();

                    discount.id = (int)reader["Id"];
                    discount.amount = (decimal)reader["Amount"];
                    discount.field = (ConditionField)(int)reader["Field"];
                    discount.type = (DiscountType)(int)reader["Type"];
                    discount.match = (string)reader["Match"];

                    lDiscounts.Add(discount);
                }
            }

            return lDiscounts.ToArray();
        }

        /// <summary>
        /// Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        public static void Delete(int id)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "DeleteDiscount", id);
            }
        }
    }

    /// <summary>
    /// The billing plan options and limitations
    /// </summary>
    [Serializable]
    [Reflection.Description("Billing Plan Options")]
    public class BillingPlanOptions //: UserRestrictions
    {
        //private bool canReadEmail = true;

        //[Reflection.Description("Can read e-mail")]
        //public bool CanReadEmail
        //{
        //    get { return canReadEmail; }
        //    set { canReadEmail = value; }
        //}

        public bool ContainsOptionWithEnabledCredits { get; set; }

        BillingPlanOption<bool> canReadEmail;
        [Reflection.Description("Can read e-mail")]
        [Reflection.AllowCredits]
        public BillingPlanOption<bool> CanReadEmail
        {
            get { return canReadEmail ?? (canReadEmail = new BillingPlanOption<bool> { Credits = 1, Value = true }); }
            set { canReadEmail = value; }
        }

        //private bool canIM = true;
        ///// <summary>
        ///// Gets or sets a value indicating whether this instance can IM.
        ///// </summary>
        ///// <value><c>true</c> if this instance can IM; otherwise, <c>false</c>.</value>
        //[Reflection.Description("Member can start Instant Messenger")]
        //public bool CanIM
        //{
        //    get { return canIM; }
        //    set { canIM = value; }
        //}

        BillingPlanOption<bool> canIM;
        [Reflection.Description("Member can start Instant Messenger")]
        [Reflection.AllowCredits]
        public BillingPlanOption<bool> CanIM
        {
            get { return canIM ?? (canIM = new BillingPlanOption<bool> { Value = true, Credits = 1 }); }
            set { canIM = value; }
        }

        BillingPlanOption<bool> canSendEcards;
        [Reflection.Description("Member can send e-cards")]
        [Reflection.AllowCredits]
        public BillingPlanOption<bool> CanSendEcards
        {
            get { return canSendEcards ?? (canSendEcards = new BillingPlanOption<bool> { Value = false, Credits = 1 }); }
            set { canSendEcards = value; }
        }

        BillingPlanOption<bool> canSendMessages;
        [Reflection.Description("Member can send messages")]
        [Reflection.AllowCredits]
        public BillingPlanOption<bool> CanSendMessages
        {
            get { return canSendMessages ?? (canSendMessages = new BillingPlanOption<bool> { Value = false, Credits = 1 }); }
            set { canSendMessages = value; }
        }

        BillingPlanOption<bool> canSendGifts;
        [Reflection.Description("Member can send gifts")]
        [Reflection.AllowCredits]
        public BillingPlanOption<bool> CanSendGifts
        {
            get { return canSendGifts ?? (canSendGifts = new BillingPlanOption<bool> { Value = false, Credits = 1 }); }
            set { canSendGifts = value; }
        }

        BillingPlanOption<bool> canSendWinks;
        [Reflection.Description("Member can send winks")]
        [Reflection.AllowCredits]
        public BillingPlanOption<bool> CanSendWinks
        {
            get { return canSendWinks ?? (canSendWinks = new BillingPlanOption<bool> { Value = false, Credits = 1 }); }
            set { canSendWinks = value; }
        }

        private BillingPlanOption<bool> canBrowseGroups;
        /// <summary>
        /// Gets or sets a value indicating whether this instance can browse groups.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can browse groups; otherwise, <c>false</c>.
        /// </value>
        [Reflection.Description("Member can browse groups")]
        public BillingPlanOption<bool> CanBrowseGroups
        {
            get
            {
                return canBrowseGroups ?? (canBrowseGroups = new BillingPlanOption<bool> { Value = true });
            }
            set
            {
                canBrowseGroups = value;
            }
        }

        private BillingPlanOption<bool> canJoinGroups;

        [Reflection.Description("Member can join groups")]
        public BillingPlanOption<bool> CanJoinGroups
        {
            get
            {
                if (canJoinGroups == null)
                {
                    canJoinGroups = new BillingPlanOption<bool>
                    {
                        Value = true
                    };
                }

                return canJoinGroups;
            }
            set
            {
                canJoinGroups = value;
            }
        }

        private BillingPlanOption<bool> canViewPhotos;

        [Reflection.Description("Can view photos")]
        [Reflection.AllowCredits]
        public BillingPlanOption<bool> CanViewPhotos
        {
            get { return canViewPhotos ?? (canViewPhotos = new BillingPlanOption<bool> { Value = true, Credits = 1 }); }
            set { canViewPhotos = value; }
        }

        private BillingPlanOption<bool> canViewExplicitPhotos;

        [Reflection.Description("Can view explicit photos")]
        public BillingPlanOption<bool> CanViewExplicitPhotos
        {
            get { return canViewExplicitPhotos ?? (canViewExplicitPhotos = new BillingPlanOption<bool> { Value = true }); }
            set { canViewExplicitPhotos = value; }
        }

        private BillingPlanOption<bool> canViewUserVideos;

        [Reflection.Description("Can view user videos")]
        [Reflection.AllowCredits]
        public BillingPlanOption<bool> CanViewUserVideos
        {
            get { return canViewUserVideos ?? (canViewUserVideos = new BillingPlanOption<bool> { Value = true, Credits = 1 }); }
            set { canViewUserVideos = value; }
        }

        private BillingPlanOption<bool> canViewStreamedVideo;

        [Reflection.Description("Can view streamed video")]
        [Reflection.AllowCredits]
        public BillingPlanOption<bool> CanViewStreamedVideo
        {
            get { return canViewStreamedVideo ?? (canViewStreamedVideo = new BillingPlanOption<bool> { Value = true, Credits = 1 }); }
            set { canViewStreamedVideo = value; }
        }

        /////////////////////////////////////////////////////////
        private BillingPlanOption<int> maxMessagesPerDay;

        [Reflection.Description("Maximum messages user can send a day (use -1 for unlimited)")]
        [Reflection.AllowCredits]
        public BillingPlanOption<int> MaxMessagesPerDay
        {
            get
            {
                if (maxMessagesPerDay == null)
                {
                    maxMessagesPerDay = new BillingPlanOption<int>
                    {
                        Value = -1,
                        Credits = 1
                    };
                }

                return maxMessagesPerDay;
            }
            set
            {
                maxMessagesPerDay = value;
            }
        }

        private BillingPlanOption<int> maxWinksPerDay;

        [Reflection.Description("Maximum winks user can send a day (use -1 for unlimited)")]
        [Reflection.AllowCredits]
        public BillingPlanOption<int> MaxWinksPerDay
        {
            get
            {
                if (maxWinksPerDay == null)
                {
                    maxWinksPerDay = new BillingPlanOption<int>
                    {
                        Value = -1,
                        Credits = 1
                    };
                }

                return maxWinksPerDay;
            }
            set
            {
                maxWinksPerDay = value;
            }
        }

        private BillingPlanOption<int> maxEcardsPerDay;

        //[Reflection.Description("Maximum e-cards user can send a day (use -1 for unlimited)")]
        [Reflection.Description("Additional credits bonus")]
        [Reflection.AllowCredits]
        public BillingPlanOption<int> MaxEcardsPerDay
        {
            get
            {
                if (maxEcardsPerDay == null)
                {
                    maxEcardsPerDay = new BillingPlanOption<int>
                    {
                        Value = -1,
                        Credits = 1
                    };
                }

                return maxEcardsPerDay;
            }
            set
            {
                maxEcardsPerDay = value;
            }
        }

        private BillingPlanOption<int> maxPhotos;

        /// <summary>
        /// Gets or sets the max photos.
        /// </summary>
        /// <value>The max photos.</value>
        [Reflection.Description("Maximum number of photos the member can upload")]
        public BillingPlanOption<int> MaxPhotos
        {
            get
            {
                if (maxPhotos == null)
                {
                    maxPhotos = new BillingPlanOption<int>
                    {
                        Value = Config.Photos.MaxPhotos
                    };
                }

                return maxPhotos;
            }
            set
            {
                maxPhotos = value;
            }
        }

        private BillingPlanOption<int> maxVideos;

        /// <summary>
        /// Gets or sets the max videos.
        /// </summary>
        /// <value>The max videos.</value>
        [Reflection.Description("Maximum number of videos the member can embed")]
        public BillingPlanOption<int> MaxVideos
        {
            get
            {
                if (maxVideos == null)
                {
                    maxVideos = new BillingPlanOption<int>
                    {
                        Value = 6//Config.Misc.MaxYouTubeVideos
                    };
                }

                return maxVideos;
            }
            set
            {
                maxVideos = value;
            }
        }

        private BillingPlanOption<bool> canCreateGroups;

        /// <summary>
        /// Gets or sets a value indicating whether this instance can create groups.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can create groups; otherwise, <c>false</c>.
        /// </value>
        [Reflection.Description("Member can create groups")]
        public BillingPlanOption<bool> CanCreateGroups
        {
            get
            {
                if (canCreateGroups == null)
                {
                    canCreateGroups = new BillingPlanOption<bool>
                    {
                        Value = true
                    };
                }

                return canCreateGroups;
            }
            set
            {
                canCreateGroups = value;
            }
        }

        private BillingPlanOption<bool> canSeeMessageStatus;

        /// <summary>
        /// Gets or sets a value indicating whether this instance can create groups.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can create groups; otherwise, <c>false</c>.
        /// </value>
        [Reflection.Description("Can see message status")]
        public BillingPlanOption<bool> CanSeeMessageStatus
        {
            get
            {
                if (canSeeMessageStatus == null)
                {
                    canSeeMessageStatus = new BillingPlanOption<bool>
                    {
                        Value = Config.Users.UsersCanSeeMessageStatus
                    };
                }

                return canSeeMessageStatus;
            }
            set
            {
                canSeeMessageStatus = value;
            }
        }

        private BillingPlanOption<bool> canRateProfiles;

        [Reflection.Description("Member can rate profiles")]
        public BillingPlanOption<bool> CanRateProfiles
        {
            get
            {
                if (canRateProfiles == null)
                {
                    canRateProfiles = new BillingPlanOption<bool>
                    {
                        Value = Config.Ratings.EnableProfileRatings
                    };
                }

                return canRateProfiles;
            }
            set
            {
                canRateProfiles = value;
            }
        }

        private BillingPlanOption<bool> canRatePhotos;

        [Reflection.Description("Member can rate photos")]
        public BillingPlanOption<bool> CanRatePhotos
        {
            get
            {
                if (canRatePhotos == null)
                {
                    canRatePhotos = new BillingPlanOption<bool>
                    {
                        Value = Config.Ratings.EnablePhotoRatings
                    };
                }

                return canRatePhotos;
            }
            set
            {
                canRatePhotos = value;
            }
        }

        private BillingPlanOption<bool> canCreateBlogs;

        /// <summary>
        /// Gets or sets a value indicating whether this instance can create blogs.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can create blogs; otherwise, <c>false</c>.
        /// </value>
        [Reflection.Description("Member can create blogs")]
        [Reflection.AllowCredits]
        public BillingPlanOption<bool> CanCreateBlogs
        {
            get
            {
                if (canCreateBlogs == null)
                {
                    canCreateBlogs = new BillingPlanOption<bool>
                    {
                        Value = Config.Misc.EnableBlogs,
                        Credits = 1
                    };
                }

                return canCreateBlogs;
            }
            set
            {
                canCreateBlogs = value;
            }
        }

        private BillingPlanOption<int> maxGroupsPerMember;

        /// <summary>
        /// Gets or sets the max groups per member.
        /// </summary>
        /// <value>The max groups per member.</value>
        [Reflection.Description("Maximum number of groups the member can join")]
        public BillingPlanOption<int> MaxGroupsPerMember
        {
            get
            {
                if (maxGroupsPerMember == null)
                {
                    maxGroupsPerMember = new BillingPlanOption<int>
                    {
                        Value = 30//Config.Groups.MaxGroupsPerMember
                    };
                }

                return maxGroupsPerMember;
            }
            set
            {
                maxGroupsPerMember = value;
            }
        }

        private BillingPlanOption<int> maxVideoUploads;

        /// <summary>
        /// Gets or sets the max video uploads.
        /// </summary>
        /// <value>The max video uploads.</value>
        [Reflection.Description("Maximum number of videos the member can upload")]
        public BillingPlanOption<int> MaxVideoUploads
        {
            get
            {
                if (maxVideoUploads == null)
                {
                    maxVideoUploads = new BillingPlanOption<int>
                    {
                        Value = 3//Config.Misc.MaxVideoUploads
                    };
                }

                return maxVideoUploads;
            }
            set
            {
                maxVideoUploads = value;
            }
        }

        private BillingPlanOption<int> maxAudioUploads;

        [Reflection.Description("Maximum number of audio uploads the member can upload")]
        public BillingPlanOption<int> MaxAudioUploads
        {
            get
            {
                if (maxAudioUploads == null)
                {
                    maxAudioUploads = new BillingPlanOption<int>
                    {
                        Value = 3
                    };
                }

                return maxAudioUploads;
            }
            set
            {
                maxAudioUploads = value;
            }
        }

        private BillingPlanOption<bool> autoApproveAnswers;

        /// <summary>
        /// Gets or sets a value indicating whether [auto approve answers].
        /// </summary>
        /// <value><c>true</c> if [auto approve answers]; otherwise, <c>false</c>.</value>
        [Reflection.Description("Auto approve answers")]
        public BillingPlanOption<bool> AutoApproveAnswers
        {
            get
            {
                if (autoApproveAnswers == null)
                {
                    autoApproveAnswers = new BillingPlanOption<bool>
                    {
                        Value = false
                    };
                }

                return autoApproveAnswers;
            }
            set
            {
                autoApproveAnswers = value;
            }
        }

        private BillingPlanOption<bool> autoApprovePhotos;

        /// <summary>
        /// Gets or sets a value indicating whether [auto approve photos].
        /// </summary>
        /// <value><c>true</c> if [auto approve photos]; otherwise, <c>false</c>.</value>
        [Reflection.Description("Auto approve photos")]
        public BillingPlanOption<bool> AutoApprovePhotos
        {
            get
            {
                if (autoApprovePhotos == null)
                {
                    autoApprovePhotos = new BillingPlanOption<bool>
                    {
                        Value = Config.Photos.AutoApprovePhotos
                    };
                }

                return autoApprovePhotos;
            }
            set
            {
                autoApprovePhotos = value;
            }
        }

        private BillingPlanOption<bool> autoApproveVideos;

        /// <summary>
        /// Gets or sets a value indicating whether [auto approve videos].
        /// </summary>
        /// <value><c>true</c> if [auto approve videos]; otherwise, <c>false</c>.</value>
        [Reflection.Description("Auto approve videos")]
        public BillingPlanOption<bool> AutoApproveVideos
        {
            get
            {
                if (autoApproveVideos == null)
                {
                    autoApproveVideos = new BillingPlanOption<bool>
                    {
                        Value = false
                    };
                }

                return autoApproveVideos;
            }
            set
            {
                autoApproveVideos = value;
            }
        }

        private BillingPlanOption<bool> autoApproveAudioUploads;

        /// <summary>
        /// Gets or sets a value indicating whether [auto approve audio uploads].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [auto approve audio uploads]; otherwise, <c>false</c>.
        /// </value>
        [Reflection.Description("Auto approve audio uploads")]
        public BillingPlanOption<bool> AutoApproveAudioUploads
        {
            get
            {
                if (autoApproveAudioUploads == null)
                {
                    autoApproveAudioUploads = new BillingPlanOption<bool>
                    {
                        Value = false
                    };
                }

                return autoApproveAudioUploads;
            }
            set
            {
                autoApproveAudioUploads = value;
            }
        }

        private BillingPlanOption<bool> autoApproveAds;

        [Reflection.Description("Auto approve classifieds")]
        public BillingPlanOption<bool> AutoApproveAds
        {
            get
            {
                if (autoApproveAds == null)
                {
                    autoApproveAds = new BillingPlanOption<bool>
                    {
                        Value = false
                    };
                }

                return autoApproveAds;
            }
            set
            {
                autoApproveAds = value;
            }
        }

        private BillingPlanOption<int> maxActiveAds;

        [Reflection.Description("Maximum active classifieds")]
        public BillingPlanOption<int> MaxActiveAds
        {
            get
            {
                if (maxActiveAds == null)
                {
                    maxActiveAds = new BillingPlanOption<int>
                    {
                        Value = 5
                    };
                }

                return maxActiveAds;
            }
            set
            {
                maxActiveAds = value;
            }
        }

        private BillingPlanOption<bool> userCanReportAbuse;

        /// <summary>
        /// Gets or sets a value indicating whether [user can report abuse].
        /// </summary>
        /// <value><c>true</c> if [user can report abuse]; otherwise, <c>false</c>.</value>
        [Reflection.Description("Can report abuse")]
        public BillingPlanOption<bool> UserCanReportAbuse
        {
            get
            {
                if (userCanReportAbuse == null)
                {
                    userCanReportAbuse = new BillingPlanOption<bool>
                    {
                        Value = true
                    };
                }

                return userCanReportAbuse;
            }
            set
            {
                userCanReportAbuse = value;
            }
        }

        private BillingPlanOption<bool> userCanUseChat;

        /// <summary>
        /// Gets or sets a value indicating whether [user can use chat].
        /// </summary>
        /// <value><c>true</c> if [user can use chat]; otherwise, <c>false</c>.</value>
        [Reflection.Description("Can use chat")]
        public BillingPlanOption<bool> UserCanUseChat
        {
            get
            {
                if (userCanUseChat == null)
                {
                    userCanUseChat = new BillingPlanOption<bool>
                    {
                        Value = true
                    };
                }

                return userCanUseChat;
            }
            set
            {
                userCanUseChat = value;
            }
        }

        private BillingPlanOption<bool> userCanAddComments;
        [Reflection.Description("Can add comments")]
        public BillingPlanOption<bool> UserCanAddComments
        {
            get
            {
                if (userCanAddComments == null)
                {
                    userCanAddComments = new BillingPlanOption<bool>
                    {
                        Value = true
                    };
                }

                return userCanAddComments;
            }
            set
            {
                userCanAddComments = value;
            }
        }

        private BillingPlanOption<bool> userCanUseSkin;
        [Reflection.Description("Can use skin")]
        public BillingPlanOption<bool> UserCanUseSkin
        {
            get
            {
                if (userCanUseSkin == null)
                {
                    userCanUseSkin = new BillingPlanOption<bool>
                    {
                        Value = Config.Users.EnableProfileSkins
                    };
                }

                return userCanUseSkin;
            }
            set
            {
                userCanUseSkin = value;
            }
        }

        private BillingPlanOption<bool> userCanEditSkin;

        /// <summary>
        /// Gets or sets a value indicating whether [user can use chat].
        /// </summary>
        /// <value><c>true</c> if [user can use chat]; otherwise, <c>false</c>.</value>
        [Reflection.Description("Can edit skin")]
        public BillingPlanOption<bool> UserCanEditSkin
        {
            get
            {
                if (userCanEditSkin == null)
                {
                    userCanEditSkin = new BillingPlanOption<bool>
                    {
                        Value = Config.Users.EnableProfileSkins
                    };
                }

                return userCanEditSkin;
            }
            set
            {
                userCanEditSkin = value;
            }
        }

        private BillingPlanOption<bool> userCanBrowseClassifieds;

        [Reflection.Description("Can browse classifieds")]
        public BillingPlanOption<bool> UserCanBrowseClassifieds
        {
            get
            {
                if (userCanBrowseClassifieds == null)
                {
                    userCanBrowseClassifieds = new BillingPlanOption<bool>
                    {
                        Value = Config.Ads.Enable
                    };
                }

                return userCanBrowseClassifieds;
            }
            set
            {
                userCanBrowseClassifieds = value;
            }
        }

        private BillingPlanOption<bool> userCanPostAd;

        [Reflection.Description("Can post classifieds")]
        public BillingPlanOption<bool> UserCanPostAd
        {
            get
            {
                if (userCanPostAd == null)
                {
                    userCanPostAd = new BillingPlanOption<bool>
                    {
                        Value = Config.Ads.Enable
                    };
                }

                return userCanPostAd;
            }
            set
            {
                userCanPostAd = value;
            }
        }

        private BillingPlanOption<bool> doNotShowBanners;

        [Reflection.Description("Do not show banners")]
        public BillingPlanOption<bool> DoNotShowBanners
        {
            get
            {
                if (doNotShowBanners == null)
                {
                    doNotShowBanners = new BillingPlanOption<bool>
                    {
                        Value = false
                    };
                }

                return doNotShowBanners;
            }
            set
            {
                doNotShowBanners = value;
            }
        }
    }

    [Serializable]
    public class BillingPlanOption<T>
    {
        [XmlIgnore]
        public int PlanID;
        //[XmlIgnore]
        //public string PlanName;
        [XmlIgnore]
        public string ID;
        [XmlIgnore]
        public string PropertyDescription;
        [XmlIgnore]
        public bool AllowCredits;
        public T Value;
        //public string PropertyName;
        public bool EnableCreditsPayment;
        public int Credits;
        public bool UpgradableToNextPlan;//AvailableInNextPlan;
    }
}
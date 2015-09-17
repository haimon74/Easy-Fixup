using System;
using System.Linq;
using System.Web;
using AspNetDating.Model;
using System.Web.Caching;

namespace AspNetDating.Classes
{
    public class BannerCode
    {
        public static CacheDependencyCollection CacheDependencies = new CacheDependencyCollection();

        #region fields

        private int id;
        private ePosition position;
        private int priority;
        private string target;
        private string code;

        public enum ePosition
        {
            DefaultFooter,
            HomeBeforeProfile,
            HomeAfterProfile,
            HomeRightBottom,
            HomeLeftBottom,
            MailboxRightTop,
            MailboxRightBottom,
            MailboxLeftBottom,
            SendMessageRight,
            ShowMessageLeftBottom,
            ShowMessageRightBottom,
            ShowUserLeftBottom,
            ShowUserRightTop,
            ShowUserRightBottom,
            ShowUserPhotosAbovePhoto,
            ShowUserPhotosUnderPhoto,
            BlogRightTop,
            BlogRightBottom,
            GroupsRightTop,
            GroupsRightBottom,
            FriendsLeft,
            FavoritesLeft,
            SearchLeftBottom,
            ProfileLeftBottom,
            ProfileRightBottom,
            SendMessageLeftBottom,
            SendMessageRightBottom,
            GiftsLeftBottom,
            FAQsLeftBottom,
            FAQsRightBottom,
            ContactUsLeftBottom,
            ContactUsRightBottom,
            BlockedUsersLeftBottom,
            BlockedUsersRightBottom,
            AddRemoveFriendLeftBottom,
            AddRemoveFriendRightBottom,
            AddRemoveFavoriteLeftBottom,
            AddRemoveFavoriteRightBottom
        }

        public enum eVisibleFor
        {
            LoggedInUsers,
            NonLoggedInVisitors
        }

        public enum eSortColumn
        {
            None,
            Priority
        }

//        public static Dictionary<string, string> bannerKeys = new Dictionary<string, string>
//                                                                      {
//                                                                          {"DefaultFooter", "Default page footer"},
//                                                                          {"HomeBeforeProfile", "Home page before profile"},
//                                                                          {"HomeAfterProfile", "Home page after profile"},
//                                                                          {"HomeRightBottom", "Home page right bottom"},
//                                                                          {"HomeLeftBottom", "Home page left bottom"},
//                                                                          {"MailboxRightTop", "Mailbox page right top"},
//                                                                          {"MailboxRightBottom", "Mailbox page right bottom"},
//                                                                          {"SendMessageRight", "Send message page right"},
//                                                                          {"ShowMessageRightBottom", "Show message page right bottom"},
//                                                                          {"ShowUserLeftBottom", "Show user page left bottom"},
//                                                                          {"ShowUserRightTop", "Show user page right top"},
//                                                                          {"ShowUserRightBottom", "Show user page right bottom"},
//                                                                          {"ShowUserPhotosAbovePhoto", "Show user photos page above photo"},
//                                                                          {"ShowUserPhotosUnderPhoto", "Show user photos page under photo"},
//                                                                          {"BlogRightTop", "Blog page right top"},
//                                                                          {"BlogRightBottom", "Blog page right bottom"},
//                                                                          {"GroupsRightTop", "Groups page right top"},
//                                                                          {"GroupsRightBottom", "Groups page right bottom"}
//                                                                      };

        #endregion

        #region Constructors

        public BannerCode() {}

        #endregion

        #region Properties

        public int ID
        {
            get { return id; }
        }

        public ePosition Position
        {
            get { return position; }
            set { position = value; }
        }

        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }


        /// <summary>
        /// If it is NULL this object is the default banner code.
        /// </summary>
        /// <value>The target.</value>
        public string Target
        {
            get { return target; }
            set { target = value; }
        }

        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        #endregion

        #region Methods

        public static BannerCode[] Fetch()
        {
            return Fetch(null, null, null, eSortColumn.None);
        }

        public static BannerCode Fetch(int id)
        {
            BannerCode[] bannerCodes = Fetch(id, null, null, eSortColumn.None);
            
            if (bannerCodes.Length > 0) return bannerCodes[0];
            
            return null;
        }

        public static BannerCode[] Fetch(ePosition position)
        {
            return Fetch(null, position, null, eSortColumn.None);
        }

        public static BannerCode[] Fetch(ePosition position, eSortColumn sortColumn)
        {
            return Fetch(null, position, null, sortColumn);
        }

        public static BannerCode FetchDefault(ePosition position)
        {
            BannerCode[] codes = Fetch(null, position, true, eSortColumn.None);

            if (codes.Length > 0) return codes[0];
            
            return null;
        }

        private static BannerCode[] Fetch(int? id, ePosition? position, bool? getDefault, eSortColumn sortColumn)
        {
            string cacheKey = String.Format("BannerCode_Fetch_{0}_{1}_{2}_{3}", id, position, getDefault, sortColumn);

            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as BannerCode[];
            }

            using (var db = new Model.AspNetDatingDataContext())
            {
                var bannerCodes = from bc in db.BannerCodes
                                  where (!id.HasValue || bc.bc_id == id)
                                        && (!position.HasValue || bc.bc_position == (int?) position)
                                        && (!getDefault.HasValue || bc.bc_target == null)
                                  select new BannerCode
                                             {
                                                 id = bc.bc_id,
                                                 position = (ePosition) bc.bc_position,
                                                 priority = bc.bc_priority,
                                                 target = bc.bc_target,
                                                 code = bc.bc_code
                                             };
                switch (sortColumn)
                {
                    case eSortColumn.None :
                        break;
                    case eSortColumn.Priority :
                        bannerCodes = bannerCodes.OrderByDescending(bc => bc.priority);
                        break;
                }

                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Cache.Insert(cacheKey, bannerCodes.ToArray(), CacheDependencies.Get(), DateTime.Now.AddMinutes(30),
                        Cache.NoSlidingExpiration);
                }

                return bannerCodes.ToArray();
            }
        }

        public void Save()
        {
            using (var db = new AspNetDatingDataContext())
            {
                var bannerCode = new Model.BannerCode()
                                     {
                                         bc_id = id,
                                         bc_position = (int) position,
                                         bc_priority = priority,
                                         bc_target = target,
                                         bc_code = code
                                     };
                if (id == 0)
                {
                    db.BannerCodes.InsertOnSubmit(bannerCode);
                }
                else
                {
                    db.BannerCodes.Attach(bannerCode, true);
                }

                db.SubmitChanges();

                if (id == 0) id = bannerCode.bc_id;

                CacheDependencies.NotifyChanged();
            }
        }

        public static void Delete(int id)
        {
            using (var db = new AspNetDatingDataContext())
            {
                var bannerCode = db.BannerCodes.Single(c => c.bc_id == id);
                db.BannerCodes.DeleteOnSubmit(bannerCode);
                db.SubmitChanges();
                CacheDependencies.NotifyChanged();
            }
        }

        #endregion


    }

    [Serializable]
    public class BannerCodeTarget
    {
        private string name;
        private string country;
        private string region;
        private string city;
        private User.eGender? gender;
        private BannerCode.eVisibleFor? visibleFor;
        private bool? paid;
        private int? fromAge = null;
        private int? toAge = null;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Country
        {
            get { return country; }
            set { country = value; }
        }

        public string Region
        {
            get { return region; }
            set { region = value; }
        }

        public string City
        {
            get { return city; }
            set { city = value; }
        }

        public User.eGender? Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        public BannerCode.eVisibleFor? VisibleFor
        {
            get { return visibleFor; }
            set { visibleFor = value; }
        }

        public bool? Paid
        {
            get { return paid; }
            set { paid = value; }
        }

        public int? FromAge
        {
            get { return fromAge; }
            set { fromAge = value; }
        }

        public int? ToAge
        {
            get { return toAge; }
            set { toAge = value; }
        }
    }
}

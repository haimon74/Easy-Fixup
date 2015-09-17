using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.ApplicationBlocks.Data;

namespace AspNetDating.Classes
{
    /// <summary>
    /// This class handles the affiliates
    /// </summary>
    [Serializable]
    public class Affiliate
    {
        #region fields

        private int? id = null;
        private string username;
        private string password;
        private string name;
        private string email;
        private string siteURL;
        private string paymentDetails;
        private bool deleted = false;
        private bool active = true;
        private int? percentage = Config.Affiliates.Percentage;
        private decimal? fixedAmount = Config.Affiliates.FixedAmount;
        private bool recurrent = Config.Affiliates.Recurrent;
        private decimal balance = 0;
        private bool requestPayment = false;
        private eSortColumn sortColumn;

        /// <summary>
        /// Specify on which column affiliates will be sorted.
        /// </summary>
        public enum eSortColumn
        {
            /// <summary>
            /// No sort.
            /// </summary>
            None,
            /// <summary>
            /// On username created column.
            /// </summary>
            Username,
            /// <summary>
            /// On name column.
            /// </summary>
            Name,
            /// <summary>
            /// On deleted column.
            /// </summary>
            Deleted,
            /// <summary>
            /// On active column
            /// </summary>
            Active,
            /// <summary>
            /// On balance column.
            /// </summary>
            Balance,
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Affiliate"/> class.
        /// </summary>
        public Affiliate()
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="Affiliate"/> class.
        /// </summary>
        /// <param name="username">The username.</param>
        public Affiliate(string username)
        {
            this.username = username;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        public int ID
        {
            get
            {
                if (id.HasValue)
                {
                    return id.Value;
                }
                else
                {
                    throw new Exception("The field ID is not set!");
                }
            }
        }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>The username.</value>
        public string Username
        {
            get { return username; }
            set 
            {
                try
                {
                    ValidateUsername(value);
                }
                catch (ArgumentNullException)
                {
                    throw new ArgumentException
                        (Lang.Trans("Invalid username!"));
                }

                username = value;
            }
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password
        {
            get { return password; }
            set
            {
                try
                {
                    ValidatePassword(value);
                }
                catch
                {
                    throw new ArgumentException
                        (Lang.Trans("Invalid password!") /*, "Password"*/);
                }

                password = FormsAuthentication
                    .HashPasswordForStoringInConfigFile(value, "sha1");
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        /// <summary>
        /// Gets or sets the site URL.
        /// </summary>
        /// <value>The site URL.</value>
        public string SiteURL
        {
            get { return siteURL; }
            set { siteURL = value; }
        }

        /// <summary>
        /// Gets or sets the payment details.
        /// </summary>
        /// <value>The payment details.</value>
        public string PaymentDetails
        {
            get { return paymentDetails; }
            set { paymentDetails = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Affiliate"/> is deleted.
        /// </summary>
        /// <value><c>true</c> if deleted; otherwise, <c>false</c>.</value>
        public bool Deleted
        {
            get { return deleted; }
            set { deleted = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Affiliate"/> is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        /// <summary>
        /// Gets or sets the percentage.
        /// </summary>
        /// <value>The percentage.</value>
        public int? Percentage
        {
            get { return percentage; }
            set { percentage = value; }
        }

        /// <summary>
        /// Gets or sets the fixed amount.
        /// </summary>
        /// <value>The fixed amount.</value>
        public decimal? FixedAmount
        {
            get { return fixedAmount; }
            set { fixedAmount = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Affiliate"/> is recurrent.
        /// </summary>
        /// <value><c>true</c> if recurrent; otherwise, <c>false</c>.</value>
        public bool Recurrent
        {
            get { return recurrent; }
            set { recurrent = value; }
        }

        /// <summary>
        /// Gets or sets the balance.
        /// </summary>
        /// <value>The balance.</value>
        public decimal Balance
        {
            get
            {
                balance *= 100;
                balance = Math.Floor(balance);
                balance /= 100;
                return balance;
            }
            set { balance = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [request payment].
        /// </summary>
        /// <value><c>true</c> if [request payment]; otherwise, <c>false</c>.</value>
        public bool RequestPayment
        {
            get { return requestPayment; }
            set { requestPayment = value; }
        }

        /// <summary>
        /// Gets or sets the sort column.
        /// </summary>
        /// <value>The sort column.</value>
        public eSortColumn SortColumn
        {
            get { return sortColumn; }
            set { sortColumn = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fetches all affiliates from DB.
        /// If there are no affiliates in DB it returns an empty array.
        /// </summary>
        /// <returns></returns>
        public static Affiliate[] Fetch()
        {
            return Fetch(null, null, null, null, null, null, null, null, null, eSortColumn.None);
        }

        /// <summary>
        /// Fetches the specified sort column.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        /// <returns></returns>
        public static Affiliate[] Fetch(eSortColumn sortColumn)
        {
            return Fetch(null, null, null, null, null, null, null, null, null, sortColumn);
        }

        /// <summary>
        /// Fetches affiliate by specified id from DB.
        /// If the affiliate doesn't exist returns NULL.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static Affiliate Fetch(int id)
        {
            Affiliate[] affiliates = Fetch(id, null, null, null, null, null, null, null, null, eSortColumn.None);

            if (affiliates.Length > 0)
            {
                return affiliates[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Fetches affiliate by specified username from DB.
        /// If the affiliate doesn't exist returns NULL.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static Affiliate Fetch(string username)
        {
            Affiliate[] affiliates = Fetch(null, username, null, null, null, null, null, null, null, eSortColumn.None);

            if (affiliates.Length > 0)
            {
                return affiliates[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Fetches all affiliates which request/don't requests payment from DB.
        /// If there are no affiliates which request/don't requests payment in DB
        /// it returns an empty array.
        /// </summary>
        /// <param name="requestPayment">if set to <c>true</c> [request payment].</param>
        /// <returns></returns>
        public static Affiliate[] Fetch(bool requestPayment)
        {
            return Fetch(null, null, false, true, null, null, null, null, requestPayment, eSortColumn.None);
        }

        /// <summary>
        /// Fetches affiliates by specified arguments.
        /// It returns an empty array if there are no affiliates in DB by specified arguments.
        /// If these arguments are null it returns all affiliates from DB.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="username">The username.</param>
        /// <param name="deleted">The deleted.</param>
        /// <param name="active">The active.</param>
        /// <param name="percentage">The percentage.</param>
        /// <param name="fixedAmount">The fixed amount.</param>
        /// <param name="recurrent">The recurrent.</param>
        /// <param name="balance">The balance.</param>
        /// <param name="requestPayment">The request payment.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <returns></returns>
        private static Affiliate[] Fetch(int? id, string username, bool? deleted, bool? active, int? percentage, decimal? fixedAmount, bool? recurrent, decimal? balance, bool? requestPayment, eSortColumn sortColumn)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn, "FetchAffiliates", id, username, deleted, active, percentage,
                                            fixedAmount, recurrent, balance, requestPayment, sortColumn);

                List <Affiliate> lAffiliates = new List<Affiliate>();

                while (reader.Read())
                {
                    Affiliate affiliate = new Affiliate();

                    affiliate.id = (int) reader["ID"];
                    affiliate.username = (string) reader["Username"];
                    affiliate.password = (string) reader["Password"];
                    affiliate.name = (string) reader["Name"];
                    affiliate.email = (string) reader["Email"];
                    affiliate.siteURL = (string) reader["SiteURL"];
                    affiliate.paymentDetails = (string) reader["PaymentDetails"];
                    affiliate.deleted = (bool) reader["Deleted"];
                    affiliate.active = (bool) reader["Active"];
                    affiliate.percentage = reader["Percentage"] == DBNull.Value ? null : (int?)reader["Percentage"];
                    affiliate.fixedAmount = reader["FixedAmount"] == DBNull.Value ? null : (decimal?)reader["FixedAmount"];
                    affiliate.recurrent = (bool) reader["Recurrent"];
                    affiliate.balance = (decimal) reader["Balance"];
                    affiliate.requestPayment = (bool) reader["RequestPayment"];

                    lAffiliates.Add(affiliate);
                }

                return lAffiliates.ToArray();
            }
        }

        /// <summary>
        /// Saves this instance in DB. If the field id is null it inserts new record in DB,
        /// otherwise updates the record.
        /// </summary>
        public void Save()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.ExecuteScalar(conn, "SaveAffiliate",
                                        id, username, password, name, email, siteURL, paymentDetails, deleted, active,
                                        percentage == 0 ? null : percentage, fixedAmount == 0 ? null : fixedAmount, recurrent, balance, requestPayment);

                if (!id.HasValue)
                {
                    id = Convert.ToInt32(result);
                }
            }
        }

        /// <summary>
        /// Deletes affiliates from DB by specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        public static void Delete(int id)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "DeleteAffiliate", id);
            }
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        protected void Load()
        {
            Affiliate affiliate = Fetch(username);

            if (affiliate == null)
            {
                throw new NotFoundException
                    (Lang.Trans("The specified account does not exist!"));
            }

            id = affiliate.id;
            username = affiliate.username;
            password = affiliate.password;
            name = affiliate.name;
            email = affiliate.email;
            siteURL = affiliate.siteURL;
            paymentDetails = affiliate.paymentDetails;
            deleted = affiliate.deleted;
            active = affiliate.active;
            percentage = affiliate.percentage;
            fixedAmount = affiliate.fixedAmount;
            recurrent = affiliate.recurrent;
            balance = affiliate.balance;
            requestPayment = affiliate.requestPayment;
        }

        #region Validators

        /// <summary>
        /// Validates the specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <exception cref="ArgumentNullException">
        /// Username is null
        /// -or-
        /// is too short. The username length is defined in admin section.
        /// -or-
        /// is too long. The username length is defined in admin section.
        /// -or-
        /// contains invalid characters.
        /// </exception>
        public static void ValidateUsername(string username)
        {
            if (username == null)
                throw new ArgumentNullException
                    (Lang.Trans("Username cannot be null!"));

            if (username.Length < Config.Affiliates.UsernameMinLength)
                throw new ArgumentException
                    (String.Format(Lang.Trans("Username too short! Should be at least {0} chars."),
                                   Config.Affiliates.UsernameMinLength));

            if (username.Length > Config.Affiliates.UsernameMaxLength)
                throw new ArgumentException
                    (String.Format(Lang.Trans("Username too long! Should be at most {0} chars."),
                                   Config.Affiliates.UsernameMaxLength));

            if (!Regex.Match(username, @"^[\w_-]+$").Success)
                throw new ArgumentException
                    (Lang.Trans("Invalid chars in username!"));
        }

        /// <summary>
        /// Validates the specified password.
        /// </summary>
        /// <param name="Password">The password.</param>
        /// <exception cref="ArgumentNullException">
        /// Password is null
        /// -or-
        /// is too short. The password length is defined in admin section.
        /// -or-
        /// is too long. The password length is defined in admin section.
        /// </exception>
        public static void ValidatePassword(string Password)
        {
            if (Password == null)
                throw new ArgumentNullException
                    (Lang.Trans("Password cannot be null!"));

            if (Password.Length < Config.Affiliates.PasswordMinLength)
                throw new ArgumentException
                    (String.Format(Lang.Trans("Password too short! Should be at least {0} chars."),
                                   Config.Affiliates.PasswordMinLength));

            if (Password.Length > Config.Affiliates.PasswordMaxLength)
                throw new ArgumentException
                    (String.Format(Lang.Trans("Password too long! Should be at most {0} chars."),
                                   Config.Affiliates.PasswordMaxLength));
        }

        #endregion

        /// <summary>
        /// Determines whether [is password identical] [the specified password].
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>
        /// 	<c>true</c> if [is password identical] [the specified password]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsPasswordIdentical(string password)
        {
            if (this.password == FormsAuthentication
                                     .HashPasswordForStoringInConfigFile(password, "sha1"))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Determines whether [is username taken] [the specified username].
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>
        /// 	<c>true</c> if [is username taken] [the specified username]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsUsernameTaken(string username)
        {
            ValidateUsername(username);

            if (Fetch(username) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Authorizes the specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public static void Authorize(string username, string password)
        {
            ValidateUsername(username);
            ValidatePassword(password);

            Affiliate affiliate = Fetch(username);

            if (!affiliate.IsPasswordIdentical(password))
                throw new AccessDeniedException
                    (Lang.Trans("The provided password is invalid!"));
        }

        #endregion
    }

    /// <summary>
    /// Contains the current affiliate session
    /// </summary>
    [Serializable]
    public class AffiliateSession : Affiliate
    {
        private bool isAuthorized = false;

        /// <summary>
        /// Gets a value indicating whether this instance is authorized.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is authorized; otherwise, <c>false</c>.
        /// </value>
        public bool IsAuthorized
        {
            get { return isAuthorized; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AffiliateSession"/> class.
        /// </summary>
        /// <param name="username">The username.</param>
        public AffiliateSession(string username)
            : base(username)
        {
            Load();
        }

        /// <summary>
        /// Checks the provided password against the database and marks
        /// the admin as authorized if match.
        /// </summary>
        public void Authorize(string password)
        {
            Authorize(Username, password);
            isAuthorized = true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class AffiliateSearchResults : SearchResults<int, Affiliate>
    {
        /// <summary>
        /// Gets or sets the affiliates.
        /// </summary>
        /// <value>The affiliates.</value>
        public int[] Affiliates
        {
            get
            {
                if (Results == null)
                    return new int[0];
                else
                    return Results;
            }
            set { Results = value; }
        }

        /// <summary>
        /// Gets the total pages.
        /// </summary>
        /// <param name="affiliatesPerPage">The affiliates per page.</param>
        /// <returns></returns>
        public new int GetTotalPages(int affiliatesPerPage)
        {
            return base.GetTotalPages(affiliatesPerPage);
        }

        /// <summary>
        /// Loads the result.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        protected override Affiliate LoadResult(int id)
        {
            return Affiliate.Fetch(id);
        }
        protected override Affiliate[] LoadResults(int[] ids)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Use this method to get the search results.
        /// </summary>
        /// <param name="Page">The page.</param>
        /// <param name="affiliatesPerPage">The affiliates per page.</param>
        /// <returns></returns>
        public new Affiliate[] GetPage(int Page, int affiliatesPerPage)
        {
            return base.GetPage(Page, affiliatesPerPage);
        }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        public Affiliate[] Get()
        {
            return GetPage(1, Int32.MaxValue);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class BasicSearchAffiliate
    {
        #region Properties

        private string username = null;

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>The username.</value>
        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        private string name = null;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string email = null;

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        private string siteURL = null;

        /// <summary>
        /// Gets or sets the site URL.
        /// </summary>
        /// <value>The site URL.</value>
        public string SiteURL
        {
            get { return siteURL; }
            set { siteURL = value; }
        }

        private bool? deleted = null;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="BasicSearchAffiliate"/> is deleted.
        /// </summary>
        /// <value><c>true</c> if deleted; otherwise, <c>false</c>.</value>
        public bool Deleted
        {
            get { return deleted.Value; }
            set { deleted = value; }
        }

        private bool? active = null;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="BasicSearchAffiliate"/> is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        public bool Active
        {
            get { return active.Value; }
            set { active = value; }
        }

        private bool? requestPayment = null;

        /// <summary>
        /// Gets or sets a value indicating whether [request payment].
        /// </summary>
        /// <value><c>true</c> if [request payment]; otherwise, <c>false</c>.</value>
        public bool RequestPayment
        {
            get { return requestPayment.Value; }
            set { requestPayment = value; }
        }

        private Affiliate.eSortColumn sortColumn;

        /// <summary>
        /// Gets or sets the sort column.
        /// </summary>
        /// <value>The sort column.</value>
        public Affiliate.eSortColumn SortColumn
        {
            get { return sortColumn; }
            set { sortColumn = value; }
        }

        private bool sortAsc;

        /// <summary>
        /// Gets or sets a value indicating whether [sort asc].
        /// </summary>
        /// <value><c>true</c> if [sort asc]; otherwise, <c>false</c>.</value>
        public bool SortAsc
        {
            get { return sortAsc; }
            set { sortAsc = value; }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicSearchAffiliate"/> class.
        /// </summary>
        public BasicSearchAffiliate()
        {
            // Set defaults
            SortColumn = Affiliate.eSortColumn.Username;
            SortAsc = false;
        }

        /// <summary>
        /// Gets the results.
        /// </summary>
        /// <returns></returns>
        public AffiliateSearchResults GetResults()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn,
                                            "BasicSearchAffiliate",
                                            username, name, email, siteURL, deleted, active,
                                            requestPayment, sortColumn);

                List<int> lResults = new List<int>();

                while (reader.Read())
                {
                    lResults.Add((int)reader["ID"]);
                }

                if (!sortAsc) lResults.Reverse();

                if (lResults.Count > 0)
                {
                    AffiliateSearchResults results = new AffiliateSearchResults();
                    results.Affiliates = lResults.ToArray();
                    return results;
                }
                else
                    return null;
            }
        }
    }
}

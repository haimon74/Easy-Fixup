using System;
using System.Data;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using ezFixUp.Classes;

namespace ezFixUp.Components
{
    public partial class BirthdayBox : UserControl
    {
        protected SmallBoxStart SmallBoxStart1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (SmallBoxStart1 != null)
                SmallBoxStart1.Title = Lang.Trans("Birthdays");

            Visible = !Config.Users.DisableAgeInformation;
            
            if (!Config.Users.DisableAgeInformation)
                LoadBirthdays();
        }

        private void LoadBirthdays()
        {
            DataTable dtBirthdays = null;

            string cacheKey = String.Format("Components_LoadBirthdays");
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                dtBirthdays = Cache[cacheKey] as DataTable;
            }
            else
            {
                dtBirthdays = new DataTable();
                dtBirthdays.Columns.Add("Date", typeof (DateTime));
                dtBirthdays.Columns.Add("Username", typeof (string));

                int count = 0;
                for (int i = 0; i < 7; i++)
                {
                    if (count >= 12) break;
                    BirthdaySearch search = new BirthdaySearch();
                    search.Birthdate = DateTime.Now.AddDays(i).Add(Config.Misc.TimeOffset);
                    if (search.Birthdate.Month == 1 && search.Birthdate.Day == 1) continue; // skip fake birthdays
                    UserSearchResults results = search.GetResults();
                    if (results == null || results.Usernames == null) continue;
                    foreach (string username in results.Usernames)
                    {
                        dtBirthdays.Rows.Add(new object[] {search.Birthdate, username});
                        ++count;
                    }
                }

                Cache.Insert(cacheKey, dtBirthdays, null, DateTime.Now.AddHours(1),
                             Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
            }

            if (dtBirthdays.Rows.Count != 0)
            {
                rptBirthdays.DataSource = dtBirthdays;
                rptBirthdays.DataBind();
            }
            else
                Visible = false;
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        #endregion
    }
}
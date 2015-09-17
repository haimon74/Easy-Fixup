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
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetDating.Classes;
using System.Configuration;

namespace AspNetDating.Components.Search
{
    public partial class SearchBoxContent : UserControl
    {
        protected SearchResults SearchResults;
        protected Button btnBasicSearchGo;
        protected LargeBoxStart LargeBoxStart1;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadStrings();
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

        private void LoadStrings()
        {
            CascadingDropDownCity.LoadingText = "[Loading cities...]".Translate();
            if (LargeBoxStart1 != null)
                LargeBoxStart1.Title = Lang.Trans("Quick Search");

            if (btnBasicSearchGo != null)
                btnBasicSearchGo.Text = Lang.Trans("Search");
            fbBasicSearchGo.Text = Lang.Trans("Search");
            
            var userSession = PageBase.GetCurrentUserSession();
            PopulateAgeCombos(userSession);
            PopulateGenderCombos(userSession);
            PopulateLocationControls(userSession);

            //pnlLocation.Visible = Config.Users.LocationPanelVisible;
            //pnlGender.Visible = !Config.Users.DisableGenderInformation;
            //pnlAge.Visible = !Config.Users.DisableAgeInformation;
        }

        private void btnBasicSearchGo_Click(object sender, EventArgs e)
        {
                BasicSearch search = new BasicSearch();

                if (Config.Users.RequireProfileToShowInSearch)
                {
                    search.HasAnswer = true;
                }
                else
                {
                    search.hasAnswer_isSet = false;
                }

                search.InterestedIn = (User.eGender)int.Parse(dropInterestedIn.SelectedValue);
                search.Gender = (User.eGender)int.Parse(dropGender.SelectedValue);

                try
                {
                    search.MinAge = int.Parse(comboAgeFrom.SelectedValue);
                    search.MaxAge = int.Parse(comboAgeTo.SelectedValue);
                }
                catch (ArgumentException)
                {
                }
                catch (FormatException)
                {
                }

                search.Country = Country;
                search.State = Region;
                search.City = City;
                
            if (txtUsername != null && txtUsername.Text != String.Empty)
                {
                    search.Username = txtUsername.Text;
                }
                Session["SessionSearchRequest"] = search;
                Response.Redirect("~/SearchList.aspx?loc=session");
        }

        protected void fbBasicSearchGo_Click(object sender, EventArgs e)
        {
            btnBasicSearchGo_Click(sender, null);
        }



        private bool _ageFromIsSet;
        public int AgeFrom
        {
            get { return Convert.ToInt32(this.comboAgeFrom.SelectedValue); }
            set
            {
                this.comboAgeFrom.SelectedValue = value.ToString();
                _ageFromIsSet = true;
            }
        }

        private bool _ageToIsSet;
        public int AgeTo
        {
            get { return Convert.ToInt32(this.comboAgeTo.SelectedValue); }
            set
            {
                this.comboAgeTo.SelectedValue = value.ToString();
                _ageToIsSet = true;
            }
        }

        private int _minAge;
        private int _maxAge;
        public int MinAge
        {
            get { return _minAge = (_minAge == 0 || _minAge < Config.Users.MinAge) ? Config.Users.MinAge : _minAge; }
            set { _minAge = (value == 0 || value < Config.Users.MinAge) ? Config.Users.MinAge : value; }
        }
        public int MaxAge
        {
            get
            {
                _maxAge = (_maxAge == 0 || _maxAge > Config.Users.MaxAge) ? Config.Users.MaxAge : _maxAge;
                return (_maxAge < MinAge) ? MinAge : _maxAge;
            }
            set { _maxAge = (value == 0 || value > Config.Users.MaxAge) ? Config.Users.MaxAge : value; }
        }

        public void PopulateAgeCombos(User user)
        {
            comboAgeFrom.Items.Clear();
            comboAgeTo.Items.Clear();
            for (int i = Config.Users.MinAge; i <= Config.Users.MaxAge; i++)
            {
                comboAgeTo.Items.Add(i.ToString());
                comboAgeFrom.Items.Add(i.ToString());
            }
            comboAgeFrom.SelectedValue = user != null && user.Age - 10 > MinAge ? (user.Age - 10).ToString() : MinAge.ToString();
            comboAgeTo.SelectedValue = user != null && user.Age + 10 < MaxAge ? (user.Age + 10).ToString() : MaxAge.ToString();
        }

        protected void comboAgeToSelectIndexChange(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }
        protected void comboAgeFromSelectIndexChange(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }



        private bool _lookingForGenderIsSet
        {
            get
            {
                return ViewState["lookingForGenderIsSet"] != null &&
                                      (bool)ViewState["lookingForGenderIsSet"];
            }
            set { ViewState["lookingForGenderIsSet"] = value; }
        }
        public int LookingForGender
        {
            get
            {
                return _lookingForGenderIsSet ? int.Parse(this.dropGender.SelectedValue) : 0;
            }
            set
            {
                this.dropGender.SelectedValue = value.ToString();
                _lookingForGenderIsSet = true;
            }
        }

        private bool _interestedInGenderIsSet
        {
            get
            {
                return ViewState["interestedInGenderIsSet"] != null &&
                       (bool)ViewState["interestedInGenderIsSet"];
            }
            set { ViewState["interestedInGenderIsSet"] = value; }
        }
        public int InterestedInGender
        {
            get
            {
                return _interestedInGenderIsSet ? int.Parse(this.dropInterestedIn.SelectedValue) : 0;
            }
            set
            {
                this.dropInterestedIn.SelectedValue = value.ToString();
                _interestedInGenderIsSet = true;
            }
        }

        public void PopulateGenderCombos(User user)
        {
            dropGender.Items.Clear();
            dropGender.Items.Add(
                new ListItem(Lang.Trans("Men"), ((int)Classes.User.eGender.Male).ToString()));
            dropGender.Items.Add(
                new ListItem(Lang.Trans("Women"), ((int)Classes.User.eGender.Female).ToString()));
            if (Config.Users.CouplesSupport)
            {
                dropGender.Items.Add(
                    new ListItem(Lang.Trans("Couples"), ((int)Classes.User.eGender.Couple).ToString()));
            }
            dropInterestedIn.Items.Clear();
            dropInterestedIn.Items.Add(
                new ListItem(Lang.Trans("Men"), ((int)Classes.User.eGender.Male).ToString()));
            dropInterestedIn.Items.Add(
                new ListItem(Lang.Trans("Women"), ((int)Classes.User.eGender.Female).ToString()));
            if (Config.Users.CouplesSupport)
            {
                dropInterestedIn.Items.Add(
                    new ListItem(Lang.Trans("Couples"), ((int)Classes.User.eGender.Couple).ToString()));
            }
            if (user != null)
            {
                InterestedInGender = ((int)user.Gender);
                LookingForGender = ((int)user.InterestedIn);
                bool toEnable = !(Page is Search3) || MatchmakerHelper.IsMatchmakerState ||
                    Convert.ToBoolean(ConfigurationManager.AppSettings["ENABLE_CHANGE_MY_GENDER_IN_SEARCH"]);
                dropGender.Enabled = toEnable;
                dropInterestedIn.Enabled = toEnable;
            }
            else
            {
                if (!_interestedInGenderIsSet)
                    InterestedInGender = ((int)Classes.User.eGender.Female);
                if (!_lookingForGenderIsSet)
                    LookingForGender = ((int)Classes.User.eGender.Male);
            }
        }



        public string Country
        {
            get
            {
                string result = this.dropCountry.SelectedValue;
                if (!Page.IsPostBack && this.dropCountry.SelectedValue.IsNullOrEmpty() &&
                        CascadingDropDownCountry.SelectedValue != null &&
                        CascadingDropDownCountry.SelectedValue.Trim(':') != "")
                    result = CascadingDropDownCountry.SelectedValue;
                return result;
            }
            set { CascadingDropDownCountry.SelectedValue = value; }
        }
        public string City
        {
            get
            {
                string result = this.dropCity.SelectedValue;
                if (!Page.IsPostBack && string.IsNullOrEmpty(dropCity.SelectedValue) &&
                        CascadingDropDownCity.SelectedValue != null &&
                        CascadingDropDownCity.SelectedValue.Trim(':') != "")
                    result = CascadingDropDownCity.SelectedValue;
                return result;
            }
            set { CascadingDropDownCity.SelectedValue = value; }
        }
        public string Region
        {
            get
            {
                string result = this.dropRegion.SelectedValue;
                if (!Page.IsPostBack && dropRegion.SelectedValue.IsNullOrEmpty() &&
                        CascadingDropDownState.SelectedValue != null &&
                        CascadingDropDownState.SelectedValue.Trim(':') != "")
                    result = CascadingDropDownState.SelectedValue;
                return result;
            }
            set { CascadingDropDownState.SelectedValue = value; }
        }
        public string CountriesServiceMethod { set { this.CascadingDropDownCountry.ServiceMethod = value; } }




        public void PopulateLocationControls(User user)
        {
            if (!Config.Users.LocationPanelVisible || user == null) return;

            if (dropCountry != null && user.Country.IsNotNullOrEmpty())
            {
                CascadingDropDownCountry.SelectedValue = user.Country;
            }
            if (dropRegion != null && user.State.IsNotNullOrEmpty())
            {
                CascadingDropDownState.SelectedValue = user.State;
            }
            if (dropCity != null && user.City.IsNotNullOrEmpty())
            {
                CascadingDropDownCity.SelectedValue = user.City;
            }
        }
    }
}
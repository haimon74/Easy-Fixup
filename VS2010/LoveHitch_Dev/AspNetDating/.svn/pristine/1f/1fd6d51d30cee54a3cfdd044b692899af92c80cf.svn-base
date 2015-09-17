using System;
using System.Configuration;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Components.Search
{
    public partial class SearchGenderPicker : System.Web.UI.UserControl
    {
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                PopulateCombos(((PageBase)Page).CurrentUserSession);
        }
        public void PopulateCombos(User user)
        {
            dropGender.Items.Clear();
            dropGender.Items.Add(
                new ListItem(Lang.Trans("Men"), ((int)User.eGender.Male).ToString()));
            dropGender.Items.Add(
                new ListItem(Lang.Trans("Women"), ((int)User.eGender.Female).ToString()));
            if (Config.Users.CouplesSupport)
            {
                dropGender.Items.Add(
                    new ListItem(Lang.Trans("Couples"), ((int)User.eGender.Couple).ToString()));
            }
            dropInterestedIn.Items.Clear();
            dropInterestedIn.Items.Add(
                new ListItem(Lang.Trans("Men"), ((int)User.eGender.Male).ToString()));
            dropInterestedIn.Items.Add(
                new ListItem(Lang.Trans("Women"), ((int)User.eGender.Female).ToString()));
            if (Config.Users.CouplesSupport)
            {
                dropInterestedIn.Items.Add(
                    new ListItem(Lang.Trans("Couples"), ((int)User.eGender.Couple).ToString()));
            }
            if (user != null)
            {
                InterestedInGender = ((int)user.Gender);
                LookingForGender = ((int)user.InterestedIn);
                bool toEnable = !(Page is Search2) || MatchmakerHelper.IsMatchmakerState ||
                    Convert.ToBoolean(ConfigurationManager.AppSettings["ENABLE_CHANGE_MY_GENDER_IN_SEARCH"]);
                dropGender.Enabled = toEnable;
                dropInterestedIn.Enabled = toEnable;
            }
            else
            {
                if (!_interestedInGenderIsSet)
                    InterestedInGender = ((int)User.eGender.Female);
                if (!_lookingForGenderIsSet)
                    LookingForGender = ((int)User.eGender.Male);
            }
        }
    }
}
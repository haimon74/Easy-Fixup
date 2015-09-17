using System;
using System.Web.UI;
using ezFixUp.Classes;

namespace ezFixUp.Components
{
    public partial class LocationPicker : UserControl
    {
        //private User user;

        //public User User
        //{
        //    set
        //    {
        //        user = value;
        //        if (user != null)
        //        {
        //            ViewState["Username"] = user.Username;
        //        }
        //        else
        //            ViewState["Username"] = null;
        //    }
        //    get
        //    {
        //        if (user == null
        //            && ViewState["Username"] != null)
        //            user = User.Load((string)ViewState["Username"]);
        //        return user;
        //    }
        //}

        public string Country { get { return this.dropCountry != null ? this.dropCountry.SelectedValue : null; } }
        public string City { get { return this.dropCity != null ? this.dropCity.SelectedValue : null; } }
        public string Region { get { return this.dropRegion != null ? this.dropRegion.SelectedValue : null; } }
        public string ZipCode { get { return this.txtZipCode != null ? this.txtZipCode.Text : null; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            CascadingDropDownCity.LoadingText = "[Loading cities...]".Translate();
        }

        public void PopulateControls(User user)
        {
            if (!Config.Users.LocationPanelVisible) return;

            if (dropCountry != null && user.Country.IsNotNullOrEmpty())
            {
                CascadingDropDownCountry.SelectedValue = user.Country;
                CascadingDropDownCountry.LoadingText = user.Country;
                CascadingDropDownCountry.PromptText = PageBase.PleaseChooseTranslated;
            }
            if (dropRegion != null && user.State.IsNotNullOrEmpty())
            {
                CascadingDropDownState.SelectedValue = user.State;
                CascadingDropDownState.LoadingText = user.State;
                CascadingDropDownState.PromptText = PageBase.PleaseChooseTranslated;
            }
            if (dropCity != null && user.City.IsNotNullOrEmpty())
            {
                CascadingDropDownCity.SelectedValue = user.City;
                CascadingDropDownCity.LoadingText = user.City;
                CascadingDropDownCity.PromptText = PageBase.PleaseChooseTranslated;
            }
            if (txtZipCode != null)
            {
                txtZipCode.Text = user.ZipCode;
            }
        }

        protected void SetCountry(object sender, EventArgs e)
        {
            dropCountry.Text = CascadingDropDownCountry.SelectedValue;
        }
        protected void SetState(object sender, EventArgs e)
        {
            dropRegion.Text = CascadingDropDownState.SelectedValue;
        }
        protected void SetCity(object sender, EventArgs e)
        {
            dropCity.Text = CascadingDropDownCity.SelectedValue;
        }
    }
}
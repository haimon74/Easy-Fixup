using System;
using System.Web;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using ezFixUp.Classes;
using ezFixUp.Services;

namespace ezFixUp.Mobile
{
    public partial class Register : PageBase
    {
        public Register()
        {
            RequiresAuthorization = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentUserSession != null
                && CurrentUserSession.IsAuthorized)
            {
                Response.Redirect("Home.aspx");
            }

            if (Config.Users.LocationPanelVisible)
                LoadCountries();

            if (!Page.IsPostBack)
            {
                PrepareStrings();
            }
        }

        private void PrepareStrings()
        {
            lblTitle.InnerText = Lang.Trans("Registration details");

            if (Config.Users.LocationPanelVisible)
                ShowLocation();
            else
                HideLocation();

            btnRegister.Text = Lang.Trans("Register >>");

            dropGender.Items.Add(
                new ListItem(Lang.Trans("Male"), ((int)Classes.User.eGender.Male).ToString()));
            dropGender.Items.Add(
                new ListItem(Lang.Trans("Female"), ((int)Classes.User.eGender.Female).ToString()));
            if (Config.Users.CouplesSupport)
            {
                dropGender.Items.Add(
                    new ListItem(Lang.Trans("Couple"), ((int)Classes.User.eGender.Couple).ToString()));
            }

            if (Config.Users.DisableGenderInformation)
                dropGender.SelectedValue = ((int)Classes.User.eGender.Male).ToString();

            if (Config.Users.InterestedInFieldEnabled)
            {
                pnlInterestedIn.Visible = !Config.Users.DisableGenderInformation;

                dropInterestedIn.Items.Add(
                    new ListItem(Lang.Trans("Male"), ((int)Classes.User.eGender.Male).ToString()));
                dropInterestedIn.Items.Add(
                    new ListItem(Lang.Trans("Female"), ((int)Classes.User.eGender.Female).ToString()));

                if (Config.Users.CouplesSupport)
                {
                    dropInterestedIn.Items.Add(
                        new ListItem(Lang.Trans("Couple"), ((int)Classes.User.eGender.Couple).ToString()));
                }

                if (Config.Users.DisableGenderInformation)
                    dropInterestedIn.SelectedValue = ((int)Classes.User.eGender.Male).ToString();
            }
            else
                pnlInterestedIn.Visible = false;

            if (Config.Users.InvitationCode == String.Empty)
                pnlInvitationCode.Visible = false;

            pnlGender.Visible = !Config.Users.DisableGenderInformation;
            pnlBirthdate.Visible = !Config.Users.DisableAgeInformation;

            if (Config.Users.DisableAgeInformation)
                datePicker1.SelectedDate = new DateTime(DateTime.Now.Year - Config.Users.MinAge, 1, 1);

            btnRegister.Text = Lang.Trans("Register >>");
        }

        private void LoadCountries()
        {
            if (dropCountry.Items.Count > 0) return;

            dropCountry.Items.Add("");
            foreach (CascadingDropDownNameValue value in Service.GetCountries(true))
            {
                ListItem item = new ListItem(value.name, value.value);
                item.Selected = value.isDefaultValue;
                dropCountry.Items.Add(item);
            }

            if (dropCountry.SelectedIndex > 0)
                dropCountry_SelectedIndexChanged(null, null);
        }

        private void HideLocation()
        {
            pnlCountry.Visible = false;
            pnlState.Visible = false;
            pnlZipCode.Visible = false;
            pnlCity.Visible = false;
        }

        private void ShowLocation()
        {
            pnlCountry.Visible = true;
            pnlState.Visible = true;
            pnlZipCode.Visible = true;
            pnlCity.Visible = true;
        }

        protected void dropGender_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlBirthdate2.Visible = dropGender.SelectedValue == ((int)Classes.User.eGender.Couple).ToString();
        }

        protected void dropCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            dropRegion.Items.Clear();
            dropRegion.Items.Add("");

            foreach (string region in Config.Users.GetRegions(dropCountry.SelectedValue))
            {
                if (Config.Users.ForceRegion.Trim().Length > 0)
                {
                    if (region.ToLower() != Config.Users.ForceRegion.Trim().ToLower()) continue;
                    ListItem item = new ListItem(region, region);
                    item.Selected = true;
                    dropRegion.Items.Add(item);
                }
                else if (region.Length == 0)
                {
                    ListItem item = new ListItem(Lang.Trans("All"), " ");
                    item.Selected = true;
                    dropRegion.Items.Add(item);
                }
                else
                    dropRegion.Items.Add(new ListItem(region, region));
            }

            if (dropRegion.SelectedIndex > 0)
                dropRegion_SelectedIndexChanged(null, null);
        }

        protected void dropRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            dropCity.Items.Clear();
            dropCity.Items.Add("");

            string[] cities = Config.Users.GetCities(dropCountry.SelectedValue,
                                                     dropRegion.SelectedValue.Trim());
            if (cities != null && cities.Length > 0)
            {
                foreach (string city in cities)
                {
                    bool isDefault = false;
                    if (Config.Users.ForceCity.Trim().Length > 0)
                    {
                        if (city.ToLower() != Config.Users.ForceCity.Trim().ToLower()) continue;
                        isDefault = true;
                    }
                    ListItem item = new ListItem(city, city);
                    item.Selected = isDefault;
                    dropCity.Items.Add(item);
                }
            }
            else
            {
                ListItem item = new ListItem("-", "-");
                item.Selected = true;
                dropCity.Items.Add(item);
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            #region Validate username

            try
            {
                if (txtUsername.Text.Length == 0)
                {
                    lblError.Text =
                        Lang.Trans("Please specify username!");
                    return;
                }

                if (Classes.User.IsUsernameTaken(txtUsername.Text))
                {
                    lblError.Text =
                        Lang.Trans("Username is already taken!");
                    return;
                }

                foreach (string reservedUsername in Config.Users.ReservedUsernames)
                {
                    if (reservedUsername == txtUsername.Text.ToLower())
                    {
                        lblError.Text = Lang.Trans("Username is reserved!");
                        return;
                    }
                }
            }
            catch (ArgumentException err) // Invalid username
            {
                lblError.Text = err.Message;
                return;
            }

            #endregion

            #region Validate e-mail address

            try
            {
                if (txtEmail.Text.Length == 0)
                {
                    lblError.Text =
                        Lang.Trans("Please specify e-mail address!");
                    return;
                }

                if (txtEmail.Text.ToLower().EndsWith("@mail.bg"))
                {
                    lblError.Text =
                        Lang.Trans("E-mails from mail.bg are not accepted!");
                    return;
                }

                if (Config.Users.CheckForDuplicateEmails && Classes.User.IsEmailUsed(txtEmail.Text))
                {
                    lblError.Text =
                        Lang.Trans("E-mail address is already used!");
                    return;
                }
            }
            catch (ArgumentException err) // Invalid e-mail address
            {
                lblError.Text = err.Message;
                return;
            }

            #endregion

            #region Validate passwords

            if (txtPassword.Text.Length == 0)
            {
                lblError.Text = Lang.Trans("Please specify password!");
                return;
            }
            if (txtPassword2.Text.Length == 0)
            {
                lblError.Text = Lang.Trans("Please verify password!");
                return;
            }
            if (txtPassword.Text != txtPassword2.Text)
            {
                lblError.Text = Lang.Trans("Passwords do not match!");
                return;
            }

            #endregion

            #region Validate name

            if (txtName.Text.Length == 0)
            {
                lblError.Text = Lang.Trans("Please enter your name!");
                return;
            }

            #endregion

            #region Validate gender

            if (dropGender.SelectedIndex == 0)
            {
                lblError.Text = Lang.Trans("Please select your gender!");
                return;
            }

            #endregion

            #region Validate InterestedIn

            if (Config.Users.InterestedInFieldEnabled)
            {
                if (dropInterestedIn.SelectedIndex == 0)
                {
                    lblError.Text = Lang.Trans("Please select who are you interested in!");
                    return;
                }
            }

            #endregion

            #region Validate birthdate1

            if (!datePicker1.ValidDateEntered)
            {
                lblError.Text = Lang.Trans("Please select your birthdate!");
                return;
            }

            #endregion

            #region Validate birthdate2

            if ((User.eGender)Convert.ToInt32(dropGender.SelectedValue) == Classes.User.eGender.Couple
                && !datePicker2.ValidDateEntered)
            {
                lblError.Text = Lang.Trans("Please select your birthdate!");
                return;
            }

            #endregion

            #region Validate agreement

            if (!cbAgreement.Checked)
            {
                lblError.Text = Lang.Trans("You must accept the agreement to proceed!");
                return;
            }

            #endregion

            #region Validate location

            if (Config.Users.LocationPanelVisible)
            {
                if (dropCountry != null && dropCountry.SelectedValue == String.Empty)
                {
                    lblError.Text = Lang.Trans("Please select your country!");
                    return;
                }

                if (dropRegion.Items.Count > 1 && dropRegion.SelectedValue == "")
                {
                    lblError.Text = Lang.Trans("Please select your state!");
                    return;
                }

                if (txtZipCode != null && txtZipCode.Text == String.Empty)
                {
                    lblError.Text = Lang.Trans("Please enter your Zip/Postal Code");
                    return;
                }

                if (dropCity != null && dropCity.SelectedValue == "")
                {
                    lblError.Text = Lang.Trans("Please select your city!");
                    return;
                }
            }

            #endregion

            #region Validate Invitation Code

            if (Config.Users.InvitationCode != String.Empty)
            {
                if (Config.Users.InvitationCode != txtInvitationCode.Text)
                {
                    lblError.Text = Lang.Trans("Invalid Invitation Code!");
                    return;
                }
            }

            #endregion

            #region Validate IP address

            if (Properties.Settings.Default.BannedCountries.Count > 0)
            {
                foreach (string countryCode in Properties.Settings.Default.BannedCountries)
                {
                    if (IPToCountry.GetCountry(Request.UserHostAddress) == countryCode.Trim())
                    {
                        lblError.Text = Lang.Trans("Registration is not allowed for your country!");
                        return;
                    }
                }
            }

            #endregion

            try
            {
                User newUser = new User(txtUsername.Text);

                #region Save location

                if (Config.Users.LocationPanelVisible)
                {
                    if (dropCountry != null)
                    {
                        newUser.Country = dropCountry.SelectedValue;
                    }
                    if (dropRegion != null)
                    {
                        newUser.State = dropRegion.SelectedValue;
                    }
                    if (txtZipCode != null)
                    {
                        newUser.ZipCode = txtZipCode.Text;
                    }
                    if (dropCity != null)
                    {
                        newUser.City = dropCity.SelectedValue;
                    }

                    Location loc = Config.Users.GetLocation(newUser.Country, newUser.State, newUser.City);

                    if (loc != null)
                    {
                        newUser.Longitude = loc.Longitude;
                        newUser.Latitude = loc.Latitude;
                    }
                }

                #endregion

                newUser.Password = txtPassword.Text;
                newUser.Email = txtEmail.Text;
                newUser.Name = txtName.Text;
                newUser.Gender = (User.eGender)Convert.ToInt32(dropGender.SelectedValue);
                newUser.Birthdate = datePicker1.SelectedDate;
                newUser.LanguageId = LanguageId;
                if (newUser.Gender == Classes.User.eGender.Couple)
                {
                    newUser.Birthdate2 = datePicker2.SelectedDate;
                }

                if (Config.Users.InterestedInFieldEnabled)
                {
                    newUser.InterestedIn = (User.eGender)Convert.ToInt32(dropInterestedIn.SelectedValue);
                }
                else
                {
                    if (Config.Users.DisableGenderInformation)
                        newUser.InterestedIn = Classes.User.eGender.Male;
                    else
                        newUser.InterestedIn = newUser.Gender == Classes.User.eGender.Male
                                                   ?
                                                       Classes.User.eGender.Female
                                                   : Classes.User.eGender.Male;
                }
                newUser.ReceiveEmails = Config.Users.EmailNotificationsDefault;

                #region Set and Delete invitedBy cookie

                if (Request.Cookies["invitedBy"] != null)
                {
                    newUser.InvitedBy = Server.HtmlEncode(Request.Cookies["invitedBy"].Value);

                    HttpCookie cookie = new HttpCookie("invitedBy");
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie);
                }

                #endregion

                #region Set and Delete affiliateID cookie

                if (Request.Cookies["affiliateID"] != null)
                {
                    newUser.AffiliateID = Convert.ToInt32(Server.HtmlEncode(Request.Cookies["affiliateID"].Value));

                    HttpCookie cookie = new HttpCookie("affiliateID");
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie);
                }

                #endregion

                newUser.Create(Request.UserHostAddress);

                if (Config.Users.SmsConfirmationRequired)
                {
                    Response.Redirect("~/SmsConfirm.aspx?username=" + newUser.Username);
                    return;
                }

                if (Config.Users.AutoActivateUsers)
                {
                    if (Config.Users.SendWelcomeMessage)
                    {
                        Message.SendWelcomeMessage(newUser);
                    }

                    UserSession userSession = null;
                    try
                    {
                        userSession = new UserSession(newUser.Username);
                        ((PageBase)Page).CurrentUserSession = userSession;
                        MatchmakerHelper.CurrentUsername = userSession.Username;
                        userSession.Authorize(Session.SessionID);
                    }
                    catch (Exception err)
                    {
                        StatusPageMessage = err.Message;
                    }

                    ((PageBase)Page).CurrentUserSession = userSession;

//                    Response.Redirect("Home.aspx");
                    Response.Redirect("ManageProfile.aspx");
                }
                else
                    StatusPageMessage = Lang.Trans
                        ("<b>Your account has been created successfully!</b><br><br>"
                         + "You will receive a confirmation e-mail shortly. In order "
                         + "to finish your registration you'll have to click the "
                         + "activation link in the e-mail.");
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (ArgumentException err)
            {
                lblError.Text = err.Message;
                return;
            }
            catch (Exception err)
            {
                lblError.Text = Lang.Trans
                    ("Unknown error has occured while trying to create "
                     + "your account! Please try again later.");
                Log(err);
                return;
            }
            Response.Redirect("Default.aspx");
        }

        protected void dropGender_SelectedIndexChanged1(object sender, EventArgs e)
        {
            pnlBirthdate2.Visible = dropGender.SelectedValue == ((int)Classes.User.eGender.Couple).ToString();
        }
    }
}

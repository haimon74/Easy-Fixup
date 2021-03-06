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
using System.Globalization;
using System.IdentityModel.Claims;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using AspNetDating.Classes;
using AspNetDating.Classes.MySpace;
using AspNetDating.Components;
using Facebook;
using Microsoft.IdentityModel.TokenProcessor;
using Photo = AspNetDating.Classes.Photo;


namespace AspNetDating
{
    public partial class Register : PageBase
    {
        protected DatePicker datePicker1, datePicker2;
        protected DropDownList dropCity;
        protected MultiView multiViewButtons;
        protected View view1, view2;
        protected LargeBoxStart LargeBoxStart1;
        //public string pleaseChooseTranslated = "--- " + Lang.Translate("please choose") + " ---";

        private string MySpaceID
        {
            get
            {
                return ViewState["MySpaceID"] as string;
            }

            set
            {
                ViewState["MySpaceID"] = value;
            }
        }

        private long? FacebookID
        {
            get
            {
                return ViewState["FacebookID"] as long?;
            }

            set
            {
                ViewState["FacebookID"] = value;
            }
        }

        private string PrimaryPhotoURL
        {
            get
            {
                return Session["PrimaryPhotoURL"] as string;
            }

            set
            {
                Session["PrimaryPhotoURL"] = value;
            }
        }

        private long[] FacebookFriendIDs
        {
            get
            {
                return ViewState["FacebookFriendIDs"] as long[];
            }

            set
            {
                ViewState["FacebookFriendIDs"] = value;
            }
        }

        private string[] FriendIDs
        {
            get
            {
                return ViewState["FriendIDs"] as string[];
            }

            set
            {
                ViewState["FriendIDs"] = value;
            }
        }

        public Register()
        {
            RequiresAuthorization = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["IsRegistrationFormValidated"] = null;
            if (CurrentUserSession != null
                  && CurrentUserSession.IsAuthorized)
            {
                Response.Redirect(MatchmakerHelper.CurrentHomePage);
            }
            asyncImageUpload.UploadedComplete += new EventHandler<AsyncFileUploadEventArgs>(asyncImageUpload_UploadedComplete);

            //if (!Page.ClientScript.IsStartupScriptRegistered(Page.GetType(),"newscript"))
            //{
            //    var sb = new StringBuilder();
            //    sb.Append("$('.reg_ddl').hover(function() { alert('play mm'); });");
            //    sb.Append("$('img').hover(function() { alert('right'); });");
            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "newscript", sb.ToString(), true);
            //}
            if (!Page.IsPostBack)
            {
                PrepareStrings();
                if (Request.Params["token"] == "1")
                    LoadTokenData();
                string fbParamVal = Request.QueryString["facebook"];
                if (Config.Misc.EnableMySpaceIntegration &&
                      !String.IsNullOrEmpty(Context.Request.QueryString["myspace"]))
                    PopulateUserDataUsingMySpace();
                var referrer = Context.Request.UrlReferrer;
                if (Config.Misc.EnableFacebookIntegration &&
                    ((referrer != null && (referrer.Query.Contains("facebook=1") ||
                                           referrer.AbsolutePath.ToLower().Contains("facebook")))
                    || (fbParamVal.IsNotNullOrEmpty() && int.Parse(fbParamVal) == 1)))
                {
                    if (!String.IsNullOrEmpty(Context.Request.QueryString["login"]))
                        divLogin.Visible = true;

                    PopulateUserDataUsingFacebook();

                    if (FacebookHelper.IsCurrentSessionConnected)
                    {
                        trUsername.Visible = false;
                        trPassword1.Visible = false;
                        trPassword2.Visible = false;
                        trCaptcha.Visible = false;
                    }
                }
            }
        }

        private void LoadTokenData()
        {
            Token token = Session["Token"] as Token;
            if (token == null) return;

            if (Classes.User.GetUsernameByTokenUniqueId(token.UniqueID) != null)
            {
                ResponseEndWithErrorMessage("The presented information card is already used for another account!");
                return;
            }

            CascadingDropDownCountry.SelectedValue = token.Claims[ClaimTypes.Country];
            CascadingDropDownState.SelectedValue = token.Claims[ClaimTypes.StateOrProvince];
            CascadingDropDownCity.SelectedValue = token.Claims[ClaimTypes.Locality];
            //txtZipCode.Text = token.Claims[ClaimTypes.PostalCode];
            txtName.Text = token.Claims[ClaimTypes.GivenName] + " " + token.Claims[ClaimTypes.Surname];
            dropGender.SelectedValue = ((int)Enum.Parse(typeof(User.eGender), token.Claims[ClaimTypes.Gender])).ToString();
            //dropStatus.SelectedValue = ((int)Enum.Parse(typeof(User.eGender), token.Claims[ClaimTypes.Gender])).ToString();
            BirthdayPicker1.CurrentText = token.Claims[ClaimTypes.DateOfBirth];
            //txtDatePicker.Text = token.Claims[ClaimTypes.DateOfBirth];
            datePicker1.SelectedDate = BirthdayPicker1.SelectedDate;
            txtEmail.Text = token.Claims[ClaimTypes.Email];
            ViewState["TokenUniqueId"] = token.UniqueID;
            divCardSpace.Visible = false;
        }

        private void PrepareStrings()
        {
            LargeBoxStart1.Title = Lang.Trans("Registration details");

            if (Config.Users.LocationPanelVisible)
                ShowLocation();
            else
                HideLocation();

            CascadingDropDownCountry.PromptText = PleaseChooseTranslated;
            CascadingDropDownState.PromptText = PleaseChooseTranslated;
            CascadingDropDownCity.PromptText = PleaseChooseTranslated;
            lblUsername.Text = Lang.Trans("Username");
            lblEmail.Text = Lang.Trans("E-Mail");
            lblPassword.Text = Lang.Trans("Password");
            lblPassword2.Text = Lang.Trans("Confirm password");
            lblName.Text = Lang.Trans("Full name");
            lblGender.Text = Lang.Trans("Gender");
            lblBirthdate.Text = Lang.Trans("Birth date");
            lblInterestedIn.Text = Lang.Trans("Interested in");
            lblInvitationCode.Text = Lang.Trans("Invitation Code");
            btnLogin.Text = Lang.Trans("Login");
            lblPhoto.Text = Lang.Trans("Primary Photo");
            cbAgreement.Text = Lang.Trans("I have read and accept the ") +
                "<span><a href='Agreement.aspx' target='_new'>" +
                  Lang.Trans("terms & conditions") + "</a> </span><span>" +
                  Lang.Trans(" and the ") + "</span><span><a href='Privacy.aspx' target='_new'>" +
                  Lang.Trans("privacy policy") + "</a></span>"
                ;
            dropGender.Items.Add(
                  new ListItem(Lang.Trans("Female"), ((int)Classes.User.eGender.Female).ToString()));
            dropGender.Items.Add(
                    new ListItem(Lang.Trans("Male"), ((int)Classes.User.eGender.Male).ToString()));
            if (Config.Users.CouplesSupport)
            {
                dropGender.Items.Add(
                      new ListItem(Lang.Trans("Couple"), ((int)Classes.User.eGender.Couple).ToString()));
            }

            if (true)
            {//Config.Users.InterestedInFieldEnabled) {
                trInterestedIn.Visible = true;

                //dropInterestedIn.Items.Add(
                //        new ListItem(Lang.Trans("All"), ((int)Classes.User.eGenderSearch.All).ToString()));
                dropInterestedIn.Items.Add(
                      new ListItem(Lang.Trans("Male"), ((int)Classes.User.eGender.Male).ToString()));
                dropInterestedIn.Items.Add(
                      new ListItem(Lang.Trans("Female"), ((int)Classes.User.eGender.Female).ToString()));

                if (Config.Users.CouplesSupport)
                {
                    dropInterestedIn.Items.Add(
                          new ListItem(Lang.Trans("Couple"), ((int)Classes.User.eGender.Couple).ToString()));
                }
                dropInterestedIn.Items.Add(
                        new ListItem(Lang.Trans("Matchmaking"), ((int)Classes.User.eGender.Matchmaker).ToString()));

                if (Config.Users.DisableGenderInformation)
                    dropInterestedIn.SelectedValue = ((int)Classes.User.eGenderSearch.All).ToString();
            }
            else trInterestedIn.Visible = false;

            if (Config.Users.InvitationCode == String.Empty)
                trInvitationCode.Visible = false;

            if (Config.Misc.EnableCaptcha && !FacebookHelper.IsCurrentSessionConnected)
            {
                lblCaptcha.Text = Lang.Trans("Enter the code shown above");
                lblAntiSpam.Text = Lang.Trans("Spam protection");
                trCaptcha.Visible = true;
            }

            pnlGender.Visible = !Config.Users.DisableGenderInformation;
            pnlBirthdate.Visible = !Config.Users.DisableAgeInformation;

            if (Config.Users.DisableAgeInformation)
            {
                datePicker1.SelectedDate = new DateTime(DateTime.Now.Year - Config.Users.MinAge, 1, 1);
                //txtDatePicker.Text = datePicker1.SelectedDate.ToString();
                BirthdayPicker1.CurrentText = datePicker1.SelectedDate.ToString();
            }

            divCardSpace.Visible = Config.Users.EnableCardSpace;
            divMySpace.Visible = Config.Misc.EnableMySpaceIntegration;
            if (Config.Misc.EnableMySpaceIntegration)
                btnUseMySpace.AlternateText = Lang.Trans("Import from MySpace");
            divFacebook.Visible = Config.Misc.EnableFacebookIntegration;

            btnRegister.Text = Lang.Trans("Register") + " � ";
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        #endregion

        public string LastAsyncFileUploadLocation
        {
            get { return (string)Session["LastAsyncFileUploadLocation"] ?? "Images/default_male.png"; }
            set { Session["LastAsyncFileUploadLocation"] = value ?? "Images/default_male.png"; }
        }
        void asyncImageUpload_UploadedComplete(object sender, AsyncFileUploadEventArgs e)
        {
            string fileRelativeLocation = "/AsyncFileUpload/Uploads/" +
                                        Path.GetFileName(e.FileName);
            string LastAsyncFileUploadLocation = MapPath("~" + fileRelativeLocation);
            if (File.Exists(LastAsyncFileUploadLocation))
                File.Delete(LastAsyncFileUploadLocation);
            asyncImageUpload.SaveAs(LastAsyncFileUploadLocation);
            PrimaryPhotoURL = Config.Urls.Home + fileRelativeLocation;
        }
        protected void asyncUploadPhoto_OnClientUploadCompleted(object sender, EventArgs e)
        {
            //Response.BinaryWrite(asyncImageUpload.FileBytes);
        }
        protected void ResponseEndWithErrorMessage(string message)
        {
            lblError.Text = Lang.Trans(message);
            lblError.Focus();
            this.UpdatePanelRegister.Update();
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {

            if (!FacebookHelper.IsCurrentSessionConnected)
            {
                #region Validate username

                Session["IsRegistrationFormValidated"] = false;

                try
                {
                    if (txtUsername.Text.Length < 6)
                    {
                        ResponseEndWithErrorMessage("Please specify username, at list 6 alphanumeric characters!");
                        return;
                    }

                    if (Classes.User.IsUsernameTaken(txtUsername.Text))
                    {
                        ResponseEndWithErrorMessage("Username is already taken!");
                        return;
                    }

                    foreach (string reservedUsername in Config.Users.ReservedUsernames)
                    {
                        if (reservedUsername == txtUsername.Text.ToLower())
                        {
                            ResponseEndWithErrorMessage("Username is reserved!");
                            return;
                        }
                    }
                }
                catch (ArgumentException err) // Invalid username
                {
                    ResponseEndWithErrorMessage(err.Message);
                    return;
                }

                #endregion

                #region Validate passwords

                if (txtPassword.Text.Length < 6)
                {
                    ResponseEndWithErrorMessage("Please specify password, at leaset 6 alphanumeric characters!");
                    return;
                }
                if (txtPassword2.Text.Length == 0)
                {
                    ResponseEndWithErrorMessage("Please verify password!");
                    return;
                }
                if (txtPassword.Text != txtPassword2.Text)
                {
                    ResponseEndWithErrorMessage("Passwords do not match!");
                    return;
                }

                #endregion

                #region Validate captcha

                if (Config.Misc.EnableCaptcha &&
                        (Session["Captcha_RandomCode"] == null || (string)Session["Captcha_RandomCode"] != txtCaptcha.Text))
                {
                    ResponseEndWithErrorMessage("Invalid verification code!");
                    return;
                }

                #endregion
            }
            else
            {
                txtUsername.Text = FacebookHelper.CurrentSessionUserID.ToString();
            }
            #region Validate e-mail address

            try
            {
                if (txtEmail.Text.Length == 0)
                {
                    ResponseEndWithErrorMessage("Please specify e-mail address!");
                    return;
                }

                if (txtEmail.Text.ToLower().EndsWith("@mail.bg"))
                {
                    ResponseEndWithErrorMessage("E-mails from mail.bg are not accepted!");
                    return;
                }

                if (Config.Users.CheckForDuplicateEmails && Classes.User.IsEmailUsed(txtEmail.Text))
                {
                    ResponseEndWithErrorMessage("E-mail address is already used!");
                    return;
                }
            }
            catch (ArgumentException err) // Invalid e-mail address
            {
                ResponseEndWithErrorMessage(err.Message);
                return;
            }

            #endregion

            #region Validate name

            if (txtName.Text.Length == 0)
            {
                ResponseEndWithErrorMessage("Please enter your name!");
                return;
            }

            #endregion

            #region Validate gender

            if (dropGender.SelectedIndex < 0)
            {
                ResponseEndWithErrorMessage("Please select your gender!");
                return;
            }
            #endregion

            #region Validate InterestedIn

            if (Config.Users.InterestedInFieldEnabled)
            {
                if (dropInterestedIn.SelectedIndex < 0)
                {
                    ResponseEndWithErrorMessage("Please select who are you interested in!");
                    return;
                }
            }

            #endregion

            #region Validate birthdate1
            //IFormatProvider culture = new CultureInfo("he-IL", true);
            //string dateFormat = "dd/MM/yyyy";
            //DateTime parsedDate;
            //if (DateTime.TryParseExact(txtDatePicker.Text, dateFormat, culture, DateTimeStyles.None, out parsedDate))
            //    datePicker1.SelectedDate = parsedDate;
            datePicker1.SelectedDate = BirthdayPicker1.SelectedDate;
            if (!datePicker1.ValidDateEntered)
            {
                ResponseEndWithErrorMessage("Please select valid birthdate!");
                return;
            }

            #endregion

            //#region Validate birthdate2
            //if((User.eGender)Convert.ToInt32(dropGender.SelectedValue) == Classes.User.eGender.Couple
            //        && !datePicker2.ValidDateEntered) 
            //{
            //    lblError.Text = Lang.Trans("Please select valid birthdate") + "!";
            //    return;
            //}
            //#endregion

            #region Validate agreement

            if (!cbAgreement.Checked)
            {
                ResponseEndWithErrorMessage("You must accept the agreement to proceed!");
                return;
            }

            #endregion

            #region Validate location

            if (Config.Users.LocationPanelVisible)
            {
                if (dropCountry != null && dropCountry.SelectedValue == String.Empty)
                {
                    ResponseEndWithErrorMessage("Please select your country!");
                    return;
                }
                //if (txtZipCode != null && txtZipCode.Text == String.Empty)
                //{
                //    lblError.Text = Lang.Trans("Please enter your Zip/Postal Code");
                //    return;
                //}

                //ifA B                                                                                                                                                                                                                                                          VVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVV(dropRegion.Items.Count >= 1 && dropRegion.SelectedValue == "") {
                //    ResponseEndWithErrorMessage("Please select your state!");
                //    return;
                //}
                //if(dropCity != null && dropCity.SelectedValue == "") {
                //    ResponseEndWithErrorMessage("Please select your city!");
                //    return;
                //}
            }

            #endregion

            #region Validate Invitation Code

            if (Config.Users.InvitationCode != String.Empty)
            {
                if (Config.Users.InvitationCode != txtInvitationCode.Text)
                {
                    ResponseEndWithErrorMessage("Invalid Invitation Code!");
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
                        ResponseEndWithErrorMessage("Registration is not allowed for your country!");
                        return;
                    }
                }
            }

            #endregion

            Session["IsRegistrationFormValidated"] = true;

            var postRegistrationPopup = @"<div class='PostRegistrationPopupBg' >
                                        <table border='0' cellpadding='0' cellspacing='0' width='100%'>
                                            <tr style='height:40px;width:100%;'>
                                                <td></td>
                                            </tr>
                                            <tr style='height:30px;width:100%;'>
                                                <td></td>
                                            </tr>
                                            <tr style='height:55px;width:100%;'>
                                                <td>
                                                    <div class='fl' style='width:220px;height:50px;margin-left:40px;cursor:pointer;' 
                                                        onclick=""window.location='ManageProfile.aspx'""></div>
                                                    <div class='fl' style='width:200px;height:50px;margin-left:20px;cursor:pointer;'
                                                        onclick=""window.location='Home.aspx'""></div>
                                                </td>
                                            </tr>
                                            <tr style='height:30px;width:100%;'>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </div>";
            //postRegistrationPopup = "";
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
                    //if (txtZipCode != null)
                    //{
                    //    newUser.ZipCode = txtZipCode.Text;
                    //}
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

                newUser.Email = txtEmail.Text;
                newUser.Name = txtName.Text;
                var pass = (-DateTime.Now.Ticks).ToString();
                if (pass.Length > 20) pass = pass.Substring(0, 20);
                newUser.Password = FacebookHelper.IsCurrentSessionConnected
                                        ? pass : txtPassword.Text;
                newUser.Gender = (User.eGender)Convert.ToInt32(dropGender.SelectedValue);
                datePicker1.SelectedDate = BirthdayPicker1.SelectedDate;// DateTime.Parse(txtDatePicker.Text);
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
                                           ? Classes.User.eGender.Female
                                           : Classes.User.eGender.Male;
                }
                newUser.ReceiveEmails = Config.Users.EmailNotificationsDefault;
                if (ViewState["TokenUniqueId"] is string)
                    newUser.TokenUniqueId = (string)ViewState["TokenUniqueId"];

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

                if (Config.Misc.EnableMySpaceIntegration)
                    newUser.MySpaceID = MySpaceID;

                if (Config.Misc.EnableFacebookIntegration)
                    newUser.FacebookID = FacebookID;

                newUser.Credits = 181818;

                newUser.Create(Request.UserHostAddress);
                StorePrimaryPhoto(newUser);
                if (File.Exists(LastAsyncFileUploadLocation))
                    File.Delete(LastAsyncFileUploadLocation);

                if (Config.Users.SmsConfirmationRequired)
                {
                    Response.Redirect("SmsConfirm.aspx?username=" + newUser.Username);
                    return;
                }

                if (Config.Users.AutoActivateUsers ||
                        (/*FacebookEmail == txtEmail.Text &&*/ IsFacebookVerified))
                {

                    if (Config.Users.SendWelcomeMessage)
                    {
                        Message.SendWelcomeMessage(newUser);
                    }

                    //StatusPageMessage = Lang.Trans("<b>Your account has been created successfully!</b><br>" +
                    //                                   "Please click on the link below to edit your profile.<br><br>");

                    //NameValueCollection link = new NameValueCollection();
                    //link.Add(Config.Urls.Home + "/ManageProfile.aspx", Config.Urls.Home + "/ManageProfile.aspx");
                    //StatusPageLink = link;
                    //((PageBase)Page).StatusPageLinkSkindId = "";
                    //((PageBase)Page).StatusPageLinkText = Config.Urls.Home + "/ManageProfile.aspx";
                    //((PageBase)Page).StatusPageLinkURL = Config.Urls.Home + "/ManageProfile.aspx";

                    UserSession userSession = null;
                    try
                    {
                        userSession = new UserSession(newUser.Username);
                        ((PageBase)Page).CurrentUserSession = userSession;
                        MatchmakerHelper.CurrentUsername = userSession.Username;
                        userSession.Authorize(Session.SessionID);

                        //if(Config.Misc.EnableMySpaceIntegration ||
                        if (Config.Misc.EnableFacebookIntegration)
                        {
                            FacebookHelper.AddFriends(newUser, 0, HttpContext.Current, null);
                        }


                        #region Autojoin to group

                        Group[] autojoinGroups = Group.Fetch(true);

                        if (autojoinGroups.Length > 0)
                        {
                            var groups =
                                  autojoinGroups.Where(
                                        g => g.Approved &&
                                        (g.AutojoinCountry == null || g.AutojoinCountry == userSession.Country) &&
                                        (g.AutojoinRegion == null || g.AutojoinRegion == userSession.State) &&
                                        (g.AutojoinCity == null || g.AutojoinCity == userSession.City));
                            foreach (Group group in groups)
                            {
                                GroupMember groupMember = new GroupMember(group.ID, userSession.Username);
                                groupMember.Active = true;
                                groupMember.Type = GroupMember.eType.Member;
                                groupMember.Save();
                                group.ActiveMembers++;
                                group.Save();
                            }
                        }

                        #endregion

                        //Response.Redirect("ManageProfile.aspx");
                        //multiViewButtons.ActiveViewIndex = 1;
                        //return;
                        StatusPageMessage = Lang.Trans("<b>Your account has been created successfully!</b><br><br>")
                                            + postRegistrationPopup;

                    }
                    catch (Exception err)
                    {
                        StatusPageMessage = err.Message;
                    }
                }
                else
                    StatusPageMessage = Lang.Trans
                                            ("<b>Your account has been created successfully!</b><br><br>"
                                             + "You will receive a confirmation e-mail shortly. In order "
                                             + "to finish your registration you'll have to click the "
                                             + "activation link in the e-mail.")
                                        + postRegistrationPopup;
                if (newUser.InterestedIn == Classes.User.eGender.Matchmaker)
                    Response.Redirect("Home_mm.aspx");
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (ArgumentException err)
            {
                ResponseEndWithErrorMessage(err.Message);
                return;
            }
            catch (Exception err)
            {
                ResponseEndWithErrorMessage("Unknown error has occured while trying to create your account! Please try again later.");
                Log(err);
                return;
            }
            Response.Redirect("ShowStatus.aspx");
        }

        protected void cbAgreement_CheckedChanged(object sender, EventArgs e)
        {
            //            txtPassword.Attributes.Add("value", txtPassword.Text);
            //            txtPassword2.Attributes.Add("value", txtPassword.Text);
            //btnRegister. = ((CheckBox) sender).Checked;
        }

        private void HideLocation()
        {
            if (trCountry != null)
            {
                trCountry.Visible = false;
            }
            if (trState != null)
            {
                trState.Visible = false;
            }
            //if (trZipCode != null)
            //{
            //    trZipCode.Visible = false;
            //}
            if (trCity != null)
            {
                trCity.Visible = false;
            }
        }

        private void ShowLocation()
        {
            if (trCountry != null)
            {
                trCountry.Visible = true;
            }
            if (trState != null)
            {
                trState.Visible = true;
            }
            //if (trZipCode != null)
            //{
            //    trZipCode.Visible = true;
            //}
            if (trCity != null)
            {
                trCity.Visible = true;
            }
        }

        protected void btnUseMySpace_Click(object sender, ImageClickEventArgs e)
        {
            PopulateUserDataUsingMySpace();
        }

        //protected void UseMySpace_Click(object sender, EventArgs e)
        //{
        //    PopulateUserDataUsingMySpace();
        //}

        private void StorePrimaryPhoto(User user)
        {
            if (PrimaryPhotoURL != null)
            {
                var request = (HttpWebRequest)WebRequest.Create(PrimaryPhotoURL);
                request.Method = "GET";
                using (var stream = request.GetResponse().GetResponseStream())
                {
                    Photo photo = new Photo();
                    System.Drawing.Image image;
                    try
                    {
                        image = System.Drawing.Image.FromStream(stream);
                        photo.Image = image;

                        photo.Primary = true;
                        photo.Name = String.Empty;
                        photo.Description = String.Empty;
                        photo.User = user;
                        photo.Save(true);
                        Photo.SetPrimary(user.Username, photo);
                        photo.Image.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Log(ex);
                    }
                }
            }
        }

        public void AddFriends(User user)
        {

            var usernames = new List<string>();

            //TODO: handle my space friends here 
            //if(FriendIDs != null && FriendIDs.Length > 0) {
            //    usernames.AddRange(Classes.User.FetchUsernamesByMySpaceID(FriendIDs));
            //}

            if (FacebookFriendIDs != null && FacebookFriendIDs.Length > 0)
            {
                var oldUsernames = new List<string>();
                oldUsernames.AddRange(Classes.User.FetchUsernamesByFacebookID(FacebookFriendIDs));
                var res = new UserSearchResults { Usernames = oldUsernames.ToArray() };
                var users = res.Get().ToList();
                var friends = FacebookFriendIDs.ToList();
                var newUsers =
                    friends.Where(id => users.Where(u => u.FacebookID == id).FirstOrDefault() == null)
                           .Select(id => new User(id.ToString())
                                             {
                                                 Active = false,
                                                 FacebookID = id
                                             });
                foreach (var newUser in newUsers)
                {
                    newUser.Create("127.0.0.1");
                }
                usernames.AddRange(Classes.User.FetchUsernamesByFacebookID(FacebookFriendIDs));
            }

            user.AddBulkToFacebookIdFriends(usernames.ToArray());

            foreach (var username in usernames)
            {
                try
                {
                    User friend = Classes.User.Load(username);
                    friend.AddToFriends(user.Username);
                }
                catch (NotFoundException)
                {
                }
            }
        }

        private void PopulateUserDataUsingMySpace()
        {
            UserData userData = null;
            Uri uri = Context.Request.Url;

            if (Context.Request.QueryString["myspace"] == null)
                uri = new Uri(uri + "?myspace=1");

            try
            {
                userData = DataAvailability.GetUserData(Context, uri);
            }
            catch (AuthorizationRequiredException authex)
            {
                Response.Redirect(authex.AuthorizationUri.AbsoluteUri);
            }
            catch (OAuth.Net.Common.OAuthRequestException)
            {
                DataAvailability.RevokeAccess(Context);
                Response.Redirect("Register.aspx?myspace=1");
            }

            if (userData != null)
            {
                if (!String.IsNullOrEmpty(userData.PrimaryEmail))
                    txtEmail.Text = userData.PrimaryEmail;

                if (userData.Name != null)
                {
                    if (!String.IsNullOrEmpty(userData.Name.GivenName))
                        txtName.Text = userData.Name.GivenName;

                    if (!String.IsNullOrEmpty(userData.Name.FamilyName))
                        txtName.Text += " " + userData.Name.FamilyName;
                }

                if (!String.IsNullOrEmpty(userData.NickName))
                    txtUsername.Text = userData.NickName;

                if (userData.Gender.HasValue)
                {
                    dropGender.SelectedValue = ((int)userData.Gender).ToString();

                    switch (userData.Gender)
                    {
                        case Classes.User.eGender.Male:
                            dropInterestedIn.SelectedValue = ((int)Classes.User.eGender.Female).ToString();
                            break;
                        case Classes.User.eGender.Female:
                            dropInterestedIn.SelectedValue = ((int)Classes.User.eGender.Male).ToString();
                            break;
                    }
                }

                if (userData.DateOfBirth.HasValue)
                {
                    datePicker1.SelectedDate = userData.DateOfBirth.Value;
                    //txtDatePicker.Text = datePicker1.SelectedDate.ToString();
                    BirthdayPicker1.CurrentText = datePicker1.SelectedDate.ToString();
                }

                string primaryPhotoURL = userData.PrimaryPhotoURL;

                if (primaryPhotoURL != null && !primaryPhotoURL.Contains("no_pic"))
                    PrimaryPhotoURL = primaryPhotoURL;

                MySpaceID = userData.ID;

                #region get friends

                string[] friendIDs = null;
                try
                {
                    friendIDs = DataAvailability.GetFriends(Context, uri);
                }
                catch (AuthorizationRequiredException authex)
                {
                    Response.Redirect(authex.AuthorizationUri.AbsoluteUri);
                }

                if (friendIDs != null && friendIDs.Length > 0)
                {
                    FriendIDs = friendIDs;
                }
                #endregion

                divCardSpace.Visible = false;
                divMySpace.Visible = false;
                divFacebook.Visible = false;
            }
        }

        private void PopulateUserDataUsingFacebook()
        {
            LoveHitchFacebookApp facebook = null;
            if (Context.Session["facebook"] != null)
            {
                facebook = (LoveHitchFacebookApp)Context.Session["facebook"];
            }
            else if ((Context.Session["facebookAccessToken"] != null) &&
                     (((String)Context.Session["facebookAccessToken"]).Length > 0))
            {
                facebook = new LoveHitchFacebookApp((String)Context.Session["facebookAccessToken"]);
            }
            else if (Request.QueryString["access_token"] != null && Request.QueryString["access_token"].Length > 0)
            {
                facebook = new LoveHitchFacebookApp(Request.QueryString["access_token"]);
            }
            else
            {
                facebook = new LoveHitchFacebookApp();
            }

            Facebook.JsonObject user;
            long id;

            if (facebook.AppSecret == null)
            {
                facebook.AppSecret = Properties.Settings.Default.Facebook_Secret_Key;
            }
            try
            {
                user = facebook.Api("/me") as Facebook.JsonObject;

                if (user != null && user["id"] != null
                    && long.TryParse((String)user["id"], out id) && id > 0)
                {
                    FacebookID = id;

                    #region log on the user if she is already registered

                    string[] usernames = Classes.User.FetchUsernamesByFacebookID(new[] { facebook.UserId });
                    if (usernames.Length > 0)
                    {
                        UserSession userSession;
                        try
                        {
                            userSession = new UserSession(usernames[0]);
                            Classes.User.AuthorizeByFacebookID(facebook.UserId);
                            ((PageBase)Page).CurrentUserSession = userSession;
                            MatchmakerHelper.CurrentUsername = userSession.Username;
                            userSession.Authorize(Session.SessionID);
                        }
                        catch (NotFoundException)
                        {
                            goto populatedate;
                        }
                        catch (AccessDeniedException err)
                        {
                            IDictionary<string, object> parameters = new Dictionary<string, object>();
                            StatusPageMessage = err.Message;
                            parameters["next"] = Config.Urls.Home + "/ShowStatus.aspx";
                            Response.Redirect(facebook.GetLogoutUrl(parameters).ToString());
                            return;
                        }
                        catch (SmsNotConfirmedException)
                        {
                            Response.Redirect("SmsConfirm.aspx?username=" + usernames[0]);
                            return;
                        }
                        catch (ArgumentException err)
                        {
                            IDictionary<string, object> parameters = new Dictionary<string, object>();
                            StatusPageMessage = err.Message;
                            parameters["next"] = Config.Urls.Home + "/ShowStatus.aspx";
                            Response.Redirect(facebook.GetLogoutUrl(parameters).ToString());
                            return;
                        }
                        catch (Exception err)
                        {
                            IDictionary<string, object> parameters = new Dictionary<string, object>();
                            Global.Logger.LogWarning(err);
                            StatusPageMessage = err.Message;
                            parameters["next"] = Config.Urls.Home + "/ShowStatus.aspx";
                            Response.Redirect(facebook.GetLogoutUrl(parameters).ToString());
                            return;
                        }
                        CurrentUserSession = userSession;
                        CurrentUserSession.LoggedInThroughFacebook = true;
                        Response.Redirect(MatchmakerHelper.CurrentHomePage);
                    }

                    #endregion

                populatedate:

                    JsonObject friends = facebook.Api("/me/friends") as JsonObject;
                    if (friends != null)
                    {
                        JsonArray friendsArr = friends["data"] as JsonArray;
                        FacebookFriendIDs = new long[friendsArr.Count];
                        for (int i = 0; i < friendsArr.Count(); i++)
                        {
                            long.TryParse((string)((JsonObject)friendsArr[i])["id"], out FacebookFriendIDs[i]);
                        }
                    }
                    if (user != null)
                    {
                        DateTime birthday;
                        if (DateTime.TryParse((String)user["birthday"], CultureInfo.InvariantCulture,
                                              DateTimeStyles.None, out birthday))
                        {
                            datePicker1.SelectedDate = birthday;
                            //txtDatePicker.Text = datePicker1.SelectedDate.ToShortDateString();
                            BirthdayPicker1.CurrentText = datePicker1.SelectedDate.ToShortDateString();
                        }
                        Classes.User.eGender? gender = null;

                        if (!String.IsNullOrEmpty((String)user["gender"]))
                        {
                            switch (((String)user["gender"]).ToLower(CultureInfo.InvariantCulture))
                            {
                                case "male":
                                    gender = Classes.User.eGender.Male;
                                    break;
                                case "female":
                                    gender = Classes.User.eGender.Female;
                                    break;
                            }
                        }
                        if (gender.HasValue)
                        {
                            dropGender.SelectedValue = ((int)gender).ToString();

                            switch (gender)
                            {
                                case Classes.User.eGender.Male:
                                    dropInterestedIn.SelectedValue = ((int)Classes.User.eGender.Female).ToString();
                                    break;
                                case Classes.User.eGender.Female:
                                    dropInterestedIn.SelectedValue = ((int)Classes.User.eGender.Male).ToString();
                                    break;
                            }
                        }

                        if (user.ContainsKey("location"))
                        {
                            JsonObject location = (JsonObject)user["location"];
                            string locationName = null;
                            if (location.ContainsKey("name"))
                            {
                                locationName = (string)location["name"];
                            }
                            if (locationName.IsNotNullOrEmpty())
                            {
                                string[] locationTokens = locationName.Split(new[] { "," },
                                                                             StringSplitOptions.RemoveEmptyEntries);
                                var fbCountry = locationTokens.Length > 2 ? locationTokens[2].Trim() : "";
                                var fbRegion = locationTokens.Length > 1 ? locationTokens[1].Trim() : "";
                                var fbCity = locationTokens.Length > 0 ? locationTokens[0].Trim() : "";

                                var allCountries = Config.Users.GetCountries();
                                var regions = new string[0];
                                var cities = new string[0];
                                foreach (var country in allCountries)
                                {
                                    this.dropCountry.Items.Add(country);
                                    if (country.Contains(fbCountry))
                                    {
                                        this.dropCountry.SelectedValue = country;
                                        CascadingDropDownState.DataBind();
                                    }
                                }
                                if (this.dropCountry.SelectedIndex > 0)
                                {
                                    regions = Config.Users.GetRegions(this.dropCountry.SelectedValue);
                                    foreach (var region in regions)
                                    {
                                        this.dropRegion.Items.Add(region);
                                        if (region.Contains(fbRegion))
                                        {
                                            this.dropRegion.SelectedValue = region;
                                            CascadingDropDownState.DataBind();
                                        }
                                    }
                                    if (this.dropRegion.SelectedIndex > 0)
                                        cities = Config.Users.GetCities(this.dropCountry.SelectedValue,
                                                                        this.dropRegion.SelectedValue);
                                    foreach (var city in cities)
                                    {
                                        this.dropCity.Items.Add(city);
                                        if (city.Contains(fbCity))
                                            dropCity.SelectedValue = city;
                                    }
                                }
                            }
                        }

                        if (user.ContainsKey("big_pic") && !String.IsNullOrEmpty((String)user["pic_big"]))
                            PrimaryPhotoURL = (String)user["pic_big"];
                        else
                            PrimaryPhotoURL = String.Format("https://graph.facebook.com/{0}/picture?type={1}",
                                                            id, Config.FacebookSettings.FacebookDefaultImageIsNormal 
                                                                    ? AspNetDating.ImageHandler.eFacebookImageType.Normal
                                                                    : AspNetDating.ImageHandler.eFacebookImageType.Large);

                        if (user.ContainsKey("email") && !String.IsNullOrEmpty((String)user["email"]))
                            FacebookEmail = (String)user["email"];
                        txtEmail.Text = FacebookEmail;

                        //var albums = facebookService.photos.getAlbums(user.uid).ToArray();
                        //albums[0].

                        txtName.Text = (String)user["name"];
                        txtUsername.Text = (String)user["username"];
                        IsFacebookVerified = false;

                        if (user.ContainsKey("verified") && (user["verified"] is bool))
                            IsFacebookVerified = (bool)user["verified"];

                        divCardSpace.Visible = false;
                        divMySpace.Visible = false;
                        divFacebook.Visible = false;
                    }
                }
            }
            catch (Exception e)
            {
            }
        }

        protected bool IsFacebookVerified { get; set; }
        protected string FacebookEmail { get; set; }

        protected void dropGender_SelectedIndexChanged(object sender, EventArgs e)
        {
            //trBirthday2.Visible = dropGender.SelectedValue == ((int) Classes.User.eGender.Couple).ToString();
        }

        protected void btnNext_Click(object sender, ImageClickEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}

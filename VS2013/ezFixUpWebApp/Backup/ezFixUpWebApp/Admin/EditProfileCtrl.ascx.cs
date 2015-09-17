/* ASPnetDating 
 * Copyright (C) 2003-2010 eStream 
 * ezFixUp.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at ezFixUp.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ezFixUp.Classes;
using ezFixUp.Components;
using ezFixUp.Components.Profile;

namespace ezFixUp.Admin
{
    /// <summary>
    ///		Summary description for EditProfileCtrl.
    /// </summary>
    public partial class EditProfileCtrl : UserControl
    {
        protected DatePicker datePicker1, datePicker2;
        //protected System.Web.UI.WebControls.Label lblError;
        protected MessageBox MessageBox;
        protected DropDownList dropCity;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadStrings();
                LoadPersonalData();
            }
            LoadProfile();
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

        private void HideLocation()
        {
            trCountry.Visible = false;
            trState.Visible = false;
            trCity.Visible = false;
            trZipCode.Visible = false;
        }

        private void ShowLocation()
        {
            trCountry.Visible = true;
            trState.Visible = true;
            trCity.Visible = true;
            trZipCode.Visible = true;
        }

        private void LoadStrings()
        {
            #region Location strings

            if (!Config.Users.LocationPanelVisible)
                HideLocation();
            //else
            //    ShowLocation();

            //foreach (string country in Config.Users.CountriesHash.Keys)
            //{
            //    dropCountry.Items.Add(country);
            //}

            //foreach (string state in Config.Users.StateHash.Keys)
            //{
            //    dropState.Items.Add(state);
            //}
            //#region Add Countries

            //dropCountry.Items.Add(new ListItem("", ""));

            //foreach (string country in Config.Users.Countries)
            //    dropCountry.Items.Add(country);

            //#endregion

            //AddRegionsForSelectedCountry();
            #endregion

            dropGender.Items.Clear();

            if (!Config.Users.DisableGenderInformation)
            {
                dropGender.Items.Add(
                new ListItem(Lang.TransA("Male"), ((int)User.eGender.Male).ToString()));
                dropGender.Items.Add(
                    new ListItem(Lang.TransA("Female"), ((int)User.eGender.Female).ToString()));
                if (Config.Users.CouplesSupport)
                {
                    dropGender.Items.Add(
                        new ListItem(Lang.TransA("Couple"), ((int)User.eGender.Couple).ToString()));
                }

                dropGender.Attributes.Add("onchange",
                                          "javascript: document.getElementById('EditProfileCtrl1_trBirthdate2').style.display = (this.selectedIndex == 2) ? '' : 'none'");
            }

            if (Config.Users.InterestedInFieldEnabled && !Config.Users.DisableGenderInformation)
            {
                trInterestedIn.Visible = true;

                dropInterestedIn.Items.Clear();
                dropInterestedIn.Items.Add(
                    new ListItem(Lang.TransA("Male"), ((int) User.eGender.Male).ToString()));
                dropInterestedIn.Items.Add(
                    new ListItem(Lang.TransA("Female"), ((int) User.eGender.Female).ToString()));
                if (Config.Users.CouplesSupport)
                {
                    dropInterestedIn.Items.Add(
                        new ListItem(Lang.TransA("Couple"), ((int) User.eGender.Couple).ToString()));
                }
            }
            else trInterestedIn.Visible = false;

            pnlGender.Visible = !Config.Users.DisableGenderInformation;
            pnlBirthdate.Visible = !Config.Users.DisableAgeInformation;

            cbReceiveEmails.Text = Lang.TransA("User will receive email notifications");
            cbProfileVisible.Text = Lang.TransA("User's profile will be visible to other members");

            btnSave.Text = Lang.TransA(" Save Changes >> ");
            if (!((AdminPageBase)Page).HasWriteAccess) btnSave.Enabled = false;
            btnCancel.Text = Lang.TransA("Cancel");

            trUserVerified.Visible = Config.Users.EnableRealPersonVerificationFunctionalityAdmin;
            cbUserVerified.Text = Lang.TransA("Verified as Genuine");

            trFeaturedMember.Visible = Config.Users.ShowFeaturedMemberOnHomePage;
            cbFeaturedMember.Text = Lang.TransA("Featured Member");
        }

        private User user;

        public User User
        {
            set
            {
                user = value;
                if (user != null)
                    hidUsername.Value = user.Username;
                else
                    hidUsername.Value = "";
            }
            get
            {
                if (user == null
                    && hidUsername.Value != "")
                    user = User.Load(hidUsername.Value);
                return user;
            }
        }

        private void LoadPersonalData()
        {
            txtName.Text = User.Name;
            datePicker1.SelectedDate = User.Birthdate;
            if (Config.Users.CouplesSupport && User.Gender == User.eGender.Couple)
            {
                datePicker2.SelectedDate = User.Birthdate2;
            }
            else
            {
                trBirthdate2.Attributes.Add("style", "display: none");
            }
            txtEmail.Text = User.Email;
            dropGender.SelectedValue = ((int) User.Gender).ToString();
            if (Config.Users.InterestedInFieldEnabled)
                dropInterestedIn.SelectedValue = ((int) User.InterestedIn).ToString();

            #region Location values

            if (Config.Users.LocationPanelVisible)
            {
                if (dropCountry != null && User.Country != String.Empty)
                {
                    CascadingDropDownCountry.SelectedValue = User.Country;
                }
                if (dropRegion != null && User.State != String.Empty)
                {
                    CascadingDropDownState.SelectedValue = User.State;
                }
                if (dropCity != null && User.City != String.Empty)
                {
                    CascadingDropDownCity.SelectedValue = User.City;
                }
                if (txtZipCode != null)
                {
                    txtZipCode.Text = User.ZipCode;
                }
            }

            #endregion

            cbReceiveEmails.Checked = User.ReceiveEmails;
            cbProfileVisible.Checked = User.ProfileVisible;

            if (Config.Users.EnableRealPersonVerificationFunctionalityAdmin)
                cbUserVerified.Checked = User.IsUserVerified(User.Username, true);

            cbFeaturedMember.Checked = User.IsFeaturedMember;
        }

        private bool ValidatePersonalData()
        {
            // Validate e-mail address
            try
            {
                if (txtEmail.Text.Length == 0)
                {
                    MessageBox.Show(Lang.TransA("Please specify e-mail address!"),
                                    Misc.MessageType.Error);
                    return false;
                }
            }
            catch (ArgumentException err) // Invalid e-mail address
            {
                MessageBox.Show(err.Message, Misc.MessageType.Error);
                return false;
            }

            // Validate passwords
            if (!(txtPassword.Text.Trim() == "" && txtPassword2.Text.Trim() == ""))
            {
                if (txtPassword.Text.Length == 0)
                {
                    MessageBox.Show(Lang.TransA("Please specify password!"), Misc.MessageType.Error);
                    return false;
                }
                if (txtPassword2.Text.Length == 0)
                {
                    MessageBox.Show(Lang.TransA("Please verify password!"), Misc.MessageType.Error);
                    return false;
                }
                if (txtPassword.Text != txtPassword2.Text)
                {
                    MessageBox.Show(Lang.TransA("Passwords do not match!"), Misc.MessageType.Error);
                    return false;
                }
            }
            // Validate name
            if (txtName.Text.Length == 0)
            {
                MessageBox.Show(Lang.TransA("Please enter your name!"), Misc.MessageType.Error);
                return false;
            }

            // Validate birthdate
			if (!datePicker1.ValidDateEntered)
			{
                MessageBox.Show(Lang.TransA("Please select your birthdate!"), Misc.MessageType.Error);
				return false;
			}
			if (Config.Users.CouplesSupport && User.Gender == User.eGender.Couple
				&& !datePicker2.ValidDateEntered)
			{
                MessageBox.Show(Lang.TransA("Please select your birthdate!"), Misc.MessageType.Error);
				return false;
			}

            return true;
        }

        private void LoadProfile()
        {
            if (User == null) return;

            ProfileTopic[] profileTopics = ProfileTopic.Fetch();
            if (profileTopics != null)
            {
                foreach (ProfileTopic topic in profileTopics)
                {
                    Table tblTopicTable = new Table();
                    plhProfile.Controls.Add(tblTopicTable);

                    tblTopicTable.CellPadding = 0;
                    tblTopicTable.CellSpacing = 0;
                    tblTopicTable.CssClass = "profile_topic";
                    TableCell headerCell = new TableCell();
                    headerCell.ColumnSpan = topic.EditColumns;
                    headerCell.Text = topic.Name;
                    headerCell.CssClass = "table_header";
                    TableRow headerRow = new TableRow();
                    headerRow.Cells.Add(headerCell);
                    tblTopicTable.Rows.Add(headerRow);

                    ControlCollection ctlQuestions = null;
                    TableRow tblRow = new TableRow();
                    tblTopicTable.Rows.Add(tblRow);
                    TableCell tblCell = null;
                    int questionNo = 0;

                    ProfileQuestion[] questions = topic.FetchQuestions();
                    if (questions != null)
                    {
                        foreach (ProfileQuestion question in questions)
                        {
                            if (!question.IsVisible(User.Gender))
                            {
                                continue;
                            }
                            if (question.EditStyle == ProfileQuestion.eEditStyle.Hidden) continue;

                            tblCell = new TableCell();
                            tblCell.CssClass = "table_cell2";
                            ctlQuestions = tblCell.Controls;

                            IProfileQuestionComponent cProfile;

                            switch (question.EditStyle)
                            {
                                case ProfileQuestion.eEditStyle.SingleLine:
                                    cProfile = (IProfileQuestionComponent)
                                               LoadControl("~/Components/Profile/EditSingleLine.ascx");
                                    ((EditSingleLine) cProfile).AdminMode = true;
                                    break;

                                case ProfileQuestion.eEditStyle.MultiLine:
                                    cProfile = (IProfileQuestionComponent)
                                               LoadControl("~/Components/Profile/EditMultiLine.ascx");
                                    ((EditMultiLine) cProfile).AdminMode = true;
                                    break;

                                case ProfileQuestion.eEditStyle.SingleChoiceSelect:
                                    cProfile = (IProfileQuestionComponent)
                                               LoadControl("~/Components/Profile/EditSingleChoiceSelect.ascx");
                                    ((EditSingleChoiceSelect) cProfile).AdminMode = true;
                                    break;

                                case ProfileQuestion.eEditStyle.MultiChoiceCheck:
                                    cProfile = (IProfileQuestionComponent)
                                               LoadControl("~/Components/Profile/EditMultiChoiceCheck.ascx");
                                    ((EditMultiChoiceCheck) cProfile).AdminMode = true;
                                    break;

                                case ProfileQuestion.eEditStyle.SingleChoiceRadio:
                                    cProfile = (IProfileQuestionComponent)
                                               LoadControl("~/Components/Profile/EditSingleChoiceRadio.ascx");
                                    ((EditSingleChoiceRadio) cProfile).AdminMode = true;
                                    break;

                                case ProfileQuestion.eEditStyle.MultiChoiceSelect:
                                    cProfile = (IProfileQuestionComponent)
                                               LoadControl("~/Components/Profile/EditMultiChoiceSelect.ascx");
                                    ((EditMultiChoiceSelect) cProfile).AdminMode = true;
                                    break;

                                default:
                                    throw new Exception(
                                        String.Format("The Style \"{0}\" is not implemented",
                                                      question.EditStyle));
                            }
                            cProfile.User = User;
                            cProfile.Question = question;

                            ctlQuestions.Add((Control) cProfile);

                            tblRow.Cells.Add(tblCell);

                            if (topic.EditColumns == 1 || ++questionNo%topic.EditColumns == 0)
                            {
                                tblRow = new TableRow();
                                tblTopicTable.Rows.Add(tblRow);
                            }
                        }
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!((AdminPageBase)Page).HasWriteAccess)
                return;
            
            if (!ValidatePersonalData())
                return;
            try
            {
                User.Name = txtName.Text;
                if (Config.Users.DisableGenderInformation)
                    User.Gender = User.eGender.Male;
                else
                    User.Gender = (User.eGender) Convert.ToInt32(dropGender.SelectedValue);

                if (Config.Users.InterestedInFieldEnabled && !Config.Users.DisableGenderInformation)
                    User.InterestedIn = (User.eGender) Convert.ToInt32(dropInterestedIn.SelectedValue);

                User.Birthdate = datePicker1.SelectedDate;
                if (Config.Users.CouplesSupport && User.Gender == User.eGender.Couple)
                {
                    user.Birthdate2 = datePicker2.SelectedDate;
                }
                User.Email = txtEmail.Text;
                if (txtPassword.Text != "")
                    User.Password = txtPassword.Text;

                #region Save Location

                if (Config.Users.LocationPanelVisible)
                {
                    if (dropCountry != null)
                    {
                        User.Country = dropCountry.SelectedValue;
                    }
                    if (dropRegion != null)
                    {
                        User.State = dropRegion.SelectedValue;
                    }
                    if (txtZipCode != null)
                    {
                        User.ZipCode = txtZipCode.Text;
                    }
                    if (dropCity != null)
                    {
                        User.City = dropCity.SelectedValue;
                    }

                    Location loc = Config.Users.GetLocation(User.Country, User.State, User.City);

                    if (loc != null)
                    {
                        User.Longitude = loc.Longitude;
                        User.Latitude = loc.Latitude;
                    }
                    else
                    {
                        User.Longitude = null;
                        User.Latitude = null;
                    }

                }

                #endregion

                User.ReceiveEmails = cbReceiveEmails.Checked;
                User.ProfileVisible = cbProfileVisible.Checked;
                if (Config.Users.EnableRealPersonVerificationFunctionalityAdmin)
                {
                    if (cbUserVerified.Checked)
                        User.SetAsVerifiedByAdmin(User.Username);
                    else
                        User.RemoveVerifiedStatusByAdmin(User.Username);
                }

                if (Config.Users.ShowFeaturedMemberOnHomePage)
                {
                    User.IsFeaturedMember = cbFeaturedMember.Checked;
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, Misc.MessageType.Error);
                return;
            }

            try
            {
                User.Update();

                IPLogger.Log(User.Username, Request.UserHostAddress, IPLogger.ActionType.AdminEditUser);
            }
            catch (SqlException)
            {
                MessageBox.Show(Lang.TransA("The E-mail already exists in the database"), Misc.MessageType.Error);
                return;
            }

            try
            {
                SaveProfileAnswers(plhProfile);
                MessageBox.Show(Lang.TransA("The account has been succesfuly updated!"), Misc.MessageType.Success);
            }
            catch (AnswerRequiredException)
            {
                //
            }
        }

        private void SaveProfileAnswers(Control control)
        {
            foreach (Control ctl in control.Controls)
            {
                if (ctl is IProfileQuestionComponent)
                {
                    try
                    {
                        ProfileAnswer answer = ((IProfileQuestionComponent) ctl).Answer;
                        if (answer.Value != "")
                        {
                            answer.Approved = true;
                            answer.Save();
                        }
                        else
                        {
                            answer.Delete();
                        }
                    }
                    catch (AnswerRequiredException)
                    {
                    }
                }
                else
                    SaveProfileAnswers(ctl);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("BrowseUsers.aspx");
        }

        //private void AddRegionsForSelectedCountry()
        //{
        //    dropState.Items.Clear();
        //    dropState.Items.Add(String.Empty);

        //    foreach (string region in Config.Users.GetRegions(dropCountry.SelectedValue))
        //    {
        //        dropState.Items.Add(region);
        //    }

        //    if (dropState.Items.Count == 1)
        //        trState.Visible = false;
        //    else
        //        trState.Visible = true;
        //}        

        //protected void dropCountry_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    AddRegionsForSelectedCountry();
        //}
    }
}
using System;
using System.Data;
using System.Web.UI.WebControls;
using ezFixUp.Classes;

namespace ezFixUp.Admin
{
    public partial class EditScheduledAnnouncements : AdminPageBase
    {
        private int? CurrentAnnouncementID
        {
            get
            {
                return (int?) ViewState["CurrentAnnouncementID"];
            }
            set
            {
                ViewState["CurrentAnnouncementID"] = value;
            }
        }

        private TextBox ckeditor = null;
        private HtmlEditor htmlEditor = null;

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.sendAnnouncement;
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Scripts.InitilizeHtmlEditor(this, phEditor, ref htmlEditor, ref ckeditor, "100%", "500px");

            if (!Page.IsPostBack)
            {
                if (!HasWriteAccess)
                {
                    btnSave.Enabled = false;
                }

                LoadStrings();
                PopulateFilters();
                LoadAnnouncements();
            }
        }

        private void LoadStrings()
        {
            btnSave.Text = Lang.TransA("Save");
            btnSend.Text = Lang.TransA("Send");
            btnAddNew.Text = Lang.TransA("Add new");
            btnSendTestEmail.Text = "Send".TranslateA();

            if (Config.Users.DisableGenderInformation)
            {
                pnlGender.Visible = false;
            }

            datePicker1.MinYear = DateTime.Now.Year;
            datePicker1.MaxYear = DateTime.Now.Year + 5;

            dgAnnouncements.Columns[0].HeaderText = Lang.TransA("Name");
            dgAnnouncements.Columns[1].HeaderText = Lang.TransA("Type");
        }

        private void PopulateFilters()
        {
            ddType.Items.Add(new ListItem("Send now".TranslateA(), "-1"));
            ddType.Items.Add(new ListItem("Send on specific date".TranslateA(), ((int)ScheduledAnnouncement.eType.SpecificDate).ToString()));
            ddType.Items.Add(new ListItem("Send days before membership renew".TranslateA(), ((int)ScheduledAnnouncement.eType.DaysBeforeMembershipEnd).ToString()));
            ddType.Items.Add(new ListItem("Send days after inactivity".TranslateA(), ((int)ScheduledAnnouncement.eType.DaysAfterInactivity).ToString()));
            ddType.Items.Add(new ListItem("Send days after registration".TranslateA(), ((int)ScheduledAnnouncement.eType.DaysAfterRegistration).ToString()));
            ddType.Items.Add(new ListItem("Send Weekly".TranslateA(), ((int)ScheduledAnnouncement.eType.Weekly).ToString()));
            ddType.Items.Add(new ListItem("Send Monthly".TranslateA(), ((int)ScheduledAnnouncement.eType.Monthly).ToString()));

            ddGender.Items.Add(new ListItem(Lang.TransA("All"), "-1"));

            if (!Config.Users.DisableGenderInformation)
            {
                ddGender.Items.Add(
                    new ListItem(Lang.TransA("Male"), ((int)Classes.User.eGender.Male).ToString()));
                ddGender.Items.Add(
                    new ListItem(Lang.TransA("Female"), ((int)Classes.User.eGender.Female).ToString()));
                ddGender.Items.Add(
                    new ListItem(Lang.TransA("Matchmaker"), ((int)Classes.User.eGender.Matchmaker).ToString()));
                if (Config.Users.CouplesSupport)
                {
                    ddGender.Items.Add(
                        new ListItem(Lang.TransA("Couple"), ((int)Classes.User.eGender.Couple).ToString()));
                }
            }

            ddPaid.Items.Add(new ListItem(Lang.TransA("All"), "-1"));
            ddPaid.Items.Add(new ListItem(Lang.TransA("Yes"), "Yes"));
            ddPaid.Items.Add(new ListItem(Lang.TransA("No"), "No"));
            ddHasPhotos.Items.Add(new ListItem(Lang.TransA("All"), "-1"));
            ddHasPhotos.Items.Add(new ListItem(Lang.TransA("Yes"), "Yes"));
            ddHasPhotos.Items.Add(new ListItem(Lang.TransA("No"), "No"));
            ddHasProfile.Items.Add(new ListItem(Lang.TransA("All"), "-1"));
            ddHasProfile.Items.Add(new ListItem(Lang.TransA("Yes"), "Yes"));
            ddHasProfile.Items.Add(new ListItem(Lang.TransA("No"), "No"));
            ddLanguage.Items.Add(new ListItem(Lang.TransA("All"), "-1"));
            foreach (Language language in Language.FetchAll())
            {
                ddLanguage.Items.Add(new ListItem(Lang.TransA(language.Name), language.Id.ToString()));
            }
        }

        private void LoadAnnouncements()
        {
            DataTable dtAnnouncements = new DataTable();
            dtAnnouncements.Columns.Add("ID");
            dtAnnouncements.Columns.Add("Name");
            dtAnnouncements.Columns.Add("Type");

            string type = null;
            ScheduledAnnouncement[] announcements = ScheduledAnnouncement.Fetch();
            foreach (ScheduledAnnouncement announcement in announcements)
            {
                switch (announcement.Type)
                {
                    case ScheduledAnnouncement.eType.SpecificDate:
                        type = String.Format("Send on {0}".TranslateA(), announcement.Date.Value.ToShortDateString());
                        break;
                    case ScheduledAnnouncement.eType.Weekly:
                        type = String.Format("Send Sunday + {0} day of the week".TranslateA(), announcement.Days.Value);
                        break;
                    case ScheduledAnnouncement.eType.Monthly:
                        type = String.Format("Send on day {0}th of the mounth".TranslateA(), announcement.Days.Value);
                        break;
                    case ScheduledAnnouncement.eType.DaysBeforeMembershipEnd:
                        type = String.Format("Send {0} days before membership renew".TranslateA(), announcement.Days.Value);
                        break;
                    case ScheduledAnnouncement.eType.DaysAfterInactivity:
                        type = String.Format("Send {0} days after inactivity".TranslateA(), announcement.Days.Value);
                        break;
                    case ScheduledAnnouncement.eType.DaysAfterRegistration:
                        type = String.Format("Send {0} days after registration".TranslateA(), announcement.Days.Value);
                        break;
                }
                dtAnnouncements.Rows.Add(new object[]
                                             {
                                                 announcement.ID,
                                                 announcement.Name,
                                                 type
                                             });
            }

            if (dtAnnouncements.Rows.Count == 0)
            {
                btnAddNew_Click(null, null);
            }

            dgAnnouncements.DataSource = dtAnnouncements;
            dgAnnouncements.DataBind();
            dgAnnouncements.Visible = dtAnnouncements.Rows.Count > 0;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            string name = txtName.Text.Trim();
            string subject = txtSubject.Text.Trim();
            string body = htmlEditor != null ? htmlEditor.Content : ckeditor.Text;
            User.eGender? gender = null;
            bool? paidMember = null;
            bool? hasPhotos = null;
            bool? hasProfile = null;
            int? languageID = null;
            string country = null;
            string region = null;
            ScheduledAnnouncement.eType type;
            DateTime? date = null;
            int? days = null;

            if (ddType.SelectedIndex == 0)
            {
                Messagebox1.Show(Lang.TransA("Please select a type!"), Misc.MessageType.Error);
                return;
            }

            if (name.Length == 0)
            {
                Messagebox1.Show(Lang.TransA("Please enter name!"), Misc.MessageType.Error);
                return;
            }

            if (subject.Length == 0)
            {
                Messagebox1.Show(Lang.TransA("Please enter subject!"), Misc.MessageType.Error);
                return;
            }

            if (body.Length == 0)
            {
                Messagebox1.Show(Lang.TransA("Please enter body!"), Misc.MessageType.Error);
                return;
            }

            if (pnlDate.Visible && !datePicker1.ValidDateEntered)
            {
                Messagebox1.Show(Lang.TransA("Please select valid date!"), Misc.MessageType.Error);
                return;
            }

            if (pnlInactivity.Visible)
            {
                int inactivityDays;
                if (!Int32.TryParse(txtInactivity.Text.Trim(), out inactivityDays))
                {
                    Messagebox1.Show(Lang.TransA("Please enter a number!"), Misc.MessageType.Error);
                    return;
                }

                days = inactivityDays;
            }

            if (pnlAfterRegistration.Visible)
            {
                int daysAfterRegistration;
                if (!Int32.TryParse(txtAfterRegistration.Text.Trim(), out daysAfterRegistration))
                {
                    Messagebox1.Show(Lang.TransA("Please enter a number!"), Misc.MessageType.Error);
                    return;
                }

                days = daysAfterRegistration;
            }
            if (pnlBeforeSubscriptionEnd.Visible)
            {
                int daysBeforeEnd;
                if (!Int32.TryParse(txtBeforeSubscriptionEnd.Text.Trim(), out daysBeforeEnd))
                {
                    Messagebox1.Show(Lang.TransA("Please enter a number!"), Misc.MessageType.Error);
                    return;
                }

                days = daysBeforeEnd;
            }
            
            if (ddGender.SelectedIndex > 0)
                gender = (User.eGender?) Convert.ToInt32(ddGender.SelectedValue);
            if (ddPaid.SelectedIndex > 0)
                paidMember = ddPaid.SelectedIndex == 1;
            if (ddHasPhotos.SelectedIndex > 0)
                hasPhotos = ddHasPhotos.SelectedIndex == 1;
            if (ddHasProfile.SelectedIndex > 0)
                hasProfile = ddHasProfile.SelectedIndex == 1;
            if (ddLanguage.SelectedIndex > 0)
                languageID = Convert.ToInt32(ddLanguage.SelectedValue);
            if (ddCountry.SelectedValue.Trim().Length > 0)
                country = ddCountry.SelectedValue.Trim();
            if (ddRegion.SelectedValue.Trim().Length > 0)
                region = ddRegion.SelectedValue.Trim();
            type = (ScheduledAnnouncement.eType) Convert.ToInt32(ddType.SelectedValue);
            if (pnlDate.Visible)
                date = datePicker1.SelectedDate;

            ScheduledAnnouncement announcement = null;

            if (!CurrentAnnouncementID.HasValue) // add new
                announcement = new ScheduledAnnouncement();
            else
                announcement = ScheduledAnnouncement.Fetch(CurrentAnnouncementID.Value);
            announcement.Name = name;
            announcement.Subject = subject;
            announcement.Body = body;
            announcement.Gender = gender;
            announcement.PaidMember = paidMember;
            announcement.HasPhotos = hasPhotos;
            announcement.HasProfile = hasProfile;
            announcement.LanguageId = languageID;
            announcement.Country = country;
            announcement.Region = region;
            announcement.Type = type;
            announcement.Date = date;
            announcement.Days = days;

            announcement.Save();

            Messagebox1.Show(Lang.TransA("Announcements has been updated successfully!"), Misc.MessageType.Success);

            pnlAnnouncement.Visible = false;
            pnlAnnouncements.Visible = true;
            LoadAnnouncements();
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            string subject = txtSubject.Text.Trim();
            string text = htmlEditor != null ? htmlEditor.Content.Trim() : ckeditor.Text.Trim();

            if (subject.Length == 0)
            {
                Messagebox1.Show(Lang.TransA("Please enter subject!"), Misc.MessageType.Error);
                return;
            }
            if (text.Length == 0)
            {
                Messagebox1.Show(Lang.TransA("Please enter body!"), Misc.MessageType.Error);
                return;
            }

            BasicSearch search = new BasicSearch();
            search.hasAnswer_isSet = false;
            if (ddGender.SelectedIndex > 0)
                search.Gender = (User.eGender)Convert.ToInt32(ddGender.SelectedValue);
            if (ddPaid.SelectedIndex > 0)
                search.Paid = ddPaid.SelectedIndex == 1;
            if (ddHasPhotos.SelectedIndex > 0)
                search.HasPhoto = ddHasPhotos.SelectedIndex == 1;
            if (ddHasProfile.SelectedIndex > 0)
                search.HasAnswer = ddHasProfile.SelectedIndex == 1;
            search.Country = ddCountry.SelectedValue.Trim();
            search.State = ddRegion.SelectedValue.Trim();
            if (ddLanguage.SelectedIndex > 0)
                search.LanguageID = Convert.ToInt32(ddLanguage.SelectedValue);

            UserSearchResults results = search.GetResults();

            if (results == null)
            {
                Messagebox1.Show(Lang.TransA("There are no users that match your criteria!"), Misc.MessageType.Success);
                return;
            }

            string[] users = results.Usernames;

            foreach (string username in users)
            {
                subject = txtSubject.Text.Trim();
                text = htmlEditor != null ? htmlEditor.Content.Trim() : ckeditor.Text.Trim();
                User user = Classes.User.Load(username);
                subject = subject.Replace("%%USER%%", user.Username);
                subject = subject.Replace("%%NAME%%", user.Name);
                text = text.Replace("%%USER%%", user.Username);
                text = text.Replace("%%NAME%%", user.Name);

                Email.Send(Config.Misc.SiteTitle, Config.Misc.SiteEmail, user.Name, user.Email, subject, text, false);
            }
            Messagebox1.Show(Lang.TransA("Your announcement has been sent!"), Misc.MessageType.Success);
            txtSubject.Text = "";
            if (htmlEditor != null)
                htmlEditor.Content = "";
            else
                ckeditor.Text = "";
        }

        protected void ddType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddType.SelectedIndex > 0)
            {
                pnlName.Visible = true;
                btnSend.Visible = false;
                btnSave.Visible = true;
            }
            else
            {
                pnlName.Visible = false;
                btnSend.Visible = true;
                btnSave.Visible = false;
            }
            var schedType = (ScheduledAnnouncement.eType) Convert.ToInt32(ddType.SelectedValue);
            pnlDate.Visible =  schedType == ScheduledAnnouncement.eType.SpecificDate;
            pnlInactivity.Visible = schedType == ScheduledAnnouncement.eType.DaysAfterInactivity
                                    || schedType == ScheduledAnnouncement.eType.Weekly
                                    || schedType == ScheduledAnnouncement.eType.Monthly; 
            pnlAfterRegistration.Visible = schedType == ScheduledAnnouncement.eType.DaysAfterRegistration;
            pnlBeforeSubscriptionEnd.Visible = schedType == ScheduledAnnouncement.eType.DaysBeforeMembershipEnd;
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            ResetFilter();
            pnlAnnouncement.Visible = true;
            pnlAnnouncements.Visible = false;
            CurrentAnnouncementID = null;
        }

        protected void dgAnnouncements_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                ResetFilter();
                pnlAnnouncement.Visible = true;
                pnlAnnouncements.Visible = false;
                btnSend.Visible = false;
                btnSave.Visible = true;
                pnlName.Visible = true;

                ScheduledAnnouncement announcement = ScheduledAnnouncement.Fetch(Convert.ToInt32(e.CommandArgument));
                if (announcement != null)
                {
                    CurrentAnnouncementID = announcement.ID;
                    txtName.Text = announcement.Name;
                    txtSubject.Text = announcement.Subject;
                    if (ckeditor != null)
                        ckeditor.Text = announcement.Body;
                    else if (htmlEditor != null)
                        htmlEditor.Content = announcement.Body;
                    ddGender.SelectedValue = announcement.Gender.HasValue
                                                 ? ((int) announcement.Gender.Value).ToString()
                                                 : "-1";
                    if (announcement.PaidMember.HasValue)
                        ddPaid.SelectedValue = announcement.PaidMember.Value ? "Yes" : "No";
                    if (announcement.HasPhotos.HasValue)
                        ddHasPhotos.SelectedValue = announcement.HasPhotos.Value ? "Yes" : "No";
                    if (announcement.HasProfile.HasValue)
                        ddHasProfile.SelectedValue = announcement.HasProfile.Value ? "Yes" : "No";
                    ddLanguage.SelectedValue = announcement.LanguageId.HasValue ? announcement.LanguageId.ToString() : "-1";
                    CascadingDropDownCountry.SelectedValue = announcement.Country ?? "";
                    CascadingDropDownState.SelectedValue = announcement.Region ?? "";
                    ddType.SelectedValue = ((int) announcement.Type).ToString();
                    switch (announcement.Type)
                    {
                        case ScheduledAnnouncement.eType.SpecificDate :
                            pnlDate.Visible = true;
                            break;
                        case ScheduledAnnouncement.eType.Weekly :
                            pnlInactivity.Visible = true;
                            if (announcement.Days.HasValue)
                                txtInactivity.Text = announcement.Days.Value.ToString();
                            break;
                        case ScheduledAnnouncement.eType.Monthly :
                            pnlInactivity.Visible = true;
                            if (announcement.Days.HasValue)
                                txtInactivity.Text = announcement.Days.Value.ToString();
                            break;
                        case ScheduledAnnouncement.eType.DaysBeforeMembershipEnd:
                            pnlBeforeSubscriptionEnd.Visible = true;
                            if (announcement.Days.HasValue)
                                txtBeforeSubscriptionEnd.Text = announcement.Days.Value.ToString();
                            break;
                        case ScheduledAnnouncement.eType.DaysAfterInactivity :
                            pnlInactivity.Visible = true;
                            if (announcement.Days.HasValue)
                                txtInactivity.Text = announcement.Days.Value.ToString();
                            break;
                        case ScheduledAnnouncement.eType.DaysAfterRegistration :
                            pnlAfterRegistration.Visible = true;
                            if (announcement.Days.HasValue)
                                txtAfterRegistration.Text = announcement.Days.Value.ToString();
                            break;
                    }

                    if (announcement.Date.HasValue)
                        datePicker1.SelectedDate = announcement.Date.Value;
                }
            }
            else if (e.CommandName == "Delete")
            {
                ScheduledAnnouncement.Delete(Convert.ToInt32(e.CommandArgument));
            }

            LoadAnnouncements();
        }

        protected void dgAnnouncements_ItemCreated(object source, DataGridItemEventArgs e)
        {
            if (e.Item.ItemIndex == -1)
                return;

            LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
            lnkDelete.Attributes.Add("onclick",
                                     String.Format("javascript: return confirm('{0}')",
                                                   Lang.TransA("Do you really want to delete this announcement?")));
        }

        private void ResetFilter()
        {
            txtName.Text = String.Empty;
            txtSubject.Text = String.Empty;
            if (htmlEditor != null)
                htmlEditor.Content = "";
            else
                ckeditor.Text = "";
            ddGender.SelectedIndex = 0;
            ddPaid.SelectedIndex = 0;
            ddHasPhotos.SelectedIndex = 0;
            ddHasProfile.SelectedIndex = 0;
            ddLanguage.SelectedIndex = 0;
            ddCountry.SelectedIndex = 0;
            ddRegion.SelectedIndex = 0;
            ddType.SelectedIndex = 0;
            datePicker1.Reset();
            txtInactivity.Text = String.Empty;
            txtAfterRegistration.Text = String.Empty;
            pnlName.Visible = false;
            pnlDate.Visible = false;
            pnlInactivity.Visible = false;
            pnlAfterRegistration.Visible = false;
            btnSend.Visible = true;
            btnSave.Visible = false;
        }

        protected void btnSendTestEmail_Click(object sender, EventArgs e)
        {
            string subject = txtSubject.Text.Trim();
            string text = htmlEditor != null ? htmlEditor.Content.Trim() : ckeditor.Text.Trim();
            string username = txtTestUsername.Text.Trim();

            if (subject.Length == 0)
            {
                Messagebox1.Show(Lang.TransA("Please enter subject!"), Misc.MessageType.Error);
                return;
            }
            if (text.Length == 0)
            {
                Messagebox1.Show(Lang.TransA("Please enter body!"), Misc.MessageType.Error);
                return;
            }
            if (username.Length == 0)
            {
                Messagebox1.Show(Lang.TransA("Please enter username!"), Misc.MessageType.Error);
                return;
            }

            User user = null;
            try
            {
                user = Classes.User.Load(username);
                subject = subject.Replace("%%USER%%", user.Username);
                subject = subject.Replace("%%NAME%%", user.Name);
                text = text.Replace("%%USER%%", user.Username);
                text = text.Replace("%%NAME%%", user.Name);

                Email.Send(Config.Misc.SiteTitle, Config.Misc.SiteEmail, user.Name, user.Email, subject, text, true);
                Messagebox1.Show(Lang.TransA("Your test announcement has been sent!"), Misc.MessageType.Success);
            }
            catch (NullReferenceException)//was NotFoundException
            {
                Messagebox1.Show(Lang.TransA("This Username was not found!"), Misc.MessageType.Error);
            }
        }
    }
}

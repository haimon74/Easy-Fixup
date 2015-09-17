using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    public partial class BrowseSpamSuspects : AdminPageBase
    {
        private string CommandName
        {
            get { return (string) ViewState["CommandName"]; }
            set { ViewState["CommandName"] = value; }
        }

        private int UsersPerPage
        {
            get { return Convert.ToInt32(ddUsersPerPage.SelectedValue); }
        }

        private string ManagedUsername
        {
            get { return ViewState["MessageRecipient"] != null ? (string) ViewState["MessageRecipient"] : null; }
            set { ViewState["MessageRecipient"] = value; }
        }

        public int CurrentPage
        {
            set
            {
                ViewState["CurrentPage"] = value;
                preparePaginator();
                loadResultsPage();
            }
            get
            {
                if (ViewState["CurrentPage"] == null
                    || (int)ViewState["CurrentPage"] == 0)
                    return 1;
                else
                    return (int)ViewState["CurrentPage"];
            }
        }

        public UserSearchResults Results
        {
            set
            {
                ViewState["SearchResults"] = value;
                CurrentPage = 1;
            }
            get
            {
                return (UserSearchResults)
                       ViewState["SearchResults"];
            }
        }

        public string SortField
        {
            get
            {
                if (ViewState["sortField"] == null)
                {
                    return "LastOnline";
                }
                else
                {
                    return (string)ViewState["sortField"];
                }
            }
            set { ViewState["sortField"] = value; }
        }

        public bool SortAsc
        {
            get
            {
                if (ViewState["sortAsc"] == null)
                {
                    return true;
                }
                else
                {
                    return Convert.ToBoolean(ViewState["sortAsc"]);
                }
            }
            set { ViewState["sortAsc"] = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.browseSpamSuspects;

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadStrings();
                populateDropDown();
                performSearch();
            }
        }

        private void loadStrings()
        {
            gvUsers.Columns[0].HeaderText = Lang.TransA("Username");
            gvUsers.Columns[1].HeaderText = Lang.TransA("Message");
            gvUsers.Columns[2].HeaderText = Lang.TransA("Action");

            lnkFirst.Text = Lang.TransA("[ First ]");
            lnkPrev.Text = Lang.TransA("[ Prev ]");
            lnkNext.Text = Lang.TransA("[ Next ]");
            lnkLast.Text = Lang.TransA("[ Last ]");

            btnDelete.Text = Lang.TransA("Delete");
            btnSendMessage.Text = Lang.TransA("Send");
            btnCancel.Text = Lang.TransA("Cancel");
        }

        private void preparePaginator()
        {
            if (Results == null || CurrentPage <= 1)
            {
                lnkFirst.Enabled = false;
                lnkPrev.Enabled = false;
            }
            else
            {
                lnkFirst.Enabled = true;
                lnkPrev.Enabled = true;
            }
            if (Results == null || CurrentPage >= Results.GetTotalPages(UsersPerPage))
            {
                lnkLast.Enabled = false;
                lnkNext.Enabled = false;
            }
            else
            {
                lnkLast.Enabled = true;
                lnkNext.Enabled = true;
            }
            if (Results != null && Results.Usernames.Length > 0)
            {
                int fromUser = (CurrentPage - 1) * UsersPerPage + 1;
                int toUser = CurrentPage * UsersPerPage;
                if (Results.Usernames.Length < toUser)
                    toUser = Results.Usernames.Length;

                lblPager.Text = String.Format(
                    Lang.TransA("Showing {0}-{1} from {2} total"),
                    fromUser, toUser, Results.Usernames.Length);
            }
            else
            {
                lblPager.Text = Lang.TransA("No Results");
            }
        }

        private void loadResultsPage()
        {
            preparePaginator();

            DataTable dtResults = new DataTable("SearchResults");

            if (Results != null)
            {
                User[] users = Results.GetPage(CurrentPage, UsersPerPage);

                dtResults.Columns.Add("Username");
                dtResults.Columns.Add("Message");

                foreach (User user in users)
                {
                    string message = MessagesSandBox.FetchOnlyOneSpamMessage(user.Username);

                    dtResults.Rows.Add(new object[]
                                     {
                                         user.Username, message ?? String.Empty
                                     });
                }
            }

            gvUsers.DataSource = dtResults;
            gvUsers.DataBind();
        }

        private void populateDropDown()
        {
            
            for (int i = 5; i <= 50; i += 5)
                ddUsersPerPage.Items.Add(i.ToString());
            ddUsersPerPage.SelectedValue = Config.AdminSettings.BrowseSpamSuspects.UsersPerPage.ToString();
        }

        private void performSearch()
        {
            BasicSearch search = new BasicSearch();
            search.active_isSet = false;
            search.hasAnswer_isSet = false;
            search.visible_isSet = false;
            search.SpamSuspected = true;

            search.SortColumn = SortField;
            search.SortAsc = SortAsc;

            Results = search.GetResults();

            loadResultsPage();
        }

        protected void ddUsersPerPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentPage = 0;
        }

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!HasWriteAccess)
                return;

            string username = (string) e.CommandArgument;

            Classes.User user = null;
            try
            {
                user = Classes.User.Load(username);
            }
            catch (NotFoundException)
            {
                return;
            }

            switch (e.CommandName)
            {
                case "DeleteUser" :
                    CommandName = e.CommandName;
                    ManagedUsername = user.Username;
                    mvSpamSuspects.SetActiveView(viewDeleteReason);
                    break;

                case "DeleteUserAndMessages":
                    CommandName = e.CommandName;
                    ManagedUsername = user.Username;
                    mvSpamSuspects.SetActiveView(viewDeleteReason);
                    break;

                case "MarkAsReviewed":
                    user.SpamSuspected = false;
                    user.Update();
                    MessagesSandBox.DeleteMessagesSandBox(user.Username, null);
                    performSearch();
                    break;

                case "SendMessage" :
                    ManagedUsername = username;
                    mvSpamSuspects.SetActiveView(viewSendMessage);
                    break;
            }
        }

        protected void gvUsers_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lnkDeleteUser = e.Row.FindControl("lnkDeleteUser") as LinkButton;
                LinkButton lnkDeleteUserAndMessages = e.Row.FindControl("lnkDeleteUserAndMessages") as LinkButton;
                LinkButton lnkMarkAsReviewed = e.Row.FindControl("lnkMarkAsReviewed") as LinkButton;
                LinkButton lnkSendMessage = e.Row.FindControl("lnkSendMessage") as LinkButton;

                if (lnkDeleteUser != null && lnkDeleteUserAndMessages != null && lnkMarkAsReviewed != null && lnkSendMessage != null)
                {
                    if (!HasWriteAccess)
                    {
                        lnkDeleteUser.Enabled = false;
                        lnkDeleteUserAndMessages.Enabled = false;
                        lnkMarkAsReviewed.Enabled = false;
                        lnkSendMessage.Enabled = false;
                    }
                }
            }
        }

        protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void btnSendMessage_Click(object sender, EventArgs e)
        {
            if (ManagedUsername != null)
            {
                string message = txtMessage.Text.Trim();

                if (message.Length > 0)
                {
                    Message.Send(Config.Users.SystemUsername, ManagedUsername, message, 0);
                    MessageBox.Show(Lang.TransA("Your message has been sent successfully!"), Misc.MessageType.Success);
                    mvSpamSuspects.SetActiveView(viewSpamSuspects);
                }
            }
        }

        protected void lnkFirst_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
                CurrentPage = 1;

            loadResultsPage();
        }

        protected void lnkPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
                CurrentPage--;

            loadResultsPage();
        }

        protected void lnkNext_Click(object sender, EventArgs e)
        {
            if (CurrentPage < Results.GetTotalPages(UsersPerPage))
                CurrentPage++;

            loadResultsPage();
        }

        protected void lnkLast_Click(object sender, EventArgs e)
        {
            if (CurrentPage < Results.GetTotalPages(UsersPerPage))
                CurrentPage = Results.GetTotalPages(UsersPerPage);

            loadResultsPage();
        }

        protected void gvUsers_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression.Length != 0)
            {
                if (e.SortExpression == SortField)
                {
                    SortAsc = !SortAsc;
                }

                SortField = e.SortExpression;
            }

            performSearch();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            mvSpamSuspects.SetActiveView(viewSpamSuspects);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (CommandName != null && CommandName != String.Empty && ManagedUsername != null)
            {
                if (CommandName == "DeleteUser")
                {
                    Classes.User.Delete(ManagedUsername, txtDeleteReason.Text);        
                }
                else if (CommandName == "DeleteUserAndMessages")
                {
                    Classes.User.Delete(ManagedUsername, txtDeleteReason.Text); 
                    int[] messagesIDs = Message.Search(0, ManagedUsername, Message.eFolder.None, null, Message.eFolder.None, 0, false, false,
                                   false, null, null);
                    foreach (int id in messagesIDs)
                    {
                        Message.Delete(id);
                    }
                    MessagesSandBox.DeleteMessagesSandBox(ManagedUsername, null);
                }
            }

            CommandName = null;
            performSearch();
            mvSpamSuspects.SetActiveView(viewSpamSuspects);
        }
    }
}

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
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    public partial class BrowseMessages : AdminPageBase
    {
        protected MessageBox MessageBox;

        private int MessagesPerPage
        {
            get { return Convert.ToInt32(dropMessagesPerPage.SelectedValue); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadStrings();
                PopulateFilters();
                PreparePaginator();

                if (Request.Params["uid"] != null && Request.Params["uid"] != String.Empty)
                {
                    txtFromUsername.Text = Request.Params["uid"];
                    btnSearch_Click(null, null);
                }
            }
        }

        private void PopulateFilters()
        {
            for (int i = 5; i <= 50; i += 5)
                dropMessagesPerPage.Items.Add(i.ToString());
            dropMessagesPerPage.SelectedValue = Config.AdminSettings.BrowseMessages.MessagesPerPage.ToString();
        }

        private void PopulateGrid()
        {
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.browseMessages;
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgMessages.ItemCreated += new DataGridItemEventHandler(dgUsers_ItemCreated);
            this.dgMessages.ItemCommand += new DataGridCommandEventHandler(dgUsers_ItemCommand);
            this.dgMessages.SortCommand += new DataGridSortCommandEventHandler(dgUsers_SortCommand);
            this.dropMessagesPerPage.SelectedIndexChanged +=
                new EventHandler(this.dropMessagesPerPage_SelectedIndexChanged);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.lnkFirst.Click += new EventHandler(lnkFirst_Click);
            this.lnkNext.Click += new EventHandler(lnkNext_Click);
            this.lnkPrev.Click += new EventHandler(lnkPrev_Click);
            this.lnkLast.Click += new EventHandler(lnkLast_Click);
        }

        #endregion

        private void dropMessagesPerPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentPage = 0;
        }

        private void LoadStrings()
        {
            btnSearch.Text = Lang.TransA("Search");
            btnGetCSV.Text = Lang.TransA("Download as CSV");

            dgMessages.Columns[0].HeaderText = Lang.TransA("Sender");
            dgMessages.Columns[1].HeaderText = Lang.TransA("Recipient");
            dgMessages.Columns[2].HeaderText = Lang.TransA("Message");
            dgMessages.Columns[3].HeaderText = Lang.TransA("Timestamp");

            lnkFirst.Text = Lang.TransA("[ First ]");
            lnkPrev.Text = Lang.TransA("[ Prev ]");
            lnkNext.Text = Lang.TransA("[ Next ]");
            lnkLast.Text = Lang.TransA("[ Last ]");
        }

        public MessageSearchResults Results
        {
            set
            {
                ViewState["SearchResults"] = value;
                CurrentPage = 1;
            }
            get
            {
                return (MessageSearchResults)
                       ViewState["SearchResults"];
            }
        }

        public int CurrentPage
        {
            set
            {
                ViewState["CurrentPage"] = value;
                PreparePaginator();
                LoadResultsPage();
            }
            get
            {
                if (ViewState["CurrentPage"] == null
                    || (int) ViewState["CurrentPage"] == 0)
                    return 1;
                else
                    return (int) ViewState["CurrentPage"];
            }
        }

        public bool PaginatorEnabled
        {
            set { pnlPaginator.Visible = value; }
        }

        private void PreparePaginator()
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
            if (Results == null || CurrentPage >= Results.GetTotalPages(MessagesPerPage))
            {
                lnkLast.Enabled = false;
                lnkNext.Enabled = false;
            }
            else
            {
                lnkLast.Enabled = true;
                lnkNext.Enabled = true;
            }
            if (Results != null && Results.Messages.Length > 0)
            {
                int fromUser = (CurrentPage - 1)*MessagesPerPage + 1;
                int toUser = CurrentPage*MessagesPerPage;
                if (Results.Messages.Length < toUser)
                    toUser = Results.Messages.Length;

                lblPager.Text = String.Format(
                    Lang.TransA("Showing {0}-{1} from {2} total"),
                    fromUser, toUser, Results.Messages.Length);
            }
            else
            {
                lblPager.Text = Lang.TransA("No Results");
            }
        }

        private void LoadResultsPage()
        {
            PreparePaginator();

            DataTable dtResults;
            if (Results != null)
            {
                Trace.Write("Loading page " + CurrentPage);
                Message[] messages = Results.GetPage(CurrentPage, MessagesPerPage);
                dtResults = FetchResultsDataTable(messages);
                tblHideSearch.Visible = true;
            }
            else
            {
                dtResults = new DataTable();
                tblHideSearch.Visible = false;
            }

            Trace.Write("Binding...");
            dgMessages.DataSource = dtResults;
            dgMessages.DataBind();
        }

        private static DataTable FetchResultsDataTable(Message[] messages)
        {
            DataTable dtResults = new DataTable("SearchResults");

            dtResults.Columns.Add("Id");
            dtResults.Columns.Add("FromUsername");
            dtResults.Columns.Add("FromGender");
            dtResults.Columns.Add("ToUsername");
            dtResults.Columns.Add("ToGender");
            dtResults.Columns.Add("Body");
            dtResults.Columns.Add("Timestamp");

            if (messages != null && messages.Length > 0)
                foreach (Message message in messages)
                {
                    dtResults.Rows.Add(new object[]
                                               {
                                                   message.Id,
                                                   message.FromUser.Username,
                                                   !Config.Users.DisableGenderInformation ? "(" + message.FromUser.Gender.ToString()[0] + ")" : String.Empty,
                                                   message.ToUser.Username,
                                                   !Config.Users.DisableGenderInformation ? "(" + message.ToUser.Gender.ToString()[0] + ")" : String.Empty,
                                                   message.Body,
                                                   message.Timestamp
                                               });
                }

            return dtResults;
        }

        private void lnkFirst_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
                CurrentPage = 1;

            LoadResultsPage();
        }

        private void lnkPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
                CurrentPage--;

            LoadResultsPage();
        }

        private void lnkNext_Click(object sender, EventArgs e)
        {
            if (CurrentPage < Results.GetTotalPages(MessagesPerPage))
                CurrentPage++;

            LoadResultsPage();
        }

        private void lnkLast_Click(object sender, EventArgs e)
        {
            if (CurrentPage < Results.GetTotalPages(MessagesPerPage))
                CurrentPage = Results.GetTotalPages(MessagesPerPage);

            LoadResultsPage();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void PerformSearch()
        {
            string fromUsername = null;
            if (txtFromUsername.Text.Trim().Length > 0)
                fromUsername = txtFromUsername.Text.Trim();
            string toUsername = null;
            if (txtToUsername.Text.Trim().Length > 0)
                toUsername = txtToUsername.Text.Trim();
            string keyword = null;
            if (txtKeyword.Text.Trim().Length > 0)
                keyword = txtKeyword.Text.Trim();

            /*
			search.SortColumn = SortField;
			search.SortAsc = SortAsc;
			*/

            if (Results == null) Results = new MessageSearchResults();
            Results.Messages = Message.Search(0, fromUsername, Message.eFolder.None, toUsername,
                                              Message.eFolder.None, 0, true, true, false, keyword, null);
            CurrentPage = 1;
            LoadResultsPage();

            btnGetCSV.Visible = Results.Messages.Length != 0;
        }

        private void dgUsers_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            LinkButton lnkDelete = (LinkButton) e.Item.FindControl("lnkDelete");

            if(!HasWriteAccess)
                lnkDelete.Enabled = false;
            else
                lnkDelete.Attributes.Add("onclick",
                                         String.Format("javascript: return confirm('{0}')",
                                                       Lang.TransA("Do you really want to delete this message?")));
        }

        private void dgUsers_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                if (!HasWriteAccess)
                    return;
                
                int messageId = Convert.ToInt32(e.CommandArgument);
                Message.Delete(messageId);
                PerformSearch();
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
                    return (string) ViewState["sortField"];
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

        private void dgUsers_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            if (e.SortExpression.Length != 0)
            {
                if (e.SortExpression == SortField)
                {
                    SortAsc = !SortAsc;
                }

                SortField = e.SortExpression;
            }

            PerformSearch();
        }

        protected void btnGetCSV_Click(object sender, EventArgs e)
        {
            if (Results != null)
            {
                Response.Clear();
                Response.ContentType = "application/text";
                Response.ContentEncoding = Encoding.UTF8;
                Response.Charset = Encoding.UTF8.EncodingName;
                Response.AppendHeader("content-disposition", "attachment; filename=results.csv");
                Response.Write(Parsers.ParseCSV(FetchResultsDataTable(Results.Get())));
                Response.End();
            }
        }
    }
}
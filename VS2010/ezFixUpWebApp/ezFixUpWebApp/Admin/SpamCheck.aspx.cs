/* ezFixUp 
 * Copyright (C) 2003-2010 eStream 
 * ezFixUp.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ezFixUp license agreement.  
 * It can be found at ezFixUp.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using ezFixUp.Classes;

namespace ezFixUp.Admin
{
    /// <summary>
    /// Summary description for SpamCheck.
    /// </summary>
    public partial class SpamCheck : AdminPageBase
    {
        protected MessageBox MessageBox;

        public SpamCheck()
        {
            RequiresAuthorization = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!Config.Users.MessageVerificationEnabled)
                {
                    StatusPageMessage = Lang.TransA("Spam check option is not currently switched on!\n You can do this from Settings at Site Management section.");
                    StatusPageMessageType = Misc.MessageType.Error;
                    Response.Redirect("~/Admin/ShowStatus.aspx");
                    return;    
                }

                LoadStrings();
                PopulateDropDown();
                PopulateDataGrid();
            }
        }

        private void PopulateDataGrid()
        {
            dgPendingMessages.PageSize = Convert.ToInt32(dropMessagesPerPage.SelectedValue);

            Message[] messages = Message.FetchNonApproved();

            if (messages == null || messages.Length == 0)
            {
                MessageBox.Show(Lang.TransA("There are no messages waiting for approval!"), Misc.MessageType.Error);
                dgPendingMessages.Visible = false;
                lblMessagesPerPage.Visible = false;
                dropMessagesPerPage.Visible = false;
            }
            else
            {
                bindMessages(messages);

                dgPendingMessages.Visible = true;
                lblMessagesPerPage.Visible = true;
                dropMessagesPerPage.Visible = true;
            }
        }

        private void bindMessages(Message[] messages)
        {
            dgPendingMessages.Columns[0].HeaderText = Lang.TransA("Sender");
            dgPendingMessages.Columns[1].HeaderText = Lang.TransA("Receiver");
            dgPendingMessages.Columns[2].HeaderText = Lang.TransA("Message");

            DataTable dtMessages = new DataTable("Messages");
            dtMessages.Columns.Add("FromUsername");
            dtMessages.Columns.Add("ToUsername");
            dtMessages.Columns.Add("Message");
            dtMessages.Columns.Add("MessageID");

            foreach (Message message in messages)
            {
                dtMessages.Rows.Add(new object[]
                                        {
                                            message.FromUser.Username,
                                            message.ToUser.Username,
                                            message.Body,
                                            message.Id
                                        }
                    );
            }

            dtMessages.DefaultView.Sort = "FromUsername";

            dgPendingMessages.DataSource = dtMessages;
            try
            {
                dgPendingMessages.DataBind();
            }
            catch (HttpException)
            {
                dgPendingMessages.CurrentPageIndex = 0;
                dgPendingMessages.DataBind();
            }
        }

        private void PopulateDropDown()
        {
            for (int i = 5; i <= 50; i += 5)
                dropMessagesPerPage.Items.Add(i.ToString());

            //HARDCODED
            dropMessagesPerPage.SelectedValue = 10.ToString();
        }

        private void LoadStrings()
        {
            lblMessagesPerPage.Text = Lang.TransA("Messages per page");
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.spamCheck;
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgPendingMessages.ItemCommand += new DataGridCommandEventHandler(dgPendingMessages_ItemCommand);
            this.dropMessagesPerPage.SelectedIndexChanged += new EventHandler(dropMessagesPerPage_SelectedIndexChanged);
            this.dgPendingMessages.PageIndexChanged +=
                new DataGridPageChangedEventHandler(dgPendingMessages_PageIndexChanged);
            this.dgPendingMessages.ItemDataBound += new DataGridItemEventHandler(dgPendingMessages_ItemDataBound);
            this.dgPendingMessages.ItemCreated += new DataGridItemEventHandler(dgPendingMessages_ItemCreated);
        }

        #endregion

        private void dgPendingMessages_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (!HasWriteAccess)
                return;
            
            if (e.CommandName == "Approve")
            {
                int id = Int32.Parse(e.CommandArgument.ToString());
                Message.ApproveMessage(id);

                Message message = Message.Fetch(id);
                message.ToUser.SendMessageNotification(message.FromUser.Username, message.Body);

                PopulateDataGrid();
            }
            else if (e.CommandName == "Reject")
            {
                int id = Int32.Parse(e.CommandArgument.ToString());
                Message.Delete(id);
                PopulateDataGrid();
            }
            else if (e.CommandName == "DeleteUser")
            {
                Classes.User.Purge(e.CommandArgument.ToString());
                PopulateDataGrid();
            }
        }

        private void dropMessagesPerPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgPendingMessages.PageSize = Convert.ToInt32(dropMessagesPerPage.SelectedValue);
            dgPendingMessages.CurrentPageIndex = 0;
            PopulateDataGrid();
        }

        private void dgPendingMessages_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            dgPendingMessages.CurrentPageIndex = e.NewPageIndex;
            PopulateDataGrid();
        }

        private void dgPendingMessages_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemIndex == -1)
                return;

            LinkButton lnkApprove = (LinkButton) e.Item.FindControl("lnkApprove");
            LinkButton lnkReject = (LinkButton) e.Item.FindControl("lnkReject");
            LinkButton lnkDeleteUser = (LinkButton) e.Item.FindControl("lnkDeleteUser");

            if (!HasWriteAccess)
            {
                lnkApprove.Enabled = false;
                lnkReject.Enabled = false;
                lnkDeleteUser.Enabled = false;
            }
        }

        private void dgPendingMessages_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            //created item is header or footer
            if (e.Item.ItemIndex == -1)
                return;

            LinkButton lnkReject = (LinkButton) e.Item.FindControl("lnkReject");
            LinkButton lnkDeleteUser = (LinkButton) e.Item.FindControl("lnkDeleteUser");

            lnkReject.Attributes.Add("onclick",
                                     String.Format("javascript: return confirm('{0}')",
                                                   Lang.TransA("Do you really want to reject this message?")));

            lnkDeleteUser.Attributes.Add("onclick",
                                         String.Format("javascript: return confirm('{0}')",
                                                       Lang.TransA("This user will be permanently deleted! Are you sure?")));
        }
    }
}
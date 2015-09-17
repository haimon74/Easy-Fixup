using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ezFixUp.Classes;

namespace ezFixUp.Admin
{
    public partial class ApproveAds : AdminPageBase
    {
        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.adsApproval;

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            if (!Config.Ads.Enable)
            {
                StatusPageMessage =
                    Lang.TransA(
                        "Classifieds option is not currently switched on!\n You can do this from Settings at Site Management section.");
                StatusPageMessageType = Misc.MessageType.Error;
                Response.Redirect("~/Admin/ShowStatus.aspx");
                return;
            }

            loadStrings();
            populateDropDown();
            populateDataGrid();
        }

        private void loadStrings()
        {
            lblGroupsPerPage.Text = Lang.TransA("Classifieds per page");

            dgPendingApproval.Columns[0].HeaderText = String.Empty;
            dgPendingApproval.Columns[1].HeaderText = Lang.TransA("Ads Details");
        }

        private void populateDropDown()
        {
            for (int i = 5; i <= 50; i += 5)
                ddAdsPerPage.Items.Add(i.ToString());
            ddAdsPerPage.SelectedValue = Config.AdminSettings.ApproveAds.AdsPerPage.ToString();
        }

        private void populateDataGrid()
        {
            dgPendingApproval.PageSize = Convert.ToInt32(ddAdsPerPage.SelectedValue);

            Ad[] ads = Ad.Fetch(false, Ad.eSortColumn.Date);

            if (ads.Length > 0)
            {
                DataTable dtAds = new DataTable("Ads");

                dtAds.Columns.Add("ID", typeof(int));
                dtAds.Columns.Add("CategoryID");
                dtAds.Columns.Add("CategoryTitle");
                dtAds.Columns.Add("PostedBy");
                dtAds.Columns.Add("AdPhotoID");
                dtAds.Columns.Add("Subject");
                dtAds.Columns.Add("Description");
                dtAds.Columns.Add("Approved");
                dtAds.Columns.Add("DateCreated");

                foreach (Ad ad in ads)
                {
                    AdsCategory category = AdsCategory.Fetch(ad.CategoryID);
                    Classes.AdPhoto[] photo = Classes.AdPhoto.FetchByAdID(ad.ID);
                    dtAds.Rows.Add(new object[]
                                       {
                                           ad.ID,
                                           category != null ? category.ID : 0,
                                           category != null ? category.Title : String.Empty,
                                           ad.PostedBy,
                                           photo.Length != 0 ? photo[0].ID : 0,
                                           Parsers.ProcessAdSubject(ad.Subject),
                                           Parsers.ProcessAdDescription(ad.Description),
                                           ad.Approved ? Lang.TransA("Yes") : Lang.TransA("No"),
                                           ad.Date
                                       });
                }

                dgPendingApproval.DataSource = dtAds;

                dgPendingApproval.Visible = true;
                lblGroupsPerPage.Visible = true;
                ddAdsPerPage.Visible = true;

                try
                {
                    dgPendingApproval.DataBind();
                }
                catch (HttpException)
                {
                    dgPendingApproval.CurrentPageIndex = 0;
                    dgPendingApproval.DataBind();
                }
            }
            else
            {
                MessageBox.Show(Lang.TransA("There are no classifieds waiting for approval!"), Misc.MessageType.Error);
                dgPendingApproval.Visible = false;
                lblGroupsPerPage.Visible = false;
                ddAdsPerPage.Visible = false;
            }
        }

        protected void dgPendingApproval_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (!HasWriteAccess)
                return;

            Ad ad = Ad.Fetch(Convert.ToInt32(e.CommandArgument));

            if (ad != null)
            {
                if (e.CommandName == "Approve")
                {
                    ad.Approved = true;
                    ad.Save();

                    try
                    {
                        User poster = Classes.User.Load(ad.PostedBy);
                        MiscTemplates.ApproveAdMessage approveAdTemplate =
                            new MiscTemplates.ApproveAdMessage(poster.LanguageId);
                        Message msg = new Message(Config.Users.SystemUsername, ad.PostedBy);
                        msg.Body = approveAdTemplate.Message.Replace("%%SUBJECT%%", ad.Subject);
                        msg.Send();
                    }
                    catch (NotFoundException ex)
                    {
                        Log(ex);
                    }
                }
                else if (e.CommandName == "Reject")
                {
                    string reason = String.Empty;
                    TextBox txtReason = (TextBox)e.Item.FindControl("txtReason");

                    try
                    {
                        User user = Classes.User.Load(ad.PostedBy);
                        MiscTemplates.RejectAdMessage rejectAdTemplate =
                            new MiscTemplates.RejectAdMessage(user.LanguageId);
                        Message msg = new Message(Config.Users.SystemUsername, ad.PostedBy);

                        reason = rejectAdTemplate.WithReasonMessage;

                        if (txtReason.Text.Trim() != String.Empty)
                        {
                            reason =
                                reason.Replace("%%REASON%%", txtReason.Text.Trim()).Replace("%%SUBJECT%%", ad.Subject);
                            msg.Body = reason;
                        }
                        else
                        {
                            msg.Body = rejectAdTemplate.WithNoReasonMessage.Replace("%%SUBJECT%%", ad.Subject);
                        }
                        msg.Send();
                    }
                    catch (NotFoundException ex)
                    {
                        Log(ex);
                    }

                    Ad.Delete(Convert.ToInt32(e.CommandArgument));
                }

                populateDataGrid();
            }
        }

        protected void dgPendingApproval_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemIndex == -1)
                return;

            Button btnApprove = (Button)e.Item.FindControl("btnApprove");
            Button btnReject = (Button)e.Item.FindControl("btnReject");
            btnApprove.Text = Lang.TransA("Approve");
            btnReject.Text = Lang.TransA("Reject");

            btnReject.Attributes.Add("onclick",
                                     String.Format("javascript: return confirm('{0}')",
                                                   Lang.TransA("Do you really want to reject this classified?")));
        }

        protected void dgPendingApproval_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemIndex == -1)
                return;

            Button btnApprove = (Button)e.Item.FindControl("btnApprove");
            Button btnReject = (Button)e.Item.FindControl("btnReject");

            if (!HasWriteAccess)
            {
                btnApprove.Enabled = false;
                btnReject.Enabled = false;
            }
        }

        protected void dgPendingApproval_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            dgPendingApproval.CurrentPageIndex = e.NewPageIndex;
            populateDataGrid();
        }

        protected void dgPendingApproval_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgPendingApproval.PageSize = Convert.ToInt32(ddAdsPerPage.SelectedValue);
            dgPendingApproval.CurrentPageIndex = 0;
            populateDataGrid();
        }
    }
}

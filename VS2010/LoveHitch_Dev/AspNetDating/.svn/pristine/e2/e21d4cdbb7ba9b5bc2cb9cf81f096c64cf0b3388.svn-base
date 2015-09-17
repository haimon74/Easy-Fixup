using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    public partial class AffiliateBanners : AdminPageBase
    {
        protected int EditedAffiliateBannerID
        {
            get { return Convert.ToInt32(ViewState["EditedAffiliateBannerID"]); }
            set { ViewState["EditedAffiliateBannerID"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!Config.Affiliates.Enable)
                {
                    StatusPageMessage =
                        Lang.TransA("Affiliates option is not currently switched on!\n You can do this from Settings at Site Management section.");
                    StatusPageMessageType = Misc.MessageType.Error;
                    Response.Redirect("~/Admin/ShowStatus.aspx");
                    return;
                }

                mvAffiliateBanners.SetActiveView(viewAffiliateBanners);
                loadStrings();
                loadAffiliateBanners();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.browseAffiliatesBanners;
            
            base.OnInit(e);
        }

        private void loadStrings()
        {
            dgGroups.Columns[0].HeaderText = Lang.TransA("Banner");
            dgGroups.Columns[1].HeaderText = Lang.TransA("Name");
            dgGroups.Columns[2].HeaderText = Lang.TransA("Manage");

            btnAddAffiliateBanner.Text = Lang.TransA("Add banner");
            if (!HasWriteAccess)
                btnUpload.Enabled = false;
            btnUpload.Text = Lang.TransA("Upload");
            btnUpdate.Text = Lang.TransA("Save");
            btnCancelUpload.Text = Lang.TransA("Cancel");
            btnCancelUpdate.Text = Lang.TransA("Cancel");

            ddDeleted.Items.Add(new ListItem(Lang.TransA("No")));
            ddDeleted.Items.Add(new ListItem(Lang.TransA("Yes")));
        }

        private void loadAffiliateBanners()
        {
            DataTable dtAffiliateBanners = new DataTable("AffiliateBanners");

            dtAffiliateBanners.Columns.Add("ID");
            dtAffiliateBanners.Columns.Add("Name");
            dtAffiliateBanners.Columns.Add("Deleted");

            AffiliateBanner[] affiliateBanners = AffiliateBanner.Fetch(false);

            if (affiliateBanners.Length > 0)
            {
                foreach (AffiliateBanner affiliateBanner in affiliateBanners)
                {
                    dtAffiliateBanners.Rows.Add(new object[]
                                                    {
                                                        affiliateBanner.ID, affiliateBanner.Name,
                                                        affiliateBanner.Deleted
                                                    });
                }
                
            }
            else
            {
                MessageBox.Show(Lang.TransA("There are no affiliate banners"), Misc.MessageType.Error);
            }

            dgGroups.DataSource = dtAffiliateBanners;
            dgGroups.DataBind();
            dgGroups.Visible = dtAffiliateBanners.Rows.Count > 0;
        }

        protected void btnAddAffiliateBanner_Click(object sender, EventArgs e)
        {
            mvAffiliateBanners.SetActiveView(viewUploadAffiliateBanner);
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess) return;

            if (txtName.Text.Trim().Length == 0)
            {
                MessageBox.Show(Lang.TransA("Please enter name!"), Misc.MessageType.Error);
                return;
            }

            AffiliateBanner affiliateBanner = new AffiliateBanner();

            affiliateBanner.Name = txtName.Text.Trim();

            if (fuAffiliateBannerImage.HasFile)
            {
                System.Drawing.Image image = null;
                try
                {
                    image = System.Drawing.Image.FromStream
                        (fuAffiliateBannerImage.PostedFile.InputStream);
                }
                catch
                {
                    MessageBox.Show(Lang.TransA("Invalid image!"), Misc.MessageType.Error);
                    return;
                }

                affiliateBanner.Image = image;
            }
            else
            {
                MessageBox.Show(Lang.TransA("Invalid image!"), Misc.MessageType.Error);
                return;
            }

            affiliateBanner.Save();

            loadAffiliateBanners();
            mvAffiliateBanners.SetActiveView(viewAffiliateBanners);
        }

        protected void dgGroups_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            AffiliateBanner affiliateBanner = AffiliateBanner.Fetch(Convert.ToInt32(e.CommandArgument));

            switch(e.CommandName)
            {
                case "Edit" :
                    if (affiliateBanner != null)
                    {
                        EditedAffiliateBannerID = affiliateBanner.ID;
                        txtNewName.Text = affiliateBanner.Name;
                        ddDeleted.SelectedIndex = affiliateBanner.Deleted ? 1 : 0;
                        mvAffiliateBanners.SetActiveView(viewEditAffiliateBanners);
                    }

                    break;

                case "Delete" :
                    if (affiliateBanner != null)
                    {
                        affiliateBanner.Deleted = true;
                        affiliateBanner.Save();
                        loadAffiliateBanners();
                    }

                    break;
            }
        }

        protected void dgGroups_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            LinkButton lnkEdit = e.Item.FindControl("lnkEdit") as LinkButton;
            LinkButton lnkDelete = e.Item.FindControl("lnkDelete") as LinkButton;

            if (lnkEdit != null && lnkDelete != null)
            {
                if (!HasWriteAccess)
                {
                    lnkDelete.Enabled = false;
                    lnkEdit.Enabled = false;
                }
                else
                    lnkDelete.Attributes.Add("onclick", String.Format("javascript: return confirm('{0}')",
                                                    Lang.TransA("Are you sure you want to delete this affiliate banner?")));
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtNewName.Text.Trim().Length == 0)
            {
                MessageBox.Show(Lang.TransA("Please enter name!"), Misc.MessageType.Error);
                return;
            }

            AffiliateBanner affiliateBanner = AffiliateBanner.Fetch(EditedAffiliateBannerID);

            if (affiliateBanner != null)
            {
                affiliateBanner.Name = txtNewName.Text.Trim();

                affiliateBanner.Deleted = ddDeleted.SelectedIndex == 0 ? false : true;

                if (fuNewBannerImage.HasFile)
                {
                    System.Drawing.Image image = null;
                    try
                    {
                        image = System.Drawing.Image.FromStream
                            (fuNewBannerImage.PostedFile.InputStream);
                    }
                    catch
                    {
                        MessageBox.Show(Lang.TransA("Invalid image!"), Misc.MessageType.Error);
                        return;
                    }

                    affiliateBanner.Image = image;
                }
                else
                {
                    MessageBox.Show(Lang.TransA("Invalid image!"), Misc.MessageType.Error);
                    return;
                }

                affiliateBanner.Save();

                loadAffiliateBanners();
                mvAffiliateBanners.SetActiveView(viewAffiliateBanners);
            }
        }

        protected void btnCancelUpdate_Click(object sender, EventArgs e)
        {
            mvAffiliateBanners.SetActiveView(viewAffiliateBanners);
        }

        protected void btnCancelUpload_Click(object sender, EventArgs e)
        {
            mvAffiliateBanners.SetActiveView(viewAffiliateBanners);
        }
    }
}

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using ezFixUp.Classes;

namespace ezFixUp.Admin
{
    public partial class ManageUserLevels : AdminPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!Config.UserScores.EnableUserLevels)
                {
                    StatusPageMessage = Lang.TransA("User Levels option is not currently switched on!\n You can do this from Settings at \"User Scores and Levels\" section.");
                    StatusPageMessageType = Misc.MessageType.Error;
                    Response.Redirect("~/Admin/ShowStatus.aspx");
                    return;
                }
                
                if (!HasWriteAccess)
                {
                    btnAddNewLevel.Enabled = false;
                    btnCreateUpdate.Enabled = false;
                }

                LoadStrings();
                PopulateDataGrid();
                pnlUserLevelInfo.Visible = false;
            }

            Reflection.GenerateSettingsTable(phLevelRestrictions, typeof(UserLevelRestrictions));
        }

        private void PopulateDataGrid()
        {
            DataTable dtUserLevels = new DataTable("UserLevels");

            dtUserLevels.Columns.Add("ID");
            dtUserLevels.Columns.Add("Level");
            dtUserLevels.Columns.Add("IconURL");
            dtUserLevels.Columns.Add("Name");
            dtUserLevels.Columns.Add("MinScore");

            UserLevel[] levels = UserLevel.LoadAll();

            foreach (UserLevel level in levels)
            {
                dtUserLevels.Rows.Add(new object[]
                                            {
                                                level.Id,
                                                level.LevelNumber,
                                                level.GetIconUrl(),
                                                level.Name,
                                                level.MinScore
                                            });
            }

            dgUserLevels.DataSource = dtUserLevels;
            dgUserLevels.DataBind();
        }

        private void LoadStrings()
        {
            btnAddNewLevel.Text = Lang.TransA("Add New Level");

            dgUserLevels.Columns[0].HeaderText = Lang.TransA("Level");
            dgUserLevels.Columns[1].HeaderText = Lang.TransA("Icon");
            dgUserLevels.Columns[2].HeaderText = Lang.TransA("Name");
            dgUserLevels.Columns[3].HeaderText = Lang.TransA("Minimum Score");
            dgUserLevels.Columns[4].HeaderText = Lang.TransA("Actions");            
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.userLevels;
            base.OnInit(e);
        }

        protected void dgUserLevels_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            int levelID = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditLevel")
            {
                UserLevelRestrictions restrictions;
                pnlUserLevelInfo.Visible = true;

                hidCurrentLevelID.Value = levelID.ToString();
                btnCreateUpdate.Text = Lang.TransA("Update");
                btnCancel.Text = Lang.TransA("Cancel");

                UserLevel level = UserLevel.Load(levelID);
                txtName.Text = level.Name;
                txtMinScore.Text = level.MinScore.ToString();
                restrictions = level.Restrictions;

                phLevelRestrictions.Controls.Clear();
                Reflection.GenerateSettingsTableFromObject(phLevelRestrictions, restrictions);
            }
            else if (e.CommandName == "DeleteLevel")
            {
                if (!HasWriteAccess)
                    return;

                if (hidCurrentLevelID.Value != "" && Convert.ToInt32(hidCurrentLevelID.Value) == levelID)
                {
                    hidCurrentLevelID.Value = "";
                    pnlUserLevelInfo.Visible = false;
                }

                UserLevel.Delete(levelID);
                PopulateDataGrid();
            }
        }

        protected void dgUserLevels_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");

            if (!HasWriteAccess)
                lnkDelete.Enabled = false;
            else
            {
                lnkDelete.Attributes.Add("onclick",
                                         String.Format("javascript: return confirm('{0}')",
                                                       Lang.TransA("Do you really want to delete this level?")));                
            }
        }

        protected void btnAddNewLevel_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            pnlUserLevelInfo.Visible = true;
            hidCurrentLevelID.Value = "";
            txtName.Text = String.Empty;
            txtMinScore.Text = String.Empty;

            btnCreateUpdate.Text = Lang.TransA("Save");
            btnCancel.Text = Lang.TransA("Cancel");

            phLevelRestrictions.Controls.Clear();
            Reflection.GenerateSettingsTable(phLevelRestrictions, typeof(UserLevelRestrictions));
        }

        protected void btnCreateUpdate_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            if (!ValidatePlanInfo())
                return;

            UserLevelRestrictions restrictions = 
                (UserLevelRestrictions)Reflection.SaveTableSettings(phLevelRestrictions, typeof(UserLevelRestrictions));

            if (hidCurrentLevelID.Value == "")
            {
                //create new level
                UserLevel level = new UserLevel(txtName.Text, Convert.ToInt32(txtMinScore.Text));

                level.Restrictions = restrictions;
                level.Save();
            }
            else
            {
                //update current one
                UserLevel level = UserLevel.Load(Convert.ToInt32(hidCurrentLevelID.Value));

                level.Name = txtName.Text;
                level.MinScore = Convert.ToInt32(txtMinScore.Text);
                level.Restrictions = restrictions;
                level.Save();
            }

            hidCurrentLevelID.Value = "";
            pnlUserLevelInfo.Visible = false;
            PopulateDataGrid();
        }

        private bool ValidatePlanInfo()
        {
            if (txtName.Text.Trim() == "")
            {
                MessageBox1.Show(Lang.TransA("Name field cannot be empty!"), Misc.MessageType.Error);
                return false;
            }

            int minscore;
            if (!Int32.TryParse(txtMinScore.Text, out minscore))
            {
                MessageBox1.Show(Lang.TransA("Minimum score should be non-negative integer value"), Misc.MessageType.Error);
                return false;
            }

            if (minscore < 0)
            {
                MessageBox1.Show(Lang.TransA("Minimum score should be non-negative!"), Misc.MessageType.Error);
                return false;
            }

            return true;
        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            hidCurrentLevelID.Value = "";
            pnlUserLevelInfo.Visible = false;
        }
    }
}

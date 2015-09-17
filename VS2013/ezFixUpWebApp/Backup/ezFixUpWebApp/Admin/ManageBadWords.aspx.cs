using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ezFixUp.Classes;

namespace ezFixUp.Admin
{
    public partial class ManageBadWords : AdminPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadStrings();
            }
        }
        
        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.manageBadWords;
            
            base.OnInit(e);
        }
        
        private void loadStrings()
        {
            if (!HasWriteAccess)
                btnSave.Enabled = false;
            
            btnSave.Text = Lang.TransA("Save");
            txtBadWords.Text = Config.Misc.BadWords;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;
            
            string badWords = txtBadWords.Text.Trim().Replace("\r", "");
            Config.Misc.EnableBadWordsRegularExpressions = cbUserRegularExpressions.Checked;

            if (cbUserRegularExpressions.Checked)
            {
                foreach (string badword in badWords.Split('\n'))
                {
                    try
                    {
                        new Regex(badword);
                    }
                    catch (ArgumentException)
                    {
                        MessageBox.Show(String.Format(Lang.TransA("{0} is invalid regular expression"), badword), Misc.MessageType.Error);
                        return;
                    }
                }
            }

            Config.Misc.BadWords = badWords;

            MessageBox.Show(Lang.TransA("Bad words has been saved successfully"), Misc.MessageType.Success);
        }
    }
}

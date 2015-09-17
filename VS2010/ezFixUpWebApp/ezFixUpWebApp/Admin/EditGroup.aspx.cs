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
using ezFixUp.Classes;

namespace ezFixUp.Admin
{
    public partial class EditGroup : AdminPageBase
    {
        public int GroupID
        {
            get
            {
                if (ViewState["CurrentGroupID"] != null)
                {
                    return (int) ViewState["CurrentGroupID"];
                }
                else
                {
                    return -1;
                }
            }

            set
            {
                ViewState["CurrentGroupID"] = value;
            }
        }

        /// <summary>
        /// Gets the group from DB and saves it in 'ViewState'.
        /// If the group doesn't exist it returns NULL.
        /// </summary>
        /// <value>The group.</value>
        public Group Group
        {
            get
            {
                if (ViewState["CurrentGroup"] == null)
                {
                    ViewState["CurrentGroup"] = Group.Fetch(GroupID);
                }

                return ViewState["CurrentGroup"] as Group;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.editGroups;

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int groupID;
                if (Int32.TryParse(Request.Params["id"], out groupID))
                {
                    GroupID = groupID;
                    EditGroupCtrl1.GroupID = groupID;
                }
                else
                {
                    return;
                }

                if (Group == null) return;
            }
        }
    }
}

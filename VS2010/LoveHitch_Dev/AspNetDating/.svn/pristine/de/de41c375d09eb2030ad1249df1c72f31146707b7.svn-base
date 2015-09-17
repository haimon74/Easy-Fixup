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

namespace AspNetDating.Components.Groups
{
    public partial class NewEvents : System.Web.UI.UserControl
    {
        public int GroupID
        {
            get
            {
                if (ViewState["CurrentGroupId"] != null)
                {
                    return (int)ViewState["CurrentGroupId"];
                }
                else
                {
                    throw new Exception("The field groupID is not set!");
                }
            }
            set
            {
                ViewState["CurrentGroupId"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadStrings();
                loadEvents();
            }
        }

        private void loadStrings()
        {
            LargeBoxStart1.Title = Lang.Trans("Upcoming events");
            lnkMore.Text = Lang.Trans("View calendar");
        }

        private void loadEvents()
        {
            DataTable dtEvents = new DataTable("GroupEvents");

            dtEvents.Columns.Add("ID");
            dtEvents.Columns.Add("GroupID");
            dtEvents.Columns.Add("Title");
            dtEvents.Columns.Add("Date");
            dtEvents.Columns.Add("Username");
            dtEvents.Columns.Add("Attenders");

            GroupEvent[] groupEvents =
                GroupEvent.Fetch(Config.Groups.MaxGroupEventsOnGroupHomePage, GroupID, DateTime.Now.Date, null, GroupEvent.eSortColumn.Date);

            if (groupEvents.Length > 0)
            {
                int events = GroupEvent.Count(GroupID, DateTime.Now.Date, null);

                if (events > Config.Groups.MaxGroupEventsOnGroupHomePage)
                {
                    pnlMore.Visible = true;
                }

                foreach (GroupEvent groupEvent in groupEvents)
                {
                    dtEvents.Rows.Add(new object[]
                                          {
                                              groupEvent.ID,
                                              groupEvent.GroupID,
                                              groupEvent.Title,
                                              groupEvent.Date.ToString(),
                                              groupEvent.Username,
                                              GroupEvent.GetAttenders(groupEvent.ID.Value).Length
                                          });
                }
            }
            else
            {
                lblError.Text = Lang.Trans("There are no upcoming events.");
            }

            rptGroupEvents.DataSource = dtEvents;
            rptGroupEvents.DataBind();
            rptGroupEvents.Visible = dtEvents.Rows.Count > 0;
        }

        protected void lnkMore_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlRewrite.CreateShowGroupEventsUrl(GroupID.ToString()));
        }
    }
}
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ezFixUp.Classes;

namespace ezFixUp.Components.Groups
{
    public partial class ViewEvents : System.Web.UI.UserControl
    {
        public int GroupID
        {
            get
            {
                if (ViewState["CurrentGroupId"] != null)
                {
                    return (int)ViewState["CurrentGroupId"];
                }
                throw new Exception("The field groupID is not set!");
            }
            set
            {
                ViewState["CurrentGroupId"] = value;
            }
        }

        public DataTable GroupEvents
        {
            get { return (DataTable) ViewState["Events"]; }
            set { ViewState["Events"] = value; }
        }

        public DateTime? EventDate
        {
            get { return (DateTime?)ViewState["EventDate"]; }
            set { ViewState["EventDate"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadStrings();
                setEventDates();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            loadEvents();
        }

        private void loadStrings()
        {
            LargeBoxStart1.Title = Lang.Trans("Group Events");
            calendar1.SelectedDate = DateTime.Now.Date;
            lblDate.Text = calendar1.SelectedDate.ToShortDateString();
        }

        private void loadEvents()
        {
            DataTable dtEvents = new DataTable("Group Events");
            dtEvents.Columns.Add("ID");
            dtEvents.Columns.Add("GroupID");
            dtEvents.Columns.Add("Title");
            dtEvents.Columns.Add("Date");
            dtEvents.Columns.Add("Username");
            dtEvents.Columns.Add("Attenders");

            GroupEvent[] groupEvents = GroupEvent.Fetch(GroupID, calendar1.SelectedDate, GroupEvent.eSortColumn.Date);

            if (groupEvents.Length > 0)
            {
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
                lblError.Text = Lang.Trans("There are no group events for this date.");
            }

            rptEvents.DataSource = dtEvents;
            rptEvents.DataBind();
        }

        protected void calendar1_SelectionChanged(object sender, EventArgs e)
        {
            lblDate.Text = calendar1.SelectedDate.ToShortDateString();
            EventDate = calendar1.SelectedDate;
        }

        private void setEventDates()
        {
            DataTable dtEvents = new DataTable("Group Events");
            DataColumn[] primaryKeys = new DataColumn[1];
            DataColumn primaryKey = new DataColumn("ID");
            dtEvents.Columns.Add(primaryKey);
            dtEvents.Columns.Add("Date", typeof(DateTime));
            primaryKeys[0] = primaryKey;
            dtEvents.PrimaryKey = primaryKeys;

            GroupEvent[] groupEvents = null;
            try
            {
                groupEvents = GroupEvent.FetchByGroupID(GroupID);
            }
            catch(Exception)
            {
                Response.Redirect("~/default.aspx");
                return;
            }

            if (groupEvents.Length > 0)
            {
                foreach (GroupEvent groupEvent in groupEvents)
                {
                    dtEvents.Rows.Add(new object[]
                                          {
                                              groupEvent.ID,
                                              groupEvent.Date.ToShortDateString(),
                                          });
                }
            }

            GroupEvents = dtEvents;
        }

        protected void calendar1_DayRender(object sender, DayRenderEventArgs e)
        {
            DataRow[] rows =
                GroupEvents.Select(
                    String.Format("Date >= #{0}# AND Date < #{1}#", e.Day.Date.ToString(CultureInfo.InvariantCulture),
                                  e.Day.Date.AddDays(1).ToString(CultureInfo.InvariantCulture)));

            if (rows != null && rows.Length > 0) e.Cell.CssClass = "eventselected";
        }
    }
}
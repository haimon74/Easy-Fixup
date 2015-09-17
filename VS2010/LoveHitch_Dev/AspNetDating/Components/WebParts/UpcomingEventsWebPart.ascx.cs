using System;
using System.Data;
using AspNetDating.Classes;

namespace AspNetDating.Components.WebParts
{
    public partial class UpcomingEventsWebPart : WebPartUserControl
    {
        protected UserSession CurrentUserSession
        {
            get { return ((PageBase)Page).CurrentUserSession; }
        }

        private bool? ControlLoaded
        {
            get
            {
                return ViewState["ControlLoaded"] as bool?;
            }

            set
            {
                ViewState["ControlLoaded"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = Lang.Translate("Upcoming Events");
            if (!IsPostBack)
            {
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!ControlLoaded.HasValue)
            {
                loadEvents();

                ControlLoaded = true;
            }
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

            int[] groupEventsIDs =
                GroupEvent.Search(null, null, null, null, DateTime.Now.Date, null, CurrentUserSession.Username,
                                  Config.Groups.MaxGroupEventsOnHomePage, GroupEvent.eSortColumn.Date);

            if (groupEventsIDs.Length > 0)
            {
                GroupEvent groupEvent = null;
                foreach (int id in groupEventsIDs)
                {
                    groupEvent = GroupEvent.Fetch(id);
                    if (groupEvent != null)
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

                rptGroupEvents.DataSource = dtEvents;
                rptGroupEvents.DataBind();
            }
            else
            {
                mvUpcomingEvents.SetActiveView(viewNoUpcomingEvents);
            }

            rptGroupEvents.Visible = dtEvents.Rows.Count > 0;
        }
    }
}
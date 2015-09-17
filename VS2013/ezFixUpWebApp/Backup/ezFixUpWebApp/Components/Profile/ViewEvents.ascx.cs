using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ezFixUp.Classes;

namespace ezFixUp.Components.Profile
{
    public partial class ViewEvents : UserControl
    {
        private User user;

        public User User
        {
            set
            {
                user = value;
                ViewState["Username"] = user != null ? user.Username : null;
            }
            get
            {
                if (user == null
                    && ViewState["Username"] != null)
                    user = User.Load((string)ViewState["Username"]);
                return user;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStrings();
            }
            else LoadEvents();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!IsPostBack) LoadEvents();
        }

        private void LoadStrings()
        {
            LargeBoxStart1.Title = "Events".Translate();
        }

        private void LoadEvents()
        {
            List<Control> eventControls = Event.PrepareEventsControls(User.Username, null, -1);

            foreach (var eventCtrl in eventControls)
            {
                plhEvents.Controls.Add(eventCtrl);
            }

            if (eventControls.Count == 0)
            {
                lblError.Text = Lang.Trans("There are no user events!");
            }
        }
    }
}
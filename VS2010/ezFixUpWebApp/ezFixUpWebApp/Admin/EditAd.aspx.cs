using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ezFixUp.Admin
{
    public partial class EditAd : AdminPageBase
    {
        public int AdID
        {
            get
            {
                if (ViewState["AdID"] != null)
                {
                    return (int)ViewState["AdID"];
                }
                else
                {
                    return -1;
                }
            }

            set
            {
                ViewState["AdID"] = value;
            }
        }

        public Classes.Ad Ad
        {
            get
            {
                if (ViewState["CurrentAd"] == null)
                {
                    ViewState["CurrentAd"] = Classes.Ad.Fetch(AdID);
                }

                return ViewState["CurrentAd"] as Classes.Ad;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.editAds;

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            int adID;
            if (Int32.TryParse(Request.Params["id"], out adID))
            {
                AdID = adID;
                EditAdCtrl1.AdID = adID;
            }
            else
            {
                return;
            }

            if (Ad == null) return;
        }
    }
}

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ezFixUp.Classes;
using PayPal.Payments.DataObjects;

namespace ezFixUp.Affiliates
{
        public class AffiliatePageBase : System.Web.UI.Page
        {
            public bool RequiresAuthorization = true;

            public AffiliateSession CurrentAffiliateSession
            {
                get { return (Global.GetSessionState().ContainsKey("CurrentAffiliateSession"))
                    ? Global.GetSessionState()["CurrentAffiliateSession"] as AffiliateSession : null; }
                set { Global.GetSessionState()["CurrentAffiliateSession"] = value; }
            }

            protected override void OnInit(EventArgs e)
            {
                if (RequiresAuthorization)
                {
                    ProcessValidation();
                }

                base.OnInit(e);
            }

            public string StatusPageMessage
            {
                set { Global.GetSessionState()["ShowStatus_Message"] = value; }
                get
                {
                    if (Global.GetSessionState()["ShowStatus_Message"] == null)
                        return null;
                    else
                        return (string)Global.GetSessionState()["ShowStatus_Message"];
                }
            }

            protected void ProcessValidation()
            {
                if (!IsUserAuthorized())
                {
                    Response.Redirect("Login.aspx?back_url=" + Server.UrlEncode(Request.Url.AbsoluteUri));
                }
            }

            public bool IsUserAuthorized()
            {
                if (CurrentAffiliateSession == null)
                    return false;

                return CurrentAffiliateSession.IsAuthorized;
            }

            public void Log(Exception ex)
            {
                ExceptionLogger.Log(Request, ex);
            }
        }
}

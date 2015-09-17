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
    public partial class ShowStatus : AdminPageBase
    {
        public ShowStatus()
        {
            RequiresAuthorization = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (StatusPageMessage != null && StatusPageMessageType != null)
            {
                MessageBox.Show(StatusPageMessage, StatusPageMessageType.Value);
                StatusPageMessage = null;
                StatusPageMessageType = null;
            }
        }
    }
}

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

namespace AspNetDating.Affiliates
{
    public partial class MessageBox : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private string cssClass;

        public string CssClass
        {
            get { return cssClass; }
            set { cssClass = value; }
        }

        public void Show(string message, Classes.Misc.MessageType messageType)
        {
            divMessage.InnerHtml = message;
            switch (messageType)
            {
                case Classes.Misc.MessageType.Error:
                    divMessage.Attributes.Add("class", "msgboxerror");
                    break;
                case Classes.Misc.MessageType.Success:
                    divMessage.Attributes.Add("class", "msgboxsuccess");
                    break;
            }
        }
    }
}
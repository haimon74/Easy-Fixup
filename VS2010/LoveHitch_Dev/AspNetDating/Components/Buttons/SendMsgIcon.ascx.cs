using System;


namespace AspNetDating.Components.Buttons
{
    public partial class SendMsgIcon : System.Web.UI.UserControl
    {
        public string Tip { set { this.btnIcon.ToolTip = value; } }
        public string Username { set { this.btnIcon.ToUsername = value; } }
        public GenericIcon.IconSizeEnum Size { set { this.btnIcon.IconSize = value; } }
        public GenericIcon.TipGravityEnum Gravity { set { this.btnIcon.TipGravity = value; } }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}
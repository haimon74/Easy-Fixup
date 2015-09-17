using System;
using AspNetDating.Classes;

namespace AspNetDating.Components.Buttons
{
    public partial class GenericIcon : System.Web.UI.UserControl
    {
        public enum IconSizeEnum
        {
            Mini, Small, Medium, Big
        } ;

        public enum TipGravityEnum
        {
            n,s,e,w
        } ;

        public TipGravityEnum TipGravity { get; set; }
        public IconSizeEnum IconSize { get; set; }
        public string IconBaseCss { get; set; }
        public string ToUsername { get; set; }
        public string UrlTemplate { get; set; }
        public string ToolTip { set { this.fbIcon.ToolTip = value.Translate(); } get { return this.fbIcon.ToolTip ?? "Tool tip is empty".Translate(); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.fbIcon.CssClass = String.Format("{0}{1} tipsy{2}", IconBaseCss, IconSize, TipGravity);
            this.fbIcon.Click += ActionWhenClick;
        }

        protected virtual void ActionWhenClick(object sender, EventArgs e)
        {
            Response.Redirect(String.Format(UrlTemplate, ToUsername ?? (string)Session["ToUsername"]));
        }
    }
}
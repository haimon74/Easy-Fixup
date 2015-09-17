using System;
using System.Web.UI.WebControls;

namespace AspNetDating.Components
{
    public partial class ModalPopup : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        public int ModalHeight
        {
            set 
            { 
                Panel1.Height = Unit.Pixel(value);
                PanelContent.Height = Unit.Pixel(value - 150);
            }
        }
        public string FunctionName { get; set; }
        public string TitleText { get; set; }
        public string LinkText { get; set; }
    }
}
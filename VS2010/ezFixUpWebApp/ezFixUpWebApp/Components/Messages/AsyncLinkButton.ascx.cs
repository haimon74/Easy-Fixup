using System;
using System.Web.UI.WebControls;
using ezFixUp.Classes;


namespace ezFixUp.Components.Messages
{
    public partial class AsyncLinkButton : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lbAsync1.Text = String.Format("<span>{0}</span>", Lang.Translate(Title));
        }

        public delegate void LinkButtonCallbackDelegate(int categoryId);
        
        public LinkButtonCallbackDelegate LinkButtonClickCallback { set; get; }
        
        public string Title { get; set; }
        public int CategoryId { get; set; }

        public LinkButton GetLinkButton()
        {
            return this.lbAsync1;
        }
        protected void lbAsync_Click(object sender, EventArgs e)
        {
            LinkButtonClickCallback(CategoryId);
        }
    }
}
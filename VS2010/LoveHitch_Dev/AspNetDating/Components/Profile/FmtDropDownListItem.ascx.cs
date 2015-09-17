using System;
using System.Web.UI.WebControls;
using AspNetDating.Classes;


namespace AspNetDating.Components.Profile
{
    public partial class FmtDropDownListItem : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lnkEditTopic1.Text = String.Format("<span>{0}</span>", Lang.Translate(TopicName));
        }

        public delegate void ListItemCallbackDelegate(string topicName);
        public delegate void ListItemOnClickEventHandler(object sender, EventArgs e);
        public ListItemCallbackDelegate ListItemClickCallback { set; get; }
        public ListItemOnClickEventHandler ListItemOnClickEventHandlerFunction { set; get; }
        public string TopicName { get; set; }
        public LinkButton GetLinkButton()
        {
            return this.lnkEditTopic1;
        }
        protected void lnkEditTopic_Click(object sender, EventArgs e)
        {
            //ListItemOnClickEventHandlerFunction(sender, e);
            ListItemClickCallback(TopicName);
        }
    }
}
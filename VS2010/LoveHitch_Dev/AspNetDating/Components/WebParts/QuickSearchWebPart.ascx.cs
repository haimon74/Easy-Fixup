using System;
using AspNetDating.Classes;

namespace AspNetDating.Components.WebParts
{
    public partial class QuickSearchWebPart : SearchBoxWebPart
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = Lang.Translate("Quick Search");
        }
    }
}
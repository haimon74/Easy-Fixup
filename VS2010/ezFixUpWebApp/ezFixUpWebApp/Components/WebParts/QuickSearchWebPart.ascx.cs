using System;
using ezFixUp.Classes;

namespace ezFixUp.Components.WebParts
{
    public partial class QuickSearchWebPart : SearchBoxWebPart
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = Lang.Translate("Quick Search");
        }
    }
}
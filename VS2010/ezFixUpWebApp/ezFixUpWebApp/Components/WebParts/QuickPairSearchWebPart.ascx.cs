using System;
using ezFixUp.Classes;

namespace ezFixUp.Components.WebParts
{
    public partial class QuickPairSearchWebPart : SearchBoxWebPart
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = Lang.Translate("Start to Pair");
            
        }
    }
}
using System;
using ezFixUp.Classes;

namespace ezFixUp
{
    public partial class Agreement : PageBase
    {
        public Agreement()
        {
            RequiresAuthorization = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            cvAgreement.Text = "PLACE YOUR AGREEMENT HERE".Translate();
        }
    }
}

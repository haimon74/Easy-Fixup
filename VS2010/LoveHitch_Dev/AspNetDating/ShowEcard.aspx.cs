using System;
using AspNetDating.Classes;

namespace AspNetDating
{
    public partial class ShowEcard : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string ecID = Request.Params["ecid"];

                if (String.IsNullOrEmpty(ecID))
                {
                    Response.Redirect("~/Default.aspx");
                    return;
                }

                loadStrings();
                loadEcard(ecID);
            }
        }

        private void loadStrings()
        {
            LargeBoxStart1.Title = "E-card".Translate();
        }

        private void loadEcard(string ecID)
        {
            Ecard ecard = Ecard.Fetch(Convert.ToInt32(ecID));

            if (ecard != null)
            {
                if (ecard.FromUsername != CurrentUserSession.Username && ecard.ToUsername != CurrentUserSession.Username)
                {
                    Response.Redirect("~/Default.aspx");
                    return;
                }

                EcardType ecardType = EcardType.Fetch(Convert.ToInt32(ecard.EcardTypeID));

                if (ecardType != null)
                {
                    var canRead = CurrentUserSession.CanReadEmail();
                    lblEcardTypeName.Text = ecardType.Name;
                    if (canRead == PermissionCheckResult.No)
                        lblEcardMessage.Visible = false;

                    lblEcardMessage.Text = canRead == PermissionCheckResult.Yes || canRead == PermissionCheckResult.YesWithCredits ||
                                            canRead == PermissionCheckResult.YesButMoreCreditsNeeded
                                               ? ecard.Message
                                               : "Upgrade to read this message".Translate();

                    if (ecard.ToUsername == CurrentUserSession.Username && !ecard.IsOpened)
                    {
                        ecard.IsOpened = true;
                        ecard.Save();    
                    }

                    pnlImage.Visible = ecardType.Type == EcardType.eType.Image || ecardType.Type == EcardType.eType.Gift;
                    pnlFlash.Visible = !pnlImage.Visible;
                }
            }
        }
    }
}

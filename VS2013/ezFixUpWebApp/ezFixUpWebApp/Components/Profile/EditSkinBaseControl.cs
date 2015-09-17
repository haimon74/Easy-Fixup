using System.Web.UI;

namespace ezFixUp.Components.Profile
{
    public abstract class EditSkinBaseControl : UserControl
    {
        public string Key
        {
            get
            {
                return ViewState["Key"] as string;
            }
            set
            {
                ViewState["Key"] = value;
            }
        }

        public abstract string GetStyleLine();
        public abstract void SetStyleLine(string line);
    }
}
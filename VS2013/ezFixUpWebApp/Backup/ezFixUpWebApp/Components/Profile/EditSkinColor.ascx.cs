using System;
using System.Text.RegularExpressions;

namespace ezFixUp.Components.Profile
{
    public partial class EditSkinColor : EditSkinBaseControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected override void OnPreRender(EventArgs e)
        {
            LoadStrings();
            base.OnPreRender(e);
        }

        private void LoadStrings()
        {
        }

        public override string GetStyleLine()
        {
            if (txtColor.Text.Trim().Length > 0)
                return "color: " + "#" + txtColor.Text + ";";
            return "color: inherit;";
        }

        public override void SetStyleLine(string line)
        {
            var match = Regex.Match(line, @"color:(.*?);", RegexOptions.IgnoreCase);
            if (match.Success)
                txtColor.Text = match.Groups[1].Value.Replace("!important", "").Trim().Replace("#", "");
        }
    }
}
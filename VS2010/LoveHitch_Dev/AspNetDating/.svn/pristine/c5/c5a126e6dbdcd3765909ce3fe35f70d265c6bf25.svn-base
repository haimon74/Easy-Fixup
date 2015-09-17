/* ASPnetDating 
 * Copyright (C) 2003-2010 eStream 
 * lovehitch.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at lovehitch.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI;
using AspNetDating.Classes;

namespace AspNetDating.Components.Profile
{
    public partial class EditSkin : UserControl
    {
        private User user;

        /// <summary>
        /// Gets the current user session.
        /// </summary>
        /// <value>The current user session.</value>
        protected UserSession CurrentUserSession
        {
            get { return ((PageBase) Page).CurrentUserSession; }
        }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>The user.</value>
        public User User
        {
            set
            {
                user = value;
                ViewState["Username"] = user != null ? user.Username : null;
            }
            get
            {
                if (user == null && ViewState["Username"] != null)
                    user = User.Load((string) ViewState["Username"]);
                return user;
            }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadStrings();
            }
            if (Visible)
                PopulateControls();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            PopulateControls();
        }

        private void PopulateControls()
        {
            if (plhEditSkinControls.Controls.Count > 0) return;

            if (User == null || User.ProfileSkin == null)
            {
                lblError.Text = "You must select a profile skin first!";
                return;
            }

            using (TextReader tr = File.OpenText(Server.MapPath(User.ProfileSkin)))
            {
                string line;
                while ((line = tr.ReadLine()) != null)
                {
                    if (!line.ToLower().Contains("editable")) continue;
                    var regex = new Regex(@"/\* ?EDITABLE\((.+)\) ?\*/",
                                          RegexOptions.IgnoreCase);
                    var match = regex.Match(line);
                    if (match.Success)
                    {
                        string title = match.Groups[1].Value;
                        var headerLine = (HeaderLine)LoadControl("~/Components/HeaderLine.ascx");
                        headerLine.Title = title.Translate();
                        plhEditSkinControls.Controls.Add(headerLine);

                        EditSkinBaseControl editControl = null;
                        if (line.ToLower().Contains("background:"))
                        {
                            editControl = (EditSkinBaseControl) LoadControl("EditSkinBackground.ascx");
                        }
                        else if (line.ToLower().Contains("color:"))
                        {
                            editControl = (EditSkinBaseControl) LoadControl("EditSkinColor.ascx");
                        }
                        if (editControl == null) continue;
                        editControl.Key = title;
                        editControl.SetStyleLine(line);
                        plhEditSkinControls.Controls.Add(editControl);
                    }
                }
            }
        }

        private void LoadStrings()
        {
            LargeBoxStart1.Title = Lang.Trans("Edit Skin");
            btnSave.Text = "Save changes".Translate();
        }

        #region Web Form Designer generated code

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        #endregion

        protected void btnSave_Click(object sender, EventArgs e)
        {
            PopulateControls();
            string fileCss;
            using (TextReader tr = File.OpenText(Server.MapPath(User.ProfileSkin)))
            {
                fileCss = tr.ReadToEnd();
            }
            if (!User.ProfileSkin.ToLower().Contains("_" + User.Username.ToLower() + ".css"))
                User.ProfileSkin = User.ProfileSkin.Replace(".css", "_" + User.Username + ".css");
            string profileSkinFile = Server.MapPath(User.ProfileSkin);

            SaveProfileSkin(fileCss, profileSkinFile);
            divPreviewSkin.Visible = true;
            User.Update();
            CurrentUserSession.ProfileSkin = User.ProfileSkin;
        }

        private void SaveProfileSkin(string fileCss, string profileSkinFile)
        {
            foreach (var control in plhEditSkinControls.Controls)
            {
                if (!(control is EditSkinBaseControl)) continue;
                var skinControl = (EditSkinBaseControl) control;

                var match = Regex.Match(fileCss, 
                                        @"^.*?/\* ?EDITABLE\(" + skinControl.Key + @"\) ?\*/.*?$",
                                        RegexOptions.IgnoreCase | RegexOptions.Multiline);
                if (!match.Success) continue;
                var importantFlag = match.Value.ToLower().Contains("!important");
                string styleLine = skinControl.GetStyleLine();
                styleLine = styleLine.Replace("{", "").Replace("}", "");
                if (importantFlag) styleLine = styleLine.Replace(";", " !important;");
                fileCss = fileCss.Replace(match.Value, String.Format("{0} /* EDITABLE({1}) */",
                                                                     styleLine, skinControl.Key));
            }

            using (TextWriter tw = File.CreateText(profileSkinFile))
            {
                tw.Write(fileCss);
            }
        }
    }
}
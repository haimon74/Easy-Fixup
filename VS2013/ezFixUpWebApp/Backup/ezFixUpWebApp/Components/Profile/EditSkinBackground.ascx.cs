using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using ezFixUp.Classes;
using ScriptManager=System.Web.UI.ScriptManager;

namespace ezFixUp.Components.Profile
{
    public partial class EditSkinBackground : EditSkinBaseControl
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
            rblAttachment.Items[0].Text = "Scroll".Translate();
            rblAttachment.Items[1].Text = "Fixed".Translate();
            cblRepeat.Items[0].Text = "Down".Translate();
            cblRepeat.Items[1].Text = "Across".Translate();
        }

        public override string GetStyleLine()
        {
            var style = new StringBuilder("background:");
            if (txtBackgroundColor.Text.Trim().Length > 0)
            {
                style.Append(" " +  "#" + txtBackgroundColor.Text.Trim());
            }
            if (fileUploadBackgroundImage.HasFile)
            {
                try
                {
                    Image image;
                    using (image = Image.FromStream(fileUploadBackgroundImage.PostedFile.InputStream))
                    {
                        var uploadedFilename = fileUploadBackgroundImage.FileName;
                        if (uploadedFilename.Contains(@"\"))
                            uploadedFilename = uploadedFilename.Substring(uploadedFilename.LastIndexOf(@"\") + 1);
                        if (uploadedFilename.Contains("/"))
                            uploadedFilename = uploadedFilename.Substring(uploadedFilename.LastIndexOf("/") + 1);
                        if (!uploadedFilename.ToLower().EndsWith(".jpg") &&
                            !uploadedFilename.ToLower().EndsWith(".bmp") &&
                            !uploadedFilename.ToLower().EndsWith(".gif") &&
                            !uploadedFilename.ToLower().EndsWith(".png"))
                            uploadedFilename += ".jpg";
                        if (uploadedFilename.Contains(".."))
                            uploadedFilename.Replace(".", "");
                        if (uploadedFilename.Contains(" "))
                            uploadedFilename.Replace(" ", "_");

                        string userFilesPath = "~/UserFiles/" + ((PageBase)Page).CurrentUserSession.Username;
                        string userFilesDir = Server.MapPath(userFilesPath);
                        if (!Directory.Exists(userFilesDir))
                        {
                            Directory.CreateDirectory(userFilesDir);
                        }

                        // Delete previous background uploads
                        try
                        {
                            foreach (var file in Directory.GetFiles(userFilesDir, "background_*.*"))
                            {
                                File.Delete(file);
                            }
                        }
                        catch (Exception err)
                        {
                            Global.Logger.LogError("EditSkinBackground", err);
                        }

                        var imageFilename = String.Format("{0}/UserFiles/{1}/background_{2}",
                            Config.Urls.Home, ((PageBase)Page).CurrentUserSession.Username, uploadedFilename);

                        try
                        {
                            image.Save(userFilesDir + "/background_" + uploadedFilename);
                        }
                        catch (Exception err)
                        {
                            Global.Logger.LogError(err);
                        }
                        style.Append(" url(" + imageFilename + ")");
                        txtBackgroundImageUrl.Text = imageFilename;
                    }
                }
                // ReSharper disable EmptyGeneralCatchClause
                catch
                // ReSharper restore EmptyGeneralCatchClause
                {
                    // Invalid image
                }
            }
            else if (txtBackgroundImageUrl.Text.Trim().Length > 0)
            {
                style.Append(" url(" + txtBackgroundImageUrl.Text.Trim() + ")");
            }
            if (rbPositionTopLeft.Checked)
                style.Append(" top left");
            else if (rbPositionTopCenter.Checked)
                style.Append(" top center");
            else if (rbPositionTopRight.Checked)
                style.Append(" top right");
            else if (rbPositionMiddleLeft.Checked)
                style.Append(" center left");
            else if (rbPositionMiddleCenter.Checked)
                style.Append(" center");
            else if (rbPositionMiddleRight.Checked)
                style.Append(" center right");
            else if (rbPositionBottomLeft.Checked)
                style.Append(" bottom left");
            else if (rbPositionBottomCenter.Checked)
                style.Append(" center bottom");
            else if (rbPositionBottomRight.Checked)
                style.Append(" bottom right");

            if (rblAttachment.SelectedIndex == 1)
                style.Append(" fixed");

            if (!cblRepeat.Items[0].Selected && !cblRepeat.Items[1].Selected)
                style.Append(" no-repeat");
            else if (cblRepeat.Items[0].Selected && !cblRepeat.Items[1].Selected)
                style.Append(" repeat-y");
            else if (cblRepeat.Items[1].Selected && !cblRepeat.Items[0].Selected)
                style.Append(" repeat-x");

            style.Replace(';', ' ');
            style.Append(';');
            return style.ToString();
        }

        public override void SetStyleLine(string line)
        {
            var match = Regex.Match(line, @"(#.*?)[\s|;]");
            if (match.Success)
                txtBackgroundColor.Text = match.Groups[1].Value.Replace("#", "");
            match = Regex.Match(line, @"url\((.*?)\)", RegexOptions.IgnoreCase);
            if (match.Success)
                txtBackgroundImageUrl.Text = match.Groups[1].Value;
            var positionRadioButtons = new[,]
                                           {
                                               {rbPositionTopLeft, rbPositionTopCenter, rbPositionTopRight},
                                               {rbPositionMiddleLeft, rbPositionMiddleCenter, rbPositionMiddleRight},
                                               {rbPositionBottomLeft, rbPositionBottomCenter, rbPositionBottomRight}
                                           };
            var positionX = 1;
            var positionY = 1;
            if (line.ToLower().Contains("left")) positionX = 0;
            if (line.ToLower().Contains("right")) positionX = 2;
            if (line.ToLower().Contains("top")) positionY = 0;
            if (line.ToLower().Contains("bottom")) positionY = 2;
            positionRadioButtons[positionY, positionX].Checked = true;

            rblAttachment.SelectedIndex = line.ToLower().Contains("fixed") ? 1 : 0;

            if (line.ToLower().Contains("repeat-x"))
                cblRepeat.Items[1].Selected = true;
            else if (line.ToLower().Contains("repeat-y"))
                cblRepeat.Items[0].Selected = true;
            else if (!line.ToLower().Contains("no-repeat"))
            {
                cblRepeat.Items[0].Selected = true;
                cblRepeat.Items[1].Selected = true;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.AccessControl;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using AspNetDating.Classes;
using ionic.utils.zip;

namespace AspNetDating.Admin
{
    public partial class ThemeManager : AdminPageBase
    {
        private XDocument themesXml;

        public XDocument ThemesXml
        {
            get
            {
                if (themesXml == null)
                {
                    if (Cache["ThemesXML"] != null)
                        themesXml = (XDocument)Cache["ThemesXML"];
                    else
                    {
                        themesXml = XDocument.Load("lovehitch.com/Themes.xml");
                        Cache.Insert("ThemesXML", themesXml, null, DateTime.Now.AddHours(2),
                                     System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                }

                return themesXml;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            dlOnlineThemes.ItemDataBound += new DataListItemEventHandler(dlOnlineThemes_ItemDataBound);

            ScriptManager.RegisterClientScriptInclude(this, typeof(Page), "jQuery",
                    "../scripts/jquery.min.js");
            ScriptManager.RegisterClientScriptInclude(this, typeof(Page), "jQuery.lightbox",
                    "../scripts/jquery.lightbox.js");
            Page.Header.Controls.Add(new LiteralControl(
                "<link href=\"../images/jquery.lightbox.css\" rel=\"stylesheet\" type=\"text/css\" />"));
            ScriptManager.RegisterStartupScript(this, typeof(Page), "lightbox",
                                                "$(function() {$('a.preview').lightBox({"
                                                + "imageLoading: '../images/loading.gif',"
                                                + "imageBtnClose: '../images/close.gif',"
                                                + "imageBtnPrev: '../images/prev.gif',"
                                                + "imageBtnNext: '../images/next.gif'});});", true);

            if (!Page.IsPostBack)
            {
                LoadStrings();
                LoadThemes();
                LoadOnlineThemes();
            }
        }

        private void LoadThemes()
        {
            var dtThemes = new DataTable();
            dtThemes.Columns.Add("Name");
            dtThemes.Columns.Add("Previews", typeof(Dictionary<string, string>));

            var themesDir = Server.MapPath("~/App_Themes");
            foreach (var directory in Directory.GetDirectories(themesDir))
            {
                var name = directory.Substring(directory.LastIndexOf('\\') + 1);
                if (name.StartsWith("_") || name.StartsWith(".") || name.StartsWith("AjaxChat")) continue;
                var previews = new Dictionary<string, string>();
                if (Directory.Exists(directory + @"\preview"))
                {
                    foreach (var file in Directory.GetFiles(directory + @"\preview", "*_t.jpg"))
                    {
                        var thumb = file.Substring(file.LastIndexOf('\\') + 1);
                        previews.Add("../App_Themes/" + name + "/preview/" + thumb,
                            "../App_Themes/" + name + "/preview/" + thumb.Replace("_t.", "."));
                    }
                }
                dtThemes.Rows.Add(name, previews);
            }

            dlThemes.DataSource = dtThemes;
            dlThemes.DataBind();
        }

        private void LoadOnlineThemes()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            var versionString = version.Major + "." + version.Minor;
            var themes = from t in ThemesXml.Descendants("Theme")
                         where t.Element("Version").Value == versionString
                         select new
                                    {
                                        Name = t.Attribute("Name").Value,
                                        Description = t.Element("Description").Value,
                                        DownloadUrl = t.Element("DownloadUrl") != null
                                                          ? t.Element("DownloadUrl").Value
                                                          : null,
                                        PurchaseUrl = t.Element("PurchaseUrl") != null
                                                          ? t.Element("PurchaseUrl").Value
                                                          : null,
                                        Previews = from p in t.Element("Previews").Descendants()
                                                   select new
                                                              {
                                                                  ThumbnailUrl = p.Attribute("ThumbnailUrl").Value,
                                                                  ImageUrl = p.Attribute("ImageUrl").Value
                                                              }
                                    };
            dlOnlineThemes.DataSource = themes;
            dlOnlineThemes.DataBind();
        }

        private void LoadStrings()
        {
            btnUploadTheme.Text = "Upload".TranslateA();
            if (!HasWriteAccess)
            {
                btnUploadTheme.Enabled = false;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.changeTheme;

            base.OnInit(e);
        }

        protected void dlThemes_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "Select" && !HasWriteAccess)
                return;

            if (e.CommandName == "Select")
            {
                Session["theme"] = null;
                Config.Misc.SiteTheme = e.CommandArgument.ToString();
                Messagebox1.Show(string.Format("Theme changed to {0}".TranslateA(),
                    e.CommandArgument), Misc.MessageType.Success);
            }

            LoadThemes();
        }

        protected void dlOnlineThemes_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "Download" && !HasWriteAccess)
                return;

            if (e.CommandName == "Download")
            {
                var theme = (string)e.CommandArgument;
                var version = Assembly.GetExecutingAssembly().GetName().Version;
                var versionString = version.Major + "." + version.Minor;
                var downloadUrl = (from t in ThemesXml.Descendants("Theme")
                                   where t.Element("Version").Value == versionString
                                         && t.Attribute("Name").Value == theme
                                   select t.Element("DownloadUrl").Value).First();

                var themesDir = Server.MapPath("~/App_Themes");
                var themeDir = themesDir + @"\" + theme;
                int counter = 1;
                while (Directory.Exists(themeDir))
                {
                    themeDir = string.Format(@"{0}\{1} ({2})", themesDir, theme, counter++);
                }
                Directory.CreateDirectory(themeDir);
                var wc = new WebClient();
                wc.DownloadFile(downloadUrl, themeDir + @"\theme.zip");

                using (var zip = ZipFile.Read(themeDir + @"\theme.zip"))
                {
                    zip.ExtractAll(themeDir);
                }
                File.Delete(themeDir + @"\theme.zip");
            }

            LoadThemes();
            Messagebox1.Show("Theme has been downloaded".TranslateA(), Misc.MessageType.Success);
        }

        protected void dlThemes_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (!HasWriteAccess)
            {
                var btnSelect = e.Item.FindControl("btnSelect") as LinkButton;

                if (btnSelect != null)
                    btnSelect.Enabled = false;
            }
        }

        void dlOnlineThemes_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (!HasWriteAccess)
            {
                var btnDownload = e.Item.FindControl("btnDownload") as LinkButton;
                var lnkPurchase = e.Item.FindControl("lnkPurchase") as HyperLink;

                if (btnDownload != null)
                    btnDownload.Enabled = false;

                if (lnkPurchase != null)
                    lnkPurchase.Enabled = false;
            }
        }

        protected void btnUploadTheme_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            if (!fileUploadTheme.HasFile)
            {
                Messagebox1.Show("Please select a theme file".TranslateA(), Misc.MessageType.Error);
            }

            var themesDir = Server.MapPath("~/App_Themes");
            var theme = fileUploadTheme.FileName.Remove(fileUploadTheme.FileName.IndexOf('.'));
            var themeDir = themesDir + @"\" + theme;
            int counter = 1;
            while (Directory.Exists(themeDir))
            {
                themeDir = string.Format(@"{0}\{1} ({2})", themesDir, theme, counter++);
            }
            Directory.CreateDirectory(themeDir);
            fileUploadTheme.SaveAs(themeDir + @"\theme.zip");

            using (var zip = ZipFile.Read(themeDir + @"\theme.zip"))
            {
                zip.ExtractAll(themeDir);
            }
            File.Delete(themeDir + @"\theme.zip");

            LoadThemes();
            Messagebox1.Show("Theme has been uploaded".TranslateA(), Misc.MessageType.Success);
        }
    }
}

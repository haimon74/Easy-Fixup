/* ezFixUp 
 * Copyright (C) 2003-2010 eStream 
 * ezFixUp.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ezFixUp license agreement.  
 * It can be found at ezFixUp.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using ezFixUp.Classes;

namespace ezFixUp.Admin
{
    /// <summary>
    /// Summary description for Home.
    /// </summary>
    public partial class Home : AdminPageBase
    {
        protected MessageBox MessageBox;

        public Home()
        {
            RequiresAuthorization = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int errorCode;
                if (!int.TryParse(Request.Params["err"], out errorCode))
                    errorCode = -1;
                ShowError(errorCode);

                if (Config.Misc.EnableFirstRunWizard)
                    Response.Redirect("ConfigurationWizard.aspx");

                prepareStats();
                checkInstallation();
                loadPluginsStatus();
                loadNews();
            }
        }

        private void checkInstallation()
        {
            checkInstallationFolder(MapPath("~/UserFiles"));
            checkInstallationFolder(Config.Directories.ImagesCacheDirectory);
            checkInstallationFolder(MapPath("~/Logs"));
            checkInstallationFolder(MapPath("~/IpData"));
            checkInstallationFolder(MapPath("~/Temp"));

            for (int i = 0; i <= 9; i++)
                checkInstallationFolder(Config.Directories.ImagesCacheDirectory + "/" + i);
            
            if (Config.Photos.EnablePhotoStack)
                checkInstallationFolder(Config.Directories.ImagesCacheDirectory + "/stacks");
            
            if (Properties.Settings.Default.StorePhotosAsFiles)
                checkInstallationFolder(Config.Directories.UserImagesDirectory);
        }

        private void checkInstallationFolder(string folder)
        {
            if (!Directory.Exists(folder))
            {
                try
                {
                    Directory.CreateDirectory(folder);
                }
                catch (Exception)
                {
                    MessageBox.Show("Unable to create folder! Please manually create " +
                        folder, Misc.MessageType.Error);
                    return;
                }
            }

            Guid guid = Guid.NewGuid();
            var testFile = folder + @"\" + guid + ".temp";
            try
            {
                TextWriter tw = File.CreateText(testFile);
                tw.Close();
                File.Delete(testFile);
            }
            catch (Exception)
            {
                MessageBox.Show("The application does not have read/write permissions to " +
                    folder + ". Please grant the necessary permissions!", Misc.MessageType.Error);
                return;
            }
        }

        private void prepareStats()
        {
            #region New registrations chart

            ChartNewRegistrations.Series["Default"]["DrawingStyle"] = "Cylinder";
            ChartNewRegistrations.Titles["Title1"].Text =
                Lang.TransA("At-a-glance: New registrations for the past 30 days");
            ChartNewRegistrations.Series["Default"].ToolTip = String.Format("#VALX\n#VALY {0}",
                                                                            "Registrations".TranslateA());
            NewUsersSearch nuSearch = new NewUsersSearch();
            nuSearch.ProfileReq = false;
            nuSearch.UsersSince = DateTime.Now.AddDays(-31);
            UserSearchResults nuResults = nuSearch.GetResults();

            List<string> xValues = new List<string>();
            List<int> yValues = new List<int>();
            List<int> newUsersCount = new List<int>(30);
            newUsersCount.AddRange(new int[30]);

            int max = 10;
            if (nuResults != null)
            {
                foreach (string username in nuResults.Usernames)
                {
                    User user = Classes.User.Load(username);
                    int dayIndex = 30 - DateTime.Now.Subtract(user.UserSince).Days;
                    if (dayIndex < 0 || dayIndex >= newUsersCount.Count) continue;
                    newUsersCount[dayIndex]++;
                }

                for (int i = 0; i < 30; i++)
                {
                    int registrations = newUsersCount[i];
                    xValues.Add(DateTime.Now.AddDays(i - 30).ToString("d MMM"));
                    yValues.Add(registrations);

                    if (max < registrations) max = registrations;
                }
            }

            ChartNewRegistrations.Series["Default"].Points.DataBindXY(xValues, yValues);

            #endregion

            #region Total users chart

            if (Config.Users.DisableGenderInformation)
            {
                ChartTotalRegistrations.Visible = false;
            }
            else
            {
                ChartTotalRegistrations.Titles["Title1"].Text = Lang.TransA("At-a-glance: Total Registrations");
                ChartTotalRegistrations.Series["Default"]["PieLabelStyle"] = "Outside";

                BasicSearch search = new BasicSearch();
                search.hasAnswer_isSet = false;
                search.hasPhoto_isSet = false;
                search.interestedIn_isSet = false;
                search.Gender = Classes.User.eGender.Male;
                UserSearchResults results = search.GetResults();
                int count = results == null ? 0 : results.Usernames.Length;

                xValues = new List<string>();
                yValues = new List<int>();

                xValues.Add(String.Format(Lang.TransA("{0} Males"), count));
                yValues.Add(count);

                search.Gender = Classes.User.eGender.Female;
                results = search.GetResults();
                count = results == null || results.Usernames == null ? 0 : results.Usernames.Length;
                xValues.Add(String.Format(Lang.TransA("{0} Females"), count));
                yValues.Add(count);

                if (Config.Users.CouplesSupport)
                {
                    search.Gender = Classes.User.eGender.Couple;
                    results = search.GetResults();
                    count = results == null || results.Usernames == null ? 0 : results.Usernames.Length;
                    if (count != 0)
                    {
                        xValues.Add(String.Format(Lang.TransA("{0} Couples"), count));
                        yValues.Add(count);
                    }
                }

                ChartTotalRegistrations.Series["Default"].Points.DataBindXY(xValues, yValues);
            }

            #endregion
        }

        private void loadPluginsStatus()
        {
            var dtPlugins = new DataTable();
            dtPlugins.Columns.Add("Icon");
            dtPlugins.Columns.Add("Name");

            if (File.Exists(Server.MapPath("~/AjaxChat/MessengerWindow.aspx")))
            {
                dtPlugins.Rows.Add("plugin", "AspNetAjaxChat loaded!");
            }
            else
            {
                dtPlugins.Rows.Add("plugin_disabled", "AspNetAjaxChat is not installed! " +
                    "<a href=\"https://www.plimus.com/jsp/buynow.jsp?contractId=2291092\" target=\"_blank\">Buy now</a> to enable private chats, file transfers, smilies, 1-to-1 instant messenger, audio/video chats and more.");
            }

            if (FaceFinderPlugin.IsInstalled)
            {
                if (Config.Photos.FindFacesForThumbnails)
                    dtPlugins.Rows.Add("plugin", "Face finder plugin loaded!");
                else
                    dtPlugins.Rows.Add("plugin_error", "Face finder plugin is installed but the 'Find faces in photos' option is not enabled in 'Settings'!");
            }
            else
            {
                dtPlugins.Rows.Add("plugin_disabled", "Face finder plugin is not installed! " +
                    "<a href=\"https://www.plimus.com/jsp/buynow.jsp?contractId=1850386\" target=\"_blank\">Buy now</a> to enable automatic face recognition " +
                    " and photo thumbnail cropping.");
            }

            if (VideoConverterPlugin.IsInstalled)
            {
                if (Config.Misc.EnableVideoUpload)
                    dtPlugins.Rows.Add("plugin", "Video converter plugin loaded!");
                else
                    dtPlugins.Rows.Add("plugin_error", "Video converter plugin is installed but the 'Enable Video Upload' option is not enabled in 'Settings'!");
            }
            else
            {
                dtPlugins.Rows.Add("plugin_disabled", "Video converter plugin is not installed! " +
                    "<a href=\"https://www.plimus.com/jsp/buynow.jsp?contractId=1850390\" target=\"_blank\">Buy now</a> to enable video file uploads.");
            }

            if (VideoStreamerPlugin.IsInstalled)
            {
                if (Config.Misc.EnableProfileVideoBroadcast)
                    dtPlugins.Rows.Add("plugin", "Video streamer plugin loaded!");
                else
                    dtPlugins.Rows.Add("plugin_error", "Video streamer plugin is installed but the 'Enable Video Broadcast' option is not enabled in 'Settings'!");
            }
            else
            {
                dtPlugins.Rows.Add("plugin_disabled", "Video streamer plugin is not installed! " +
                    "<a href=\"https://www.plimus.com/jsp/buynow.jsp?contractId=1850466\" target=\"_blank\">Buy now</a> to enable live web cam video streaming to profile (with FMS or RED5 streaming server).");
            }

            rptPlugins.DataSource = dtPlugins;
            rptPlugins.DataBind();
        }

        private void loadNews()
        {
            try
            {
                XDocument xdoc = XDocument.Load("https://feeds.feedburner.com/ezFixUpHelpdesk-LatestTopics");

                var topics = from item in xdoc.Descendants("item")
                             select new
                                        {
                                            // ReSharper disable PossibleNullReferenceException
                                            Title = item.Element("title").Value,
                                            Category = item.Element("category").Value,
                                            Description = item.Element("description").Value,
                                            Link = item.Element("link").Value,
                                            PubDate = item.Element("pubDate").Value
                                            // ReSharper restore PossibleNullReferenceException
                                        };

                rptHelpdeskTopics.DataSource = topics;
                rptHelpdeskTopics.DataBind();
            }
            catch (Exception err)
            {
                Global.Logger.LogInfo(err);
            }
        }

        private void ShowError(int errorCode)
        {
            switch (errorCode)
            {
                case 1:
                    MessageBox.Show(Lang.TransA("Access denied!"), Misc.MessageType.Error);
                    break;
                case -1:
                default:
                    break;
            }
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            if (CurrentAdminSession != null)
                Privileges = Classes.Admin.eAccess.ReadWrite;
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        #endregion
    }
}
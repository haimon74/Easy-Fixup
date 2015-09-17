/* ASPnetDating 
 * Copyright (C) 2003-2010 eStream 
 * ezFixUp.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at ezFixUp.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Linq;
using ezFixUp.Classes;

namespace ezFixUp
{
    /// <summary>
    /// Summary description for ActivateAccount.
    /// </summary>
    public partial class ActivateAccount : PageBase
    {
        public ActivateAccount()
        {
            RequiresAuthorization = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Put user code to initialize the page here
            if (!Page.IsPostBack)
            {
                string username = Request.Params["username"];
                string guid = Request.Params["guid"];

                if (username != null)
                {
                    User user;
                    try
                    {
                        user = Classes.User.Load(username);
                    }
                    catch (NotFoundException)
                    {
                        StatusPageMessage =
                            Lang.Trans(
                                "Your registration has expired! Please register and create your account again.<br><br>");
                        Response.Redirect("Registr.aspx");
                        return;
                    }
                    if (Classes.User.IsValidPendingGuid(username, guid))
                    {
                        user.Active = true;
                        user.Update();

                        if (Config.Users.SendWelcomeMessage)
                        {
                            Message.SendWelcomeMessage(user);
                        }

                        Classes.User.RemovePendingGuids(guid, username);

                        StatusPageMessage = Lang.Trans("<b>Your registration has been confirmed!</b><br>" +
                                                       "Please click on the link below to edit your profile.<br><br>");

                        //NameValueCollection link = new NameValueCollection();
                        //link.Add(Config.Urls.Home + "/ManageProfile.aspx", Config.Urls.Home + "/ManageProfile.aspx");
                        //StatusPageLink = link;
                        ((PageBase)Page).StatusPageLinkSkindId = "";
                        ((PageBase)Page).StatusPageLinkText = Config.Urls.Home + "/ManageProfile.aspx";
                        ((PageBase)Page).StatusPageLinkURL = Config.Urls.Home + "/ManageProfile.aspx";

                        UserSession userSession = null;
                        try
                        {
                            userSession = new UserSession(username);
                            userSession.Authorize(Session.SessionID);
                            ((PageBase)Page).CurrentUserSession = userSession;

                            #region Autojoin to group

                            Group[] autojoinGroups = Group.Fetch(true);

                            if (autojoinGroups.Length > 0)
                            {
                                var groups =
                                    autojoinGroups.Where(
                                        g => g.Approved &&
                                        (g.AutojoinCountry == null || g.AutojoinCountry == userSession.Country) &&
                                        (g.AutojoinRegion == null || g.AutojoinRegion == userSession.State) &&
                                        (g.AutojoinCity == null || g.AutojoinCity == userSession.City));
                                foreach (Group group in groups)
                                {
                                    GroupMember groupMember = new GroupMember(group.ID, userSession.Username);
                                    groupMember.Active = true;
                                    groupMember.Type = GroupMember.eType.Member;
                                    groupMember.Save();
                                    group.ActiveMembers++;
                                    group.Save();
                                }
                            }

                            #endregion
                        }
                        catch (Exception err)
                        {
                            StatusPageMessage = err.Message;
                        }

                        Response.Redirect("ShowStatus.aspx");
                    }
                    else
                    {
                        StatusPageMessage =
                            Lang.Trans(
                                "<b>Your account is already confirmed or the provided confirmation URL is wrong</b><br><br>");
                        Response.Redirect("ShowStatus.aspx");
                    }
                }
            }
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
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
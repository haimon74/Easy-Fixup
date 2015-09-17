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
using ezFixUp.Classes;
using ezFixUp.Components;

namespace ezFixUp
{
    /// <summary>
    /// Summary description for AddRemoveFavorite.
    /// </summary>
    public partial class AddRemoveFavorite : PageBase
    {
        private string userID;
        private string source;
        private string command;

        protected void Page_PreRender(object sender, EventArgs e)
        {
            lnkBack.OnClientClick = String.Format("window.location='https://{0}';return false;", Global.GetSessionState()["RefererPage"]);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!Config.Users.EnableFavorites)
                {
                    Response.Redirect("~/Default.aspx");
                    return;
                }

                if (!ValidateParameters())
                    return;

                source = (string) Request.Params["src"];
                command = (string) Request.Params["cmd"];
                userID = (string) Request.Params["uid"];

                LoadStrings();
                DoThings();
            }
            source = (string) Request.Params["src"];
            command = (string) Request.Params["cmd"];
            userID = (string) Request.Params["uid"];
        }

        private bool ValidateParameters()
        {
            if (Request.Params["src"] == null ||
                Request.Params["uid"] == null ||
                Request.Params["cmd"] == null)
                return false;

            string uid = (string) Request.Params["uid"];
            string cmd = (string) Request.Params["cmd"];
            string src = (string) Request.Params["src"];

            if (src != "profile" && src != "favorites" && src != "search")
                return false;
            
            if (cmd != "add" && cmd != "remove" && cmd != "block")
                return false;
            
            if (uid.IsNullOrEmpty())
                return false;

            //if (src == "favorites" && cmd != "remove")
            //    return false;

            return Classes.User.Load(uid) != null;
        }

        private void LoadStrings()
        {
            SideMenuStart1.Title = Lang.Trans("Actions");
            LargeBoxStart1.Title = Lang.Trans("Message");

            lnkBack.Text = Lang.Trans("<< Previous Page");
            pnlBackLink.Visible = true;
        }

        private void DoThings()
        {
            if (command == "add")
            {
                Classes.User.eAddFavouriteResult result =
                    CurrentUserSession.AddToFavourites(userID);
                switch (result)
                {
                    case Classes.User.eAddFavouriteResult.eAlreadyAdded:
                        lblMessage.CssClass = "error";
                        lblMessage.Text = Lang.Trans("The user you selected already exists in your favourite list");
                        break;
                    case Classes.User.eAddFavouriteResult.eInvalidUsername:
                        lblMessage.CssClass = "error";
                        lblMessage.Text = Lang.Trans("No such user!");
                        break;
                    case Classes.User.eAddFavouriteResult.eMaximumFavouritesReached:
                        lblMessage.CssClass = "error";
                        lblMessage.Text =
                            String.Format(
                                Lang.Trans(
                                    "The maximum favourite users of {0} is reached! Please remove someone and try again!"),
                                Config.Users.MaxFavouriteUsers);
                        break;
                    case Classes.User.eAddFavouriteResult.eSuccess:
                        lblMessage.CssClass = "message";
                        lblMessage.Text = String.Format(Lang.Trans("{0} has been added to your favourite list"), userID);
                        
                        break;
                }
            }
            else if (command == "remove")
            {
                if (CurrentUserSession.IsUserInFavouriteList(userID))
                {
                    CurrentUserSession.RemoveFromFavourites(userID);

                    lblMessage.CssClass = "message";
                    lblMessage.Text = String.Format(Lang.Trans("{0} has been removed from your favourite list"), userID);
                }
                else
                {
                    lblMessage.CssClass = "error";
                    lblMessage.Text = Lang.Trans("No such user!");
                }
            }
            else if (command == "block")
            {
                if (!CurrentUserSession.IsUserBlocked(userID))
                {
                    CurrentUserSession.BlockUser(userID);
                    lblMessage.CssClass = "message";
                    lblMessage.Text = String.Format(Lang.Trans("{0} has been added to your blocked list"), userID);
                }
                else
                {
                    lblMessage.CssClass = "message";
                    lblMessage.Text = Lang.Trans("Already in blocked list!");
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

        protected void lnkBack_Click(object sender, EventArgs e)
        {
            if (lnkBack.CommandArgument == "profile")
                Response.Redirect(UrlRewrite.CreateShowUserUrl(userID));
            else if (lnkBack.CommandArgument == "favorites")
                Response.Redirect("~/Favorites.aspx");
            else if (lnkBack.CommandArgument == "search")
                Response.Redirect("~/Search3.aspx");
        }

        protected void lnkFavourites_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Favorites.aspx");
        }
    }
}
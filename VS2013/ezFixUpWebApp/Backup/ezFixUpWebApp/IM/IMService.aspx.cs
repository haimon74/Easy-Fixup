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
using System.Text;
using System.Web.UI;
using ezFixUp.Classes;

namespace ezFixUp.IM
{
    /// <summary>
    /// Summary description for IMService.
    /// </summary>
    public partial class IMService : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Global.Logger.LogStatus("IMService", Request.RawUrl);

            string command = Request.QueryString["command"];

            switch (command)
            {
                case "getphoto":
                    GetPhoto();
                    break;
                case "blockuser":
                    BlockUser();
                    break;
                case "addtofavourites":
                    AddToFavourites();
                    break;
                case "validateuser":
                    ValidateUser();
                    break;
                case "validateadmin":
                    ValidateAdmin();
                    break;
                case "videopendingapproval":
                    VideoPendingApproval();
                    break;
                case "videodeleted":
                    VideoDeleted();
                    break;
            }
        }

        private void VideoDeleted()
        {
            string username = Request.QueryString["username"];
            string result = "undefined";

            try
            {
                User user = Classes.User.Load(username);
                VideoProfile.Delete(username);
                result = "true";
            }
            catch (NotFoundException)
            {
                result = "false";
            }

            Response.Clear();
            Response.Write(String.Format("&success={0}&", result));
            Response.Flush();

            return;
        }

        private void VideoPendingApproval()
        {
            string username = Request.QueryString["username"];
            string result = "undefined";

            try
            {
                User user = Classes.User.Load(username);
                VideoProfile.Save(username);
                result = "true";
            }
            catch (NotFoundException)
            {
                result = "false";
            }

            Response.Clear();
            Response.Write(String.Format("&success={0}&", result));
            Response.Flush();

            return;
        }

        private void ValidateUser()
        {
            string username = Request.QueryString["username"];
            string sessionID = Request.QueryString["sessID"];

            string result = "undefined";

            if (username != null && sessionID != null)
            {
                try
                {
                    User user = Classes.User.Load(username);
                    if (user.LastSessionID == sessionID)
                        result = "true";
                    else
                        result = "false";
                }
                catch (NotFoundException)
                {
                    result = "false";
                }
            }

            Global.Logger.LogStatus("IMService ValidateUser", username + " " + sessionID + " " + result);

            Response.Clear();
            Response.Write(String.Format("&success={0}&", result));
            Response.Flush();

            return;
        }

        private void ValidateAdmin()
        {
            string username = Request.QueryString["username"];
            string sessionID = Request.QueryString["sessID"];

            string result = "undefined";

            if (username != null && sessionID != null)
            {
                try
                {
                    Classes.Admin admin = Classes.Admin.Load(username);
                    if (admin.LastSessionID == sessionID)
                        result = "true";
                    else
                        result = "false";
                }
                catch (NotFoundException)
                {
                    result = "false";
                }
            }

            Response.Clear();
            Response.Write(String.Format("&success={0}&", result));
            Response.Flush();

            return;
        }

        private void GetPhoto()
        {
            string username = Request.QueryString["username"];
            string width = Request.QueryString["width"];
            string height = Request.QueryString["height"];

            if (username != null && width != null && height != null)
            {
                int photoId;
                try
                {
                    photoId = Photo.GetPrimary(username).Id;
                }
                catch (NotFoundException)
                {
                    photoId = 0;
                }

                string url = ImageHandler.CreateImageUrl(photoId, int.Parse(width), int.Parse(height), false, true, true);
                    //"~/Image.ashx?id={0}&width={1}&height={2}", photoId, width, height);
                Response.Redirect(url);
            }
        }

        private void BlockUser()
        {
            string currentUsername = Request.QueryString["currentusername"];
            string targetUsername = Request.QueryString["targetusername"];

            if (currentUsername != null & targetUsername != null)
            {
                User user;
                try
                {
                    user = Classes.User.Load(currentUsername);
                }
                catch (NotFoundException) { return; }
                user.BlockUser(targetUsername);
            }
        }

        private void AddToFavourites()
        {
            string currentUsername = Request.QueryString["currentusername"];
            string targetUsername = Request.QueryString["targetusername"];

            string result = "undefined";

            if (currentUsername != null & targetUsername != null)
            {
                User user;
                try
                {
                    user = Classes.User.Load(currentUsername);
                }
                catch (NotFoundException)
                {
                    result = Lang.Trans("No such user!");
                    goto writeresponse;
                }


                User.eAddFavouriteResult res = user.AddToFavourites(targetUsername);

                switch (res)
                {
                    case Classes.User.eAddFavouriteResult.eInvalidUsername:
                        result = Lang.Trans("No such user!");
                        break;
                    case Classes.User.eAddFavouriteResult.eSuccess:
                        try
                        {
                            result =
                                String.Format(Lang.Trans("{0} has been added to your favourite list"),
                                              targetUsername);
                        }
                        catch (NotFoundException) { result = Lang.Trans("No such user!"); }
                        break;
                    case Classes.User.eAddFavouriteResult.eAlreadyAdded:
                        result = Lang.Trans("The user already exists in your favourite list");
                        break;
                    case Classes.User.eAddFavouriteResult.eMaximumFavouritesReached:
                        result =
                            String.Format(
                                Lang.Trans(
                                    "The maximum favourite users of {0} is reached! Please remove someone and try again!"),
                                Config.Users.MaxFavouriteUsers);
                        break;
                }
            }
            else
                result = Lang.Trans("Invalid parameters");

            writeresponse:
            Response.Clear();
            Response.ContentEncoding = Encoding.UTF8;
            Response.Write(String.Format("&result={0}&", result));
            Response.Flush();
            return;
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
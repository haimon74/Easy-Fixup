using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    public partial class UploadLogo : AdminPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadStrings();
            }

            if (imgLogo != null)
                imgLogo.ImageUrl = String.Format("~/Images/logo.png?seed={0}", new System.Random().NextDouble());
        }

        private void LoadStrings()
        {
            btnUpload.Text = Lang.TransA("Upload");
            if (!HasWriteAccess)
                btnUpload.Enabled = false;
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.uploadLogo;
     
            base.OnInit(e);
        }

        protected void btnUpload_Click(object sender, System.EventArgs e)
        {
            System.Drawing.Image image = null;
            try
            {
                image = System.Drawing.Image.FromStream
                    (ufLogo.PostedFile.InputStream);
                image.Save(Server.MapPath("~/Images/logo.png"), ImageFormat.Png);
            }
            catch (System.Runtime.InteropServices.ExternalException)
            {
                Messagebox1.Show(Lang.TransA("The application does not have file permissions to overwrite the image."), Misc.MessageType.Error);
                return;
            }
            catch (System.ArgumentException)
            {
                Messagebox1.Show(Lang.TransA("Invalid image!"), Misc.MessageType.Error);
                return;
            }
        }
    }
}

using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;

namespace AspNetDating.Components
{
    //[ToolboxData("<{0}:FlashUpload runat=server></{0}:FlashUpload>")]
    //[PersistChildren(true), ParseChildren(true), Designer(typeof(FlashUpload))]
    public class FlashUpload : Control
    {
        //private const string FLASH_SWF = "AspNetDating.Components.3rdParty.FlashFileUpload.swf";

        [Category("Behavior")]
        [Description("The page to upload files to.")]
        [DefaultValue("")]
        public string UploadPage
        {
            get
            {
                object o = ViewState["UploadPage"];
                if (o == null)
                    return "";
                return o.ToString();
            }
            set { ViewState["UploadPage"] = value; }
        }

        [Category("Behavior")]
        [Description("Query Parameters to pass to the Upload Page.")]
        [DefaultValue("")]
        public string QueryParameters
        {
            get
            {
                object o = ViewState["QueryParameters"];
                if (o == null)
                    return "";
                return o.ToString();
            }
            set { ViewState["QueryParameters"] = value; }
        }

        [Category("Behavior")]
        [Description("Javascript function to call when all files are uploaded.")]
        [DefaultValue("")]
        public string OnUploadComplete
        {
            get
            {
                object o = ViewState["OnUploadComplete"];
                if (o == null)
                    return "";
                return o.ToString();
            }
            set { ViewState["OnUploadComplete"] = value; }
        }

        [Category("Behavior")]
        [Description("The maximum file size that can be uploaded, in bytes (0 for no limit).")]
        public decimal UploadFileSizeLimit
        {
            get
            {
                object o = ViewState["UploadFileSizeLimit"];
                if (o == null)
                    return 0;
                return (decimal) o;
            }
            set { ViewState["UploadFileSizeLimit"] = value; }
        }

        [Category("Behavior")]
        [Description("The total number of bytes that can be uploaded (0 for no limit).")]
        public decimal TotalUploadSizeLimit
        {
            get
            {
                object o = ViewState["TotalUploadSizeLimit"];
                if (o == null)
                    return 0;
                return (decimal) o;
            }
            set { ViewState["TotalUploadSizeLimit"] = value; }
        }

        [Category("Behavior")]
        [Description(
            "The description of file types that you want uploads restricted to (ex. Images (*.JPG;*.JPEG;*.JPE;*.GIF;*.PNG;))"
            )]
        [DefaultValue("")]
        public string FileTypeDescription
        {
            get
            {
                object o = ViewState["FileTypeDescription"];
                if (o == null)
                    return "";
                return o.ToString();
            }
            set { ViewState["FileTypeDescription"] = value; }
        }

        [Category("Behavior")]
        [Description("The file types to restrict uploads to (ex. *.jpg; *.jpeg; *.jpe; *.gif; *.png;)")]
        [DefaultValue("")]
        public string FileTypes
        {
            get
            {
                object o = ViewState["FileTypes"];
                if (o == null)
                    return "";
                return o.ToString();
            }
            set { ViewState["FileTypes"] = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Page.Form.Enctype = "multipart/form-data";
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);

            string url = "FlashResource.ashx?resname=FlashFileUpload";
            string obj = string.Format("<object classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\"" +
                                       "codebase=\"https://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,0,0\"" +
                                       "width=\"575\" height=\"375\" id=\"fileUpload\" align=\"middle\">" +
                                       "<param name=\"allowScriptAccess\" value=\"sameDomain\" />" +
                                       "<param name=\"movie\" value=\"{0}\" />" +
                                       "<param name=\"quality\" value=\"high\" />" +
                                       "<param name=\"wmode\" value=\"transparent\">" +
                                       "<PARAM NAME=FlashVars VALUE='{3}{4}{5}{6}{7}&uploadPage={1}?{2}'>" +
                                       "<embed src=\"{0}\"" +
                                       "FlashVars='{3}{4}{5}{6}{7}&uploadPage={1}?{2}'" +
                                       "quality=\"high\" wmode=\"transparent\" width=\"575\" height=\"375\" " +
                                       "name=\"fileUpload\" align=\"middle\" allowScriptAccess=\"sameDomain\" " +
                                       "type=\"application/x-shockwave-flash\" " +
                                       "pluginspage=\"https://www.macromedia.com/go/getflashplayer\" />" +
                                       "</object>",
                                       url,
                                       ResolveUrl(UploadPage),
                                       HttpContext.Current.Server.UrlEncode(QueryParameters),
                                       string.IsNullOrEmpty(OnUploadComplete)
                                           ? ""
                                           : "&completeFunction=" + OnUploadComplete,
                                       string.IsNullOrEmpty(FileTypes)
                                           ? ""
                                           : "&fileTypes=" + HttpContext.Current.Server.UrlEncode(FileTypes),
                                       string.IsNullOrEmpty(FileTypeDescription)
                                           ? ""
                                           : "&fileTypeDescription=" +
                                             HttpContext.Current.Server.UrlEncode(FileTypeDescription),
                                       TotalUploadSizeLimit > 0 ? "&totalUploadSize=" + TotalUploadSizeLimit : "",
                                       UploadFileSizeLimit > 0 ? "&fileSizeLimit=" + UploadFileSizeLimit : ""
                );
            writer.Write(obj);
        }
    }
}
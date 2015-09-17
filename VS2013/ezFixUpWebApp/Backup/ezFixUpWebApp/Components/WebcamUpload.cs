using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;

namespace ezFixUp.Components
{
    public class WebcamUpload : Control
    {
        [Category("Behavior")]
        [Description("The page to upload files to.")]
        [DefaultValue("")]
        public string UploadPage
        {
            get
            {
                object o = ViewState["WebcamUploadPage"];
                if (o == null)
                    return "";
                return o.ToString();
            }
            set { ViewState["WebcamUploadPage"] = value; }
        }

        [Category("Behavior")]
        [Description("Query Parameters to pass to the Upload Page.")]
        [DefaultValue("")]
        public string QueryParameters
        {
            get
            {
                object o = ViewState["WebcamQueryParameters"];
                if (o == null)
                    return "";
                return o.ToString();
            }
            set { ViewState["WebcamQueryParameters"] = value; }
        }

        [Category("Behavior")]
        [Description("Javascript function to call when all files are uploaded.")]
        [DefaultValue("")]
        public string OnUploadComplete
        {
            get
            {
                object o = ViewState["WebcamOnUploadComplete"];
                if (o == null)
                    return "";
                return o.ToString();
            }
            set { ViewState["WebcamOnUploadComplete"] = value; }
        }

        [Category("Behavior")]
        [Description("The maximum file size that can be uploaded, in bytes (0 for no limit).")]
        public decimal UploadFileSizeLimit
        {
            get
            {
                object o = ViewState["WebcamUploadFileSizeLimit"];
                if (o == null)
                    return 0;
                return (decimal)o;
            }
            set { ViewState["WebcamUploadFileSizeLimit"] = value; }
        }

        [Category("Behavior")]
        [Description("The total number of bytes that can be uploaded (0 for no limit).")]
        public decimal TotalUploadSizeLimit
        {
            get
            {
                object o = ViewState["WebcamTotalUploadSizeLimit"];
                if (o == null)
                    return 0;
                return (decimal)o;
            }
            set { ViewState["WebcamTotalUploadSizeLimit"] = value; }
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
                object o = ViewState["WebcamFileTypeDescription"];
                if (o == null)
                    return "";
                return o.ToString();
            }
            set { ViewState["WebcamFileTypeDescription"] = value; }
        }

        [Category("Behavior")]
        [Description("The file types to restrict uploads to (ex. *.jpg; *.jpeg; *.jpe; *.gif; *.png;)")]
        [DefaultValue("")]
        public string FileTypes
        {
            get
            {
                object o = ViewState["WebcamFileTypes"];
                if (o == null)
                    return "";
                return o.ToString();
            }
            set { ViewState["WebcamFileTypes"] = value; }
        }

        [Category("Behavior")]
        [Description("The page to upload files to.")]
        [DefaultValue("")]
        public string Width
        {
            get
            {
                object o = ViewState["WebcamWidth"];
                if (o == null)
                    return "";
                return o.ToString();
            }
            set { ViewState["WebcamWidth"] = value; }
        }

        [Category("Behavior")]
        [Description("The page to upload files to.")]
        [DefaultValue("")]
        public string Height
        {
            get
            {
                object o = ViewState["WebcamHeight"];
                if (o == null)
                    return "";
                return o.ToString();
            }
            set { ViewState["WebcamHeight"] = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Page.Form.Enctype = "multipart/form-data";
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);

            string url = "FlashResource.ashx?resname=webcamvid";
            string obj = string.Format("<object classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\"" +
                                       "codebase=\"https://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,0,0\"" +
                                       "width=\"{8}\" height=\"{9}\" id=\"webcamuploader\" align=\"middle\">" +
                                       "<param name=\"allowScriptAccess\" value=\"sameDomain\" />" +
                                       "<param name=\"movie\" value=\"{0}\" />" +
                                       "<param name=\"quality\" value=\"high\" />" +
                                       "<param name=\"bgcolor\" value=\"#FFFFFF\" />" +
                                       "<param name=\"wmode\" value=\"transparent\">" +
                                       "<PARAM NAME=FlashVars VALUE='{3}{4}{5}{6}{7}&uploadPage={1}?{2}'>" +
                                       "<embed src=\"{0}\"" +
                                       "FlashVars='{3}{4}{5}{6}{7}&uploadPage={1}?{2}'" +
                                       "quality=\"high\" wmode=\"transparent\" width=\"{8}\" height=\"{9}\" bgcolor=\"#FFFFFF\" " +
                                       "name=\"webcamuploader\" align=\"middle\" allowScriptAccess=\"sameDomain\" " +
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
                                       UploadFileSizeLimit > 0 ? "&fileSizeLimit=" + UploadFileSizeLimit : "",
                                       Width, Height
                );
            writer.Write(obj);
        }
    }
}

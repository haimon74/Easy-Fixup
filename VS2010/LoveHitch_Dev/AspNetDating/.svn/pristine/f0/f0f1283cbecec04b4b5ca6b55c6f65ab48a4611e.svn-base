using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;

namespace AspNetDating.Services
{
    /// <summary>
    /// Summary description for HeaderService
    /// </summary>
    [WebService(Namespace = "AspNetDating.Services")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class HeaderService : System.Web.Services.WebService
    {

        [WebMethod]
        public string GetHeader()
        {
            // Create a new Page and add the control to it.
            Page page = new Page();
            UserControl ctl =
              (UserControl)page.LoadControl("~/Components/Header.ascx");
            page.Controls.Add(ctl);

            // Render the page and capture the resulting HTML.
            StringWriter writer = new StringWriter();
            HttpContext.Current.Server.Execute(page, writer, false);

            // Return that HTML, as a string.
            return writer.ToString();
        }
    }
}

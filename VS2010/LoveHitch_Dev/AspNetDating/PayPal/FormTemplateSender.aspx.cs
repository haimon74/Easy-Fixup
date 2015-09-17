using System;
using System.Globalization;
using System.IO;
using System.Web.UI;
using AspNetDating.Classes;

namespace AspNetDating.PayPal
{
    public partial class FormTemplateSender : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var response = Page.Response;
            var request = Page.Request;
            var template = request.Params["template"] + ".txt";
            var itemId = request.Params["id"];
            var username = request.Params["un"];

            var html = String.Empty;

            switch (template)
            {
                case "buynow.txt":
                case "buynowSandBox.txt":
                case "modifySubscription.txt":
                case "modifySubscriptionSandBox.txt":
                    BillingPlan plan = BillingPlan.Fetch(Convert.ToInt32(itemId));
                    int subscriptionID = Subscription.Create(username, plan.ID, "PayPal");
                    using (var reader = new StreamReader(Server.MapPath("~/")+template))
                    {
                        string buyNowTemplate = reader.ReadToEnd();
                        html = String.Format(buyNowTemplate,
                                             Config.AdminSettings.Payments.PayPalEmail,
                                             plan.Title, plan.ID,
                                             plan.Amount.ToString("0.00", CultureInfo.InvariantCulture),
                                             plan.Cycle, plan.CycleUnit.ToString().ToCharArray()[0],
                                             subscriptionID, Config.Urls.ThankYou, Config.Urls.Cancel,
                                             Config.Urls.PayPalIPN);
                    }
                    break;
                case "buycredits.txt":
                case "buycreditsSandBox.txt":
                    CreditsPackage package = CreditsPackage.Fetch(Convert.ToInt32(itemId));
                    using (var reader = new StreamReader(Server.MapPath("~/")+template))
                    {
                        html = String.Format(reader.ReadToEnd(), Config.AdminSettings.Payments.PayPalEmail,
                                             package.Name, package.ID,
                                             package.Price.ToString("0.00", CultureInfo.InvariantCulture),
                                             username, Config.Urls.ThankYou, Config.Urls.Cancel,
                                             Config.Urls.PayPalIPN);
                    }
                    break;
            }

            response.Clear();
            response.Write(html);
            response.Flush();
            response.End();

        }
    }
}
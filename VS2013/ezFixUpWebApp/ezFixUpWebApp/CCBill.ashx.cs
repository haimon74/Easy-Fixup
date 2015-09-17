using System;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using ezFixUp.Classes;
using System.Globalization;

namespace ezFixUp
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "https://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CCBill : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string secretKey = context.Request.Params["secretKey"];

            if (String.IsNullOrEmpty(secretKey))
            {
                Global.Logger.LogError("CCBill", "SecretKey is null or empty string!");
                return;
            }

            if (Properties.Settings.Default.CCBillSecretKey != secretKey)
            {
                Global.Logger.LogError("CCBill",
                    String.Format("The secret key '{0}' provided in the web.config file does not match the one passed in the URL '{1}'!",
                    Properties.Settings.Default.CCBillSecretKey, secretKey));
                return;
            }


            string ccBillSubscriptionID = context.Request.Params["subscription_id"];
            string username = context.Request.Params["user"];
            string initialPrice = context.Request.Params["initialPrice"];
            int recurringPeriod = Convert.ToInt32(context.Request.Params["recurringPeriod"]);
            bool isCredits = context.Request.Params["isCredits"] == "1";


            User user = Classes.User.Load(username);
            //if (user.BillingDetails == null)
            //    user.BillingDetails = new BillingDetails();
            //user.BillingDetails.SubscriptionId = subscriptionId;

            if (isCredits/*Config.Credits.Required*/)
            {
                var packages = CreditsPackage.Fetch();
                var package = packages.FirstOrDefault(p => p.Price == decimal.Parse(initialPrice, CultureInfo.InvariantCulture));

                if (package != null)
                {
                    user.Credits += package.Quantity;
                    user.Update(true);
                }
                else
                {
                    Global.Logger.LogError("CCBill", String.Format("There is no credits package with a price {0}!", initialPrice));
                    return;
                }

                Global.Logger.LogError("CCBill", String.Format("{0} credits added to {1}'s account.", package.Quantity, username));
            }
            else
            {
                BillingPlan billingPlan = null;
                foreach (BillingPlan plan in BillingPlan.Fetch())
                {
                    int days = 0;
                    switch (plan.CycleUnit)
                    {
                        case CycleUnits.Days:
                            days = plan.Cycle;
                            break;
                        case CycleUnits.Months:
                            days = plan.Cycle * 30;
                            break;
                        case CycleUnits.Weeks:
                            days = plan.Cycle * 7;
                            break;
                        case CycleUnits.Years:
                            days = plan.Cycle * 365;
                            break;
                    }

                    if (billingPlan == null) billingPlan = plan;
                    if (days == recurringPeriod)
                    {
                        billingPlan = plan;
                        break;
                    }
                }

                //string subscriptionID = context.Request.Params["subscriptionID"];
                int subscriptionID = Subscription.Create(username, billingPlan.ID, "CCBill");

                var subscription = Subscription.Fetch(subscriptionID);

                //subscription.PlanID = billingPlan.ID;
                subscription.Custom = ccBillSubscriptionID;
                subscription.Update();
                subscription.Activate(DateTime.Now, billingPlan);
                //int subscriptionID = Subscription.Create(user.Username, billingPlan.ID, "CCBill");
                //Subscription newSubscription = Subscription.Fetch(subscriptionID);

                Global.Logger.LogError("CCBill", "Subscription activated - " + username);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

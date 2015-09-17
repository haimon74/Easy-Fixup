using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Text;
using AspNetDating.Classes;

namespace AspNetDating.DaoPay
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "https://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class StatusService : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //Multicalling Extended is not implemented 

            string ipAddress = context.Request.UserHostAddress;

            if (!Properties.Settings.Default.DaoPayGatewayIPs.Contains(ipAddress))
                return;

            string orderID = context.Request.QueryString["tid"];
            string status = context.Request.QueryString["stat"];
            string appcode = context.Request.QueryString["appcode"];

            //custom params
            string username = context.Request.QueryString["username"];
            string billingPlanID = context.Request.QueryString["bpid"];
            string creditsPackageID = context.Request.QueryString["cpid"];

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("DaoPay order ID: {0}", orderID ?? "NULL");
            sb.AppendLine();
            sb.AppendFormat("Service Status:{0}", status ?? "NULL");
            sb.AppendLine();

            sb.AppendFormat("BillingPlan ID: {0}", billingPlanID ?? "NULL");
            sb.AppendLine();
            sb.AppendFormat("CreditsPackage ID:{0}", creditsPackageID ?? "NULL");
            sb.AppendLine();

            if (status == "connect")
            {
                string duration = context.Request.QueryString["duration"];
                string calltime = context.Request.QueryString["calltime"];
                string countrycode = context.Request.QueryString["countrycode"];

                sb.AppendFormat("Duration: {0}", duration ?? "NULL");
                sb.AppendLine();
                sb.AppendFormat("Call Time:{0}", calltime ?? "NULL");
                sb.AppendLine();
                sb.AppendFormat("Country Code:{0}", countrycode ?? "NULL");
                sb.AppendLine();
            }
            else if (status == "part" || status == "ok")
            {
                string paid = context.Request.QueryString["paid"];
                string currency = context.Request.QueryString["currency"];
                string origin = context.Request.QueryString["origin"];
                string prodprice = context.Request.QueryString["prodprice"];
                string prodcurrency = context.Request.QueryString["prodcurrency"];

                sb.AppendFormat("Paid: {0}", paid ?? "NULL");
                sb.AppendLine();
                sb.AppendFormat("Currency:{0}", currency ?? "NULL");
                sb.AppendLine();
                sb.AppendFormat("Origin:{0}", origin ?? "NULL");
                sb.AppendLine();
                sb.AppendFormat("Product Price: {0}", prodprice ?? "NULL");
                sb.AppendLine();
                sb.AppendFormat("Product Currency:{0}", prodcurrency ?? "NULL");
                sb.AppendLine();

                if (status == "part")
                { 
                    
                }
                else if (status == "ok")
                {
                    string payout = context.Request.QueryString["payout"];

                    sb.AppendFormat("Payout:{0}", payout ?? "NULL");
                    sb.AppendLine();


                    if (creditsPackageID != null)
                    {
                        int packageID;
                        if (Int32.TryParse(creditsPackageID, out packageID))
                        {
                            var package = CreditsPackage.Fetch(packageID);

                            User user = User.Load(username);
                            user.Credits += package.Quantity;
                            user.Update(true);

                            string description = String.Format("Credits fee ({0}, {1})", package.Price,
                                                               DateTime.Now);
                            int paymentHistoryID = Payments.SavePaymentHistory(user.Username, "DaoPay",
                                                                               package.Price, description,
                                                                               "DaoPay OrderID:" + orderID,
                                                                               1);

                            #region Apply affiliate commission

                            AffiliateCommission.ApplyCommission(user.Username, paymentHistoryID, package.Price,
                                                                "DaoPay OrderID:" + orderID);

                            #endregion
                        }
                        else
                        {
                            LogStatus("DaoPay: Invalid CreditsPackageID");
                            return;
                        }
                    }
                    else if (billingPlanID != null)
                    {
                        int planID;
                        if (Int32.TryParse(billingPlanID, out planID))
                        {
                            var plan = BillingPlan.Fetch(planID);

                            User user = User.Load(username);

                            int subscriptionID = Subscription.Create(username, planID, "DaoPay");

                            var subscription = Subscription.Fetch(subscriptionID);
                            subscription.Activate(DateTime.Now, plan);

                            Subscription.RequestCancellation(subscriptionID);

                            string description = String.Format("Subscription fee ({0}, {1})", plan.Amount, DateTime.Now);

                            int paymentHistoryID = Payments.SavePaymentHistory(username, "DaoPay", (decimal)plan.Amount, description, "DaoPay OrderID:" + orderID, 1);

                            #region Apply affiliate commission

                            AffiliateCommission.ApplyCommission(subscription.Username, paymentHistoryID, (decimal)plan.Amount, "DaoPay OrderID:" + orderID);

                            #endregion

                        }
                        else
                        {
                            LogStatus("DaoPay: Invalid BillingPlanID");
                            return;
                        }
                    }
                }
            }

            LogStatus(sb.ToString());
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private void LogStatus(string text)
        {
            try
            {
                Global.Logger.LogError("LogIPN", text);
            }
            catch
            {
            }   
        }
    }
}

/* ASPnetDating 
 * Copyright (C) 2003-2010 eStream 
 * lovehitch.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at lovehitch.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Threading;
using System.Timers;
using AspNetDating;
using Microsoft.ApplicationBlocks.Data;
using PayPal.Payments.Common.Utility;
using PayPal.Payments.DataObjects;
using PayPal.Payments.Transactions;
using Timer=System.Timers.Timer;
using System.Text.RegularExpressions;
using System.Collections;
using System.Linq;

namespace AspNetDating.Classes
{
    public class Payments
    {
        private static Timer tPayments;
        private static bool paymentsLock = false;

        public static void InitializePaymentsTimer()
        {
            tPayments = new Timer();
            tPayments.AutoReset = true;
            tPayments.Interval = TimeSpan.FromHours(1).TotalMilliseconds;
            tPayments.Elapsed += new ElapsedEventHandler(tPayments_Elapsed);
            tPayments.Start();

            // Run payment processing the 1st time
            tPayments_Elapsed(null, null);
        }

        private static void tPayments_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!Config.Misc.SiteIsPaid)
                return;
            
            DateTime dtLastTick = DBSettings.Get("Payments_LastTimerTick", DateTime.Now);

            if (DateTime.Now.Subtract(dtLastTick) >= TimeSpan.FromHours(24))
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(AsyncProcessPaymentsQueue), null);
                DBSettings.Set("Payments_LastTimerTick", DateTime.Now);
            }

            if (Config.AdminSettings.Payments.PaymentProcessors.FirstOrDefault(pp => pp == "CCBill") != null &&
                !String.IsNullOrEmpty(Properties.Settings.Default.CCBillSecretKey))
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(AsyncProcessPaymentsCCBillPayments), null);
            }
        }

        private static void AsyncProcessPaymentsCCBillPayments(object data)
        {
            CCBill.ProcessCCBillPayments();
        }

        private static void AsyncProcessPaymentsQueue(object data)
        {
            if (paymentsLock)
            {
                return;
            }

            try
            {
                paymentsLock = true;

                BasicSearch search = new BasicSearch();
                search.deleted_isSet = false;
                search.active_isSet = false;
                search.hasAnswer_isSet = false;
                search.visible_isSet = false;
                search.Paid = true;

                UserSearchResults results = search.GetResults();
                if (results != null && results.Usernames != null)
                    foreach (string username in results.Usernames)
                    {
                        CheckPaymentStatus(username);
                    }
            }
            catch (Exception err)
            {
                Global.Logger.LogError("AsyncProcessEmailQueue", err);
            }
            finally
            {
                paymentsLock = false;
            }
        }

        public static void CheckPaymentStatus(string username)
        {
            User user = null;
            try
            {
                user = User.Load(username);
            }
            catch (NotFoundException)
            {
                return;
            }


            Subscription subscription = Subscription.FetchActiveSubscription(username);
            if (subscription == null || subscription.RenewDate > DateTime.Now || subscription.PaymentProcessor == "CCBill")
                return;

            if (subscription.CancellationRequested || user.Deleted)
            {
                Subscription.Cancel(subscription.ID);
                user.Paid = false;
                user.Update(true);
                return;
            }

            BillingPlan plan = BillingPlan.FetchBySubscriptionID(subscription.ID);
            plan.ApplyDiscounts(user);

            TransactionDetails transactionDetails = null;

            if (subscription.PaymentProcessor == "PayflowPro" ||
                subscription.PaymentProcessor == "Authorize.NET")
            {
                transactionDetails = TransactionDetails.FromBillingDetails(
                    user.BillingDetails);
                transactionDetails.Amount = Convert.ToDecimal(plan.Amount);
            }

            IPaymentGateway gateway;
            switch (subscription.PaymentProcessor)
            {
                case "PayflowPro":
                    gateway = new PayflowPro();
                    break;
                case "Authorize.NET":
                    gateway = new AuthorizeNet();
                    break;
                case "Check":
                    gateway = new Check();
                    break;
                default:
                    Global.Logger.LogError("CheckPaymentStatus", "Selected payment processor was not recognized!");
                    return;
            }


            eGatewayResponse gatewayResponse;

            if (subscription.PaymentProcessor == "Check")
            {
                gatewayResponse = eGatewayResponse.Declined;
            }
            else
            {
                gatewayResponse = gateway.SubmitTransaction(username, transactionDetails,
                                                    "Subscription fee (" +
                                                    plan.Amount.ToString("c") + ", " +
                                                    subscription.RenewDate.ToShortDateString() +
                                                    ")");
            }

            if (gatewayResponse == eGatewayResponse.Approved)
            {
                subscription.Renew(plan);

                // send an email for every new charge
                #region Send an email

                MiscTemplates.SubscriptionCharge sendChargeNotificationEmail = new MiscTemplates.SubscriptionCharge(user.LanguageId);
                Email.Send(Config.Urls.Home, user.Email,
                                                sendChargeNotificationEmail.GetFormattedSubject(Config.Misc.SiteTitle),
                                                sendChargeNotificationEmail.GetFormattedBody(Config.Urls.Home, user.Username), false);

                #endregion
                
            }
            else
            {
                if (DateTime.Now.Subtract(subscription.RenewDate) > TimeSpan.FromDays(3))
                {
                    Subscription.Cancel(subscription.ID);
                    user.Paid = false;
                    user.Update(true);
                }
            }
        }

        public static int SavePaymentHistory(string username, string paymentprocessor, decimal amount, string description,
                                              string notes, int status)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.ExecuteScalar(conn, "SavePaymentHistory", username, paymentprocessor, amount, description,
                                          notes, status);

                return Convert.ToInt32(result);
            }
        }
    }

    [Serializable]
    public class BillingDetails
    {
        public string CardNumber;
        public int CardExpirationMonth;
        public int CardExpirationYear;

        public string FirstName;
        public string LastName;
        public string Address;
        public string City;
        public string State;
        public string Zip;
        public string Country;
        public string Phone;
    }

    public class TransactionDetails : BillingDetails
    {
        public decimal Amount;

        public static TransactionDetails FromBillingDetails(BillingDetails billingDetails)
        {
            TransactionDetails transactionDetails = new TransactionDetails();
            transactionDetails.FirstName = billingDetails.FirstName;
            transactionDetails.LastName = billingDetails.LastName;
            transactionDetails.Address = billingDetails.Address;
            transactionDetails.City = billingDetails.City;
            transactionDetails.State = billingDetails.State;
            transactionDetails.Zip = billingDetails.Zip;
            transactionDetails.Country = billingDetails.Country;
            transactionDetails.Phone = billingDetails.Phone;
            transactionDetails.CardNumber = billingDetails.CardNumber;
            transactionDetails.CardExpirationMonth = billingDetails.CardExpirationMonth;
            transactionDetails.CardExpirationYear = billingDetails.CardExpirationYear;
            return transactionDetails;
        }
    }

    public enum eGatewayResponse
    {
        Approved = 1,
        Declined = 2,
        Error = 3
    }

    public interface IPaymentGateway
    {
        string Name { get;}        
        eGatewayResponse SubmitTransaction(string username, TransactionDetails details, string description);
    }

    public class AuthorizeNet : IPaymentGateway
    {
        private const string GATEWAY_URL = "https://secure.authorize.net/gateway/transact.dll";
        private const string GATEWAY_URL_TEST = "https://test.authorize.net/gateway/transact.dll";

        #region IPaymentGateway Members

        public string Name { get { return "Authorize.NET"; } }        
        public eGatewayResponse SubmitTransaction(string username, TransactionDetails details,
                                                  string description)
        {
            String result = "";
            string strPost;
            strPost = String.Format("x_login={0}&x_tran_key={1}&x_method=CC&x_type=AUTH_CAPTURE"
                                    + "&x_amount={2}&x_delim_data=TRUE&x_delim_char=|&x_relay_response=FALSE"
                                    + "&x_card_num={3}&x_exp_date={4}&x_test_request={5}&x_version=3.1",
                                    Properties.Settings.Default.AuthorizeNetLogin,
                                    Properties.Settings.Default.AuthorizeNetTranKey,
                                    details.Amount, details.CardNumber,
                                    details.CardExpirationMonth.ToString() + details.CardExpirationYear.ToString(),
                                    Properties.Settings.Default.AuthorizeNetTest.ToString().ToUpper());

            Global.Logger.LogInfo("AuthorizeNet_SubmitTransaction", strPost);

            StreamWriter myWriter = null;

            HttpWebRequest objRequest = (HttpWebRequest) WebRequest.Create(
                                                             Properties.Settings.Default.AuthorizeNetTest.ToString()
                                                                 .ToUpper() == "TRUE"
                                                                 ? GATEWAY_URL_TEST
                                                                 : GATEWAY_URL);
            objRequest.Method = "POST";
            objRequest.ContentLength = strPost.Length;
            objRequest.ContentType = "application/x-www-form-urlencoded";

            try
            {
                myWriter = new StreamWriter(objRequest.GetRequestStream());
                myWriter.Write(strPost);
            }
            catch (Exception err)
            {
                Global.Logger.LogError("AuthorizeNet_SubmitTransaction", err);
                return eGatewayResponse.Error;
            }
            finally
            {
                myWriter.Close();
            }

            HttpWebResponse objResponse = (HttpWebResponse) objRequest.GetResponse();
            using (StreamReader sr =
                new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                sr.Close();
            }

            Global.Logger.LogInfo("AuthorizeNet_SubmitTransaction", result);

            string[] results = result.Split('|');
            int status = -1;
            try
            {
                status = Convert.ToInt32(results[0]);
            }
            catch (FormatException)
            {
            }

            int paymentHistoryID =
                Payments.SavePaymentHistory(username, "AuthorizeNet", details.Amount, description, result, status);
            
            if (status == 1)
            {
                AffiliateCommission.ApplyCommission(username, paymentHistoryID, details.Amount, result);
            }

            switch (status)
            {
                case 1:
                    return eGatewayResponse.Approved;
                case 2:
                    return eGatewayResponse.Declined;
                default:
                    return eGatewayResponse.Error;
            }
        }

        #endregion
    }

    public class Check : IPaymentGateway
    {
        public string Name
        {
            get { return "Check"; }
        }

        public eGatewayResponse SubmitTransaction(string username, TransactionDetails details, string description)
        {
            int status = 1; //Approved

            Payments.SavePaymentHistory(username, Name, details.Amount, description, String.Empty, status);

            return eGatewayResponse.Approved;
        }
    }

    public class PayflowPro : IPaymentGateway
    {
        public string Name { get { return "PayflowPro"; } }
        public eGatewayResponse SubmitTransaction(string username, TransactionDetails details,
                                                  string description)
        {
            string requestID = PayflowUtility.RequestId;

            string user = PayflowUtility.AppSettings("PayflowUser"); 
            string vendor = PayflowUtility.AppSettings("PayflowVendor");
            string partner = PayflowUtility.AppSettings("PayflowPartner");
            string password = PayflowUtility.AppSettings("PayflowPassword");

            UserInfo userInfo = new UserInfo(user, vendor, partner, password);
            
            bool testServer;
            if (!Boolean.TryParse(PayflowUtility.AppSettings("PayflowTestServer"), out testServer))
                return eGatewayResponse.Error;
            
            string certPath = AppDomain.CurrentDomain.BaseDirectory;
            certPath += testServer ? @"bin\testcerts" : @"bin\certs";

            string host = PayflowUtility.AppSettings("PAYFLOW_HOST");

            PayflowConnectionData connection = new PayflowConnectionData(host, certPath);

            Invoice inv = new Invoice();

            string currency = PayflowUtility.AppSettings("PayflowCurrency");
            Currency amt = new Currency(details.Amount, currency);
            amt.Round = true;
            
            inv.Amt = amt;
            inv.CustRef = username;
            inv.Comment1 = username;

            BillTo bill = new BillTo();

            bill.FirstName = details.FirstName;
            bill.LastName = details.LastName;
            bill.Zip = details.Zip;
            bill.City = details.City;
            bill.State = details.State;
            bill.Street = details.Address;
            bill.PhoneNum = details.Phone;
            bill.BillToCountry = details.Country;
            
            inv.BillTo = bill;

            string cardNumber = details.CardNumber.Replace("-", String.Empty);
            
            string expMonth = details.CardExpirationMonth.ToString();
            if (expMonth.Length == 1)
                expMonth = "0" + expMonth;
            
            string expYear = details.CardExpirationYear.ToString();
            expYear = expYear.Substring(expYear.Length - 2);
            
            CreditCard cc = new CreditCard(cardNumber, expMonth + expYear);
            CardTender card = new CardTender(cc);

            SaleTransaction trans = new SaleTransaction(userInfo, connection, inv, card, requestID);

            trans.Verbosity = "MEDIUM";

            Response resp = trans.SubmitTransaction();
            if (resp != null)
            {
                Global.Logger.LogInfo("PayflowPro_SubmitTransaction_Request", resp.RequestString);
                Global.Logger.LogInfo("PayflowPro_SubmitTransaction_Response", resp.ResponseString);
                Global.Logger.LogInfo("PayflowPro_SubmitTransaction_Response_Normalized", resp.TransactionResponse.RespMsg);
                
                if (resp.TransactionResponse.Result == 0)
                {
                    int paymentHistoryID = Payments.SavePaymentHistory(username, "PayflowPro", details.Amount, description, resp.TransactionResponse.RespMsg, 1);

                    AffiliateCommission.ApplyCommission(username, paymentHistoryID, details.Amount, resp.TransactionResponse.RespMsg);

                    return eGatewayResponse.Approved;
                }
                else
                {
                    Payments.SavePaymentHistory(username, "PayflowPro", details.Amount, description, resp.TransactionResponse.RespMsg, 2);
                    return eGatewayResponse.Error;
                }
            }
            
            return eGatewayResponse.Error;
        }
    }

    public class CCBill
    {
        public static void ProcessCCBillPayments()
        {
            DateTime dtLastCCBillCheck = DBSettings.Get("Payments_LastCCBillCheck", DateTime.Now.AddHours(-12));

            double ccbillTimeOffset = Convert.ToDouble(ConfigurationManager.AppSettings["CCBillTimeOffset"]);
            string ccbillUrl = String.Format(ConfigurationManager.AppSettings["CCBillDataLinkUrl"],
                dtLastCCBillCheck.AddHours(ccbillTimeOffset).ToString("yyyyMMddHHmmss"),
                DateTime.Now.AddHours(ccbillTimeOffset).ToString("yyyyMMddHHmmss"));
            WebClient wc = new WebClient();
            string ccbillResponse = wc.DownloadString(ccbillUrl);

            Global.Logger.LogError("CCBill Response - " + ccbillResponse);
            string[] lines = Regex.Split(ccbillResponse, @"\n");

            foreach (string line in lines)
            {
                if (line.Trim().Length == 0) continue;
                string[] cells = Regex.Split(line, ",");

                try
                {
                    if (cells[0].Trim('"') == "NEW")
                    {
                        /* -- subscription creation handled by posback handler; datalink commented
                        string subscriptionId = cells[3].Trim('"');
                        string username = cells[7].Trim('"');
                        int initialPeriod = Convert.ToInt32(cells[18].Trim('"'));

                        User user = User.Load(username);
                        if (user.BillingDetails == null)
                            user.BillingDetails = new BillingDetails();
                        user.BillingDetails.SubscriptionId = subscriptionId;

                        BillingPlan billingPlan = null;
                        foreach (BillingPlan plan in BillingPlan.Fetch())
                        {
                            if (billingPlan == null) billingPlan = plan;
                            if (plan.Cycle == initialPeriod)
                            {
                                billingPlan = plan;
                                break;
                            }
                        }

                        int subscriptionID = Subscription.Create(user.Username, billingPlan.ID);
                        Subscription newSubscription = Subscription.Fetch(subscriptionID);
                        newSubscription.Activate(DateTime.Now, billingPlan);
                        */

                        //Global.Logger.LogError("Subscription activated - " + line);
                    }
                    else if (cells[0].Trim('"') == "REBILL")
                    {
                        string subscriptionId = cells[3].Trim('"'); //ccbill subscription id

                        //BasicSearch userSearch = new BasicSearch();
                        //userSearch.Paid = true;
                        //userSearch.Active = true;
                        //UserSearchResults results = userSearch.GetResults();
                        //if (results != null && results.Usernames != null)
                        //{
                        //    foreach (string username in results.Usernames)
                        //    {
                        //        User user = User.Load(username);
                        //        if (user.BillingDetails != null
                        //            && user.BillingDetails.SubscriptionId == subscriptionId)
                        //        {
                        //            Subscription subscription = Subscription.FetchActiveSubscription(username);
                        //            if (subscription == null) break;
                        //            BillingPlan plan = BillingPlan.FetchBySubscriptionID(subscription.ID);
                        //            subscription.Renew(plan);
                        //            break;
                        //        }
                        //    }
                        //}

                        Subscription subscription = Subscription.Fetch(subscriptionId/*this is a ccbill subscription id*/);

                        if (subscription != null)
                        {
                            BillingPlan plan = BillingPlan.FetchBySubscriptionID(subscription.ID);
                            subscription.Renew(plan);
                            Global.Logger.LogError("Subscription rebilled - " + line);

                            #region Send an email

                            try
                            {
                                var user = User.Load(subscription.Username);

                                MiscTemplates.SubscriptionCharge sendChargeNotificationEmail = new MiscTemplates.SubscriptionCharge();
                                Email.Send(Config.Urls.Home, user.Email,
                                                                sendChargeNotificationEmail.GetFormattedSubject(Config.Misc.SiteTitle),
                                                                sendChargeNotificationEmail.GetFormattedBody(Config.Urls.Home, user.Username), false);
                            }
                            catch (NotFoundException)
                            { 
                            }

                            #endregion
                        }
                        else
                        {
                            Global.Logger.LogError("Subscription rebilled but no subscription with such id was found in the database " + line);
                        }
                        
                    }
                    else if (cells[0].Trim('"') == "EXPIRE" || cells[0].Trim('"') == "CHARGEBACK" ||
                             cells[0].Trim('"') == "VOID")
                    {
                        string subscriptionId = cells[3].Trim('"'); //ccbill subscription id

                        //BasicSearch userSearch = new BasicSearch();
                        //userSearch.Paid = true;
                        //userSearch.Active = true;
                        //UserSearchResults results = userSearch.GetResults();
                        //if (results != null && results.Usernames != null)
                        //{
                        //    foreach (string username in results.Usernames)
                        //    {
                        //        User user = User.Load(username);
                        //        if (user.BillingDetails != null
                        //            && user.BillingDetails.SubscriptionId == subscriptionId)
                        //        {
                        //            Subscription subscription = Subscription.FetchActiveSubscription(username);
                        //            if (subscription == null) break;
                        //            Subscription.Cancel(subscription.ID);
                        //            break;
                        //        }
                        //    }
                        //}

                        Subscription subscription = Subscription.Fetch(subscriptionId);

                        if (subscription != null)
                        {
                            Subscription.Cancel(subscription.ID);
                        }

                        Global.Logger.LogError("Subscription cancelled - " + line);
                    }
                }
                catch (Exception err)
                {
                    Global.Logger.LogError("Payments_LastCCBillCheck", "Error while handling payment line: " + err);
                }
            }

            DBSettings.Set("Payments_LastCCBillCheck", DateTime.Now);
        }
    }
}
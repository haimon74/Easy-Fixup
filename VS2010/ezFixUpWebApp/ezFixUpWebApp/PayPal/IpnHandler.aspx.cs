using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using ezFixUp;
using ezFixUp.Classes;
using PayPal.Web.Controls;

namespace ASPNET.StarterKit.Commerce
{
    /// <summary>
    /// Handles the IPN post back message from PayPal, converts the IPN message into a 
    /// corresponding <see cref="IpnTransaction"/> and then calls the <see cref="IpnService"/>
    /// web service to process this IPN message.
    /// </summary>
    /// <remarks>
    /// This page is used to handle the current PayPal IPN system. Future releases may
    /// not require this page.
    /// </remarks>
    public partial class IpnHandler : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strFormValues = Request.Form.ToString();
            string strNewValue;
            string strResponse;

            // Create the request back
            HttpWebRequest req;
            if (Config.AdminSettings.Payments.PayPalSandbox)
                req = (HttpWebRequest)WebRequest.Create("https://www.sandbox.paypal.com/cgi-bin/webscr");
            else
                req = (HttpWebRequest)WebRequest.Create("https://www.paypal.com/cgi-bin/webscr");

            // Set values for the request back
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            strNewValue = strFormValues + "&cmd=_notify-validate";
            req.ContentLength = strNewValue.Length;

            // Write the request back IPN strings
            StreamWriter stOut = new StreamWriter(req.GetRequestStream(), Encoding.Default);
            stOut.Write(strNewValue);
            stOut.Close();

            // Do the request to PayPal and get the response
            StreamReader stIn = new StreamReader(req.GetResponse().GetResponseStream());
            strResponse = stIn.ReadToEnd();
            stIn.Close();

            // Confirm whether the IPN was VERIFIED or INVALID. If INVALID, just ignore the IPN
            if (strResponse == "VERIFIED")
            {
                // Create the IpnTransaction
                IpnTransaction ipn = CreateIpnTransaction();
                LogIPN("VERIFIED " + ipn.RawIPN);

                // Process the Transaction
                try
                {
                    ProcessTransaction(ipn);
                }
                catch (Exception ex) { LogIPN("EXCEPTION " + ex.ToString()); }

            }
            else
            {
                LogIPN(strResponse + strFormValues);
            }
        }

        private bool ProcessTransaction(IpnTransaction ipnTx)
        {
            // TODO: Determine what you want todo with the IPN transaction message. 
            // For example, typically an ecommerce site would verify the order details with
            // the database to verify that the price was not changed and that the order was
            // actually placed by the original customer.

            // The following code will check for an existing shopping cart order 
            // and update it's payment information. If the order was not found, 
            // then this IPN is ignored. Also note that "Buy Now" purchases will 
            // not have a corresponding order and thus IPN messages will be ignored.

            // Verify that the order is valid
            if (ipnTx.Custom == null || ipnTx.Custom == string.Empty)
            {
                return false;
            }
            //else
            //{
            //    if (!Config.Credits.Required)
            //    {
            //        // Try converting to int, should be an int and convertable if 
            //        // original order was placed by PayPalCommerce
            //        try
            //        {
            //            subscriptionID = Int32.Parse(ipnTx.Custom);
            //        }
            //        catch (Exception)
            //        {
            //            throw new Exception(String.Format("Invalid subscription id - {0}", ipnTx.Custom));
            //        }
            //    }
            //}

            //if (Config.Credits.Required)
            //{
            if (ipnTx.TransactionType == TransactionType.Web_Accept)
            {
                int packageId;

                try
                {
                    packageId = Int32.Parse(ipnTx.Items[0].ItemNumber);
                }
                catch (Exception)
                {
                    throw new Exception(String.Format("Invalid package id - {0}", ipnTx.Items[0].ItemNumber));
                }

                CreditsPackage package = CreditsPackage.Fetch(packageId);

                if (package == null)
                {
                    throw new Exception(String.Format("There is no package with id = {0}", packageId));
                }

                if (package.Price != (decimal)ipnTx.McGross)
                    throw new Exception(String.Format("Invalid amount! The amount from the package {0} differs from the value {1} ",
                    package.Price, (decimal)ipnTx.McGross));

                User user;
                try
                {
                    user = ezFixUp.Classes.User.Load(ipnTx.Custom);
                }
                catch (NotFoundException)
                {
                    throw new Exception(String.Format("There is no user with username {0}", ipnTx.Custom));
                }

                user.Credits += package.Quantity;
                user.Update(true);

                #region Apply affiliate commission

                string description = String.Format("Credits fee ({0}, {1})", package.Price, DateTime.Now);
                string notes = Config.AdminSettings.Payments.PayPalSandbox ? "SANDBOX" : "LIVEMODE";
                int paymentHistoryID = Payments.SavePaymentHistory(user.Username, "PayPal", package.Price, description, notes, 1);

                AffiliateCommission.ApplyCommission(user.Username, paymentHistoryID, package.Price, notes);

                #endregion
            }
            //}
            else
            {
                int subscriptionID;

                try
                {
                    subscriptionID = Int32.Parse(ipnTx.Custom);
                }
                catch (Exception)
                {
                    throw new Exception(String.Format("Invalid subscription id - {0}", ipnTx.Custom));
                }

                if (ipnTx.TransactionType == TransactionType.Subscr_Signup)
                {
                    float amount = ipnTx.MC_AMOUNT3;//ipnTx.SubscriptionItems[2].Amount;
                    int cycle = ipnTx.SubscriptionItems[2].Period;
                    int cycleUnits = Convert.ToInt32(ipnTx.SubscriptionItems[2].TimeUnits);

                    //some validations
                    BillingPlan plan = BillingPlan.FetchBySubscriptionID(subscriptionID);
                    if (amount != plan.Amount)
                    {
                        return false;
                    }

                    if (cycle != plan.Cycle ||
                        cycleUnits !=
                        Convert.ToInt32(plan.CycleUnit)
                        )
                    {
                        return false;
                    }

                    if (Config.AdminSettings.Payments.PayPalEmail.ToLower() != ipnTx.ReceiverEmail.ToLower())
                    {
                        LogIPN(String.Format("The paypal email in the web.config file '{0}' is different from '{1}' that is configured in the merchant account",
                            Config.AdminSettings.Payments.PayPalEmail.ToLower(), ipnTx.ReceiverEmail.ToLower()));
                        return false;
                    }

                    //activate subscription
                    Subscription subscription = Subscription.Fetch(subscriptionID);
                    subscription.Activate(ipnTx.SubscrDate, plan);

                    string description = String.Format("Subscription fee ({0}, {1})", plan.Amount, DateTime.Now);
                    string notes = Config.AdminSettings.Payments.PayPalSandbox ? "SANDBOX" : "LIVEMODE";
                    int paymentHistoryID = Payments.SavePaymentHistory(subscription.Username, "PayPal", (decimal)plan.Amount, description, notes, 1);

                    #region Apply affiliate commission

                    AffiliateCommission.ApplyCommission(subscription.Username, paymentHistoryID, (decimal)plan.Amount, notes);

                    #endregion
                }

                if (ipnTx.TransactionType == TransactionType.Subscr_Payment && ipnTx.PaymentStatus == PaymentStatus.Completed)
                {
                    Subscription subscription = Subscription.Fetch(subscriptionID);
                    BillingPlan plan = BillingPlan.FetchBySubscriptionID(subscriptionID);

                    if (!subscription.Confirmed || subscription.RenewDate >= DateTime.Now.AddDays(3))
                        return true;

                    subscription.Renew(plan);

                    string description = String.Format("Subscription fee ({0}, {1})", plan.Amount, DateTime.Now);
                    string notes = Config.AdminSettings.Payments.PayPalSandbox ? "SANDBOX" : "LIVEMODE";
                    int paymentHistoryID = Payments.SavePaymentHistory(subscription.Username, "PayPal", (decimal)plan.Amount, description, notes, 1);

                    #region Apply affiliate commission

                    AffiliateCommission.ApplyCommission(subscription.Username, paymentHistoryID, (decimal)plan.Amount, notes);

                    #endregion
                }

                if (ipnTx.TransactionType == TransactionType.Subscr_Modify)
                {
                    float amount = ipnTx.MC_AMOUNT3;//ipnTx.SubscriptionItems[2].Amount;
                    int cycle = ipnTx.SubscriptionItems[2].Period;
                    int cycleUnits = Convert.ToInt32(ipnTx.SubscriptionItems[2].TimeUnits);

                    BillingPlan plan = BillingPlan.FetchByPlanData(amount, cycle, cycleUnits);

                    Subscription subscription = Subscription.Fetch(subscriptionID);
                    subscription.RenewDate = ipnTx.SubscrEffectiveDate;

                    DateTime effectiveDate = ipnTx.SubscrEffectiveDate;

                    switch (plan.CycleUnit)
                    {
                        case CycleUnits.Days:
                            subscription.RenewDate = subscription.RenewDate.AddDays(plan.Cycle);
                            break;
                        case CycleUnits.Weeks:
                            subscription.RenewDate = subscription.RenewDate.AddDays(plan.Cycle * 7);
                            break;
                        case CycleUnits.Months:
                            subscription.RenewDate = subscription.RenewDate.AddMonths(plan.Cycle);
                            break;
                        case CycleUnits.Years:
                            subscription.RenewDate = subscription.RenewDate.AddYears(plan.Cycle);
                            break;
                    }

                    subscription.PlanID = plan.ID;

                    subscription.Update();
                }

                switch (ipnTx.TransactionType)
                {
                    case TransactionType.Subscr_Cancel:
                        Subscription.RequestCancellation(subscriptionID);
                        break;
                    case TransactionType.Subscr_Eot:
                        //case TransactionType.Subscr_Cancel:
                        //change the user's status to INACTIVE
                        Subscription.Cancel(subscriptionID);
                        ezFixUp.Classes.User.SetAsPaidUser(subscriptionID, false);
                        break;
                }
            }
            return true;
        }

        #region Helper Methods used to create an IpnTransaction object

        /// <summary>
        /// Creates an <see cref="IpnTransaction"/> based on the current instance of the IPN message.
        /// </summary>
        /// <remarks>
        /// This will use the current <see cref="HttpRequest.Form"/> values.
        /// </remarks>
        /// <returns>An instance of <see cref="IpnTransaction"/> that represents the current IPN.</returns>
        private IpnTransaction CreateIpnTransaction()
        {
            IpnTransaction ipn = new IpnTransaction();

            // Do some preprocessing first
            if (Request.Form["txn_type"] == "cart")
            {
                // Figure out how many items in the cart
                int cartCount = Int32.Parse(Request.Form["num_cart_items"] ?? "1");

                for (int i = 0; i < cartCount; i++)
                {
                    ipn.Items.Add(new CartItem());
                }
            }

            // Loop through all the keys in the Form
            Debug.Assert(Request.Form != null, "Form was null!");
            Debug.Assert(Request.Form.Keys != null, "Form Keys was null!");
            foreach (string key in Request.Form.Keys)
            {
                try
                {
                    SetIpnTransactionProperties(key, Request.Form[key], ipn);
                }
                catch (Exception)
                {
                    // An error in parsing the key/value will be skipped
                    continue;
                }
            }

            ipn.RawIPN = Request.Form.ToString();
            return ipn;
        }

        /// <summary>
        /// Converts a date and time format returned by the IPN message into a 
        /// <see cref="DateTime"/> value.
        /// <remarks>
        /// Abides by the IPN date time format as specified in the IPN specifications
        /// dated 9/19/2003. Must always be in the following sample format:<br/>
        /// <code>18:30:30 Jan 01, 2000 PDT</code>
        /// </remarks>
        /// </summary>
        /// <param name="strIpnDate">The original IPN date and time.</param>
        /// <returns>A <see cref="DateTime"/> of the IPN date and time.</returns>
        private static DateTime ConvertIpnDate(string strIpnDate)
        {
            // Get rid of the TimeZone at the end of the IPN Date Time
            string date = strIpnDate.Substring(0, strIpnDate.Length - 3).Trim();

            int nHour, nMinute, nSecond;
            int nMonth, nDay, nYear;

            try
            {
                // Hard coded beginning/length values

                nHour = Int32.Parse(date.Substring(0, 2));
                nMinute = Int32.Parse(date.Substring(3, 2));
                nSecond = Int32.Parse(date.Substring(6, 2));

                nMonth = DateTime.ParseExact(date.Substring(9, 3), "MMM", null).Month;
                nDay = Int32.Parse(date.Substring(13, 2));
                nYear = Int32.Parse(date.Substring(17, 4));

                return new DateTime(nYear, nMonth, nDay, nHour, nMinute, nSecond);
            }
            catch (FormatException e)
            {
                // Invalid date time format, throw the error
                throw e;
            }
        }


        /// <summary>
        /// Helper function used to set the property value for an <see cref="IpnTransaction"/> object
        /// for the given key.
        /// </summary>
        /// <param name="key">The key used to find the property.</param>
        /// <param name="val">The value to set to the property.</param>
        /// <param name="ipn">The <see cref="IpnTransaction"/> to set to.</param>
        private void SetIpnTransactionProperties(string key, string val, IpnTransaction ipn)
        {
            Debug.Assert(key != null, "Cannot find property with key == null!");
            Debug.Assert(val != null, "Cannot set property with val == null!");
            Debug.Assert(ipn != null, "Cannot set property with ipn == null!");


            // Set Date/Time Properties
            if (SetIpnTransactionProperties_Date(key, val, ipn))
            {
                return;
            }

                // Set Buyer Information Properties
            else if (SetIpnTransactionProperties_Customer(key, val, ipn))
            {
                return;
            }

                // Set Product Information Properties
            else if (SetIpnTransactionProperties_Product(key, val, ipn))
            {
                return;
            }

                // Set Subscriptions Information Properties
            else if (SetIpnTransactionProperties_SubscriptionItem(key, val, ipn))
            {
                return;
            }

                // Set Generically
            else
            {
                ipn.SetProperty(key, val);
            }
        }

        #region SetIpnTransactionProperties helper methods

        /// <summary>
        /// Used to set IPN values related to a subscription.
        /// </summary>
        /// <remarks>
        /// This method should not be used directly. Instead use
        /// <see cref="SetIpnTransactionProperties"/>.
        /// </remarks>
        /// <param name="key">The key used to find the property.</param>
        /// <param name="val">The value to set to the property.</param>
        /// <param name="ipn">The <see cref="IpnTransaction"/> to set to.</param>
        /// <returns>TRUE if a property was set, otherwise returns FALSE.</returns>
        private bool SetIpnTransactionProperties_SubscriptionItem(string key, string val, IpnTransaction ipn)
        {
            // Only want to set these values
            int nSubItemIndex;
            switch (key)
            {
                case "amount1":
                case "amount2":
                case "amount3":
                    nSubItemIndex = Int32.Parse(key.Substring(6)) - 1;
                    ipn.SubscriptionItems[nSubItemIndex].Amount = Convert.ToSingle(val);
                    break;

                case "period1":
                case "period2":
                case "period3":
                    nSubItemIndex = Int32.Parse(key.Substring(6)) - 1;

                    // Parse the period IPN variable into it's parts
                    string strPeriod, strTimeUnit;
                    strPeriod = val.Substring(0, val.IndexOf(' '));
                    strTimeUnit = val.Substring(val.IndexOf(' ') + 1);

                    // Set corresponding SubscriptionItems properties
                    ipn.SubscriptionItems[nSubItemIndex].Period = Convert.ToInt32(strPeriod);
                    switch (strTimeUnit.ToUpper())
                    {
                        case "D":
                            ipn.SubscriptionItems[nSubItemIndex].TimeUnits = SubscriptionPeriodUnits.Days;
                            break;

                        case "W":
                            ipn.SubscriptionItems[nSubItemIndex].TimeUnits = SubscriptionPeriodUnits.Weeks;
                            break;

                        case "M":
                            ipn.SubscriptionItems[nSubItemIndex].TimeUnits = SubscriptionPeriodUnits.Months;
                            break;

                        case "Y":
                            ipn.SubscriptionItems[nSubItemIndex].TimeUnits = SubscriptionPeriodUnits.Years;
                            break;
                    }
                    break;

                default:
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Used to set IPN values related to a customer.
        /// </summary>
        /// <remarks>
        /// This method should not be used directly. Instead use
        /// <see cref="SetIpnTransactionProperties"/>.
        /// </remarks>
        /// <param name="key">The key used to find the property.</param>
        /// <param name="val">The value to set to the property.</param>
        /// <param name="ipn">The <see cref="IpnTransaction"/> to set to.</param>
        /// <returns>TRUE if a property was set, otherwise returns FALSE.</returns>
        private bool SetIpnTransactionProperties_Customer(string key, string val, IpnTransaction ipn)
        {
            // Only want to set these values

            switch (key)
            {
                case "first_name":
                    ipn.CustomerInfo.FirstName = val;
                    break;

                case "last_name":
                    ipn.CustomerInfo.LastName = val;
                    break;

                case "payer_business_name":
                    ipn.CustomerInfo.BusinessName = val;
                    break;

                case "address_name":
                    ipn.CustomerInfo.AddressName = val;
                    break;

                case "address_street":
                    ipn.CustomerInfo.Address1 = val;
                    break;

                case "address_city":
                    ipn.CustomerInfo.City = val;
                    break;

                case "address_state":
                    ipn.CustomerInfo.State = val;
                    break;

                case "address_zip":
                    ipn.CustomerInfo.Zip = val;
                    break;

                case "address_country":
                    ipn.CustomerInfo.Country = val;
                    break;

                case "address_status":
                    ipn.SetProperty("address_status", val);
                    break;

                case "payer_email":
                    ipn.CustomerInfo.Email = val;
                    break;

                case "payer_id":
                    ipn.CustomerInfo.PayerID = val;
                    break;

                case "payer_status":
                    ipn.SetProperty("payer_status", val);
                    break;

                default:
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Used to set IPN values related to a product.
        /// </summary>
        /// <remarks>
        /// This method should not be used directly. Instead use
        /// <see cref="SetIpnTransactionProperties"/>.
        /// </remarks>
        /// <param name="key">The key used to find the property.</param>
        /// <param name="val">The value to set to the property.</param>
        /// <param name="ipn">The <see cref="IpnTransaction"/> to set to.</param>
        /// <returns>TRUE if a property was set, otherwise returns FALSE.</returns>
        private bool SetIpnTransactionProperties_Product(string key, string val, IpnTransaction ipn)
        {
            if (key.StartsWith("item_name"))
            {
                int itemIndex;

                try
                {
                    itemIndex = Int32.Parse(key.Substring(9)); // 9 is itemIndex after "item_name"
                }
                catch (Exception)
                {
                    // If no itemIndex, then just single item
                    itemIndex = 1;
                }

                if (ipn.Items.Count < itemIndex)
                {
                    for (int i = ipn.Items.Count; i < itemIndex; i++)
                    {
                        ipn.Items.Add(new CartItem());
                    }
                }

                ShoppingItem item = ipn.Items[itemIndex - 1];
                item.ItemName = val;
            }
            else if (key.StartsWith("item_number"))
            {
                int itemIndex;

                try
                {
                    itemIndex = Int32.Parse(key.Substring(11)); // 11 is itemIndex after "item_number"
                }
                catch (Exception)
                {
                    // If no itemIndex, then just single item
                    itemIndex = 1;
                }

                if (ipn.Items.Count < itemIndex)
                {
                    for (int i = ipn.Items.Count; i < itemIndex; i++)
                    {
                        ipn.Items.Add(new CartItem());
                    }
                }

                ShoppingItem item = ipn.Items[itemIndex - 1];
                item.ItemNumber = val;
            }
            else if (key.StartsWith("quantity"))
            {
                int itemIndex;

                try
                {
                    itemIndex = Int32.Parse(key.Substring(8)); // 8 is itemIndex after "quantity"
                }
                catch (Exception)
                {
                    // If no itemIndex, then just single item
                    itemIndex = 1;
                }

                if (ipn.Items.Count < itemIndex)
                {
                    for (int i = ipn.Items.Count; i < itemIndex; i++)
                    {
                        ipn.Items.Add(new CartItem());
                    }
                }

                CartItem item = (CartItem)ipn.Items[itemIndex - 1];
                item.Quantity = Int32.Parse(val);
            }
            else
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Used to set IPN values with date and time information.
        /// </summary>
        /// <remarks>
        /// This method should not be used directly. Instead use
        /// <see cref="SetIpnTransactionProperties"/>.
        /// </remarks>
        /// <param name="key">The key used to find the property.</param>
        /// <param name="val">The value to set to the property.</param>
        /// <param name="ipn">The <see cref="IpnTransaction"/> to set to.</param>
        /// <returns>TRUE if a property was set, otherwise returns FALSE.</returns>
        private bool SetIpnTransactionProperties_Date(string key, string val, IpnTransaction ipn)
        {
            if (key == "payment_date")
            {
                ipn.PaymentDate = ConvertIpnDate(val);
            }
            else if (key == "auction_closing_date")
            {
                ipn.PaymentDate = ConvertIpnDate(val);
            }
            else
            {
                return false;
            }

            return true;
        }

        #endregion

        #endregion

        private void LogIPN(string text)
        {
            try
            {
                Global.Logger.LogError("LogIPN", text);
            }
            catch
            {
            }

            /*
		    FileLogger fileLogger = Logger.CreateFileLogger(
				Config.Directories.Home + "/Logs/ipns.log","\n\"{ts}{z}\",{msg}\n");

			fileLogger.Open();
			fileLogger.Log(text);
			fileLogger.Close();
             * */
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
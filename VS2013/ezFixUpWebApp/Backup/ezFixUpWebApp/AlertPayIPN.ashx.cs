using System;
using System.Web;
using ezFixUp.Classes;
using Settings = ezFixUp.Properties.Settings;

namespace ezFixUp
{
    // ReSharper disable InconsistentNaming
    public class AlertPayIPN : IHttpHandler
    {
        // Security code variable
        private string ap_SecurityCode;

        // Customer info variables
        private string ap_CustFirstName, ap_CustLastName, ap_CustAddress, ap_CustCity;
        private string ap_CustCountry, ap_CustZip, ap_CustEmailAddress;

        // Common transaction variables
        private string ap_ReferenceNumber, ap_Status, ap_PurchaseType, ap_Merchant;
        private string ap_ItemName, ap_ItemCode, ap_Description, ap_Quantity, ap_Amount, ap_AdditionalCharges;
        private string ap_ShippingCharges, ap_TaxAmount, ap_DiscountAmount, ap_TotalAmount, ap_Currency;
        private string ap_Test;

        // Custom fields
        private string ap_Apc_1, ap_Apc_2, ap_Apc_3, ap_Apc_4, ap_Apc_5, ap_Apc_6;

        // Subscription variables
        private string ap_SubscriptionReferenceNumber, ap_TimeUnit, ap_PeriodLength, ap_PeriodCount, ap_NextRunDate;
        private string ap_TrialTimeUnit, ap_TrialPeriodLength, ap_TrialAmount;

        public void ProcessRequest(HttpContext context)
        {
            setSecurityCodeVariable(context);

            if (ap_SecurityCode != Settings.Default.AlertPayCode)
            {
                // The Data is NOT sent by AlertPay.
                // Take appropriate action 
                Global.Logger.LogWarning("Unauthorized AlertPay attempt with code " + ap_SecurityCode);
            }
            else
            {
                if (ap_Test == "1")
                {
                    // Your site is currently being integrated with AlertPay IPN for TESTING PURPOSES
                    // ONLY. Don't store any information in your Production database and don't process
                    // this transaction as a real order.
                    Global.Logger.LogWarning("Test mode transaction from AlertPay has been received and ignored");
                }
                else
                {
                    // Initialize variables
                    setCustomerInfoVariables(context);
                    setCommonTransactionVariables(context);

                    // Initialize the custom field variables.
                    setCustomFields(context);

                    // If the transaction is subscription-based (recurring payment), initialize the
                    // Subscription variables too.
                    if (ap_PurchaseType.ToLower() == "subscription")
                    {
                        setSubscriptionVariables(context);
                    }

                    if (ap_ReferenceNumber.Length == 0 && ap_TrialAmount != "0")
                    {
                        // Invalid reference number. The reference number is invalid because the ap_ReferenceNumber doesn't
                        // contain a value and the ap_TrialAmount is not equal to 0.
                    }
                    else
                    {
                        if (ap_Status.ToLower() == "success")
                        {
                            // Transaction is complete. It means that the amount was paid successfully.
                            // Process the order here.

                            // Process non-subscription order.
                            if (ap_PurchaseType.ToLower() != "subscription")
                            {
                                // NOTE: The subscription variables are not applicable here. Don't use them.

                                    int packageID;
                                    try
                                    {
                                        packageID = Convert.ToInt32(ap_ItemCode);
                                    }
                                    catch (Exception)
                                    {
                                        Global.Logger.LogError(
                                            "Invalid credits package ID provided by AlertPay postback");
                                        throw;
                                    }

                                    CreditsPackage package = CreditsPackage.Fetch(packageID);

                                    if (package == null)
                                    {
                                        throw new Exception("There is no package with id " + packageID);
                                    }

                                    User user;
                                    try
                                    {
                                        user = Classes.User.Load(ap_Apc_1);
                                    }
                                    catch (NotFoundException)
                                    {
                                        throw new Exception(String.Format("There is no user with username {0}",
                                                                          ap_Apc_1));
                                    }

                                    user.Credits += package.Quantity;
                                    user.Update(true);

                                    #region Apply affiliate commission

                                    string description = String.Format("Credits fee ({0}, {1})", package.Price,
                                                                       DateTime.Now);
                                    int paymentHistoryID = Payments.SavePaymentHistory(user.Username, "AlertPay",
                                                                                       package.Price, description,
                                                                                       ap_ReferenceNumber,
                                                                                       1);

                                    AffiliateCommission.ApplyCommission(user.Username, paymentHistoryID, package.Price,
                                                                        ap_ReferenceNumber);

                                    #endregion
                            }
                            // Process the subscription order. Use ap_SubscriptionReferenceNumber to uniquely identify
                            // this particular subscription transaction.
                            else
                            {
                                // Check whether the trial is free or not
                                if (ap_TrialAmount == "0")
                                {
                                    // Process the free trial here.
                                    // NOTE: The ap_ReferenceNumber is always empty for trial periods and therefore you
                                    // should not use it.
                                }
                                else
                                {
                                    // The is not a free trial and ap_TrialAmount contains some amount and the
                                    // ap_ReferenceNumber contains a valid transaction reference number.

                                    var subscriptionID = Convert.ToInt32(ap_Apc_1);
                                    if (ap_Status.ToLower() == "success") // First payment
                                    {
                                        // Activate subscription
                                        Subscription subscription = Subscription.Fetch(subscriptionID);
                                        BillingPlan plan = BillingPlan.FetchBySubscriptionID(subscriptionID);
                                        subscription.Activate(DateTime.Now, plan);

                                        string description = String.Format("Subscription fee ({0}, {1})",
                                                                           plan.Amount, DateTime.Now);
                                        int paymentHistoryID = Payments.SavePaymentHistory(subscription.Username,
                                            "AlertPay", (decimal)plan.Amount, description, ap_SubscriptionReferenceNumber, 1);

                                        AffiliateCommission.ApplyCommission(subscription.Username, paymentHistoryID,
                                            (decimal)plan.Amount, ap_SubscriptionReferenceNumber);
                                    }
                                    else if (ap_Status.ToLower() == "subscription-payment-success")
                                    {
                                        Subscription subscription = Subscription.Fetch(subscriptionID);
                                        BillingPlan plan = BillingPlan.FetchBySubscriptionID(subscriptionID);

                                        if (!subscription.Confirmed || subscription.RenewDate >= DateTime.Now.AddDays(1))
                                            return;

                                        subscription.Renew(plan);

                                        string description = String.Format("Subscription fee ({0}, {1})",
                                            plan.Amount, DateTime.Now);
                                        int paymentHistoryID = Payments.SavePaymentHistory(subscription.Username,
                                            "AlertPay", (decimal)plan.Amount, description, ap_SubscriptionReferenceNumber, 1);

                                        AffiliateCommission.ApplyCommission(subscription.Username, paymentHistoryID,
                                            (decimal)plan.Amount, ap_SubscriptionReferenceNumber);
                                    }
                                    else if (ap_Status.ToLower() == "subscription-expired")
                                    {
                                        Subscription.Cancel(subscriptionID);
                                        Classes.User.SetAsPaidUser(subscriptionID, false);
                                    }
                                    else if (ap_Status.ToLower() == "subscription-canceled")
                                    {
                                        Subscription.RequestCancellation(subscriptionID);
                                    } 
                                }
                            }
                        }
                        else
                        {
                            // Transaction cancelled means seller explicitely cancelled the subscription or AlertPay 					// cancelled or it was cancelled since buyer didnt have enough money after resheduling after 					// two times.
                            // Take Action appropriately
                        }
                    }
                }
            }
        }

        private void setSecurityCodeVariable(HttpContext context)
        {
            ap_SecurityCode = context.Request.Form["ap_securitycode"];
        }

        private void setCustomerInfoVariables(HttpContext context)
        {
            ap_CustFirstName = context.Request.Form["ap_custfirstname"];
            ap_CustLastName = context.Request.Form["ap_custlastname"];
            ap_CustAddress = context.Request.Form["ap_custaddress"];
            ap_CustCity = context.Request.Form["ap_custcity"];
            ap_CustCountry = context.Request.Form["ap_custcountry"];
            ap_CustZip = context.Request.Form["ap_custzip"];
            ap_CustEmailAddress = context.Request.Form["ap_custemailaddress"];
            ap_PurchaseType = context.Request.Form["ap_purchasetype"];
            ap_Merchant = context.Request.Form["ap_merchant"];
        }

        private void setCommonTransactionVariables(HttpContext context)
        {
            ap_ItemName = context.Request.Form["ap_itemname"];
            ap_Description = context.Request.Form["ap_description"];
            ap_Quantity = context.Request.Form["ap_quantity"];
            ap_Amount = context.Request.Form["ap_amount"];
            ap_AdditionalCharges = context.Request.Form["ap_additionalcharges"];
            ap_ShippingCharges = context.Request.Form["ap_shippingcharges"];
            ap_TaxAmount = context.Request.Form["ap_taxamount"];
            ap_DiscountAmount = context.Request.Form["ap_discountamount"];
            ap_TotalAmount = context.Request.Form["ap_totalamount"];
            ap_Currency = context.Request.Form["ap_currency"];
            ap_ReferenceNumber = context.Request.Form["ap_referencenumber"];
            ap_Status = context.Request.Form["ap_status"];
            ap_ItemCode = context.Request.Form["ap_itemcode"];
            ap_Test = context.Request.Form["ap_test"];
        }

        private void setSubscriptionVariables(HttpContext context)
        {
            ap_SubscriptionReferenceNumber = context.Request.Form["ap_subscriptionreferencenumber"];
            ap_TimeUnit = context.Request.Form["ap_timeunit"];
            ap_PeriodLength = context.Request.Form["ap_periodlength"];
            ap_PeriodCount = context.Request.Form["ap_periodcount"];
            ap_NextRunDate = context.Request.Form["ap_nextrundate"];
            ap_TrialTimeUnit = context.Request.Form["ap_trialtimeunit"];
            ap_TrialPeriodLength = context.Request.Form["ap_trialperiodlength"];
            ap_TrialAmount = context.Request.Form["ap_trialamount"];
        }

        private void setCustomFields(HttpContext context)
        {
            ap_Apc_1 = context.Request.Form["apc_1"];
            ap_Apc_2 = context.Request.Form["apc_2"];
            ap_Apc_3 = context.Request.Form["apc_3"];
            ap_Apc_4 = context.Request.Form["apc_4"];
            ap_Apc_5 = context.Request.Form["apc_5"];
            ap_Apc_6 = context.Request.Form["apc_6"];
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

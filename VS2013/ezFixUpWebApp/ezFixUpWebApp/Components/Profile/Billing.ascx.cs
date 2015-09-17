﻿using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ezFixUp.Classes;

namespace ezFixUp.Components.Profile
{
    public partial class Billing : UserControl
    {
        public string CssClass { set { LargeBoxStart.CssClass = value; } }

        private User user;

        public User User
        {
            set
            {
                user = value;
                ViewState["Username"] = user != null ? user.Username : null;
            }
            get
            {
                if (user == null && ViewState["Username"] != null)
                    user = User.Load((string)ViewState["Username"]);
                return user;
            }
        }

        private Subscription activeSubscription;

        public Subscription ActiveSubscription
        {
            get { return activeSubscription ?? (activeSubscription = Subscription.FetchActiveSubscription(User.Username)); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                string pakageId = Request.Params["pkg_id"];
                string planId = Request.Params["plan_id"];
                if ((radioSubscription.Checked && planId.IsNotNullOrEmpty())
                    || (radioCredits.Checked && pakageId.IsNotNullOrEmpty()))
                    RedirectToPayPal();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // Do not move to PageLoad!
            PrepareInterface();
        }

        private void PrepareInterface()
        {
            cvPaymentTypeInfo.Text = String.Format("<div class=\"hint\">{0}</div>", "Payment type info".Translate());
            cvCheckPayment.Text = "Check payment details".Translate();
            LargeBoxStart.Title = Lang.Trans("Billing");
            hlPaymentType.Title = "Select payment type".Translate();
            radioSubscription.Text = "Subscription based membership".Translate();
            radioCredits.Text = "Credits based membership".Translate();
            hlSubscriptionTypes.Title = "Select subscription plan".Translate();
            hlCreditPackages.Title = "Select credits package".Translate();
            hlPaymentMethods.Title = "Select payment method".Translate();
            hlCouponPayment.Title = "Proceed with coupon code".Translate();
            hlOutsidePayment.Title = "Proceed with payment".Translate();
            hlCreditCardPayment.Title = "Enter credit card details".Translate();
            hlCheckPayment.Title = "Payment instructions".Translate();

            #region Determine whether subscriptions and/or credits are supported

            var billingplans = BillingPlan.Fetch();
            var options = billingplans.Select(p => p.Options).ToList();
            options.Add(Config.Users.GetNonPayingMembersOptions());
            var supportsSubscriptions = billingplans.Length > 0;

            var creditsPackages = CreditsPackage.Fetch();
            //var packNames = creditsPackages.Select(p => p.Name).ToList();
            var supportsCredits = creditsPackages.Length > 0;

            foreach (var option in options)
            {
                var optionsAllowCredits = from o in typeof(BillingPlanOptions).GetProperties()
                                          where o.PropertyType.IsGenericType &&
                                                o.PropertyType.GetGenericTypeDefinition() ==
                                                typeof(BillingPlanOption<>)
                                          select o.GetValue(option, null).GetType()
                                              .GetField("EnableCreditsPayment").GetValue(o.GetValue(option, null));
                if (optionsAllowCredits.Any(o => (bool)o))
                {
                    supportsCredits = true;
                    break;
                }
            }

            if (!supportsSubscriptions && !supportsCredits)
            {
                ((PageBase)Page).StatusPageMessage =
                    "There are no billing plans or credit packages configured!".Translate();
                Response.Redirect("ShowStatus.aspx");
            }

            if (!supportsCredits)
            {
                radioSubscription.Checked = true;
                radioPaymentType_CheckedChanged(null, null);
                divPaymentType.Visible = false;
            }
            else if (!supportsSubscriptions)
            {
                radioCredits.Checked = true;
                radioPaymentType_CheckedChanged(null, null);
                divPaymentType.Visible = false;
            }

            #endregion

            #region Check redirect parameters and prepare available payment types

            if (Global.GetSessionState()["BillingPlanOption"] is BillingPlanOption<bool>)
            {
                var option = (BillingPlanOption<bool>)Global.GetSessionState()["BillingPlanOption"];
                Global.GetSessionState()["BillingPlanOption"] = null;

                string upgradeMessage;
                if (option.UpgradableToNextPlan && !option.EnableCreditsPayment)
                {
                    upgradeMessage = "You have to upgrade your plan in order to use this feature".Translate();
                    if (supportsCredits) // Credits are supported but the feature do not require them
                    {
                        radioSubscription.Checked = true;
                        radioPaymentType_CheckedChanged(null, null);
                        divPaymentType.Visible = false;
                    }
                }
                else if (option.EnableCreditsPayment && !option.UpgradableToNextPlan)
                {
                    upgradeMessage = "You need to purchase more credits in order to use this feature".Translate();
                    if (supportsSubscriptions) // Subscriptions are supported but the feature do not require them
                    {
                        radioCredits.Checked = true;
                        radioPaymentType_CheckedChanged(null, null);
                        divPaymentType.Visible = false;
                    }
                }
                else if (option.UpgradableToNextPlan && option.EnableCreditsPayment)
                {
                    upgradeMessage =
                        "You need to purchase more credits or upgrade your plan in order to use this feature".Translate();
                }
                else
                {
                    // Should not be here
                    return;
                }

                hlPaymentMessage.Title = upgradeMessage;
                divPaymentMessage.Visible = true;
            }

            #endregion

            #region Load active subscription and adjust available options

            if (ActiveSubscription != null && supportsSubscriptions && !radioCredits.Checked)
            {
                radioSubscription.Checked = true;
                radioPaymentType_CheckedChanged(null, null);
            }

            #endregion
        }

        protected void radioPaymentType_CheckedChanged(object sender, EventArgs e)
        {
            if (radioSubscription.Checked)
            {
                divSubscriptionTypes.Visible = true;
                divCreditPackages.Visible = false;

                BillingPlan[] plans = BillingPlan.Fetch();
                BillingPlan.ApplyDiscounts(User, plans);

                DataTable dt1 = new DataTable();
                dt1.Columns.Add("Name");
                dt1.Columns.Add("Cycles", typeof(int));
                dt1.Columns.Add("Amount");
                dt1.Columns.Add("Bonus");
                dt1.Columns.Add("PlanId", typeof(int));

                rlPlans.Items.Clear();


                foreach (BillingPlan plan in plans)
                {
                    var cycleUnitString = Lang.Trans(plan.CycleUnit.ToString().ToLower());
                    if (plan.Cycle == 1) cycleUnitString = cycleUnitString.TrimEnd('s');
                    var item = new ListItem(String.Format("{0} - {1} per {2} {3}".Translate(),
                                                          plan.Title, plan.Amount.ToString("c"), plan.Cycle,
                                                          cycleUnitString),
                                            plan.ID.ToString());
                    rlPlans.Items.Add(item);
                    dt1.Rows.Add(plan.Title, plan.Cycle,
                                String.Format("{0:0.0}", plan.Amount),
                                plan.Options.MaxEcardsPerDay.Credits.ToString(), //use as bonus plan credits
                                plan.ID);
                }
                dlSubscriptionPlans.DataSource = dt1;
                dlSubscriptionPlans.DataBind();

                rlPlans.SelectedIndex = 0;

                if (ActiveSubscription != null)
                {
                    ListItem currentPlanListItem = rlPlans.Items.FindByValue(ActiveSubscription.PlanID.ToString());
                    string planName;

                    if (currentPlanListItem != null)
                    {
                        planName = currentPlanListItem.Text;
                        rlPlans.Items.Remove(currentPlanListItem);
                    }
                    else
                        planName = Lang.Trans("no longer available.");

                    rlPlans.Items.Insert(0, new ListItem("Keep your current plan", "-1", true));
                    rlPlans.SelectedIndex = 0;
                    lblCurrentPlan.Text = Lang.Trans("Your current plan is ") + planName;

                    // If subscription will not rebill
                    if (activeSubscription.CancellationRequested)
                    {
                        lblCurrentPlan.Text += "<BR>" + Lang.Trans("Your subscription expires on: ") +
                                              activeSubscription.RenewDate.ToShortDateString();
                    }
                    else
                    {
                        if (ActiveSubscription.PaymentProcessor != "Check")
                        {
                            var cancelItem =
                                new ListItem(Lang.Trans("Cancel your site subscription"),
                                             Convert.ToString(Int32.MaxValue));
                            rlPlans.Items.Add(cancelItem);
                            rlPlans.SelectedIndex = 0;
                        }
                        lblCurrentPlan.Text += "<BR>" + Lang.Trans("Subscription renewal date: ") +
                                              activeSubscription.RenewDate.ToShortDateString();
                    }
                }
            }
            else if (radioCredits.Checked)
            {
                divCreditPackages.Visible = true;
                divSubscriptionTypes.Visible = false;

                CreditsPackage[] creditsPackages = CreditsPackage.Fetch();

                DataTable dt2 = new DataTable();
                dt2.Columns.Add("Name");
                dt2.Columns.Add("Credits", typeof(int));
                dt2.Columns.Add("Amount");
                dt2.Columns.Add("PlanId", typeof(int));

                rlCreditPackages.Items.Clear();
                foreach (CreditsPackage package in creditsPackages)
                {
                    rlCreditPackages.Items.Add(
                        new ListItem(String.Format("{0} - {1} credits for {2}".Translate(),
                                                   package.Name, package.Quantity, package.Price.ToString("C")),
                                     package.ID.ToString()));
                    dt2.Rows.Add(package.Name, package.Quantity, String.Format("{0:0.0}", package.Price), package.ID);
                }
                dlCreditPackages.DataSource = dt2;
                dlCreditPackages.DataBind();
                rlCreditPackages.SelectedIndex = 0;
            }
            else
            {
                return;
            }

            //divPaymentMethods.Visible = true;
            //radiolistPaymentMethods.Items.Clear();
            //foreach (string paymentProcessor in Config.AdminSettings.Payments.PaymentProcessors)
            //{
            //    if (radioSubscription.Checked && ActiveSubscription != null &&
            //        ActiveSubscription.PaymentProcessor != paymentProcessor) continue;
            //    radiolistPaymentMethods.Items.Add(
            //        new ListItem(String.Format("<img src=\"images/{0}.jpg\" alt=\"{0}\" title=\"{0}\">",
            //                                   paymentProcessor), paymentProcessor));
            //}
            //if (radiolistPaymentMethods.Items.Count == 1)
            //{
            //    radiolistPaymentMethods.SelectedIndex = 0;
            //    radiolistPaymentMethods_SelectedIndexChanged(null, null);
            //}
        }

        protected void radiolistPaymentMethods_SelectedIndexChanged(object sender, EventArgs e)
        {
            divOutsidePayment.Visible = false;
            divCreditCardPayment.Visible = false;
            divCheckPayment.Visible = false;

            switch (radiolistPaymentMethods.SelectedValue)
            {
                case "Coupon":
                    divCouponPayment.Visible = true;
                    btnUseCoupon.Text = "Complete payment with Coupon »".Translate();
                    break;
                case "CCBill":
                    divOutsidePayment.Visible = true;
                    btnOutsidePayment.Text = "Complete payment with CCBill »".Translate();
                    break;
                case "PayPal":
                    divOutsidePayment.Visible = true;
                    btnOutsidePayment.Text = "Complete payment with PayPal »".Translate();
                    break;
                case "AlertPay":
                    divOutsidePayment.Visible = true;
                    btnOutsidePayment.Text = "Complete payment with AlertPay »".Translate();
                    break;
                case "DaoPay":
                    divOutsidePayment.Visible = true;
                    btnOutsidePayment.Text = "Complete payment with DaoPay »".Translate();
                    break;
                case "PayflowPro":
                    divCreditCardPayment.Visible = true;
                    btnCreditCardPayment.Text = "Complete payment with Payflow Pro »".Translate();
                    LoadBillingDetails();
                    break;
                case "Authorize.NET":
                    divCreditCardPayment.Visible = true;
                    btnCreditCardPayment.Text = "Complete payment with Authorize.NET »".Translate();
                    LoadBillingDetails();
                    break;
                case "Check":
                    divCheckPayment.Visible = true;
                    break;
            }
        }

        private void LoadBillingDetails()
        {
            if (User.BillingDetails == null) return;
            txtFirstName.Text = User.BillingDetails.FirstName;
            txtLastName.Text = User.BillingDetails.LastName;
            txtAddress.Text = User.BillingDetails.Address;
            txtCity.Text = User.BillingDetails.City;
            txtState.Text = User.BillingDetails.State;
            txtZip.Text = User.BillingDetails.Zip;
            txtCountry.Text = User.BillingDetails.Country;
            txtPhone.Text = User.BillingDetails.Phone;
            if (user.BillingDetails.CardNumber != null && user.BillingDetails.CardNumber.Length > 4)
            {
                txtCardNumber.Text = "XXXXXXXXXXXX" + User.BillingDetails.CardNumber.Substring(
                                                          user.BillingDetails.CardNumber.Length - 5);
            }
            ddMonth.SelectedIndex = User.BillingDetails.CardExpirationMonth;
            ddYear.SelectedItem.Text = User.BillingDetails.CardExpirationYear.ToString();
        }

        protected void btnOutsidePayment_Click(object sender, EventArgs e)
        {
            switch (radiolistPaymentMethods.SelectedValue)
            {
                case "CCBill":
                    RedirectToCCBill();
                    break;
                case "PayPal":
                    RedirectToPayPal();
                    break;
                case "AlertPay":
                    RedirectToAlertPay();
                    break;
                case "DaoPay":
                    RedirectToDaoPay();
                    break;
            }
        }

        private void RedirectToCCBill()
        {
            string formHtml = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">" +
                    "<html><head><title>Payment</title></head>" +
                    "<body onload=\"PaymentForm.submit();\">" +
                    "<form action='https://bill.ccbill.com/jpost/signup.cgi' method=POST name=\"PaymentForm\">" +
                    "<input type=hidden name=clientAccnum value='{0}'>" +
                    "<input type=hidden name=clientSubacc value='{1}'>" +
                    "<input type=hidden name=formName value='{2}'>" +
                    "<input type=hidden name=language value='English' >" +
                    "<input type=hidden name=allowedTypes value='{3}' >" +
                    "<input type=hidden name=subscriptionTypeId value='{4}' >" +
                    "<input type=hidden name=user value='{5}' >" +
                    "<input type=hidden name=isCredits value='{6}' >" +
                    "</form>" +
                    "</body>" +
                    "</html>";

            if (radioSubscription.Checked)
            {
                formHtml = String.Format(formHtml,
                    ConfigurationManager.AppSettings["CCBillAccountNumber"],
                    ConfigurationManager.AppSettings["CCBillSubaccount"],
                    ConfigurationManager.AppSettings["CCBillFormname"],
                    ConfigurationManager.AppSettings["CCBillAllowedTypes"],
                    ConfigurationManager.AppSettings["CCBillSubscriptionType"],
                    User.Username, "0");
            }
            else if (radioCredits.Checked)
            {
                formHtml = String.Format(formHtml,
                    ConfigurationManager.AppSettings["CCBillAccountNumber"],
                    ConfigurationManager.AppSettings["CCBillSubaccount"],
                    ConfigurationManager.AppSettings["CCBillFormname"],
                    ConfigurationManager.AppSettings["CCBillAllowedTypesCredits"],
                    ConfigurationManager.AppSettings["CCBillSubscriptionTypeCredits"],
                    User.Username, "1");
            }

            Response.Clear();
            Response.Write(formHtml);
            Response.End();
        }

        private void RedirectToPayPal()
        {
            var template = String.Empty;
            var itemIdStr = String.Empty;
            var username = User.Username;

            if (radioSubscription.Checked)
            {
                //itemIdStr = rlPlans.SelectedItem.Value;
                itemIdStr = Request.Params["plan_id"];
                BillingPlan plan = BillingPlan.Fetch(Convert.ToInt32(itemIdStr));

                //if cancellation selected
                if (plan.ID == Int32.MaxValue)
                {
                    Response.Redirect(String.Format(Config.AdminSettings.Payments.PayPalSandbox
                                                        ? "https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_subscr-find&alias={0}"
                                                        : "https://www.paypal.com/cgi-bin/webscr?cmd=_subscr-find&alias={0}",
                                                    Config.AdminSettings.Payments.PayPalEmail));
                }

                template = Config.AdminSettings.Payments.PayPalSandbox
                               ? (ActiveSubscription == null ? "buynowSandBox" : "modifySubscriptionSandBox")
                               : (ActiveSubscription == null ? "buynow" : "modifySubscription");
            }
            else if (radioCredits.Checked)
            {
                template = Config.AdminSettings.Payments.PayPalSandbox
                                ? "buycreditsSandBox"
                                : "buycredits";
                //itemIdStr = rlCreditPackages.SelectedItem.Value;
                itemIdStr = Request.Params["pkg_id"];
            }
            var url = String.Format("~/PayPal/FormTemplateSender.aspx?template={0}&un={1}&id={2}", template, username, itemIdStr);
            HttpContext.Current.Response.Redirect(url);
            //HttpContext.Current.Server.Transfer(url);
        }
        private void RedirectToPayPalObsolete()
        {
            var html = String.Empty;
            if (radioSubscription.Checked)
            {
                BillingPlan plan = BillingPlan.Fetch(Convert.ToInt32(rlPlans.SelectedItem.Value));

                //if cancellation selected
                if (plan.ID == Int32.MaxValue)
                {
                    Response.Redirect(String.Format(Config.AdminSettings.Payments.PayPalSandbox
                                                        ? "https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_subscr-find&alias={0}"
                                                        : "https://www.paypal.com/cgi-bin/webscr?cmd=_subscr-find&alias={0}",
                                                    Config.AdminSettings.Payments.PayPalEmail));
                }

                int subscriptionID = Subscription.Create(User.Username, plan.ID, "PayPal");

                var buyNow = Path.Combine(Server.MapPath("."),
                                      Config.AdminSettings.Payments.PayPalSandbox
                                      ? (ActiveSubscription == null ? "buynowSandBox.txt" : "modifySubscriptionSandBox.txt")
                                      : (ActiveSubscription == null ? "buynow.txt" : "modifySubscription.txt"));

                using (var reader = new StreamReader(@buyNow))
                {
                    string buyNowTemplate = reader.ReadToEnd();
                    html = String.Format(buyNowTemplate,
                                         Config.AdminSettings.Payments.PayPalEmail,
                                         plan.Title, plan.ID,
                                         plan.Amount.ToString("0.00", CultureInfo.InvariantCulture),
                                         plan.Cycle, plan.CycleUnit.ToString(),
                                         subscriptionID, Config.Urls.ThankYou, Config.Urls.Cancel,
                                         Config.Urls.PayPalIPN);
                }
            }
            else if (radioCredits.Checked)
            {
                CreditsPackage package = CreditsPackage.Fetch(Convert.ToInt32(rlCreditPackages.SelectedItem.Value));
                var buyNow = Path.Combine(Server.MapPath("."),
                                      Config.AdminSettings.Payments.PayPalSandbox
                                          ? "buycreditsSandBox.txt"
                                          : "buycredits.txt");
                using (var reader = new StreamReader(@buyNow))
                {
                    html = String.Format(reader.ReadToEnd(), Config.AdminSettings.Payments.PayPalEmail,
                                      package.Name, package.ID,
                                      package.Price.ToString("0.00", CultureInfo.InvariantCulture),
                                      User.Username, Config.Urls.ThankYou, Config.Urls.Cancel);
                }
            }

            HttpResponse resp = HttpContext.Current.Response;
            resp.Clear();
            resp.Write(html);
            resp.Flush();
            resp.End();
        }

        private void RedirectToAlertPay()
        {
            var html = String.Empty;

            if (radioSubscription.Checked)
            {
                int planID = Convert.ToInt32(rlPlans.SelectedItem.Value);

                //if cancellation selected
                if (planID == Int32.MaxValue)
                {
                    ((PageBase)Page).StatusPageMessage =
                        "Please log in to your AlertPay account to cancel your subscription".Translate();
                    Response.Redirect("ShowStatus.aspx");
                }
                if (ActiveSubscription != null)
                {
                    ((PageBase)Page).StatusPageMessage =
                        "Please log in to your AlertPay account to modify your subscription".Translate();
                    Response.Redirect("ShowStatus.aspx");
                }

                BillingPlan plan = BillingPlan.Fetch(planID);
                int subscriptionID = Subscription.Create(User.Username, plan.ID, "AlertPay");
                string buyNow = Path.Combine(Server.MapPath("."), "subscribealertpay.txt");

                using (var reader = new StreamReader(@buyNow))
                {
                    html = String.Format(reader.ReadToEnd(), Config.AdminSettings.Payments.AlertPayEmail,
                                         plan.Title, Config.Urls.ThankYou, plan.ID, plan.Title,
                                         plan.Amount.ToString("0.00", CultureInfo.InvariantCulture),
                                         subscriptionID, plan.CycleUnit.ToString().TrimEnd('s'),
                                         plan.Cycle);
                }
            }
            else if (radioCredits.Checked)
            {
                int planID = Convert.ToInt32(rlCreditPackages.SelectedItem.Value);
                CreditsPackage package = CreditsPackage.Fetch(planID);
                string buyNow = Path.Combine(Server.MapPath("."), "buyalertpay.txt");

                using (var reader = new StreamReader(@buyNow))
                {
                    html = String.Format(reader.ReadToEnd(), Config.AdminSettings.Payments.AlertPayEmail,
                                         package.Name, Config.Urls.ThankYou, package.ID, package.Name,
                                         package.Price.ToString("0.00", CultureInfo.InvariantCulture),
                                         Config.Urls.Cancel, User.Username);
                }
            }

            HttpResponse resp = Context.Response;
            resp.Clear();
            resp.Write(html);
            resp.Flush();
            resp.End();
        }

        private void RedirectToDaoPay()
        {
            var redirectURL = String.Empty;
            if (radioSubscription.Checked)
            {
                int planID = Convert.ToInt32(rlPlans.SelectedItem.Value);
                BillingPlan plan = BillingPlan.Fetch(planID);
                redirectURL =
                    String.Format("https://daopay.com/payment/?appcode={0}&price={1}&username={2}&bpid={3}",
                                  Properties.Settings.Default.DaoPayApplicationCode,
                                  plan.Amount.ToString(CultureInfo.InvariantCulture),
                                  User.Username, plan.ID);
            }
            else if (radioCredits.Checked)
            {
                int planID = Convert.ToInt32(rlCreditPackages.SelectedItem.Value);
                CreditsPackage package = CreditsPackage.Fetch(planID);
                redirectURL =
                    String.Format("https://daopay.com/payment/?appcode={0}&price={1}&username={2}&cpid={3}",
                                  Properties.Settings.Default.DaoPayApplicationCode,
                                  package.Price.ToString(CultureInfo.InvariantCulture),
                                  User.Username, package.ID);
            }

            Response.Redirect(redirectURL);
        }

        protected void btnCreditCardPayment_Click(object sender, EventArgs e)
        {
            switch (radiolistPaymentMethods.SelectedValue)
            {
                case "Authorize.NET":
                    ProcessPaymentThroughPaymentGateway(new AuthorizeNet());
                    break;
                case "PayflowPro":
                    ProcessPaymentThroughPaymentGateway(new PayflowPro());
                    break;
            }
        }

        private void ProcessPaymentThroughPaymentGateway(IPaymentGateway gateway)
        {
            // Update billing details
            if (User.BillingDetails == null)
                User.BillingDetails = new BillingDetails();
            User.BillingDetails.FirstName = txtFirstName.Text.Trim();
            User.BillingDetails.LastName = txtLastName.Text.Trim();
            User.BillingDetails.Address = txtAddress.Text.Trim();
            User.BillingDetails.City = txtCity.Text.Trim();
            User.BillingDetails.State = txtState.Text.Trim();
            User.BillingDetails.Zip = txtZip.Text.Trim();
            User.BillingDetails.Country = txtCountry.Text.Trim();
            User.BillingDetails.Phone = txtPhone.Text.Trim();
            if (txtCardNumber.Text.IndexOf("XXXX") < 0)
                User.BillingDetails.CardNumber = txtCardNumber.Text.Trim();
            try
            {
                User.BillingDetails.CardExpirationMonth = Convert.ToInt32(ddMonth.SelectedItem.Text);
                User.BillingDetails.CardExpirationYear = Convert.ToInt32(ddYear.SelectedItem.Text);
            }
            catch (FormatException)
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "errorAlert",
                                                            String.Format("<script>var alert_string = {0}';</script>",
                                                                          "Invalid expiration date!"));
                return;
            }

            User.Update();

            if (radioCredits.Checked)
            {
                int planID = Convert.ToInt32(rlCreditPackages.SelectedItem.Value);
                CreditsPackage package = CreditsPackage.Fetch(planID);
                if (package == null)
                {
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "errorAlert",
                                                                String.Format("<script>var alert_string = '{0}';</script>",
                                                                              "Selected credits package is not in the database"));
                    return;
                }

                TransactionDetails transactionDetails = TransactionDetails.FromBillingDetails(
                    User.BillingDetails);
                transactionDetails.Amount = Convert.ToDecimal(package.Price);
                eGatewayResponse gatewayResponse = gateway.SubmitTransaction(User.Username, transactionDetails,
                                                                             "Purchased credits (" +
                                                                             package.Price.ToString("c") + ", " +
                                                                             DateTime.Now.ToShortDateString() + ")");
                switch (gatewayResponse)
                {
                    case eGatewayResponse.Approved:
                        User.Credits += package.Quantity;
                        User.Update(true);
                        Response.Redirect("ThankYou.aspx");
                        return;
                    case eGatewayResponse.Declined:
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "errorAlert",
                                                                    String.Format("<script>var alert_string = '{0}';</script>",
                                                                                  Lang.Trans(
                                                                                      "Your credit card has been declined!")));
                        return;
                    case eGatewayResponse.Error:
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "errorAlert",
                                                                    String.Format("<script>var alert_string = '{0}';</script>",
                                                                                  Lang.Trans(
                                                                                      "There has been an error while processing your transaction!")));
                        return;
                }
            }
            else
            {
                int planID = Convert.ToInt32(rlPlans.SelectedItem.Value);

                //if cancellation selected
                if (planID == Int32.MaxValue && activeSubscription != null)
                {
                    Subscription.RequestCancellation(activeSubscription.ID);
                    return;
                }

                if (ActiveSubscription != null)
                {
                    ActiveSubscription.PlanID = planID;
                    ActiveSubscription.PaymentProcessor = radiolistPaymentMethods.SelectedValue;
                    ActiveSubscription.Update();

                    ((PageBase)Page).CurrentUserSession.BillingPlanOptions = null;
                    ((PageBase)Page).StatusPageMessage = Lang.Trans("Your subscription has been changed successfully!");
                    Response.Redirect("ShowStatus.aspx");
                }

                //gets selected plan data
                BillingPlan plan = BillingPlan.Fetch(planID);
                if (plan == null)
                {
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "errorAlert",
                                                                String.Format("<script>var alert_string = '{0}';</script>",
                                                                              "Selected billing plan is not in the database"));
                    return;
                }

                int subscriptionID = Subscription.Create(User.Username, plan.ID, gateway.Name);
                Subscription newSubscription = Subscription.Fetch(subscriptionID);


                TransactionDetails transactionDetails = TransactionDetails.FromBillingDetails(
                    User.BillingDetails);
                transactionDetails.Amount = Convert.ToDecimal(plan.Amount);
                eGatewayResponse gatewayResponse = gateway.SubmitTransaction(User.Username, transactionDetails,
                                                                             "First subscription fee (" +
                                                                             plan.Amount.ToString("c") + ", " +
                                                                             DateTime.Now.ToShortDateString() + ")");
                switch (gatewayResponse)
                {
                    case eGatewayResponse.Approved:
                        newSubscription.Activate(DateTime.Now, plan);
                        var billingPlanOptions = ((PageBase)Page).CurrentUserSession.BillingPlanOptions;
                        if (billingPlanOptions.MaxEcardsPerDay.AllowCredits)
                        {
                            ((PageBase)Page).CurrentUserSession.Credits += billingPlanOptions.MaxEcardsPerDay.Credits;
                            ((PageBase)Page).CurrentUserSession.Update();
                        }
                        Response.Redirect("ThankYou.aspx");
                        return;
                    case eGatewayResponse.Declined:
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "errorAlert",
                                                                    String.Format("<script>var alert_string = '{0}';</script>",
                                                                                  Lang.Trans(
                                                                                      "Your credit card has been declined!")));
                        return;
                    case eGatewayResponse.Error:
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "errorAlert",
                                                                    String.Format("<script>var alert_string = '{0}';</script>",
                                                                                  Lang.Trans(
                                                                                      "There has been an error while processing your transaction!")));
                        return;
                }
            }
        }

        private bool CouponEnsured(string text)
        {
            if (text.IsNullOrEmpty())
                return false;

            var coupons = Coupon.FetchAll();
            DateTime now = DateTime.Now;
            var codes = coupons.Where(c => c.StartDate <= now
                                           && now <= c.DueDate
                                           && c.Used < c.MaxUse
                                           && !c.OnlyForNewUser)
                               .Select(c => c.Code).ToList();

            bool isCouponValid = (codes != null && codes.Count() > 0 && codes.Contains(text));
            return (isCouponValid && !Coupon.IsCouponUsedByUsername(text, ((PageBase)Page).CurrentUserSession.Username));
        }

        private void UseCoupon(string text)
        {
            // process coupon
            var coupon = Coupon.Fetch(text);
            var username = PageBase.GetCurrentUserSession().Username;

            switch ((CouponType.eCouponType)coupon.CouponTypeId)
            {
                case CouponType.eCouponType.PremiumMonth1:
                case CouponType.eCouponType.PremiumMonth2:
                case CouponType.eCouponType.PremiumMonth3:

                    var allBillingPlans = BillingPlan.Fetch();
                    BillingPlan plan = allBillingPlans.Where(p => p.Cycle == 1).FirstOrDefault();
                    if (ActiveSubscription == null)
                    {
                        var subId = Subscription.Create(username, plan.ID, "coupon = " + text);
                        ActiveSubscription.Activate(DateTime.Now);
                    }
                    else
                    {
                        ActiveSubscription.Renew(plan);
                    }

                    if (coupon.CouponTypeId > (int)CouponType.eCouponType.PremiumMonth1)
                        ActiveSubscription.Renew(plan);

                    if (coupon.CouponTypeId > (int)CouponType.eCouponType.PremiumMonth2)
                        ActiveSubscription.Renew(plan);

                    break;

                case CouponType.eCouponType.Discount10pct:
                    break;
                case CouponType.eCouponType.Discount15pct:
                    break;
                case CouponType.eCouponType.Discount20pct:
                    break;
                case CouponType.eCouponType.Discount25pct:
                    break;
                case CouponType.eCouponType.Discount30pct:
                    break;
                case CouponType.eCouponType.Gift1:
                    break;
                case CouponType.eCouponType.Gift2:
                    break;
                case CouponType.eCouponType.Gift3:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Coupon.SaveUsernameUsesCoupon(((PageBase)Page).CurrentUserSession.Username, text);
            Coupon.UpdateUsedCounterByAddCount(text, 1);
        }

        protected void btnUseCoupon_Click(object sender, EventArgs e)
        {
            var inputCode = txtCoupon.Text.Trim();
            if (CouponEnsured(inputCode))
            {
                UseCoupon(inputCode);
                lblCouponMsg.Text = "Coupon processed successfuly".Translate();
                lblCouponMsg.BackColor = Color.Lime;
            }
            else
            {
                lblCouponMsg.Text = "Coupon code is not valid".Translate();
                lblCouponMsg.BackColor = Color.Red;
            }
        }
    }
}

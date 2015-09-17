/* ezFixUp 
 * Copyright (C) 2003-2010 eStream 
 * lovehitch.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ezFixUp license agreement.  
 * It can be found at lovehitch.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ezFixUp.Data
{
    /// <summary>
    /// Summary description for Subscription.
    /// </summary>
    public class Subscription
    {
        public Subscription()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region Properties

        private int id;

        public int ID
        {
            get { return id; }
        }

        private string username;

        public string Username
        {
            get { return username; }
        }

        private int planID;

        public int PlanID
        {
            get { return planID; }
            set { planID = value; }
        }

        private DateTime orderDate;

        public DateTime OrderDate
        {
            get { return orderDate; }
            set { orderDate = value; }
        }

        private bool confirmed;

        public bool Confirmed
        {
            get { return confirmed; }

            set { confirmed = value; }
        }

        private bool cancelled;

        public bool Cancelled
        {
            get { return cancelled; }
        }

        private bool cancellationRequested;

        public bool CancellationRequested
        {
            get { return cancellationRequested; }
        }

        private DateTime renewDate;

        public DateTime RenewDate
        {
            get { return renewDate; }

            set { renewDate = value; }
        }

        private string paymentProcessor;

        public string PaymentProcessor
        {
            get { return paymentProcessor; }
            set { paymentProcessor = value; }
        }

        private string custom;

        public string Custom
        {
            get { return custom; }
            set { custom = value; }
        }
        #endregion

        public static int Create(string username, int planID, string paymentProcessor)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object returnValue = SqlHelper.GetDB().ExecuteScalar(
                                                             "CreateSubscription", username, planID, paymentProcessor);
                return Convert.ToInt32(returnValue);
            }
        }

        public static void Cancel(int id)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery(
                                          "CancelSubscription", id);
            }
        }

        public static Subscription Fetch(int id)
        {
            return Fetch(id, null);
        }

        public static Subscription Fetch(string customField)
        {
            return Fetch(null, customField);
        }

        private static Subscription Fetch(int? id, string customField)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = (SqlDataReader) 
                    SqlHelper.GetDB().ExecuteReader(
                                            "FetchSubscription", id, customField);

                if (reader.Read())
                {
                    Subscription subscription = new Subscription();

                    subscription.id = (int) reader["ID"];
                    subscription.username = (string) reader["Username"];
                    subscription.planID = (int) reader["PlanID"];
                    subscription.orderDate = (DateTime) reader["OrderDate"];
                    subscription.renewDate = (DateTime) reader["RenewDate"];
                    subscription.confirmed = (bool) reader["Confirmed"];
                    subscription.cancelled = (bool) reader["Cancelled"];
                    subscription.cancellationRequested = (bool) reader["CancellationRequested"];
                    subscription.paymentProcessor = (string)reader["PaymentProcessor"];
                    subscription.custom = reader["Custom"] as string;

                    return subscription;
                }
                else return null;
            }
        }

        public static Subscription FetchActiveSubscription(string username)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = (SqlDataReader) 
                    SqlHelper.GetDB().ExecuteReader(
                                            "FetchActiveSubscription", username);

                if (reader.Read())
                {
                    Subscription subscription = new Subscription();

                    subscription.id = (int) reader["ID"];
                    subscription.username = (string) reader["Username"];
                    subscription.planID = (int) reader["PlanID"];
                    subscription.orderDate = (DateTime) reader["OrderDate"];
                    subscription.renewDate = (DateTime) reader["RenewDate"];
                    subscription.confirmed = (bool) reader["Confirmed"];
                    subscription.cancelled = false;
                    subscription.cancellationRequested = (bool) reader["CancellationRequested"];
                    subscription.paymentProcessor = (string)reader["PaymentProcessor"];
                    subscription.custom = reader["Custom"] as string;

                    return subscription;
                }
                return null;
            }
        }

        public static List<Subscription> FetchActiveElapsedSubscriptions(DateTime fromDate, DateTime toDate)
        {
            var subscriptions = new List<Subscription>();
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = (SqlDataReader)
                    SqlHelper.GetDB().ExecuteReader(
                                            "FetchActiveElapsedSubscriptions", 
                                            fromDate, toDate);

                while (reader.Read())
                {
                    Subscription subscription = new Subscription();

                    subscription.id = (int) reader["ID"];
                    subscription.username = (string) reader["Username"];
                    subscription.planID = (int) reader["PlanID"];
                    subscription.orderDate = (DateTime) reader["OrderDate"];
                    subscription.renewDate = (DateTime) reader["RenewDate"];
                    subscription.confirmed = (bool) reader["Confirmed"];
                    subscription.cancelled = false;
                    subscription.cancellationRequested = (bool) reader["CancellationRequested"];
                    subscription.paymentProcessor = (string)reader["PaymentProcessor"];
                    subscription.custom = reader["Custom"] as string;

                    subscriptions.Add(subscription);
                }
                return subscriptions;
            }
        }

        public static void RequestCancellation(int id)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "RequestSubscriptionCancellation",
                                          id);
            }
        }

        public void Update()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "UpdateSubscription",
                                          id, planID, orderDate,
                                          renewDate, confirmed, paymentProcessor, custom);
            }
        }

        public void Activate(DateTime subscriptionDate)
        {
            BillingPlan plan = BillingPlan.Fetch(planID);
            Activate(subscriptionDate, plan);
        }

        public void Activate(DateTime subscriptionDate, BillingPlan billingPlan)
        {
            OrderDate = subscriptionDate;
            RenewDate = OrderDate;

            Confirmed = true;

            Renew(billingPlan);

            //change the user's status to ACTIVE
            User.SetAsPaidUser(id, true);
        }

        public void Renew(BillingPlan billingPlan)
        {
            switch (billingPlan.CycleUnit)
            {
                case CycleUnits.Days:
                    RenewDate = RenewDate.AddDays(billingPlan.Cycle);
                    break;
                case CycleUnits.Weeks:
                    RenewDate = RenewDate.AddDays(billingPlan.Cycle*7);
                    break;
                case CycleUnits.Months:
                    RenewDate = RenewDate.AddMonths(billingPlan.Cycle);
                    break;
                case CycleUnits.Years:
                    RenewDate = RenewDate.AddYears(billingPlan.Cycle);
                    break;
            }

            Update();
        }
    }
}
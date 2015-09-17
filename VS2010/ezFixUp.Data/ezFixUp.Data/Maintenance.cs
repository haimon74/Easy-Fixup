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
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Timers;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Timer=System.Timers.Timer;

namespace ezFixUp.Classes
{
    /// <summary>
    /// The class that performs cleanup and maintenance of the database
    /// </summary>
    public static class Maintenance
    {
        private static Timer timerMaintenance;
        private static bool purgeNotActivatedLock = false;
        private static bool performMaintenanceLock = false;

        public static void InitializeTimers()
        {
            timerMaintenance = new Timer();
            timerMaintenance.AutoReset = true;
            timerMaintenance.Interval = TimeSpan.FromHours(1).TotalMilliseconds;
            timerMaintenance.Elapsed += new ElapsedEventHandler(timerMaintenance_Elapsed);
            timerMaintenance.Start();
        }

        private static void timerMaintenance_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now.Subtract(DBSettings.Get("Maintenance_NotActivatedUsersLastProcessed", 
                DateTime.Now)) >= TimeSpan.FromHours(6))
            {
                ThreadPool.QueueUserWorkItem(AsyncProcessNotActivatedUsers);
                DBSettings.Set("Maintenance_NotActivatedUsersLastProcessed", DateTime.Now);
            }

            if (DateTime.Now.Subtract(DBSettings.Get("Maintenance_LastMaintenancePerformed", 
                DateTime.Now)) >= TimeSpan.FromDays(1))
            {
                ThreadPool.QueueUserWorkItem(AsyncPerformMaintenance);
                DBSettings.Set("Maintenance_LastMaintenancePerformed", DateTime.Now);
            }
        }

        private static void AsyncProcessNotActivatedUsers(object data)
        {
            if (purgeNotActivatedLock)
                return;

            try
            {
                purgeNotActivatedLock = true;

                Global.Logger.LogStatus("Maintenance", "Process not activated users starting " + DateTime.Now.ToShortTimeString());
                foreach (User inactiveUser in User.GetInactiveUsers(Config.Maintenance.NotActivatedUsersPurgePeriod))
                {
                    User.Purge(inactiveUser.Username);
                }
                Global.Logger.LogStatus("Maintenance", "Process not activated users ending " + DateTime.Now.ToShortTimeString());
            }
            catch (Exception err)
            {
                Global.Logger.LogError("AsyncProcessNotActivatedUsers", err);
            }
            finally
            {
                purgeNotActivatedLock = false;
            }
        }

        private static void AsyncPerformMaintenance(object data)
        {
            if (performMaintenanceLock)
                return;

            try
            {
                performMaintenanceLock = true;

                Global.Logger.LogStatus("Maintenance", "Perform maintenance starting " + DateTime.Now.ToShortTimeString());
                using (SqlConnection conn = Config.DB.Open())
                {
                    SqlHelper.GetDB().ExecuteNonQuery(  "PerformMaintenance", (int?) 3600);
                }
                Global.Logger.LogStatus("Maintenance", "Perform maintenance ending " + DateTime.Now.ToShortTimeString());
            }
            catch (Exception err)
            {
                Global.Logger.LogError("AsyncPerformMaintenance", err);
            }
            finally
            {
                performMaintenanceLock = false;
            }
        }
    }
}
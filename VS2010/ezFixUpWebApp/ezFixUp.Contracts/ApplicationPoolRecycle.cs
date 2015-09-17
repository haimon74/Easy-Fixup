using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;

namespace ezFixUp.Contracts
{
    public static class ApplicationPoolRecycle
    {
        /// <summary>Attempts to recycle current application pool</summary>
        /// <returns>Boolean indicating if application pool was successfully recycled</returns>
        public static bool RecycleCurrentApplicationPool()
        {
            try
            {
                // Application hosted on IIS that supports App Pools, like 6.0 and 7.0
                if (IsApplicationRunningOnAppPool())
                {
                    // Get current application pool name
                    string appPoolId = GetCurrentApplicationPoolId();

                    // Recycle current application pool
                    RecycleApplicationPool(appPoolId);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private static bool IsApplicationRunningOnAppPool()
        {
            // Application is not hosted on IIS
            if (!AppDomain.CurrentDomain.FriendlyName.StartsWith("/LM/"))
                return false;

            // Application hosted on IIS that doesn't support App Pools, like 5.1
            if (!DirectoryEntry.Exists("IIS://Localhost/W3SVC/AppPools"))
                return false;

            return true;
        }

        private static string GetCurrentApplicationPoolId()
        {
            string virtualDirPath = AppDomain.CurrentDomain.FriendlyName;
            virtualDirPath = virtualDirPath.Substring(4);
            int index = virtualDirPath.Length + 1;
            index = virtualDirPath.LastIndexOf("-", index - 1, index - 1);
            index = virtualDirPath.LastIndexOf("-", index - 1, index - 1);
            virtualDirPath = "IIS://localhost/" + virtualDirPath.Remove(index);

            var virtualDirEntry = new DirectoryEntry(virtualDirPath);
            return virtualDirEntry.Properties["AppPoolId"].Value.ToString();
        }

        private static void RecycleApplicationPool(string appPoolId)
        {
            string appPoolPath = "IIS://localhost/W3SVC/AppPools/" + appPoolId;

            var appPoolEntry = new DirectoryEntry(appPoolPath);
            appPoolEntry.Invoke("Recycle");
        }
    }

}

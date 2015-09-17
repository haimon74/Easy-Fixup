using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace AspNetDating.Classes
{
    public class ProfileSkin
    {
        public static string[] AvailableSkins
        {
            get
            {
                if (HttpContext.Current.Cache != null 
                    && HttpContext.Current.Cache["AvailableSkins"] is string[])
                {
                    return (string[]) HttpContext.Current.Cache["AvailableSkins"];
                }

                string skinsPath = System.Web.Hosting.HostingEnvironment.MapPath("~/skins");
                if (skinsPath == null) return null;
                var skins = new List<string>();
                foreach (var skinFolder in Directory.GetDirectories(skinsPath))
                {
                    skins.Add(String.Format("Skins/{0}/style.css", 
                        skinFolder.Substring(skinFolder.LastIndexOf('\\') + 1)));
                }
                string[] skinsArray = skins.ToArray();
                if (HttpContext.Current.Cache != null)
                    HttpContext.Current.Cache.Add("AvailableSkins", skinsArray, null,
                                                  DateTime.Now.AddMinutes(10), Cache.NoSlidingExpiration,
                                                  CacheItemPriority.Default, null);
                return skinsArray;
            }
        }
    }
}

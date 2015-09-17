using System.Collections.Generic;
using System.Web.Caching;

namespace AspNetDating.Classes
{
    public class ManualCacheDependency : CacheDependency
    {
        public void NotifyDependencyChanged()
        {
            NotifyDependencyChanged(this, null);
        }
    }

    public class CacheDependencyCollection
    {
        private readonly List<CacheDependency> dependencies;

        public CacheDependencyCollection()
        {
            dependencies = new List<CacheDependency>();
        }

        public CacheDependency Get()
        {
            //if (!dependencies.ContainsKey(accountID))
            //    dependencies.Add(accountID, new List<CacheDependency>());
            var dependency = new ManualCacheDependency();
            lock (dependencies)
            {
                dependencies.Add(dependency);
            }
            return dependency;
        }

        //public void NotifyChanged()
        //{
        //    NotifyChanged(Account.CurrentAccount.ID);
        //}

        public void NotifyChanged()
        {
            //if (!dependencies.ContainsKey(accountID)) return;
            lock (dependencies)
            {
                foreach (var cacheDependency in dependencies)
                {
                    ((ManualCacheDependency)cacheDependency).NotifyDependencyChanged();
                }
                dependencies.Clear();
            }
        }
    }
}

using System;
using System.Data;
using System.Web.Caching;
using AspNetDating.Classes;

namespace AspNetDating.Components.Groups
{
    public partial class NewGroups : System.Web.UI.UserControl
    {
        public delegate void NewGroupsBoundEventHandler(object sender, GroupsBoundEventArgs e);
        public class GroupsBoundEventArgs
        {
            public GroupsBoundEventArgs(int count)
            {
                this.count = count;
            }
            
            private int count;

            public int Count
            {
                get { return count; }
                set { count = value; }
            }
        }
        public event NewGroupsBoundEventHandler NewGroupsBoundEvent;

        protected virtual void OnNewGroupsBound(GroupsBoundEventArgs e)
        {
            if (NewGroupsBoundEvent != null)
            {
                NewGroupsBoundEvent(this, e);
            }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (Config.Groups.EnableGroups)
            {
                #region Load New Groups (thumbnails)

                DataTable dtNewGroups;

                string cacheKey = "NewGroups.ascx";
                if (Cache[cacheKey] != null && Cache[cacheKey] is DataTable)
                {
                    dtNewGroups = (DataTable)Cache[cacheKey];
                }
                else
                {
                    Group[] groups =
                        Group.Fetch(Config.Groups.NumberOfNewGroups, true, Group.eSortColumn.DateCreated);
                    dtNewGroups = new DataTable();
                    dtNewGroups.Columns.Add("GroupID");
                    dtNewGroups.Columns.Add("Name");
                    dtNewGroups.Columns.Add("AccessLevel");

                    foreach (Group group in groups)
                    {
                        string accessLevel = null;
                        
                        switch(group.AccessLevel){
                            case Group.eAccessLevel.Public:
                                accessLevel = Lang.Trans("Public Group");
                                break;
                            case Group.eAccessLevel.Moderated:
                                accessLevel = Lang.Trans("Moderated Group");
                                break;
                            case Group.eAccessLevel.Private:
                                accessLevel = Lang.Trans("Private Group");
                                break;
                        }
                        dtNewGroups.Rows.Add(
                            new object[]
                                {
                                    group.ID, group.Name, accessLevel
                                });
                    }

                    Cache.Add(cacheKey, dtNewGroups, null, DateTime.Now.Add(TimeSpan.FromMinutes(10)),
                              Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
                }

                dlNewGroups.DataSource = dtNewGroups;
                dlNewGroups.DataBind();

                OnNewGroupsBound(new GroupsBoundEventArgs(dtNewGroups.Rows.Count));

                #endregion
            }
        }
    }
}
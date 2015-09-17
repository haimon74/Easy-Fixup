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
using System.Data;
using AspNetDating.Classes;

namespace AspNetDating.Components.WebParts
{
    [Editable]
    public partial class NewTopicsWebPart : WebPartUserControl
    {
        protected UserSession CurrentUserSession
        {
            get { return ((PageBase) Page).CurrentUserSession; }
        }

        private bool? ControlLoaded
        {
            get
            {
                return ViewState["ControlLoaded"] as bool?;
            }

            set
            {
                ViewState["ControlLoaded"] = value;
            }
        }
        

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = Lang.Translate("New Topics");
            if (!IsPostBack)
            {

            }
        }

        private void loadStrings()
        {
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!ControlLoaded.HasValue)
            {
                loadStrings();
                loadNewTopics();

                ControlLoaded = true;
            }
        }

        private void loadNewTopics()
        {
            if (Config.Groups.EnableGroups)
            {
                GroupTopic[] newTopics = GroupTopic.FetchNewTopics(CurrentUserSession.Username);
                if (newTopics.Length > 0)
                {
                    mvNewTopics.SetActiveView(vNewTopics);
                    DataTable dtNewTopicsCount = new DataTable("NewTopics");

                    dtNewTopicsCount.Columns.Add("GroupTopicID");
                    dtNewTopicsCount.Columns.Add("GroupTopicName");
                    dtNewTopicsCount.Columns.Add("GroupTopicDateCreated");
                    dtNewTopicsCount.Columns.Add("GroupID");
                    dtNewTopicsCount.Columns.Add("GroupName");
                    dtNewTopicsCount.Columns.Add("Username");
                    dtNewTopicsCount.Columns.Add("ImageID", typeof (int));

                    string groupName = String.Empty;
                    foreach (GroupTopic newTopic in newTopics)
                    {
                        Group group = Group.Fetch(newTopic.GroupID);

                        if (group != null)
                        {
                            groupName = group.Name;
                        }
                        int imageID = 0;
                        try
                        {
                            imageID = Photo.GetPrimary(newTopic.Username).Id;
                        }
                        catch (NotFoundException)
                        {
                            try
                            {
                                User user = User.Load(newTopic.Username);
                                imageID = ImageHandler.GetPhotoIdByGender(user.Gender);
                            }
                            catch (NotFoundException)
                            {
                            }
                        }

                        dtNewTopicsCount.Rows.Add(new object[]
                                                      {
                                                          newTopic.ID,
                                                          newTopic.Name,
                                                          newTopic.DateCreated.ToShortDateString(),
                                                          newTopic.GroupID,
                                                          groupName,
                                                          newTopic.Username,
                                                          imageID
                                                      });
                    }

                    rptNewGroupTopics.DataSource = dtNewTopicsCount;
                    rptNewGroupTopics.DataBind();
                }
                else
                    mvNewTopics.SetActiveView(vNoNewTopics);

                rptNewGroupTopics.Visible = newTopics.Length > 0;
            }
        }
    }
}
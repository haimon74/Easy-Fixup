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
using System.Web.Caching;
using System.Web.UI;
using AspNetDating.Classes;

namespace AspNetDating.Components
{
    [Themeable(true)]
    public partial class NewVideos : UserControl
    {
        private User.eGender gender;
        private bool genderSearch = false;

        public User.eGender Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                genderSearch = true;
            }
        }

        public int RepeatColumns
        {
            get
            {
                return dlNewVideos.RepeatColumns;
            }
            set
            {
                dlNewVideos.RepeatColumns = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            
            #region Load New Videos (thumbnails)
            DataTable dtNewVideos;

            string cacheKey = String.Format("NewVideos.ascx_{0}", genderSearch ? gender.ToString() : "null");
            if (Cache[cacheKey] != null && Cache[cacheKey] is DataTable && Session["theme"] == null)
            {
                dtNewVideos = (DataTable)Cache[cacheKey];
            }
            else
            {
                VideoUploadsSearch search = new VideoUploadsSearch();
                search.Approved = true;
                search.IsPrivate = false;
                search.SortColumn = VideoUploadsSearch.eSortColumn.ID;
                search.VideosCount = dlNewVideos.RepeatColumns * Config.Photos.MaxRowsVideosOnMainPage;
                if (genderSearch)
                    search.Gender = Gender;
                VideoUploadSearchResults nvResults = search.GetResults();
                dtNewVideos = new DataTable();
                dtNewVideos.Columns.Add("Username");
                dtNewVideos.Columns.Add("VideoId", typeof (int));
                dtNewVideos.Columns.Add("ThumbnailUrl");
                dtNewVideos.Columns.Add("Age");
                dtNewVideos.Columns.Add("Gender");
                if (nvResults != null && nvResults.Ids != null)
                {
                    foreach (int id in nvResults.Ids)
                    {
                        User user;
                        VideoUpload videoUpload;
                        try
                        {
                            videoUpload = VideoUpload.Load(id);
                            user = User.Load(videoUpload.Username);
                        }
                        catch (NotFoundException)
                        {
                            continue;
                        }

                        string videoThumbnail = String.Format("{0}/UserFiles/{1}/video_{2}.png", Config.Urls.Home,
                                                            user.Username, id);
                        string age = !Config.Users.DisableGenderInformation ? user.Age.ToString() : String.Empty;
                        string g = !Config.Users.DisableGenderInformation ? user.Gender.ToString() : String.Empty;
                        dtNewVideos.Rows.Add(
                            new object[]
                                {
                                    user.Username, id, videoThumbnail, age, g
                                });
                    }

                    Cache.Add(cacheKey, dtNewVideos, null, DateTime.Now.Add(TimeSpan.FromMinutes(10)),
                              Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
                }
            }

            if (dtNewVideos.Rows.Count > 0)
            {
                dlNewVideos.DataSource = dtNewVideos;
                dlNewVideos.DataBind();
            }

            #endregion
        }
    }
}
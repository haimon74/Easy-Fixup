using System;
using ezFixUp.Classes;

namespace ezFixUp.Admin
{
    public partial class EditMetaTags : AdminPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!HasWriteAccess)
                    btnSave.Enabled = false;
                
                LoadStrings();
                PrepareTags();

                txtDefaultTitle.Text = Config.SEO.DefaultTitleTemplate;
                txtDefaultMetaDescription.Text = Config.SEO.DefaultMetaDescriptionTemplate;
                txtDefaultMetaKeywords.Text = Config.SEO.DefaultMetaKeywordsTemplate;
                txtShowUserTitle.Text = Config.SEO.ShowUserTitleTemplate;
                txtShowUserMetaDescription.Text = Config.SEO.ShowUserMetaDescriptionTemplate;
                txtShowUserMetaKeywords.Text = Config.SEO.ShowUserMetaKeywordsTemplate;
                txtShowGroupTitle.Text = Config.SEO.ShowGroupTitleTemplate;
                txtShowGroupMetaDescription.Text = Config.SEO.ShowGroupMetaDescriptionTemplate;
                txtShowGroupMetaKeywords.Text = Config.SEO.ShowGroupMetaKeywordsTemplate;
                txtShowGroupTopicTitle.Text = Config.SEO.ShowGroupTopicTitleTemplate;
                txtShowGroupTopicMetaDescription.Text = Config.SEO.ShowGroupTopicMetaDescriptionTemplate;
                txtShowGroupTopicMetaKeywords.Text = Config.SEO.ShowGroupTopicMetaKeywordsTemplate;
                txtAdsTitle.Text = Config.SEO.AdsTitleTemplate;
                txtAdsMetaDescription.Text = Config.SEO.AdsMetaDescriptionTemplate;
                txtAdsMetaKeywords.Text = Config.SEO.AdsMetaKeywordsTemplate;
                txtChangeLostPasswordTitle.Text = Config.SEO.ChangeLostPasswordTitleTemplate;
                txtChangeLostPasswordMetaDescription.Text = Config.SEO.ChangeLostPasswordMetaDescriptionTemplate;
                txtChangeLostPasswordMetaKeywords.Text = Config.SEO.ChangeLostPasswordMetaKeywordsTemplate;
                txtDefaultPageTitle.Text = Config.SEO.DefaultPageTitleTemplate;
                txtDefaultPageMetaDescription.Text = Config.SEO.DefaultPageMetaDescriptionTemplate;
                txtDefaultPageMetaKeywords.Text = Config.SEO.DefaultPageMetaKeywordsTemplate;
                txtGroupsTitle.Text = Config.SEO.GroupsTitleTemplate;
                txtGroupsMetaDescription.Text = Config.SEO.GroupsMetaDescriptionTemplate;
                txtGroupsMetaKeywords.Text = Config.SEO.GroupsMetaKeywordsTemplate;
                txtLoginTitle.Text = Config.SEO.LoginTitleTemplate;
                txtLoginMetaDescription.Text = Config.SEO.LoginMetaDescriptionTemplate;
                txtLoginMetaKeywords.Text = Config.SEO.LoginMetaKeywordsTemplate;
                txtLostPasswordTitle.Text = Config.SEO.LostPasswordTitleTemplate;
                txtLostPasswordMetaDescription.Text = Config.SEO.LostPasswordMetaDescriptionTemplate;
                txtLostPasswordMetaKeywords.Text = Config.SEO.LostPasswordMetaKeywordsTemplate;
                txtNewsTitle.Text = Config.SEO.NewsTitleTemplate;
                txtNewsMetaDescription.Text = Config.SEO.NewsMetaDescriptionTemplate;
                txtNewsMetaKeywords.Text = Config.SEO.NewsMetaKeywordsTemplate;
                txtRegister.Text = Config.SEO.RegisterTitleTemplate;
                txtRegisterMetaDescription.Text = Config.SEO.RegisterMetaDescriptionTemplate;
                txtRegisterMetaKeywords.Text = Config.SEO.RegisterMetaKeywordsTemplate;
                txtSearchTitle.Text = Config.SEO.SearchTitleTemplate;
                txtSearchMetaDescription.Text = Config.SEO.SearchMetaDescriptionTemplate;
                txtSearchMetaKeywords.Text = Config.SEO.SearchMetaKeywordsTemplate;
                txtSendProfileTitle.Text = Config.SEO.SendProfileTitleTemplate;
                txtSendProfileMetaDescription.Text = Config.SEO.SendProfileMetaDescriptionTemplate;
                txtSendProfileMetaKeywords.Text = Config.SEO.SendProfileMetaKeywordsTemplate;
                txtShowAdTitle.Text = Config.SEO.ShowAdTitleTemplate;
                txtShowAdMetaDescription.Text = Config.SEO.ShowAdMetaDescriptionTemplate;
                txtShowAdMetaKeywords.Text = Config.SEO.ShowAdMetaKeywordsTemplate;
                txtShowGroupEventsTitle.Text = Config.SEO.ShowGroupEventsTitleTemplate;
                txtShowGroupEventsMetaDescription.Text = Config.SEO.ShowGroupEventsMetaDescriptionTemplate;
                txtShowGroupEventsMetaKeywords.Text = Config.SEO.ShowGroupEventsMetaKeywordsTemplate;
                txtShowGroupPhotosTitle.Text = Config.SEO.ShowGroupPhotosTitleTemplate;
                txtShowGroupPhotosMetaDescription.Text = Config.SEO.ShowGroupPhotosMetaDescriptionTemplate;
                txtShowGroupPhotosMetaKeywords.Text = Config.SEO.ShowGroupPhotosMetaKeywordsTemplate;
                txtSmsConfirmTitle.Text = Config.SEO.SmsConfirmTitleTemplate;
                txtSmsConfirmMetaDescription.Text = Config.SEO.SmsConfirmMetaDescriptionTemplate;
                txtSmsConfirmMetaKeywords.Text = Config.SEO.SmsConfirmMetaKeywordsTemplate;
                txtTopChartsTitle.Text = Config.SEO.TopChartsTitleTemplate;
                txtTopChartsMetaDescription.Text = Config.SEO.TopChartsMetaDescriptionTemplate;
                txtTopChartsMetaKeywords.Text = Config.SEO.TopChartsMetaKeywordsTemplate;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.editMetaTags;            
            base.OnInit(e);
        }
        
        private void LoadStrings()
        {
            btnSave.Text = Lang.TransA("Save");
        }

        private void PrepareTags()
        {
            string tags = "";
            foreach (ProfileTopic topic in ProfileTopic.Fetch())
            {
                ProfileQuestion[] questions = ProfileQuestion.FetchByTopicID(topic.ID);
                
                if (questions == null) continue;

                foreach (ProfileQuestion question in questions)
                {
                    tags += String.Format("<li><b>%%Q_{0}%%</b> - Profile -> {1}</li>", question.Id, question.Name);
                }
            }
            ltrTags.Text = tags;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            Config.SEO.DefaultTitleTemplate = txtDefaultTitle.Text.Trim();
            Config.SEO.DefaultMetaDescriptionTemplate = txtDefaultMetaDescription.Text.Trim();
            Config.SEO.DefaultMetaKeywordsTemplate = txtDefaultMetaKeywords.Text.Trim();
            Config.SEO.ShowUserTitleTemplate = txtShowUserTitle.Text.Trim();
            Config.SEO.ShowUserMetaDescriptionTemplate = txtShowUserMetaDescription.Text.Trim();
            Config.SEO.ShowUserMetaKeywordsTemplate = txtShowUserMetaKeywords.Text.Trim();
            Config.SEO.ShowGroupTitleTemplate = txtShowGroupTitle.Text.Trim();
            Config.SEO.ShowGroupMetaDescriptionTemplate = txtShowGroupMetaDescription.Text.Trim();
            Config.SEO.ShowGroupMetaKeywordsTemplate = txtShowGroupMetaKeywords.Text.Trim();
            Config.SEO.ShowGroupTopicTitleTemplate = txtShowGroupTopicTitle.Text.Trim();
            Config.SEO.ShowGroupTopicMetaDescriptionTemplate = txtShowGroupTopicMetaDescription.Text.Trim();
            Config.SEO.ShowGroupTopicMetaKeywordsTemplate = txtShowGroupTopicMetaKeywords.Text.Trim();
            Config.SEO.AdsTitleTemplate = txtAdsTitle.Text.Trim();
            Config.SEO.AdsMetaDescriptionTemplate = txtAdsMetaDescription.Text.Trim();
            Config.SEO.AdsMetaKeywordsTemplate = txtAdsMetaKeywords.Text.Trim();
            Config.SEO.ChangeLostPasswordTitleTemplate = txtChangeLostPasswordTitle.Text.Trim();
            Config.SEO.ChangeLostPasswordMetaDescriptionTemplate = txtChangeLostPasswordMetaDescription.Text.Trim();
            Config.SEO.ChangeLostPasswordMetaKeywordsTemplate = txtChangeLostPasswordMetaKeywords.Text.Trim();
            Config.SEO.DefaultPageTitleTemplate = txtDefaultPageTitle.Text.Trim();
            Config.SEO.DefaultPageMetaDescriptionTemplate = txtDefaultPageMetaDescription.Text.Trim();
            Config.SEO.DefaultPageMetaKeywordsTemplate = txtDefaultPageMetaKeywords.Text.Trim();
            Config.SEO.GroupsTitleTemplate = txtGroupsTitle.Text.Trim();
            Config.SEO.GroupsMetaDescriptionTemplate = txtGroupsMetaDescription.Text.Trim();
            Config.SEO.GroupsMetaKeywordsTemplate = txtGroupsMetaKeywords.Text.Trim();
            Config.SEO.LoginTitleTemplate = txtLoginTitle.Text.Trim();
            Config.SEO.LoginMetaDescriptionTemplate = txtLoginMetaDescription.Text.Trim();
            Config.SEO.LoginMetaKeywordsTemplate = txtLoginMetaKeywords.Text.Trim();
            Config.SEO.LostPasswordTitleTemplate = txtLostPasswordTitle.Text.Trim();
            Config.SEO.LostPasswordMetaDescriptionTemplate = txtLostPasswordMetaDescription.Text.Trim();
            Config.SEO.LostPasswordMetaKeywordsTemplate = txtLostPasswordMetaKeywords.Text.Trim();
            Config.SEO.NewsTitleTemplate = txtNewsTitle.Text.Trim();
            Config.SEO.NewsMetaDescriptionTemplate = txtNewsMetaDescription.Text.Trim();
            Config.SEO.NewsMetaKeywordsTemplate = txtNewsMetaKeywords.Text.Trim();
            Config.SEO.RegisterTitleTemplate = txtRegister.Text.Trim();
            Config.SEO.RegisterMetaDescriptionTemplate = txtRegisterMetaDescription.Text.Trim();
            Config.SEO.RegisterMetaKeywordsTemplate = txtRegisterMetaKeywords.Text.Trim();
            Config.SEO.SearchTitleTemplate = txtSearchTitle.Text.Trim();
            Config.SEO.SearchMetaDescriptionTemplate = txtSearchMetaDescription.Text.Trim();
            Config.SEO.SearchMetaKeywordsTemplate = txtSearchMetaKeywords.Text.Trim();
            Config.SEO.SendProfileTitleTemplate = txtSendProfileTitle.Text.Trim();
            Config.SEO.SendProfileMetaDescriptionTemplate = txtSendProfileMetaDescription.Text.Trim();
            Config.SEO.SendProfileMetaKeywordsTemplate = txtSendProfileMetaKeywords.Text.Trim();
            Config.SEO.ShowAdTitleTemplate = txtShowAdTitle.Text.Trim();
            Config.SEO.ShowAdMetaDescriptionTemplate = txtShowAdMetaDescription.Text.Trim();
            Config.SEO.ShowAdMetaKeywordsTemplate = txtShowAdMetaKeywords.Text.Trim();
            Config.SEO.ShowGroupEventsTitleTemplate = txtShowGroupEventsTitle.Text.Trim();
            Config.SEO.ShowGroupEventsMetaDescriptionTemplate = txtShowGroupEventsMetaDescription.Text.Trim();
            Config.SEO.ShowGroupEventsMetaKeywordsTemplate = txtShowGroupEventsMetaKeywords.Text.Trim();
            Config.SEO.ShowGroupPhotosTitleTemplate = txtShowGroupPhotosTitle.Text.Trim();
            Config.SEO.ShowGroupPhotosMetaDescriptionTemplate = txtShowGroupPhotosMetaDescription.Text.Trim();
            Config.SEO.ShowGroupPhotosMetaKeywordsTemplate = txtShowGroupPhotosMetaKeywords.Text.Trim();
            Config.SEO.SmsConfirmTitleTemplate = txtSmsConfirmTitle.Text.Trim();
            Config.SEO.SmsConfirmMetaDescriptionTemplate = txtSmsConfirmMetaDescription.Text.Trim();
            Config.SEO.SmsConfirmMetaKeywordsTemplate = txtSmsConfirmMetaKeywords.Text.Trim();
            Config.SEO.TopChartsTitleTemplate = txtTopChartsTitle.Text.Trim();
            Config.SEO.TopChartsMetaDescriptionTemplate = txtTopChartsMetaDescription.Text.Trim();
            Config.SEO.TopChartsMetaKeywordsTemplate = txtTopChartsMetaKeywords.Text.Trim();

            MessageBox1.Show(Lang.TransA("Meta tags templates have been saved successfully!"), Misc.MessageType.Success);
        }
    }
}
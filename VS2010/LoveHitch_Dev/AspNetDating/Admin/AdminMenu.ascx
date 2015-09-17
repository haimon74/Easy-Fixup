<%@ Import namespace="AspNetDating.Classes"%>
<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AdminMenu.ascx.cs" Inherits="AspNetDating.Admin.AdminMenu"
    TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>

<div id="leftnav">
    <div class="navlabel">
        <%= Lang.TransA("User Management") %>
    </div>
    <div class="navblock">
        <ul>
            <li><a href="BrowseUsers.aspx">
                <%= Lang.TransA("Browse Members") %>
            </a></li>
            <li><a href="BrowsePhotos.aspx">
                <%= Lang.TransA("Browse Photos") %>
            </a></li>
            <li><a href="BrowseMessages.aspx">
                <%= Lang.TransA("Browse Messages") %>
            </a></li>
            <li><a href="BrowseSpamSuspects.aspx">
                <%= Lang.TransA("Browse Spam Suspects") %>
            </a></li>
            <li><a href="BrowseVideoUploads.aspx">
                <%= Lang.TransA("Browse Video Uploads") %>
            </a></li>
            <li><a href="ApprovePhotos.aspx">
                <%= Lang.TransA("Approve Photos") %>
            </a></li>
            <li><a href="ApprovePhotos2.aspx">
                <%= Lang.TransA("Approve Photos2") %>
            </a></li>
            <li id="pnlApproveSalutePhotos" runat="server"><a href="ApproveSalutePhotos.aspx">
                <%= Lang.TransA("Approve Salute Photos") %>
            </a></li>            
            <!--
            <li id="pnlApproveVideos" runat="server"><a href="ApproveVideos.aspx">
                <%= Lang.TransA("Approve Videos") %>
            </a></li>
            -->
            <li id="pnlApproveVideoUploads" runat="server"><a href="ApproveVideoUploads.aspx">
                <%= Lang.TransA("Approve Video Uploads") %>
            </a></li>
            <li id="pnlApproveAudioUploads" runat="server"><a href="ApproveAudioUploads.aspx">
                <%= Lang.TransA("Approve Audio Uploads") %>
            </a></li>
            <li><a href="ApproveAnswers.aspx">
                <%= Lang.TransA("Approve Answers") %>
            </a></li>
            <li><a href="ApproveBlogPosts.aspx">
                <%= Lang.TransA("Approve Blog Posts") %>
            </a></li>
            <li><a href="EditScheduledAnnouncements.aspx">
                <%= Lang.TransA("Send Announcement") %>
            </a></li>
            <li id="pnlSpamCheck" runat="server"><a href="SpamCheck.aspx">
                <%= Lang.TransA("Spam Check") %>
            </a></li>
            <li id="pnlAbuseReports" runat="server"><a href="AbuseReports.aspx">
                <%= Lang.TransA("Abuse Reports") %>
            </a></li> 
            <li><a href="ManageUserLevels.aspx">
                <%= Lang.TransA("User Levels") %>
            </a></li>                                    
        </ul>
    </div>
    <div class="navlabel">
        <%= Lang.TransA("Contests") %>
    </div>
    <div class="navblock">
        <ul>     
            <li id="pnlManageContests" runat="server"><a href="ManageContests.aspx">
                <%= Lang.TransA("Manage Contests") %>
            </a></li>
            <li id="pnlContestEntries" runat="server"><a href="ContestEntries.aspx">
                <%= Lang.TransA("Contest Entries") %>
            </a></li>            
        </ul>
    </div> 
    <div class="navlabel">
        <%= Lang.TransA("Classifieds") %>
    </div>
    <div class="navblock">
        <ul>     
            <li><a href="EditAdsCategories.aspx">
                <%= Lang.TransA("Manage Categories")%>
            </a></li>
            <li><a href="ApproveAds.aspx">
                <%= Lang.TransA("Approve Classifieds") %>
            </a></li>            
        </ul>
    </div>            
    <div class="navlabel">
        <%= Lang.TransA("Group Management") %>
    </div>
    <div class="navblock">
        <ul>
            <li><a href="ManageGroupCategories.aspx">
                <%= Lang.TransA("Manage Categories") %>
            </a></li>
            <li><a href="BrowseGroups.aspx">
                <%= Lang.TransA("Browse Groups") %>
            </a></li>
            <li><a href="ApproveGroups.aspx">
                <%= Lang.TransA("Approve Groups") %>
            </a></li>
        </ul>
    </div>
    <div class="navlabel">
        <%= Lang.TransA("Messages Management") %>
    </div>
    <div class="navblock">
        <ul>
            <li><a href="EditMessageQuestions.aspx">
                <%= Lang.TransA("Edit Message Questions")%>                
            </a></li>
            
        </ul>
    </div>
    <div class="navlabel">
        <%= Lang.TransA("Site Management") %>
    </div>
    <div class="navblock">
        <ul>
            <li><a href="EditFAQs.aspx">
                <%= Lang.TransA("Edit FAQs") %>
            </a></li>
            <li><a href="EditLanguages.aspx">
                <%= Lang.TransA("Edit Languages") %>
            </a></li>
            <li><a href="EditTopics.aspx">
                <%= Lang.TransA("Edit Topics") %>
                &amp;
                <%= Lang.TransA("Questions") %>
            </a></li>
            <li><a href="EditNews.aspx">
                <%= Lang.TransA("Edit News") %>
            </a></li>
            <%--<li><a href="EditEcardTypes.aspx">
                <%= Lang.TransA("Manage e-Cards") %>
            </a></li>          --%>
            <li><a href="EditWinks.aspx">
                <%= Lang.TransA("Manage Winks") %>
            </a></li>
            <li><a href="EditGifts.aspx">
                <%= Lang.TransA("Manage Virtual Gifts") %>
            </a></li>
            <li><a href="ManagePolls.aspx">
                <%= Lang.TransA("Manage Polls") %>
            </a></li>            
            <li><a href="ManageBadWords.aspx">
                <%= Lang.TransA("Edit Bad Words") %>
            </a></li>
            <li><a href="EditTemplates.aspx">
                <%= Lang.TransA("Edit Templates") %>
            </a></li>
            <li><a href="EditGoogleAnalytics.aspx">
                <%= Lang.TransA("Edit Google Analytics") %>
            </a></li>
            <li><a href="EditBanners.aspx">
                <%= Lang.TransA("Edit Banners") %>
            </a></li>
            <li><a href="UploadLogo.aspx">
                <%= Lang.TransA("Upload Site Logo") %>
            </a></li>                        
            <li><a href="EditContentPages.aspx">
                <%= Lang.TransA("Edit Content Pages") %>
            </a></li>
            <li><a href="EditContentViews.aspx">
                <%= Lang.TransA("Edit Content Views") %>
            </a></li>  
            <li><a href="EditPopupTips.aspx">
                <%= Lang.TransA("Edit Popup Tips") %>
            </a></li>  
            <li><a href="EditStrings.aspx">
                <%= Lang.TransA("Text Management") %>
            </a></li>
            <li><a href="ThemeManager.aspx">
                <%= Lang.TransA("Themes Manager") %>
            </a></li>
            <li><a href="Settings.aspx">
                <%= Lang.TransA("Settings") %>
            </a></li>
        </ul>
    </div>
    <div class="navlabel">
        <%= Lang.TransA("Admin Management") %>
    </div>
    <div class="navblock">
        <ul>
            <li><a href="BrowseAdmins.aspx">
                <%= Lang.TransA("Browse Admins") %>
            </a></li>
        </ul>
    </div>
    <div class="navlabel">
        <%= Lang.TransA("Payment Management") %>
    </div>
    <div class="navblock">
        <ul>
            <li><a href="BillingSettings.aspx">
                <%= Lang.TransA("Billing Settings") %>
            </a></li>
            <li><a href="CreditsPackages.aspx">
                <%= Lang.TransA("Credits Packages") %>
            </a></li>
        </ul>
    </div>
    <div class="navlabel">
        <%= Lang.TransA("SEO Management") %>
    </div>
    <div class="navblock">
        <ul>
            <li><a href="EditMetaTags.aspx">
                <%= Lang.TransA("Edit Meta Tags") %>
            </a></li>
        </ul>
    </div>
    <div class="navlabel">
        <%= Lang.TransA("Affiliate Management") %>
    </div>
    <div class="navblock">
        <ul>
            <li><a href="EditCoupons.aspx">
                <%= Lang.TransA("Edit Coupons") %>
            </a></li>
            <li><a href="BrowseAffiliates.aspx">
                <%= Lang.TransA("Browse Affiliates") %>
            </a></li>
            <li><a href="AffiliatePayments.aspx">
                <%= Lang.TransA("Payment Requests") %>
            </a></li>
            <li><a href="PaymentHistory.aspx">
                <%= Lang.TransA("Payment History") %>
            </a></li>
            <li><a href="CommissionsHistory.aspx">
                <%= Lang.TransA("Affiliate Commissions") %>
            </a></li>
            <li><a href="AffiliateBanners.aspx">
                <%= Lang.TransA("Affiliate Banners") %>
            </a></li>
        </ul>
    </div>
    <div class="navlabel">
        <%= Lang.TransA("Statistics")%>
    </div>
    <div class="navblock">
        <ul>
            <li><a href="NewUsersStats.aspx">
                <%= Lang.TransA("New Users Stats")%>
            </a></li>
            <li><a href="OnlineUsersStats.aspx">
                <%= Lang.TransA("Online Users Stats")%>
            </a></li>
        </ul>
    </div>
    <div class="navlabel">
        <%= Lang.TransA("Extras") %>
    </div>
    <div class="navblock">
        <ul>
            <li><a href="GenerateGadgets.aspx">
                <%= Lang.TransA("Sideshow Gadgets") %>
            </a></li>
        </ul>
    </div>
    <div class="navlabel">
        <%= Lang.TransA("Dummy Data") %>
    </div>
    <div class="navblock">
        <ul>
            <li><a href="CreateDummyProfiles.aspx">
                <%= Lang.TransA("Create Profiles") %>
            </a></li>
            <li><a href="ImportDummyProfiles.aspx">
                <%= Lang.TransA("Import Profiles") %>
            </a></li>
        </ul>
    </div>
</div>

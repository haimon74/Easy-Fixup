<%@ Page Language="C#" AutoEventWireup="true" Codebehind="EditMetaTags.aspx.cs" Inherits="AspNetDating.Admin.EditMetaTags" %>

<%@ Register Src="MessageBox.ascx" TagName="MessageBox" TagPrefix="uc2" %>

<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head runat="server">
	    <title><%= Lang.TransA("Edit Meta Tags") %></title>
    	<link rel="stylesheet" type="text/css" href="images/style.css" media="all">
	</head>
	<body>
    <form id="form1" runat="server">
        <uc1:AdminHeader ID="AdminHeader1" runat="server"></uc1:AdminHeader>
        <div id="layout">
	        <div id="crupms">
            	<img src="images/i_edittemplates.jpg">
            	<div id="crumpstxt"><%= Lang.TransA("Edit Meta Tags") %></div>
            </div>
            <div id="sidebar">
                <uc1:AdminMenu ID="AdminMenu1" runat="server"></uc1:AdminMenu>
            </div>
            <div id="main">
            	<div id="content-head">
                	<div class="ch-head"><%= Lang.TransA("Edit Meta Tags") %></div>
                    <div class="ch-description">
                        <%= Lang.TransA("Use this section to edit your website title and meta tags...") %>
                    </div>
				</div>
                <uc2:MessageBox ID="MessageBox1" runat="server" />
                <table cellpadding="0" cellspacing="0" width="350px" style="margin-left:6px">
                    <tr>
                        <td colspan="2" class="table_header">
                            <%= Lang.TransA("Available tags") %>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell">
                            <ul>
                                <li><b>%%USERNAME%%</b> - Username</li>
                                <li><b>%%AGE%%</b> - User age</li>
                                <li><b>%%GENDER%%</b> - User gender</li>
                                <li><b>%%COUNTRY%%</b> - User country</li>
                                <li><b>%%STATE%%</b> - User state</li>
                                <li><b>%%ZIP%%</b> - User ZIP code</li>
                                <li><b>%%CITY%%</b> - User city
                                <asp:Literal ID="ltrTags" runat="server"></asp:Literal></li>
                                <li><b>%%CATEGORIES%%</b> - Group categories</li>
                                <li><b>%%NAME%%</b> - Group name</li>
                                <li><b>%%DATECREATED%%</b> - Group creation date</li>
                                <li><b>%%OWNER%%</b> - Group owner</li>
                                <li><b>%%TYPE%%</b> - Group type</li>
                                <li><b>%%MEMBERS%%</b> - Number of group members</li>
                                <li><b>%%NAME%%</b> - Topic name</li>
                                <li><b>%%GROUP%%</b> - Topic group</li>
                                <li><b>%%SUBJECT%%</b> - Classified subject</li>
                                <li><b>%%CATEGORY%%</b> - Classified category</li>
                                <li><b>%%DATE%%</b> - Classified creation date</li>
                                <li><b>%%EXPIRATIONDATE%%</b> - Classified expiration date</li>
                                <li><b>%%LOCATION%%</b> - Classified location</li>
                                <li><b>%%POSTEDBY%%</b> - Classified poster</li>
                            </ul>
                        </td>
                    </tr>
                </table>
				<div class="separator10"></div>
                <table cellpadding="0" cellspacing="0" width="800" style="margin-left:6px">
                    <tr>
                        <td colspan="2" class="table_header">
                            <%= Lang.TransA("Meta tags") %>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("Default Title") %>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtDefaultTitle" width="100%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("Default Meta Description") %>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtDefaultMetaDescription" width="100%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("Default Meta Keywords") %>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtDefaultMetaKeywords" width="100%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("ShowUser.aspx, ShowUserBlog.aspx, ShowUserEvents.aspx, ShowUserPhotos.aspx Title")%>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtShowUserTitle" width="100%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("ShowUser.aspx, ShowUserBlog.aspx, ShowUserEvents.aspx, ShowUserPhotos.aspx Meta Description")%>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtShowUserMetaDescription" runat="server" width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("ShowUser.aspx, ShowUserBlog.aspx, ShowUserEvents.aspx, ShowUserPhotos.aspx Meta Keywords")%>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtShowUserMetaKeywords" runat="server" width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("Ads.aspx Title") %>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtAdsTitle" width="100%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("Ads.aspx Meta Description") %>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtAdsMetaDescription" runat="server" width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("Ads.aspx Meta Keywords") %>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtAdsMetaKeywords" runat="server" width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("ChangeLostPassword.aspx Title") %>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtChangeLostPasswordTitle" width="100%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("ChangeLostPassword.aspx Meta Description")%>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtChangeLostPasswordMetaDescription" runat="server" width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("ChangeLostPassword.aspx Meta Keywords")%>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtChangeLostPasswordMetaKeywords" runat="server" width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("Default.aspx Title") %>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtDefaultPageTitle" width="100%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("Default.aspx Meta Description")%>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtDefaultPageMetaDescription" runat="server" width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("Default.aspx Meta Keywords")%>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtDefaultPageMetaKeywords" runat="server" width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("Groups.aspx Title") %>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtGroupsTitle" width="100%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("Groups.aspx Meta Description")%>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtGroupsMetaDescription" runat="server" width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("Groups.aspx Meta Keywords")%>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtGroupsMetaKeywords" runat="server" width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("Login.aspx Title") %>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtLoginTitle" width="100%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("Login.aspx Meta Description")%>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtLoginMetaDescription" runat="server" width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("Login.aspx Meta Keywords")%>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtLoginMetaKeywords" runat="server" width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("LostPassword.aspx Title")%>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtLostPasswordTitle" width="100%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("LostPassword.aspx Meta Description")%>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtLostPasswordMetaDescription" runat="server" width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("LostPassword.aspx Meta Keywords")%>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtLostPasswordMetaKeywords" runat="server" width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("News.aspx Title")%>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtNewsTitle" width="100%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("News.aspx Meta Description")%>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtNewsMetaDescription" runat="server" width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("News.aspx Meta Keywords")%>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtNewsMetaKeywords" runat="server" width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("Registration.aspx Title")%>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtRegister" width="100%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("Registration.aspx Meta Description")%>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtRegisterMetaDescription" runat="server" width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("Registration.aspx Meta Keywords")%>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtRegisterMetaKeywords" runat="server" width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("Search.aspx Title")%>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtSearchTitle" width="100%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("Search.aspx Meta Description")%>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtSearchMetaDescription" runat="server" width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("Search.aspx Meta Keywords")%>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtSearchMetaKeywords" runat="server" width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("SendProfile.aspx Title")%>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtSendProfileTitle" width="100%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("SendProfile.aspx Meta Description")%>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtSendProfileMetaDescription" runat="server" width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("SendProfile.aspx Meta Keywords")%>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtSendProfileMetaKeywords" runat="server" width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("ShowAd.aspx Title") %>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtShowAdTitle" width="100%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("ShowAd.aspx Meta Description") %>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtShowAdMetaDescription" width="100%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("ShowAd.aspx Meta Keywords") %>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtShowAdMetaKeywords" width="100%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("ShowGroup.aspx Title") %>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtShowGroupTitle" width="100%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("ShowGroup.aspx Meta Description") %>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtShowGroupMetaDescription" width="100%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("ShowGroup.aspx Meta Keywords") %>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtShowGroupMetaKeywords" width="100%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("ShowGroupEvents.aspx Title")%>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtShowGroupEventsTitle" width="100%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("ShowGroupEvents.aspx Meta Description")%>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtShowGroupEventsMetaDescription" width="100%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("ShowGroupEvents.aspx Meta Keywords") %>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtShowGroupEventsMetaKeywords" width="100%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("ShowGroupPhotos.aspx Title")%>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtShowGroupPhotosTitle" width="100%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("ShowGroupPhotos.aspx Meta Description")%>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtShowGroupPhotosMetaDescription" width="100%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("ShowGroupPhotos.aspx Meta Keywords") %>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtShowGroupPhotosMetaKeywords" width="100%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("ShowGroupTopics.aspx Title") %>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtShowGroupTopicTitle" width="100%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("ShowGroupTopics.aspx Meta Description") %>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtShowGroupTopicMetaDescription" width="100%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("ShowGroupTopics.aspx Meta Keywords") %>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtShowGroupTopicMetaKeywords" width="100%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("SmsConfirm.aspx Title")%>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtSmsConfirmTitle" width="100%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("SmsConfirm.aspx Meta Description")%>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtSmsConfirmMetaDescription" width="100%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("SmsConfirm.aspx Meta Keywords")%>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtSmsConfirmMetaKeywords" width="100%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("TopCharts.aspx Title")%>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtTopChartsTitle" width="100%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("TopCharts.aspx Meta Description")%>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtTopChartsMetaDescription" width="100%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_cell" width="30%">
                            <%= Lang.TransA("TopCharts.aspx Meta Keywords") %>
                        </td>
                        <td class="table_cell" width="70%">
                            <asp:TextBox ID="txtTopChartsMetaKeywords" width="100%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <div class="separator10"></div>
                <div align="center">
                    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" />
                </div>
                <div class="separator10"></div>
            </div>
	        </div>
	    </form>
	</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfigurationWizard.aspx.cs" Inherits="AspNetDating.Admin.ConfigurationWizard" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<html xmlns="https://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="images/style.css" media="all">
</head>
<body onLoad="showHideAll()">
    <form id="form1" runat="server">
        <uc1:AdminHeader ID="AdminHeader1" runat="server"></uc1:AdminHeader>
        <script language="javascript" type="text/javascript">
            function showHideDiv(checkBoxID, divID) {
                if (document.getElementById(checkBoxID) != null) {
                    if (document.getElementById(checkBoxID).checked)
                        document.getElementById(divID).style.display = '';
                    else
                        document.getElementById(divID).style.display = 'none';
                }
            }
            function showHideAll() {
                showHideDiv('<%= cbEnableClassifiedAds.ClientID %>', 'divClassifiedAdsOptions');
                showHideDiv('<%= cbEnableBlogs.ClientID %>', 'divBlogsOptions');
                showHideDiv('<%= cbEnableCommunityGroups.ClientID %>', 'divCommunityGroupOptions');
                showHideDiv('<%= cbEnableVideoFileUploads.ClientID %>', 'divVideoFileUploadsOptions');
                showHideDiv('<%= cbEnableYouTubeVideosEmbedding.ClientID %>', 'divYouTubeVideosEmbeddingOptions');
                showHideDiv('<%= cbEnableMP3FileUploads.ClientID %>', 'divMP3FileUploadsOptions');
                showHideDiv('<%= cbAllowExplicitPhotos.ClientID %>', 'divExplicitPhotosOptions');
                showHideDiv('<%= cbAllowUsersToRatePhotos.ClientID %>', 'divRatePhotosOptions');
                showHideDiv('<%= cbAllowUsersToUseSkins.ClientID %>', 'divSkinOptions');
                showHideDiv('<%= cbBlockSendingMultipleSimilarMessages.ClientID %>', 'divBlockSendingMultipleSimilarMessagesOptions');
                showHideDiv('<%= cbManuallyApproveInitialUserMessages.ClientID %>', 'divInitialMessagesApprovalOptions');
            }                                        
        </script>        
        <div id="layout" style="text-align:center">
        	<div class="ConfigurationWizardWrap" align="left">
                <div id="content-head" align="left">
                    <div class="ch-head"  align="left">
                        <%= Lang.TransA("Configuration Wizard") %>
                    </div>
                    <div class="ch-description">
                    </div>
                </div>   
                <div class="ConfigurationWizardContent" align="left">
                    <uc1:messagebox id="MessageBox" runat="server"></uc1:messagebox>        
                    <asp:Wizard ID="wzWizard" DisplaySideBar="false" ActiveStepIndex="0"
                        runat="server" onactivestepchanged="wzWizard_ActiveStepChanged"
                        onnextbuttonclick="wzWizard_NextButtonClick" 
                        onfinishbuttonclick="wzWizard_FinishButtonClick">
                        <NavigationStyle HorizontalAlign="Center" />

                        
        <%--                <SideBarStyle VerticalAlign="Top"  />
                        <SideBarTemplate>
                            <asp:DataList runat="server" ShowHeader="false" ID="SideBarList" BackColor="White">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="SideBarButton" Font-Bold="true" />
                                </ItemTemplate>
                            </asp:DataList>
                        </SideBarTemplate>--%>
                        <WizardSteps>
                            <asp:WizardStep ID="wsSiteDetails" runat="server" Title="Site Details">        
                                <table cellpadding="0" cellspacing="0" border="0" class="ConfigurationWizardTable">
	                            <tr>
	                                <td align="center" class="table_header" colspan="2">
	                                    <%= Lang.TransA("Step 1 - Site Details") %></td>
	                            </tr>                                
	                            <tr><td colspan="2" class="table_cell ConfigurationStepsInfo"><%= "Fill in some basic details for your site to get you started. Don't worry - you can always change them later.".TranslateA() %></td></tr>
                                <tr>
                                	<td class="table_cell" colspan="2">
                                    <b><%= "Site name".TranslateA() %></b> - <%= "the name of your site as it will appear in the browser title and e-mail notifications".TranslateA() %>
                                    <div class="separator10"></div>
                                    <asp:TextBox CssClass="ConfigurationWizardTextBox" ID="txtSiteName" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                	<td class="table_cell" colspan="2">
                                    <b><%= "Site e-mail".TranslateA() %></b> - <%= "the e-mail address which your site will use to send e-mail notifications (e.g. noreply@yoursite.com)".TranslateA()%><div class="separator10"></div>
                                    <asp:TextBox CssClass="ConfigurationWizardTextBox" ID="txtSiteEmail" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="table_cell" colspan="2">
                                    <b><%= "Site logo".TranslateA() %></b> - <%= "select your site logo (roughly 170x50)".TranslateA()%>
                                    <div class="separator10"></div><asp:FileUpload ID="fuLogo"  runat="server" /></td>
                                </tr>
                                <tr>
                                	<td class="table_cell" colspan="2"><b><%= "Site type".TranslateA() %></b> - <%= "select your site type and primary audience".TranslateA()%><asp:RadioButtonList ID="rblSiteType" runat="server"></asp:RadioButtonList></td>
                                </tr> 
                                <tr>
	                                <td class="table_cell" colspan="2"><b><%= "Site languages".TranslateA() %></b> - <%= "AspNetDating comes with several language packs which you can use right away. If you need to use another language you can enter it in the textbox below. Additional languages can be added from the \"Edit Languages\" page.".TranslateA()%> <asp:CheckBoxList ID="cblSiteLanguages" runat="server"></asp:CheckBoxList>
                                        <asp:CheckBox ID="cbOtherLanguage" onclick="javascript:focusOtherLanguageTextBox();" runat="server" />
                                        <ajaxToolkit:TextBoxWatermarkExtender ID="tbweOtherLanguage" runat="server" TargetControlID="txtOtherLanguage"></ajaxToolkit:TextBoxWatermarkExtender>
                                        <asp:TextBox ID="txtOtherLanguage" onfocus="javascript:checkOtherLanguage();" runat="server"></asp:TextBox>
                                        <script language="javascript" type="text/javascript">
                                            function focusOtherLanguageTextBox() {
                                                if (document.getElementById('<%= cbOtherLanguage.ClientID %>').checked)
                                                    document.getElementById('<%= txtOtherLanguage.ClientID %>').focus();
                                            }
                                            function checkOtherLanguage() {
                                                document.getElementById('<%= cbOtherLanguage.ClientID %>').checked = true;
                                            }                                    
                                        </script>                                 
                                	</td>
                                </tr>
        <%--                        <tr>
                                    <td colspan="2">
        
                                    </td>
                                </tr>--%>
                                </table>
                            </asp:WizardStep>
                            <asp:WizardStep ID="wsSiteSettings" Title="Site Settings" runat="server">
                                <table cellpadding="0" cellspacing="0" border="0" class="ConfigurationWizardTable">
	                            <tr>
	                                <td align="center" class="table_header" colspan="2">
	                                    <%= Lang.TransA("Step 2 - Site Settings") %></td>
	                            </tr>   
	                            <tr><td colspan="2" class="table_cell ConfigurationStepsInfo"><%= "Select the features and add-ons that you would like to use. You can always enable or disable features from the \"Settings\" section. Additional minor settings and fine-tuning are also available from the \"Setting\" section".TranslateA() %></td></tr>                               
                                <tr><td class="table_cell"><asp:CheckBox ID="cbEnableAjaxChatRoom" runat="server" /></td></tr>
                                <tr><td class="table_cell"><asp:CheckBox ID="cbEnableMessenger" runat="server" /></td></tr>
                                <tr><td class="table_cell"><asp:CheckBox ID="cbEnableCoolIris" runat="server" /></td></tr>
                                <tr><td class="table_cell"><asp:CheckBox ID="cbEnablePhotoAlbums" runat="server" /></td></tr>
                                <tr><td class="table_cell"><asp:CheckBox ID="cbEnableClassifiedAds" onclick="showHideDiv(this.id, 'divClassifiedAdsOptions')" runat="server" />
                                        <div id="divClassifiedAdsOptions" style="display:none">
                                            <li><asp:CheckBox ID="cbOnlyRegisteredUsersCanBrowseClassifiedAds" runat="server" /></li>
                                            <li><asp:CheckBox ID="cbAllowUsersToLeaveCommentsOnTheClassifiedAds" runat="server" /></li>
                                        </div>
                                </td></tr>
                                <tr><td class="table_cell"><asp:CheckBox ID="cbEnableBlogs" onclick="showHideDiv(this.id, 'divBlogsOptions')" runat="server" />
                                        <div id="divBlogsOptions" style="display:none">
                                            <li><asp:CheckBox ID="cbEnableBlogPostApproval" runat="server" /></li>
                                        </div>
                                </td></tr>    
                                <tr><td class="table_cell"><asp:CheckBox ID="cbEnableCommunityGroups" onclick="showHideDiv(this.id, 'divCommunityGroupOptions')" runat="server" />
                                        <div id="divCommunityGroupOptions" style="display:none">
                                            <li><%= "Maximum number of groups a user can join ".TranslateA() %><asp:TextBox ID="txtMaximumGroupsToJoin" runat="server" /></li>
                                            <li><asp:CheckBox ID="cbEnableAjaxChatRoomsInGroups" runat="server" /></li>
                                            <li><asp:CheckBox ID="cbAllowUsersToCreateGroups" runat="server" /></li>
                                        </div>
                                </td></tr>
                                <tr><td class="table_cell"><asp:CheckBox ID="cbEnableVideoFileUploads" onclick="showHideDiv(this.id, 'divVideoFileUploadsOptions')" runat="server" />
                                        <div id="divVideoFileUploadsOptions" style="display:none">
                                            <li><%= "Maximum number of uploaded videos per user ".TranslateA()%><asp:TextBox ID="txtMaximumUploadedVideosPerUser" runat="server" /></li>
                                        </div>
                                </td></tr>
                                <tr><td class="table_cell"><asp:CheckBox ID="cbEnableYouTubeVideosEmbedding" onclick="showHideDiv(this.id, 'divYouTubeVideosEmbeddingOptions')" runat="server" />
                                        <div id="divYouTubeVideosEmbeddingOptions" style="display:none">
                                            <li><%= "Maximum number of YouTube videos per user ".TranslateA()%><asp:TextBox ID="txtMaximumYouTubeVideosPerUser" runat="server" /></li>
                                        </div>
                                </td></tr>
                                <tr><td class="table_cell"><asp:CheckBox ID="cbEnableMP3FileUploads" onclick="showHideDiv(this.id, 'divMP3FileUploadsOptions')" runat="server" />
                                        <div id="divMP3FileUploadsOptions" style="display:none">
                                            <li><%= "Maximum number of MP3 files per user ".TranslateA()%><asp:TextBox ID="txtMaximumMP3FilesPerUser" runat="server" /></li>
                                        </div>
                                </td></tr> 
                                <tr><td class="table_cell"><asp:CheckBox ID="cbEnableLiveWebcamVideoStreaming" runat="server" /></td></tr>                                                                                                                                                                                               
                                <tr><td class="table_cell"><asp:CheckBox ID="cbEnableGadgets" runat="server" /></td></tr> 
                                <tr><td class="table_cell"><asp:CheckBox ID="cbEnableAdBlockerBlocker" runat="server" /></td></tr> 
                                <tr><td class="table_cell"><asp:CheckBox ID="cbEnableSkypeIntegration" runat="server" /></td></tr>
                                <tr><td class="table_cell"><asp:CheckBox ID="cbEnableECards" runat="server" /></td></tr>
                                <tr><td class="table_cell"><asp:CheckBox ID="cbEnableFriends" runat="server" /></td></tr>
                                <tr><td class="table_cell"><asp:CheckBox ID="cbEnableFavorites" runat="server" /></td></tr>
                                <tr><td class="table_cell"><asp:CheckBox ID="cbEnableDistanceSearch" runat="server" /></td></tr>
                                </table>
                            </asp:WizardStep>
                            <asp:WizardStep ID="wsUserSettings" Title="User Settings" runat="server">
                                <table cellpadding="0" cellspacing="0" border="0" class="ConfigurationWizardTable">
	                            <tr>
	                                <td align="center" class="table_header" colspan="2">
	                                    <%= Lang.TransA("Step 3 - User Settings") %></td>
	                            </tr>            
	                            <tr><td colspan="2" class="table_cell ConfigurationStepsInfo"><%= "Configure the user settings related to photos, videos, mp3 files, ratings, etc".TranslateA()%></td></tr>                     
                                    <tr><td class="table_cell"><asp:CheckBox ID="cbPhotoApprovalRequiredByAdministrator" runat="server" /></td></tr>
                                    <tr><td class="table_cell"><asp:CheckBox ID="cbAllowExplicitPhotos" onclick="showHideDiv(this.id, 'divExplicitPhotosOptions')" runat="server" />
                                            <div id="divExplicitPhotosOptions" style="display:none">
                                                <li><asp:CheckBox ID="cbAlwaysMakeExplicitPhotosPrivate" runat="server" /></li>
                                                <li><%= "Minimum age to see explicit photos ".TranslateA()%><asp:TextBox ID="txtMinimumAgeToSeeExplicitPhotos" runat="server" /></li>
                                            </div>
                                    </td class="table_cell"></tr>
                                    <tr><td class="table_cell"><asp:CheckBox ID="cbAllowPrivatePhotos" runat="server" /></td></tr>                    
                                    <tr><td class="table_cell"><%= "Maximum number of uploaded photos per user ".TranslateA()%><asp:TextBox ID="txtMaximumPhotosPerUser" runat="server" /></td></tr>
                                    <tr><td class="table_cell"><asp:CheckBox ID="cbAllowUsersToCommentOnProfiles" runat="server" /></td></tr>
                                    <tr><td class="table_cell"><asp:CheckBox ID="cbAllowUsersToCommentOnPhotos" runat="server" /></td></tr>
                                    <tr><td class="table_cell"><asp:CheckBox ID="cbAllowUsersToRateProfiles" runat="server" /></td></tr>
                                    <tr><td class="table_cell"><asp:CheckBox ID="cbAllowUsersToRatePhotos" onclick="showHideDiv(this.id, 'divRatePhotosOptions')" runat="server" />
                                            <div id="divRatePhotosOptions" style="display:none">
                                                <li><asp:CheckBox ID="cbEnableHotOrNotStyle" runat="server" /></li>
                                            </div>
                                    </td></tr>
                                    <tr><td class="table_cell"><asp:CheckBox ID="cbAllowUsersToSetStatus" runat="server" /></td></tr>
                                    <tr><td class="table_cell"><asp:CheckBox ID="cbAllowUsersToUseSkins" onclick="showHideDiv(this.id, 'divSkinOptions')" runat="server" />
                                            <div id="divSkinOptions" style="display:none">
                                                <li><asp:CheckBox ID="cbAllowUsersToCustomizeSkin" runat="server" /></li>
                                            </div>
                                    </td></tr>
                                    <tr><td class="table_cell"><asp:CheckBox ID="cbAllowUsersToSpecifyRelationships" runat="server" /></td></tr>                                           
                                </table>                    
                            </asp:WizardStep>
                            <asp:WizardStep ID="wsAntiSpamSettings" Title="Anti-spam Settings" runat="server">
                                <table cellpadding="0" cellspacing="0" border="0"  class="ConfigurationWizardTable">
	                            <tr>
	                                <td align="center" class="table_header" colspan="2">
	                                    <%= Lang.TransA("Step 4 - Anti-spam Settings")%></td>
	                            </tr>       
	                            <tr><td colspan="2" class="table_cell ConfigurationStepsInfo"><%= "Configure the anti-spam software features".TranslateA()%></td></tr>                          
                                    <tr><td class="table_cell"><asp:CheckBox ID="cbEnableCaptchaValidation" runat="server" /></td></tr>
                                    <tr><td class="table_cell"><asp:CheckBox ID="cbRequireEmailConfirmation" runat="server" /></td></tr>
                                    <tr><td class="table_cell"><asp:CheckBox ID="cbEnableEmailAndPhoneNumberFiltering" runat="server" /></td></tr>
                                    <tr><td class="table_cell"><asp:CheckBox ID="cbBlockSendingMultipleSimilarMessages" onclick="showHideDiv(this.id, 'divBlockSendingMultipleSimilarMessagesOptions')" runat="server" />
                                            <div id="divBlockSendingMultipleSimilarMessagesOptions" style="display:none">
                                                <li><%= "Number of similar messages to trigger the block ".TranslateA() %><asp:TextBox ID="txtNumberOfSimilarMessages" runat="server" /></li>
                                            </div>
                                    </td></tr>    
                                    <tr><td class="table_cell"><asp:CheckBox ID="cbManuallyApproveInitialUserMessages" onclick="showHideDiv(this.id, 'divInitialMessagesApprovalOptions')" runat="server" />
                                            <div id="divInitialMessagesApprovalOptions" style="display:none">
                                                <li><%= "Number of messages per user to be approved ".TranslateA()%><asp:TextBox ID="txtMessagesPerUserToBeApproved" runat="server" /></li>
                                            </div>
                                    </td></tr>
                                    <tr><td class="table_cell"><%= "Maximum number of messages per day ".TranslateA()%><asp:TextBox ID="txtMaximumMessagesPerDay" runat="server" /></td></tr>
                                    <tr><td class="table_cell"><%= "Maximum users contacted per day ".TranslateA()%><asp:TextBox ID="txtMaximumUsersContactedPerDay" runat="server" /></td></tr>                            
                                    <tr><td class="table_cell"><asp:CheckBox ID="cbAllowUsersToReportProfileAbuse" runat="server" /></td></tr>                      
                                    <tr><td class="table_cell"><asp:CheckBox ID="cbAllowUsersToReportPhotoAbuse" runat="server" /></td></tr>
                                    <tr><td class="table_cell"><asp:CheckBox ID="cbAllowUsersToReportMessageAbuse" runat="server" /></td></tr>                                                                              
                                </table>
                            </asp:WizardStep>
                            <asp:WizardStep ID="wsFinish" runat="server" StepType="Complete">
                                FINITO
                            </asp:WizardStep>                      
                        </WizardSteps>
                    </asp:Wizard>
                </div>
            </div>            
        </div>
    </form>
</body>
</html>

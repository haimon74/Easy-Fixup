<%@ Import namespace="ezFixUp.Classes"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditGroup.ascx.cs" Inherits="ezFixUp.Components.Groups.EditGroup" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="~/Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="~/Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="~/Components/HeaderLine.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
    <ajaxToolkit:CascadingDropDown ID="CascadingDropDownCountry" runat="server" TargetControlID="dropCountry"
        Category="Country" ServicePath="~/Services/Service.asmx" ServiceMethod="GetCountries" />
    <ajaxToolkit:CascadingDropDown ID="CascadingDropDownState" runat="server" TargetControlID="dropRegion"
        Category="Region" ServicePath="~/Services/Service.asmx" ServiceMethod="GetRegionsByCountry"
        ParentControlID="dropCountry" />
    <ajaxToolkit:CascadingDropDown ID="CascadingDropDownCity" runat="server" TargetControlID="dropCity"
        Category="City" ServicePath="~/Services/Service.asmx" 
        ServiceMethod="GetCitiesByCountryAndRegion" ParentControlID="dropRegion" />
        
<uc1:largeboxstart id="LargeBoxStart1" runat="server"></uc1:largeboxstart>
	<div class="srvmsg"><asp:Label ID="lblError" CssClass="error" runat="server" EnableViewState="False"></asp:Label></div>
    <asp:MultiView ID="mvEditGroup" runat="server">
        <asp:View ID="viewMain" runat="server">
            <asp:Label ID="lblEditGroup" CssClass="note" runat="server" Visible="false"></asp:Label>
			<div class="buttons">
            	<asp:Button ID="btnEditGroup" runat="server" Visible="false" OnClick="btnEditGroup_Click" />
            </div>
			<div id="divSeparatorEditGroup" runat="server" class="separator"></div>
			<div id="divLineEditGroup" runat="server" class="line"></div>
            <asp:Label ID="lblTransferOwnership" CssClass="note" runat="server" Visible="false"></asp:Label>
            <br />
			<div class="buttons">
            	<asp:Button ID="btnTransferOwnership" runat="server" Visible="false" OnClick="btnTransferOwnership_Click" />
			</div>
			<div id="divSeparatorTransferOwnership" runat="server" class="separator"></div>
			<div id="divLineTransferOwnership" runat="server" class="line"></div>
            <asp:Label ID="lblDeleteGroup" CssClass="note" runat="server" Visible="false"></asp:Label>
            <br />
			<div class="buttons">
            	<asp:Button ID="btnDeleteGroup" runat="server" OnClick="btnDeleteGroup_Click" Visible="false"/>
            </div>
        </asp:View>
        <asp:View ID="viewEditGroup" runat="server">
        	<div class="wrap-sized">
				<div class="wrap-img">
					<img class="photoframe" src="GroupIcon.ashx?groupID=<%= CurrentGroup.ID.ToString() %>&width=90&height=90&diskCache=1" />
				</div>
				<div class="wrap-info">
					<table cellpadding="0" cellspacing="0">
						<tr>
							<td width="30%">
								<asp:Label ID="lblGroupImage" runat="server"></asp:Label>
								<div class="separator"></div>
							</td>
							<td>
								<asp:FileUpload ID="fuGroupImage" runat="server" />
								<div class="separator"></div>
							</td>
						</tr>
						<tr>
							<td>
								<asp:Label ID="lblAccessLevel" runat="server"></asp:Label>
							</td>
							<td>
								<asp:DropDownList ID="ddAccessLevel" AutoPostBack="true" CssClass="dropdownlist" runat="server" OnSelectedIndexChanged="ddAccessLevel_SelectedIndexChanged"></asp:DropDownList>
							</td>
						</tr>
					</table>
				</div>
				<div class="clear"></div>
				<asp:Label ID="lblName" runat="server"></asp:Label>
				<br>
				<asp:TextBox CssClass="titletextbox" ID="txtName" runat="server"></asp:TextBox>
				<br />
				<div class="separator"></div>
				<asp:Label ID="lblCategories" runat="server"></asp:Label><br />
				<asp:ListBox CssClass="listbox" ID="lbCategories" runat="server" SelectionMode="Multiple">
				</asp:ListBox>
				<br />
				<div class="hint">*<asp:Label ID="lblExplanation" runat="server"></asp:Label></div>
				<asp:Label ID="lblDescription" runat="server"></asp:Label>
				<br />
				<div class="clear"></div>
				<asp:TextBox ID="txtDescription" runat="server" CssClass="multilinetextbox" TextMode="MultiLine" Rows="5" Columns="50">
				</asp:TextBox>
                <br />
                <div class="separator"></div>
                <div id="pnlAgeRestriction" runat="server">
   			        <%= Lang.Trans("Minimum age restriction") %>&nbsp;<asp:DropDownList ID="ddAgeRestriction" runat="server"></asp:DropDownList>
                </div>
                <div class="separator"></div>
                <div id="pnlAutomaticallyJoin" runat="server" visible="false">
                    <%= Lang.Trans("Users automatically join group")%>&nbsp;<asp:CheckBox ID="cbAutomaticallyJoin" runat="server" AutoPostBack="true"
                        oncheckedchanged="cbAutomaticallyJoin_CheckedChanged" />
                </div>
                <div class="separator"></div>
                <div id="pnlCountry" runat="server" visible="false">
                    <%= Lang.Trans("Country") %>&nbsp;<asp:DropDownList ID="dropCountry" CssClass="dropdownlist" runat="server"></asp:DropDownList>
                </div>
                <div class="separator"></div>
                <div id="pnlState" runat="server" visible="false">
                    <%= Lang.Trans("Region/State") %>
                    <asp:DropDownList ID="dropRegion" CssClass="dropdownlist" runat="server">
                    	<asp:ListItem Value=""></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="separator"></div>
                <div id="pnlCity" runat="server" visible="false">
                    <%= Lang.Trans("City") %>
                    <asp:DropDownList ID="dropCity" CssClass="dropdownlist" runat="server">
                    	<asp:ListItem Value=""></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="separator"></div>
                <!-- begin group permissions -->
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td></td>
                        <td class="center"><%= Lang.Trans("Non Members") %></td>
                        <td class="center"><%= Lang.Trans("Members") %></td>
                        <td class="center"><%= Lang.Trans("V.I.P.") %></td>
                    </tr>
                    <tr>
                        <td><%= Lang.Trans("View group") %></td>
                        <td class="center"><asp:CheckBox ID="cbViewGroupNonMembers" runat="server" /></td>
                        <td class="center"><asp:CheckBox ID="cbViewGroupMembers" runat="server" /></td>
                        <td class="center"><asp:CheckBox ID="cbViewGroupVip" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><%= Lang.Trans("View message board") %></td>
                        <td class="center"><asp:CheckBox ID="cbViewMessageBoardNonMembers" runat="server" /></td>
                        <td class="center"><asp:CheckBox ID="cbViewMessageBoardMembers" runat="server" /></td>
                        <td class="center"><asp:CheckBox ID="cbViewMessageBoardVip" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><%= Lang.Trans("View gallery") %></td>
                        <td class="center"><asp:CheckBox ID="cbViewGalleryNonMembers" runat="server" /></td>
                        <td class="center"><asp:CheckBox ID="cbViewGalleryMembers" runat="server" /></td>
                        <td class="center"><asp:CheckBox ID="cbViewGalleryVip" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><%= Lang.Trans("View members") %></td>
                        <td class="center"><asp:CheckBox ID="cbViewMembersNonMembers" runat="server" /></td>
                        <td class="center"><asp:CheckBox ID="cbViewMembersMembers" runat="server" /></td>
                        <td class="center"><asp:CheckBox ID="cbViewMembersVip" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><%= Lang.Trans("View events") %></td>
                        <td class="center"><asp:CheckBox ID="cbViewEventsNonMembers" runat="server" /></td>
                        <td class="center"><asp:CheckBox ID="cbViewEventsMembers" runat="server" /></td>
                        <td class="center"><asp:CheckBox ID="cbViewEventsVip" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><%= Lang.Trans("Upload photo") %></td>
                        <td class="center"><asp:CheckBox ID="cbUploadPhotoNonMembers" runat="server" /></td>
                        <td class="center"><asp:CheckBox ID="cbUploadPhotoMembers" runat="server" /></td>
                        <td class="center"><asp:CheckBox ID="cbUploadPhotoVip" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><%= Lang.Trans("Use chat") %></td>
                        <td class="center"><asp:CheckBox ID="cbUseChatNonMembers" runat="server" /></td>
                        <td class="center"><asp:CheckBox ID="cbUseChatMembers" runat="server" /></td>
                        <td class="center"><asp:CheckBox ID="cbUseChatVip" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><%= Lang.Trans("Add topic") %></td>
                        <td class="center"><asp:CheckBox ID="cbAddTopicNonMembers" runat="server" /></td>
                        <td class="center"><asp:CheckBox ID="cbAddTopicMembers" runat="server" /></td>
                        <td class="center"><asp:CheckBox ID="cbAddTopicVip" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><%= Lang.Trans("Add post") %></td>
                        <td class="center"><asp:CheckBox ID="cbAddPostNonMembers" runat="server" /></td>
                        <td class="center"><asp:CheckBox ID="cbAddPostMembers" runat="server" /></td>
                        <td class="center"><asp:CheckBox ID="cbAddPostVip" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><%= Lang.Trans("Add event") %></td>
                        <td class="center"><asp:CheckBox ID="cbAddEventNonMembers" runat="server" /></td>
                        <td class="center"><asp:CheckBox ID="cbAddEventMembers" runat="server" /></td>
                        <td class="center"><asp:CheckBox ID="cbAddEventVip" runat="server" /></td>
                    </tr>
                </table>
                <div class="separator"></div>
                <asp:Label ID="lblTerms" runat="server"></asp:Label><br/>
                <asp:TextBox ID="txtTerms" TextMode="MultiLine" CssClass="multilinetextbox" runat="server"></asp:TextBox><br />   				
                <asp:UpdatePanel ID="UpdatePanelPhotoDetails" runat="server">
                    <ContentTemplate>
                        <div id="pnlQuestion" Visible="false" runat="server">
                            <br />
                            <asp:Label ID="lblQuestion" runat="server"></asp:Label><br/>
                            <asp:TextBox ID="txtQuestion" runat="server" CssClass="multilinetextbox" TextMode="MultiLine" Rows="5" Columns="50"></asp:TextBox>
                            <br />
                            <div class="hint">*<%= Lang.Trans("You may leave this field blank") %></div>            
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddAccessLevel" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>				
				<div class="buttons">
					<asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" />
					<asp:Button ID="btnCancelUpdate" runat="server" OnClick="btnCancelUpdate_Click" />
				</div>
			</div>
		</asp:View>
        <asp:View ID="viewTransferOwnership" runat="server">
        <div class="center">
            <asp:DropDownList ID="ddGroupMembers" runat="server"></asp:DropDownList>
            <asp:Button ID="btnTransfer" runat="server" OnClick="btnTransfer_Click" />
            <asp:Button ID="btnCancelTransfer" runat="server" OnClick="btnCancelTransfer_Click" />
        </div>
        </asp:View>
            <asp:View ID="viewDeleteGroup" runat="server">
            <%= Lang.Trans("Do you really want to delete this group? This is an irreversible action!") %><br />
            <div class="buttons">
	            <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" />
	            <asp:Button ID="btnCancelDelete" runat="server" OnClick="btnCancelDelete_Click"/>
            </div>
        </asp:View>
    </asp:MultiView>
<uc1:largeboxend id="LargeBoxEnd1" runat="server"></uc1:largeboxend>
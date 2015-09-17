<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PostAd.aspx.cs" Inherits="AspNetDating.PostAd" %>
<%@ Import Namespace="AspNetDating.Classes"%>

<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="HTMLEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
<div id="sidebar">
	    <uc1:smallboxstart id="SmallBoxStart1" runat="server"></uc1:smallboxstart>
	    <div class="SideMenuItem"><a href='Ads.aspx' class="SideMenuLink"><%= "All Classifieds".Translate() %></a></div>
		<uc1:smallboxend id="SmallBoxEnd1" runat="server"></uc1:smallboxend>
</div>
<div id="main">
    <uc1:largeboxstart id="LargeBoxStart1" runat="server"></uc1:largeboxstart>
            <div class="srvmsg"><asp:Label ID="lblError" CssClass="error" runat="server" EnableViewState="False"></asp:Label></div>
            <asp:Panel ID="pnlEditAd" runat="server">
            	<table cellpadding="5" cellspacing="0" width="100%">
            		<tr>
            			<td width="60px">
            				<%= "Category" %>:
            			</td>
            			<td>
			                <asp:DropDownList ID="ddCategories" CssClass="dropdownlist" runat="server" AutoPostBack="true"
			                    onselectedindexchanged="ddCategories_SelectedIndexChanged"></asp:DropDownList>
			                <asp:DropDownList ID="ddSubcategories" runat="server" CssClass="dropdownlist" Visible="false"></asp:DropDownList>
            			</td>
            		</tr>
            		<tr>
            			<td>
            				<%= "Subject".Translate() %>:
            			</td>
            			<td>
            				<asp:TextBox ID="txtSubject" CssClass="textbox" runat="server"></asp:TextBox>
            			</td>
            		</tr>
            		<tr>
            			<td>
            				<%= "Location".Translate() %>:
            			</td>
            			<td>
            				<asp:TextBox ID="txtLocation" CssClass="textbox" runat="server"></asp:TextBox>
            			</td>
            		</tr>
            		<tr>
            			<td>
            				<%= "Life Time".Translate() %>:
            			</td>
            			<td>
            				<asp:DropDownList ID="ddExpiration" CssClass="dropdownlist" runat="server"></asp:DropDownList>
            			</td>
            		</tr>
            		<tr>
            			<td valign="top">
            				<%= "Message" %>:
            			</td>
            			<td align="center">
            				<asp:PlaceHolder ID="phEditor" runat="server"></asp:PlaceHolder>
            			</td>
            		</tr>
            	</table>
            </asp:Panel>
            <asp:Panel ID="pnlUploadAdPhotos" runat="server">
                <asp:Repeater ID="rptAdPhotos" runat="server" 
                    onitemcommand="rptAdPhotos_ItemCommand" onitemcreated="rptAdPhotos_ItemCreated">
                    <ItemTemplate>
                        <%# AspNetDating.AdPhoto.RenderImageTag((int)Eval("AdPhotoID"), 90, 90, null, true) %>
                        <asp:LinkButton ID="lnkDelete" runat="server" CommandArgument='<%# Eval("AdPhotoID") %>' CommandName="DeletePhoto"></asp:LinkButton>
                        <br />
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Repeater ID="rptAddPhoto" runat="server">
                    <ItemTemplate>
                        <%= "Image file".Translate() %>&nbsp;<asp:FileUpload ID="fuAdPhoto" runat="server" /><div class="separator6"></div>
                        <%= "Image description".Translate() %><br />
                        <asp:TextBox ID="txtAdPhotoDescription" CssClass="textbox" Width="100%" runat="server"></asp:TextBox>
                    </ItemTemplate>
                    <SeparatorTemplate>
                    	<div class="separator6"></div>
                    	<div class="line"></div>
                    	<div class="separator6"></div>
                    </SeparatorTemplate>
                </asp:Repeater>
            </asp:Panel>
            <div class="center">
            	<div class="separator"></div>
            	<asp:Button ID="btnSave" runat="server" onclick="btnSave_Click" />
            </div>
    <uc1:largeboxend id="LargeBoxEnd1" runat="server"></uc1:largeboxend>
</div>
</asp:Content>

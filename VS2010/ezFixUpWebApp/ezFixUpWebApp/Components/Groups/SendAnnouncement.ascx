<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SendAnnouncement.ascx.cs" Inherits="ezFixUp.Components.Groups.SendAnnouncement" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Import Namespace="ezFixUp.Classes" %>

<uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"></uc1:LargeBoxStart>
<div class="srvmsg"><asp:Label ID="lblError" CssClass="error" runat="server" EnableViewState="False"></asp:Label></div>
<div class="wrap-sized">
	<div class="clear"></div>
	<div class="separator"></div>
	<div id="pnlSendAnnoucement" runat="server">
	    <span id="pnlGender" runat="server">
		    <%= Lang.Trans("Send to") %>
		    <asp:DropDownList ID="ddGender" CssClass="textbox" runat="server" />
		    <div class="clear"></div>
		    <div class="separator"></div>
		</span>
		<span id="pnlAge" runat="server">
		    <div class="left">
	            <%= Lang.Trans("Age Range") %>
	            <%= Lang.Trans("from") %>&nbsp;
	        </div>
	        <div class="left">
	            <table cellspacing="0" cellpadding="0">
	                <tr>
	                    <td valign="top">
	                        <asp:TextBox ID="txtAgeFrom" runat="server" CssClass="textbox" Size="2" MaxLength="2" />
	                        <%= Lang.Trans("to") %>
	                        <asp:TextBox ID="txtAgeTo" runat="server" CssClass="textbox" Size="2" MaxLength="2" />
	                    </td>
	                </tr>
	            </table>
		    </div>
		    <div class="clear"></div>
		    <div class="separator"></div>
		</span>
		<%= Lang.Trans("Enter your announcement") %>
		<div class="clear"></div>
		<asp:TextBox ID="txtAnnouncement" runat="server" CssClass="multilinetextbox" TextMode="MultiLine"></asp:TextBox>
	</div>
</div>
<div class="separator"></div>
<div class="buttons">
    <asp:Button ID="btnSendAnnouncement" runat="server" OnClick="btnSendAnnouncement_Click"/>
</div>
<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"></uc1:LargeBoxEnd>
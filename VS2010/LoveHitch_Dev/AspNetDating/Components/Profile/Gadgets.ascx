<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="../LargeBoxStart.ascx" %>
<%@ Control Language="c#" AutoEventWireup="True" Codebehind="Gadgets.ascx.cs" Inherits="AspNetDating.Components.Profile.GadgetsCtrl"
    TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="../LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="../HeaderLine.ascx" %>
<uc1:largeboxstart id="LargeBoxStart" runat="server">
</uc1:largeboxstart><input id="hidUsername" type="hidden" name="hidUsername" runat="server">
<uc1:HeaderLine id="hlGadgets" runat="server"></uc1:HeaderLine>
<div class="note">
	<%= Lang.Trans("Gadgets are easy-to-use mini programs that give you information at a glance and provide easy access to frequently used tools. You will need Windows Vista Sidebar in order to use gadgets.") %>
</div>
<div class="label"><%= Lang.Trans("Available gadgets:") %></div>
<table cellpadding="4" cellspacing="0" class="gadgets">
	<tr>
		<td>
		    <%= Lang.Trans("New Users Gadget") %>
		</td>
		<td>
		    <asp:Button ID="btnDownloadNewUsersGadget" runat="server" OnClick="btnDownloadNewUsersGadget_Click"></asp:Button>
		</td>
    </tr>
	<tr>
		<td>
		    <%= Lang.Trans("Quick Stats Gadget") %>
		</td>
		<td>
		    <asp:Button ID="btnDownloadQuickStatsGadget" runat="server" OnClick="btnDownloadQuickStatsGadget_Click"></asp:Button>
		</td>
    </tr>
</table>
<uc1:LargeBoxEnd id="LargeBoxEnd" runat="server"></uc1:LargeBoxEnd>

<%@ Control Language="c#" AutoEventWireup="True" Codebehind="ViewMultiChoiceBig.ascx.cs" Inherits="ezFixUp.Components.Profile.ViewMultiChoiceBig" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<div class="skincolor">
<asp:Label CssClass="label" id="lblName" runat="server" />:
<asp:Panel ID="pnlMessage" Runat="server">
	<asp:Label id="lblMessage" Runat="server"></asp:Label>
</asp:Panel>
<asp:Panel ID="pnlValues" Runat="server">
	<asp:Repeater id="rptValues" Runat="server">
		<HeaderTemplate><ul class="prof-info-values"></HeaderTemplate>
		<ItemTemplate>
			<li>
				<div class="value skincolor"><%# Container.DataItem %></div>
			</li>
		</ItemTemplate>
		<FooterTemplate></ul></FooterTemplate>
	</asp:Repeater>
</asp:Panel>
</div>

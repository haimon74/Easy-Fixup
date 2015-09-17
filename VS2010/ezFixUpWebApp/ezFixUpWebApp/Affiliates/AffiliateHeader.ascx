<%@ Import namespace="ezFixUp.Classes"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AffiliateHeader.ascx.cs" Inherits="ezFixUp.Affiliates.AffiliateHeader" %>
<asp:ScriptManager ID="ScriptManagerMaster" runat="server" />
<asp:UpdateProgress ID="UpdateProgressMaster" runat="server">
    <ProgressTemplate>
        <asp:Image ID="imgLoadingProgress" ImageUrl="~/Images/loading2.gif" runat="server" />
    </ProgressTemplate>
</asp:UpdateProgress>
<div id="headerwrap">
	<div id="logo">
		<div id="headerlogin">
			<asp:Label id="lblWelcome" runat="server"></asp:Label>
			<asp:LinkButton id="lnkLogout" runat="server" CssClass="exitlink" OnClick="lnkLogout_Click"></asp:LinkButton>
		</div>
	</div>
	<div id="glance">
		<asp:Panel ID="pnlLogout" Runat="server">
			<table cellspacing="0" cellpadding="0" class="header-table">
				<tr>
					<td align="left" class="header-links">
					<%= Lang.Trans("Your balance is") %>:
                            <b><%= Affiliate.Balance.ToString("C") %></b>
					</td>
				</tr>
			</table>
		</asp:Panel>
	</div>
</div>
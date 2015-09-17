<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreditsPackages.ascx.cs" Inherits="ezFixUp.Components.Profile.CreditsPackages" %>
<%@ Import namespace="ezFixUp.Classes"%>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="../HeaderLine.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="../LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="../LargeBoxStart.ascx" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<uc1:largeboxstart id="LargeBoxStart" runat="server"></uc1:largeboxstart><INPUT id="hidUsername" type="hidden" name="hidUsername" runat="server">
<uc1:headerline id="hlPaymentSettings" runat="server"></uc1:headerline><br>
<div id="pnlPaymentProcessors" runat="server">
<%=Lang.Trans("Select payment method:")%>
<asp:DropDownList id="ddPaymentProcessors" CssClass="dropdownlist" runat="server" 
        AutoPostBack="True" 
        onselectedindexchanged="ddPaymentProcessors_SelectedIndexChanged"></asp:DropDownList>
<br />
</div>
<asp:Label ID="lblCurrentPlan" Runat="server"></asp:Label><br>
<asp:Label ID="lblMessage" Runat="server"></asp:Label>
<div class="wrap-view">
<asp:RadioButtonList id="rlCreditsPackages" runat="server"></asp:RadioButtonList>
</div>
<asp:Label ID="lblRenewalDate" Runat="server"></asp:Label>
<div id="divBillingDetails" visible="false" runat="server">
	<div class="separator"></div>
	<%=Lang.Trans("Billing details:")%>
	<div class="wrap-view">
		<table cellpadding="4" cellspacing="0" class="settings">
			<tr>
				<td>
					First name				
				</td>
				<td>
					<asp:TextBox ID="txtFirstName" Runat="server"></asp:TextBox>
				</td>
			<tr>
				<td>
					Last name
				</td>
				<td>
					<asp:TextBox ID="txtLastName" Runat="server"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
					Address
				</td>
				<td>
					<asp:TextBox ID="txtAddress" Runat="server"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
					City
				</td>
				<td>
					<asp:TextBox ID="txtCity" Runat="server"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
					State
				</td>
				<td>
					<asp:TextBox ID="txtState" Runat="server"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
					ZIP
				</td>
				<td>
					<asp:TextBox ID="txtZip" Runat="server"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
					Country
				</td>
				<td>
					<asp:TextBox ID="txtCountry" Runat="server"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
					Phone
					<div class="separator"></div>
				</td>
				<td>
					<asp:TextBox ID="txtPhone" Runat="server"></asp:TextBox>
					<div class="separator"></div>
				</td>
			</tr>
			<tr>
				<td>
					Card Number
				</td>
				<td>
					<asp:TextBox ID="txtCardNumber" Runat="server"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
					Card Expiration
				</td>
				<td>
					<asp:DropDownList CssClass="dropdownlist" ID="ddMonth" Runat="server">
						<asp:ListItem></asp:ListItem>
						<asp:ListItem>01</asp:ListItem>
						<asp:ListItem>02</asp:ListItem>
						<asp:ListItem>03</asp:ListItem>
						<asp:ListItem>04</asp:ListItem>
						<asp:ListItem>05</asp:ListItem>
						<asp:ListItem>06</asp:ListItem>
						<asp:ListItem>07</asp:ListItem>
						<asp:ListItem>08</asp:ListItem>
						<asp:ListItem>09</asp:ListItem>
						<asp:ListItem>10</asp:ListItem>
						<asp:ListItem>11</asp:ListItem>
						<asp:ListItem>12</asp:ListItem>
					</asp:DropDownList>
					<asp:DropDownList CssClass="dropdownlist" ID="ddYear" Runat="server">
						<asp:ListItem></asp:ListItem>
						<asp:ListItem>2010</asp:ListItem>
						<asp:ListItem>2011</asp:ListItem>
						<asp:ListItem>2012</asp:ListItem>
						<asp:ListItem>2013</asp:ListItem>
						<asp:ListItem>2014</asp:ListItem>
					</asp:DropDownList>
				</td>
			</tr>
		</table>
	</div>
</div>
<div class="buttons">
	<asp:Button id="btnNext" runat="server" onclick="btnNext_Click"></asp:Button>
</div>
<uc1:LargeBoxEnd id="LargeBoxEnd" runat="server"></uc1:LargeBoxEnd>
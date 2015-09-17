<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Register.aspx.cs" Inherits="ezFixUp.Affiliates.Register" %>

<%@ Register TagPrefix="uc1" TagName="AffiliateHeader" Src="~/Affiliates/AffiliateHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="ezFixUp.Classes" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title><%= Lang.Trans("Register") %></title>
		<link rel="stylesheet" type="text/css" href="images/style.css" media="all" />
	</head>
	<body>
	<form id="Form1" method="post" runat="server">
	<uc1:AffiliateHeader id="AffiliateHeader1" runat="server"></uc1:AffiliateHeader>
	<div id="layout">
			    <center>
			    <div class="separator10"></div>
			    <div class="separator10"></div>
			    <table id="stable" class="border" cellspacing="0" cellpadding="0">
					<tr>
						<td colspan="2" class="loginbox-head"><%= Lang.Trans("Register") %></td>
				    </tr>
				    <tr>
						<td class="table_cell"><%= Lang.Trans("Username") %></td>
						<td class="table_cell"><asp:TextBox id="txtUsername" runat="server"></asp:TextBox></td>
				   	</tr>
				    <tr>
						<td class="table_cell"><%= Lang.Trans("Password") %></td>
					    <td class="table_cell"><asp:TextBox id="txtPassword" runat="server" TextMode="Password"></asp:TextBox></td>
				    </tr>
				    <tr>
						<td class="table_cell"><%= Lang.Trans("Confirm Password") %></td>
					    <td class="table_cell"><asp:TextBox id="txtPasswordConfirm" runat="server" TextMode="Password"></asp:TextBox></td>
				    </tr>
				    <tr>
						<td class="table_cell"><%= Lang.Trans("Name") %></td>
						<td class="table_cell"><asp:TextBox id="txtName" runat="server"></asp:TextBox></td>
				   	</tr>
				   	<tr>
						<td class="table_cell"><%= Lang.Trans("Email") %></td>
						<td class="table_cell"><asp:TextBox id="txtEmail" runat="server"></asp:TextBox></td>
				   	</tr>
				   	<tr>
						<td class="table_cell"><%= Lang.Trans("Site URL")%></td>
						<td class="table_cell"><asp:TextBox id="txtSiteUrl" runat="server"></asp:TextBox></td>
				   	</tr>
				   	<tr>
						<td class="table_cell" valign="top"><%= Lang.Trans("Payment Details")%></td>
						<td class="table_cell"><asp:TextBox id="txtPaymentDetails" runat="server" TextMode="MultiLine" Rows="10" Columns="50"></asp:TextBox></td>
				   	</tr>
				    <tr>
						<td colspan="2" class="table_cell">
							<div class="add-buttons">
								<asp:Button id="btnRegister" runat="server" OnClick="btnRegister_Click"></asp:Button>
							</div>
					    </td>
				    </tr>
				    <tr>
						<td colspan="2">
							<uc1:MessageBox id="MessageBox" runat="server"></uc1:MessageBox>					    
						</td>
				    </tr>
				</table>
 	            </center>
		    </div>
		</form>
	</body>
</html>

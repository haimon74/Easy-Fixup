<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Estream.AjaxChat.Admin.Login" Title="Login Page" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link rel="stylesheet" type="text/css" href="Images/style.css" media="all" />
</head>
<body class="login">
    <form id="form1" runat="server">
    <div>
        <table id="loginbox" border="0" cellspacing="0" cellpadding="0">
			<tr>
				<td colspan="2" class="loginbox-head">Authorize</td>
		    </tr>
		    <tr>
				<td class="loginbox-label">Username</td>
				<td class="loginbox-txt"><asp:TextBox id="txtUsername" runat="server"></asp:TextBox></td>
		   	</tr>
		    <tr>
				<td class="loginbox-label">Password</td>
			    <td class="loginbox-txt"><asp:TextBox id="txtPassword" runat="server" TextMode="Password"></asp:TextBox></td>
		    </tr>
		    <tr>
				<td colspan="2" class="add-buttons">
					<asp:Button id="btnLogin" runat="server" Text=" Login >> " onclick="btnLogin_Click"></asp:Button>
			    </td>
		    </tr>
		    <tr>
				<td colspan="2">
					<uc1:MessageBox id="MessageBox" runat="server"></uc1:MessageBox>					    
				</td>
		    </tr>
		</table>    
    </div>
    </form>
</body>
</html>

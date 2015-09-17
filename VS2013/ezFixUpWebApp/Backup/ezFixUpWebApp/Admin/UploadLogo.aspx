<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadLogo.aspx.cs" Inherits="ezFixUp.Admin.UploadLogo" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Import Namespace="ezFixUp.Classes" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "https://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="https://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="images/style.css" media="all">
</head>
<body>
    <form id="form1" runat="server">
        <uc1:AdminHeader ID="AdminHeader1" runat="server"></uc1:AdminHeader>
        <div id="layout">
            <div id="crupms">
                <img src="images/i_home.jpg">
                <div id="crumpstxt">
                    <%= Lang.TransA("Upload Logo") %>
                </div>
            </div>
            <div id="sidebar">
                <uc1:AdminMenu ID="Adminmenu1" runat="server"></uc1:AdminMenu>
            </div>
            <div id="main">
                <uc1:MessageBox id="Messagebox1" runat="server"></uc1:MessageBox>    
                <br /><br /><br /><br />
                <asp:Image ID="imgLogo" Runat="server"></asp:Image>
		        <table>
					<tr>
						<td class="table_header" align="center" colSpan="2"><%= Lang.TransA("Upload Logo") %></td>
					</tr>
					<tr>
						<td class="table_cell"><%= Lang.TransA("Image file") %></td>
						<td class="table_cell"><INPUT id="ufLogo" type="file" runat="server" NAME="ufLogo"></td>
					</tr>
					<tr>
						<td class="table_cell" align="center" colSpan="2"><asp:Button id="btnUpload" OnClick="btnUpload_Click" Runat="server"></asp:Button></td>
					</tr>
				</table>                        
            </div>
        </div>        
    </form>
</body>
</html>

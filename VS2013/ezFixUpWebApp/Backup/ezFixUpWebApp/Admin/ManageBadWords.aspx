<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageBadWords.aspx.cs" Inherits="ezFixUp.Admin.ManageBadWords" %>

<%@ Import Namespace="ezFixUp.Classes" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<html>
	<head>
	    <title><%= Lang.TransA("Manage Bad Words") %></title>
	    <link rel="stylesheet" type="text/css" href="images/style.css" media="all">
	</head>
	<body>
	    <form id="form1" runat="server">
            <uc1:AdminHeader ID="AdminHeader1" runat="server"></uc1:AdminHeader>
			<div id="layout">
            	<div id="crupms">
                	<img src="images/i_spam.jpg" />
                    <div id="crumpstxt"><%= Lang.TransA("Manage Bad Words") %></div>
				</div>
                <div id="sidebar">
                	<uc1:AdminMenu ID="AdminMenu1" runat="server"></uc1:AdminMenu>
                </div>
                <div id="main">
                <table width="100%" cellpadding="0" cellspacing="0">
                	<tr>
                		<td></td>
                	</tr>
                </table>
				    <div id="content-head">
                    	<div class="ch-head"><%= Lang.TransA("Manage Bad Words")%></div>
                        <div class="ch-description">
                        	<%= Lang.TransA("In this section you can enter bad words or modify existing ones...")%>
                        </div>
                    </div>   
                    <uc1:messagebox id="MessageBox" runat="server"></uc1:messagebox>   
					<table cellpadding="0" cellspacing="0" id="stable">
					    <tr>
					        <td width="20"><asp:CheckBox ID="cbUserRegularExpressions" runat="server" /></td>
					        <td align="left"><%= Lang.TransA("Use regular expressions") %></td>
					    </tr>
						<tr>
							<td colspan="2">
                              	<asp:TextBox ID="txtBadWords" runat="server" TextMode="multiLine" CssClass="multilinetextbox"></asp:TextBox>
							</td>
						</tr>
					</table>
					<div class="info">* <%= Lang.TransA("Every word have to be on new line") %></div>
                    <div class="separator10"></div>
                    <div class="add-buttons" style="width:508px">
	                    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" />
                    </div>
                    <div class="separator06"></div>
	            </div>
	        </div>
	    </form>
	</body>
</html>

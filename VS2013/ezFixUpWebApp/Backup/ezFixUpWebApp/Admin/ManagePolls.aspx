﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManagePolls.aspx.cs" Inherits="ezFixUp.Admin.ManagePolls" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register Src="../Components/DatePicker.ascx" TagName="DatePicker" TagPrefix="uc2" %>
<%@ Import Namespace="ezFixUp.Classes"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "https://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="https://www.w3.org/1999/xhtml" >
<head runat="server">
		<title> <%= Lang.TransA("Manage Polls") %></title>
		<link rel="stylesheet" type="text/css" href="images/style.css" media="all" />
</head>
<body>
		<form id="Form1" method="post" runat="server">
			<uc1:AdminHeader id="AdminHeader1" runat="server"></uc1:AdminHeader>
			<div id="layout">
				<div id="crupms">
					<img src="images/i_polls.jpg"> 
				</div>
				<div id="sidebar">
					<uc1:AdminMenu id="Adminmenu1" runat="server"></uc1:AdminMenu>
				</div>
			    <div id="main">
					<div id="content-head">
						<div class="ch-head"><%= Lang.TransA("Poll Management") %></div>
						<div class="ch-description"><%= Lang.TransA("Poll Management")%></div>
					</div>
					<uc1:messagebox id="MessageBox" runat="server"></uc1:messagebox>
					<table cellpadding="0" cellspacing="0" width="100%" class="filter">
				        <tr>
					        <td class="table_header"><%= Lang.Trans("Polls") %></td>
				        </tr>
						<tr>
							<td class="table_cell">
							    <asp:DropDownList ID="ddPolls" runat="server" AutoPostBack="true" 
                                    onselectedindexchanged="ddPolls_SelectedIndexChanged"></asp:DropDownList>
                                    <div class="separator06"></div>
                                <asp:Button ID="btnDelete" runat="server" onclick="btnDelete_Click" /> <br />
                                <div id="divPoll" runat="server">
                                    <%= "Poll Question:".TranslateA() %> <asp:TextBox ID="txtQuestion" runat="server"></asp:TextBox><br />
							        <asp:DataList ID="dlChoices" runat="server">
							            <ItemTemplate>
							                <asp:HiddenField ID="hidID" Value='<%# Eval("ID") %>' runat="server" />
							                <%# "Choice".TranslateA() + " " + Eval("Number") %> <asp:TextBox ID="txtChoiceValue" Text='<%# Eval("ChoiceValue") %>' runat="server"></asp:TextBox>
							            </ItemTemplate>
							            <SeparatorTemplate>
                                    <div class="separator06"></div>
							            </SeparatorTemplate>
							        </asp:DataList>
							        <div class="separator10"></div>
                                    <%="Start Date:".Translate() %> <uc2:DatePicker ID="dpFromDate" MinYear="2009" MaxYear="2019"  runat="server" />
                                    <div class="separator06"></div>
                                    <%="End Date:".Translate() %> <uc2:DatePicker ID="dpToDate" runat="server" MinYear="2009" MaxYear="2019" />							        
                                    <div class="separator06"></div>
                                    <%="Show results until:".Translate() %> <uc2:DatePicker ID="dpShowResultsUntil" runat="server" MinYear="2009" MaxYear="2019" />
							        <div class="separator06"></div>
							        <div align="center"><asp:Button ID="btnSave" runat="server" onclick="btnSave_Click" /></div>
							    </div>
							</td>
						</tr>
					</table>
					<div class="separator10"></div>
				</div>
			</div>
		</form>
</body>
</html>